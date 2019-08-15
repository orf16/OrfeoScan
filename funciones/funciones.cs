using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;

namespace funciones
{
    public class funciones
    {
        public string conni = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = iduCluster-Scan.idu.gov.co)(PORT = 1975))(CONNECT_DATA = (SERVICE_NAME = gesdoc.idu.gov.co))); User Id = ow_orfeo; Password = TESTING;";
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

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
