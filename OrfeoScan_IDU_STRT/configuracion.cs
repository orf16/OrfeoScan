using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.IO;

namespace OrfeoScan_IDU_STRT
{
    public partial class configuracion : Form
    {
        string IDU_path_img = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["DPATH1"].Value;
        string IDU_path_pdf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["EPATH1"].Value;
        string[,] nombresconpass = new string[2, 53];
        List<string> ListaUsuarios = new List<string>();
        string ftp = "";
        public string configvalue1 = ConfigurationManager.AppSettings["FTP_IDU_USER"];
        funciones.funciones funciones = new funciones.funciones();
        public configuracion()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["FTP_IDU_USER"].Value = cbUsuarios.Text;
            for (int i = 0; i < 53; i++)
            {
                for (int i1 = 0; i1 < 2; i1++)
                {
                    if (i1 == 0)
                    {
                        if (cbUsuarios.Text== nombresconpass[0,i])
                        {
                            config.AppSettings.Settings["FTP_IDU_PASSWORD"].Value = nombresconpass[1, i];
                        }
                    }
                }
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show("Usuario FTP guardado");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(IDU_path_img))
            {
                DirectoryInfo di = Directory.CreateDirectory(IDU_path_img);
            }
            if (!Directory.Exists(IDU_path_pdf))
            {
                DirectoryInfo di = Directory.CreateDirectory(IDU_path_pdf);
            }
            crear_folder_año(IDU_path_img);
            crear_folder_año(IDU_path_pdf);
            MessageBox.Show("Creación de carpetas IDU ejecutado");
        }
        private void crear_folder_dependencia(string path)
        {
            if (funciones.conexion_test(funciones.conni))
            {
                string sql = "SELECT DISTINCT DEPE_CODI FROM OW_ORFEO.DEPENDENCIA";
                OracleConnection con = new OracleConnection(funciones.conni);
                try
                {
                    con.Open();
                    OracleCommand command = new OracleCommand(sql, con);
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int DEPE_CODI = 0;
                        if (int.TryParse(reader[0].ToString(), out DEPE_CODI))
                        {
                            if (!Directory.Exists(path+ DEPE_CODI.ToString()+@"\"))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(path + DEPE_CODI.ToString() + @"\");
                            }
                            crear_folder_docs(path + DEPE_CODI.ToString() + @"\");
                        }
                    }
                    funciones.desconectar(con);
                }
                catch (Exception)
                {
                    funciones.desconectar(con);
                }
            }
        }
        private void crear_folder_año(string path)
        {
            List<int> listaAños = Enumerable.Range(1950, DateTime.Now.Year - 1950 + 1).ToList();
            foreach (var año in listaAños)
            {
                if (!Directory.Exists(path + año.ToString() + @"\"))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path + año.ToString() + @"\");
                }
                crear_folder_dependencia(path + año.ToString() + @"\");
            }
            if (!Directory.Exists(path + "temp" + @"\"))
            {
                DirectoryInfo di = Directory.CreateDirectory(path + "temp" + @"\");
            }
        }
        private void crear_folder_docs(string path)
        {
            if (!Directory.Exists(path + @"\docs\"))
            {
                DirectoryInfo di = Directory.CreateDirectory(path + @"\docs\");
            }
        }
        private void configuracion_Load(object sender, EventArgs e)
        {
            this.Icon = OrfeoScan_IDU_STRT.Properties.Resources.icon;
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            for (int i = 0; i < 53; i++)
            {
                for (int i1 = 0; i1 < 2; i1++)
                {
                    if (i1==0)
                    {
                        string valorUsuario = "digitalizador" + (i + 1).ToString().PadLeft(2, '0');
                        nombresconpass[i1, i] = valorUsuario;
                        cbUsuarios.Items.Add(valorUsuario);
                    }
                    else
                    {
                        nombresconpass[i1, i] = "D1g1t4l#0129";
                    }
                }
            }
            var ftp_user = config.AppSettings.Settings["FTP_IDU_USER"].Value;
            foreach (var item in cbUsuarios.Items)
            {
                if (item.ToString()== ftp_user)
                {
                    cbUsuarios.Text = ftp_user;
                }
            }
            ftp = config.AppSettings.Settings["FTP_SERVER"].Value + config.AppSettings.Settings["FTP_P1"].Value+ config.AppSettings.Settings["FTP_ROUTE"].Value+ config.AppSettings.Settings["FTP_P2"].Value;
            if (ftp!=null)
            {
                txtFTP.Text = ftp;
            }

            if (config.AppSettings.Settings["PRINTER_NAME"].Value!=null)
            {
                textBox1.Text = config.AppSettings.Settings["PRINTER_NAME"].Value;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string st = txtFTP.Text;
            if (st.Length==8)
            {
                string s1 = st.Substring(0, 2);
                string s2 = st.Substring(2, 2);
                string s3 = st.Substring(4, 2);
                string s4 = st.Substring(6, 2);
                config.AppSettings.Settings["FTP_SERVER"].Value = s1;
                config.AppSettings.Settings["FTP_P1"].Value = s2;
                config.AppSettings.Settings["FTP_ROUTE"].Value = s3;
                config.AppSettings.Settings["FTP_P2"].Value = s4;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show("Servidor Guardado");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                config.AppSettings.Settings["PRINTER_NAME"].Value = textBox1.Text;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                MessageBox.Show("Impresora por defecto guardada");
            }
        }
    }
}
