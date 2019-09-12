using model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrfeoScan_IDU_STRT
{
    public partial class Contenedor : Form
    {
        private USUARIO usuarioScanOrfeo;
        public Contenedor(USUARIO usuario)
        {
            usuarioScanOrfeo = usuario;
            InitializeComponent();
        }
        private void Contenedor_FormClosed(object sender, FormClosedEventArgs e)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);
            foreach (Form f in openForms)
            {
                f.Close();
            }
        }
        private void Contenedor_Load(object sender, EventArgs e)
        {
            tssl_valor_dependencia.Text = usuarioScanOrfeo.DEPE_CODI.ToString();
            tssl_valor_usuario.Text = usuarioScanOrfeo.USUA_NOMB.ToString();
            foreach (Control control in this.Controls)
            {
                MdiClient client = control as MdiClient;
                if (!(client == null))
                {
                    client.BackColor = Color.White;
                    break;
                }
            }
            this.Text = "SCANORFEO";
            //this.Icon = ADALETVERIFICADOR.Properties.Resources.logo_adalet1;
            ScanOrfeo ScanOrfeo = new ScanOrfeo(usuarioScanOrfeo);
            ScanOrfeo.MdiParent = this;
            ScanOrfeo.Show();
        }
    }
}
