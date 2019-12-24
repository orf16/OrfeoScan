using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;


namespace OrfeoScan_IDU_STRT.funciones
{
    public class funciones
    {
        //pruebas
        //public string conni = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = iduCluster-Scan.idu.gov.co)(PORT = 1975))(CONNECT_DATA = (SERVICE_NAME = gesdoc.idu.gov.co))); User Id = ow_orfeo; Password = TESTING; Pooling=False;";
        //Producción
        public string conni = "Data Source=(DESCRIPTION=    (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=iducluster02-scan )(PORT=1521)))    (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orfeo.idu.gov.co)));User Id=ow_orfeo ;Password=Orf3011g2014 ; Pooling=False;";

        public bool conexion_test(string cadena)
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
            }
            return false;
        }
        public void conectar(OracleConnection con)
        {
            try
            {
                con.Open();
            }
            catch (Exception)
            {
                con = null;
            }
        }
        public void desconectar(OracleConnection con)
        {
            try
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }
        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}
