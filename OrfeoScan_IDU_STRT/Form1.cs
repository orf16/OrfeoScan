using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OrfeoScan_IDU_STRT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string sql0 = "select * from usuario where upper(usua_login)='" + UCase(username) + "'";
            string sql0 = "select * from usuario where upper(usua_login)='"+("caeslava2").ToUpper()+"'";
            if (probarconexionsisipec("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=iduCluster-Scan.idu.gov.co)(PORT=1975))(CONNECT_DATA=(SERVICE_NAME=gesdoc.idu.gov.co)));User Id = ow_orfeo; Password = TESTING;"))
            {

            }
        }
        public bool probarconexionsisipec(string cadena)
        {
            try
            {
                OracleConnection con = new OracleConnection(cadena);
                con.Open();
                con.Close();
                con.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No es posible conectarse a la base de datos");
            }
            return false;
        }
        public bool conectar(string cadena)
        {
            try
            {
                OracleConnection con = new OracleConnection(cadena);
                con.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public bool desconectar(OracleConnection conn)
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }
}
