using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using model;
using System.Configuration;
using System.IO;

namespace OrfeoScan_IDU_STRT
{
    public partial class Login : Form
    {
        funciones.funciones funciones = new funciones.funciones();
        public USUARIO usuario;
        public int tipo_usuario=0;
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            this.Icon = OrfeoScan_IDU_STRT.Properties.Resources.icon;
            if (funciones.conexion_test(funciones.conni))
            {
                tSSL2.Text = "Conectado";
            }
            else
            {
                tSSL2.Text = "Sin Conexión";
            }
            string[] Dependencia = new string[4];

            if (Directory.Exists(@"D:\imgidu\work"))
            {
                DirectoryInfo di = new System.IO.DirectoryInfo(@"D:\imgidu\work");
                foreach (FileInfo file in di.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            
            
        }
        private void btn_ingresar_Click(object sender, EventArgs e)
        {


            string username = txtUsuario.Text.ToUpper();
            string password = txtContraseña.Text;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (funciones.conexion_test(funciones.conni))
                {
                    if (validarusuario(username, password)!=null)
                    {
                        if (usuario.USUA_DIGITALIZADOR>0)
                        {
                            tipo_usuario = usuario.USUA_DIGITALIZADOR;
                            ScanOrfeo Contenedor = new ScanOrfeo(usuario);
                            Contenedor.Show();
                            this.Hide();
                        }
                        else
                            MessageBox.Show("El usuario de Orfeo no tiene permiso para acceder al digitalizador");
                    }
                    else
                        MessageBox.Show("Usuario o Contraseña es incorrecta, vuelva a intentar");
                }
                else
                    MessageBox.Show("No es posible una conexión con la base de datos");
            }
            else
                MessageBox.Show("Digite el nombre de usuario y la contraseña");
        }
        private USUARIO validarusuario(string username, string password)
        {
            var md5_text = funciones.MD5Hash(password);
            password = md5_text.Substring(1, 26);
            string sql = "SELECT USUA_CODI, DEPE_CODI,PERM_RADI,USUA_NOMB,USUA_LOGIN,USUA_DOC FROM OW_ORFEO.USUARIO WHERE USUA_LOGIN='" + username + "' and USUA_PASW='" + password + "'";
            OracleConnection con = new OracleConnection(funciones.conni);
            try
            {
                con.Open();
                OracleCommand command = new OracleCommand(sql, con);
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new USUARIO();
                        int USUA_CODI = 0;
                        int USUA_DIGITALIZADOR = 1;
                        int DEPE_CODI = 0;
                        if (int.TryParse(reader[0].ToString(), out USUA_CODI))
                            usuario.USUA_CODI = USUA_CODI;
                        //if (int.TryParse(reader[1].ToString(), out USUA_DIGITALIZADOR))
                        //    usuario.USUA_DIGITALIZADOR = USUA_DIGITALIZADOR;
                        if (int.TryParse(reader[1].ToString(), out DEPE_CODI))
                            usuario.DEPE_CODI = DEPE_CODI;
                        usuario.PERM_RADI = (string)reader[2];
                        usuario.USUA_NOMB = (string)reader[3];
                        usuario.USUA_LOGIN = (string)reader[4];
                        usuario.USUA_DOC = (string)reader[5];
                        usuario.USUA_DIGITALIZADOR = 1;
                        funciones.desconectar(con);
                        return usuario;
                    }
                    else
                        funciones.desconectar(con);
                }
            }
            catch (Exception)
            {
                funciones.desconectar(con);
                return null;
            }
            return null;
        }

        private void Login_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_ingresar_Click(null, null);
        }

        private void txtUsuario_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_ingresar_Click(null, null);
        }

        private void txtContraseña_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_ingresar_Click(null, null);
        }
    }
}
