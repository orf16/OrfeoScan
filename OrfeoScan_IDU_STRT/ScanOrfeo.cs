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
using System.Security.Cryptography;
using OrfeoScan_IDU_STRT.funciones;
using model;
using System.Configuration;
using System.Drawing.Printing;
using NTwain;
using NTwain.Data;
using System.Reflection;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using System.IO;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net;
using System.Drawing.Drawing2D;
using funciones;

namespace OrfeoScan_IDU_STRT
{
    public partial class ScanOrfeo : Form
    {
        //Variables de Scanner
        ImageCodecInfo _tiffCodecInfo;
        TwainSession _twain;
        bool _stopScan;
        bool _loadingCaps;
        List<System.Drawing.Image> imagenes = new List<System.Drawing.Image>();
        List<PictureBox> boxes = new List<PictureBox>();
        int numero_box = 0;
        bool btnAllSettings = false;
        private int HoJas;
        private string Inf_radicadoIMG1;
        private string Inf_radicadoIMG2;
        private string Inf_radicadoIMG3;
        private string numeroExpediente;
        private string espCodi;
        private string varFechaSistema = "";
        private string varIsNull = "";
        private bool Doc_Anexo;
        private int paginaActual = 0;

        //Variables de formularios
        funciones.funciones funciones = new funciones.funciones();
        RawPrinterHelper RawPrinterHelper = new RawPrinterHelper();
        private string varConcat = "";
        private string varSubstr = "";
        private string varRadi_Fech_radi = "";
        private USUARIO usuarioScanOrfeo;
        private string DEPRADICADO;
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private string DirTraB;
        string digitalizador_user = "digitalizador40";
        string digitalizador = "D1g1t4l#0129";
        int[] pageRange = new int[2] { 0, 2 };
        string pref_pag = "PAG ";

        public List<System.Drawing.Image> TiffCarga = new List<System.Drawing.Image>();

        public ScanOrfeo(USUARIO usuario)
        {
            usuarioScanOrfeo = usuario;
            InitializeComponent();
            foreach (var enc in ImageCodecInfo.GetImageEncoders())
            {
                if (enc.MimeType == "image/tiff") { _tiffCodecInfo = enc; break; }
            }
        }
        private void ScanOrfeo_Load(object sender, EventArgs e)
        {
            cBoxtRadicado_load();
            int index_cBoxtRadicado = 0;
            foreach (var item in cBoxtRadicado.Items)
            {
                if (item.ToString() == "Entrada")
                {
                    cBoxtRadicado.SelectedIndex = index_cBoxtRadicado;
                }
                index_cBoxtRadicado++;
            }
            varConcat = "||";
            varSubstr = "substr";
            varRadi_Fech_radi = "TO_CHAR(a.RADI_FECH_RADI,'YYYYMM')";
            varFechaSistema = "sysdate";
            varIsNull = "nvl";
            DirTraB = config.AppSettings.Settings["DPATH"].Value;

            SetupTwain();
            foreach (var src in _twain)
            {
                var srcBtn = new ToolStripMenuItem(src.Name);
                srcBtn.Tag = src;
                srcBtn.Click += SourceMenuItem_Click;
                srcBtn.Checked = _twain.CurrentSource != null && _twain.CurrentSource.Name == src.Name;
                seleccionarEscanerToolStripMenuItem.DropDownItems.Insert(0, srcBtn);
            }
            bool primerEscaner = true;

            foreach (var btn in seleccionarEscanerToolStripMenuItem.DropDownItems)
            {
                if (primerEscaner)
                {
                    var srcBtn = btn as ToolStripMenuItem;
                    var src = srcBtn.Tag as DataSource;
                    if (src.Open() == ReturnCode.Success)
                    {
                        srcBtn.Checked = true;
                        btnStartCapture.Enabled = true;
                        LoadSourceCaps();
                    }
                }
                primerEscaner = false;
            }
        }
        private void cBoxtRadicado_load()
        {
            cBoxtRadicado.Items.Clear();
            if (funciones.conexion_test(funciones.conni))
            {
                string sql = "SELECT SGD_TRAD_CODIGO, SGD_TRAD_DESCR FROM OW_ORFEO.SGD_TRAD_TIPORAD ORDER BY SGD_TRAD_CODIGO";
                OracleConnection con = new OracleConnection(funciones.conni);
                try
                {
                    con.Open();
                    OracleCommand command = new OracleCommand(sql, con);
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int SGD_TRAD_CODIGO = 0;
                        if (int.TryParse(reader[0].ToString(), out SGD_TRAD_CODIGO))
                        {
                            cBoxtRadicado.Items.Add(SGD_TRAD_CODIGO.ToString() + " " + (string)reader[1]);
                        }
                    }
                    funciones.desconectar(con);
                }
                catch (Exception ex)
                {
                    funciones.desconectar(con);
                }
            }
        }
        private void buscarRadicadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBoxResult result = InputBox.Show("Digite el registro de radicado a consultar", "Consultar Radicado", string.Empty, 100, 0);
            if (result.ReturnCode == DialogResult.OK)
            {
                if (result.Text.Length > 4)
                {
                    if (!string.IsNullOrEmpty(usuarioScanOrfeo.DEPE_CODI.ToString().Trim()))
                        BuscarRadicado(result.Text);
                    else
                        MessageBox.Show("Debe seleccionar una dependencia a Buscar");
                }
                else
                    MessageBox.Show("El número de radicado debe tener más de 4 caracteres");
            }
        }
        private void buscarEnTodosLosRadicadoOExpedientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBoxResult result = InputBox.Show("Registro a Buscar", "Buscar Expediente", string.Empty, 100, 0);
            if (result.ReturnCode == DialogResult.OK)
            {
                if (result.Text.Length > 4)
                {
                    if (!string.IsNullOrEmpty(usuarioScanOrfeo.DEPE_CODI.ToString().Trim()))
                        BuscarRadicadoExpediente(result.Text);
                    else
                        MessageBox.Show("Debe seleccionar una dependencia a Buscar");
                }
                else
                    MessageBox.Show("El número de expediente debe tener más de 4 caracteres");
            }
        }
        private void BuscarRadicadoExpediente(string numradicado)
        {
            limpiar_informacion_radicado();
            dataGridView1.DataSource = null;
            string IISQL;
            OracleConnection con = new OracleConnection(funciones.conni);
            IISQL = " SELECT 'EXPEDIENTE' as TIPO,S.DEPE_CODI AS DEPENDENCIA, S.SGD_EXP_NUMERO AS NÚMERO_EXPEDIENTE,(SELECT COUNT(*) FROM SGD_AEX_ANEXOEXPEDIENTE A WHERE A.SGD_AEX_EXPEDIENTE = S.SGD_EXP_NUMERO) AS NÚM_ANEXOS ,S.SGD_SEXP_PAREXP1 AS ASUNTO,S.SGD_SEXP_PAREXP3 AS NOMBRE_Y_DOCUMENTO,S.SGD_SEXP_FECH AS FECHA ";
            IISQL = IISQL + " ";
            IISQL = IISQL + " FROM SGD_SEXP_SECEXPEDIENTES S ";
            IISQL = IISQL + " WHERE S.SGD_EXP_NUMERO LIKE '%" + numradicado.Trim() + "%' ";
            IISQL = IISQL + " ORDER BY S.SGD_SEXP_FECH DESC ";
            try
            {
                con.Open();
                show_loading_panel(1092, 182, 74, 163);
                OracleCommand command = new OracleCommand(IISQL, con);
                OracleDataReader reader = command.ExecuteReader();
                OracleDataAdapter sda = new OracleDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Close();
                con.Dispose();
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                hide_loading_panel();
            }
            catch (Exception ex)
            {
                hide_loading_panel();
                con.Close();
                con.Dispose();
                MessageBox.Show(ex.ToString());
            }
            System.Windows.Forms.Clipboard.SetText(IISQL);
        }
        private void BuscarRadicado(string numradicado)
        {
            limpiar_informacion_radicado();
            dataGridView1.DataSource = null;
            string IISQL;
            OracleConnection con = new OracleConnection(funciones.conni);
            if (impresiónDeSobresToolStripMenuItem.Checked)
                IISQL = "Select 'RADICADO' as TIPO, a.RADI_NUME_RADI AS NUMERO_RADICADO,a.RADI_FECH_RADI AS FECHA,renv.SGD_RENV_NOMBRE AS DESTINO,renv.SGD_RENV_DIR AS DIRECCIÓN,renv.SGD_RENV_DEPTO AS DEPARTAMENTO,renv.SGD_RENV_MPIO AS MUNICIPIO,a.RA_ASUN AS ASUNTO from Radicado a,dependencia b, sgd_renv_regenvio renv where a.RADI_DEPE_ACTU=b.DEPE_CODI AND a.RADI_NUME_RADI=renv.RADI_NUME_SAL AND a.RADI_CHAR_RADI LIKE '" + DateTime.Now.Year.ToString() + usuarioScanOrfeo.DEPE_CODI.ToString().Substring(0, 3) + "%'";
            else
                IISQL = "Select 'RADICADO' as TIPO, a.RADI_NUME_HOJA PAGINAS,a.RADI_NUME_RADI NUMERO_RADICADO,a.RADI_FECH_RADI FECHA,a.RA_ASUN ASUNTO, a.RADI_DEPE_ACTU DEPENDENCIA_ACTUAL,a.RADI_PATH PATH  from Radicado a where a.radi_nume_radi is not null  ";

            IISQL += " and a.radi_char_radi like '%" + numradicado.Trim() + "%' ";
            try
            {
                show_loading_panel(361, 174, 414, 36);
                con.Open();
                OracleCommand command = new OracleCommand(IISQL, con);
                OracleDataReader reader = command.ExecuteReader();
                OracleDataAdapter sda = new OracleDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Close();
                con.Dispose();
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                hide_loading_panel();
            }
            catch (Exception ex)
            {
                hide_loading_panel();
                con.Close();
                con.Dispose();
                MessageBox.Show(ex.ToString());
            }
            System.Windows.Forms.Clipboard.SetText(IISQL);
        }
        private void anexarImagenAUnRadicadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!anexarImagenAUnRadicadoToolStripMenuItem.Checked)
            {
                Doc_Anexo = true;
            }
            else
            {
                Doc_Anexo = false;
            }
            Verificar_Envio();
        }
        private void Verificar_Envio()
        {
            if (anexarImagenAUnRadicadoToolStripMenuItem.Checked)
            {
                btn_enviar_1.Visible = false;
                btn_enviar_2.Visible = true;
            }
            else
            {
                btn_enviar_2.Visible = false;
                btn_enviar_1.Visible = true;
            }
        }
        private void buscarSoloEnTipoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cBoxtRadicado.Text.Trim()))
            {
                InputBoxResult result = InputBox.Show("Registro a Buscar Tipo de Radicacion " + cBoxtRadicado.Text, "Consultar Radicado por Tipo", string.Empty, 100, 0);
                if (result.ReturnCode == DialogResult.OK)
                {
                    if (result.Text.Length > 4)
                    {
                        if (!string.IsNullOrEmpty(usuarioScanOrfeo.DEPE_CODI.ToString().Trim()))
                            BuscarRadicadoTP(result.Text);
                        else
                            MessageBox.Show("Debe seleccionar una dependencia a Buscar");
                    }
                    else
                        MessageBox.Show("Introduzca mínimo 4 caracteres");
                }
            }
            else
                MessageBox.Show("Debe seleccionar un Tipo antes de iniciar la consulta");
        }
        private void BuscarRadicadoTP(string numradicado)
        {
            limpiar_informacion_radicado();
            dataGridView1.DataSource = null;
            string IISQL;
            OracleConnection con = new OracleConnection(funciones.conni);
            if (impresiónDeSobresToolStripMenuItem.Checked)
                IISQL = "Select a.RADI_PATH PATH, a.RADI_NUME_HOJA PAGINAS,a.RADI_NUME_RADI,a.RADI_FECH_RADI,a.RA_ASUN, a.RADI_NOMB " + varConcat + " a.RADI_PRIM_APEL" + varConcat + " a.RADI_SEGU_APEL AS RADI_NOMB,a.RADI_DEPE_ACTU,renv.SGD_RENV_NOMBRE,renv.SGD_RENV_DIR,renv.SGD_RENV_DEPTO,renv.SGD_RENV_MPIO,a.RADI_USUA_ACTU,b.depe_nomb," + varRadi_Fech_radi + " as anomes_rad from Radicado a,dependencia b, sgd_renv_regenvio renv where a.RADI_DEPE_ACTU=b.DEPE_CODI AND a.RADI_NUME_RADI=renv.RADI_NUME_SAL  AND a.RADI_CHAR_RADI LIKE '" + DateTime.Now.Year.ToString() + usuarioScanOrfeo.DEPE_CODI.ToString().Substring(0, 3) + "%'";
            else
                IISQL = "Select a.RADI_NUME_HOJA,a.RADI_NUME_RADI,a.RADI_FECH_RADI,a.RA_ASUN, a.RADI_DEPE_ACTU,a.RADI_PATH,a.RADI_USUA_ACTU from Radicado a where a.radi_nume_radi is not null  ";
            IISQL = IISQL + " and a.radi_char_radi like '%" + numradicado.Trim() + "%' ";

            string tipoRad = cBoxtRadicado.Text.Trim().Substring(0, 1);
            IISQL = IISQL + " and " + varSubstr + "(radi_char_radi,5,3) =  '" + usuarioScanOrfeo.DEPE_CODI.ToString().Substring(0, 3) + "'";
            IISQL = IISQL + " and a.radi_char_radi like '%" + tipoRad + "'";
            try
            {
                show_loading_panel(361, 174, 414, 36);
                con.Open();
                OracleCommand command = new OracleCommand(IISQL, con);
                OracleDataReader reader = command.ExecuteReader();
                OracleDataAdapter sda = new OracleDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Close();
                con.Dispose();
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                hide_loading_panel();
            }
            catch (Exception ex)
            {
                hide_loading_panel();
                con.Close();
                con.Dispose();
                MessageBox.Show(ex.ToString());
            }
            System.Windows.Forms.Clipboard.SetText(IISQL);
        }
        private void buscarEnTodosLosRadicadosMasivaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBoxResult result = InputBox.Show("Registro a Buscar Tipo de Radicacion Masiva, Digite el nombre exacto del Grupo de masiva o el PRIMER NUMERO DE RADICADO DEL GRUPO que ha enviado ", "Consultar Radicado por Masiva", string.Empty, 100, 0);
            if (result.ReturnCode == DialogResult.OK)
            {
                if (result.Text.Length > 4)
                {
                    if (!string.IsNullOrEmpty(usuarioScanOrfeo.DEPE_CODI.ToString().Trim()))
                        BuscarRadicadoMasiva(result.Text);
                    else
                        MessageBox.Show("Debe seleccionar una dependencia a Buscar");
                }
                else
                    MessageBox.Show("Introduzca mínimo 4 caracteres");
            }
        }
        private void BuscarRadicadoMasiva(string numradicado)
        {
            limpiar_informacion_radicado();
            dataGridView1.DataSource = null;
            string IISQL = string.Empty;
            OracleConnection con = new OracleConnection(funciones.conni);
            if (impresiónDeSobresToolStripMenuItem.Checked)
            {
                IISQL = "Select a.RADI_NUME_HOJA,a.RADI_NUME_RADI,a.RADI_FECH_RADI,a.RA_ASUN, CONCAT(CONCAT(CONCAT(CONCAT(a.RADI_NOMB , ' '), a.RADI_PRIM_APEL),' '), a.RADI_SEGU_APEL) AS RADI_NOMB,a.RADI_DEPE_ACTU,renv.SGD_RENV_NOMBRE,renv.SGD_RENV_DIR,renv.SGD_RENV_DEPTO,renv.SGD_RENV_MPIO,a.RADI_USUA_ACTU,b.depe_nomb," + varRadi_Fech_radi + " as anomes_rad from Radicado a,dependencia b, sgd_renv_regenvio renv where a.RADI_DEPE_ACTU=b.DEPE_CODI AND a.RADI_NUME_RADI=renv.RADI_NUME_SAL  ";
                IISQL = IISQL + " and renv.radi_nume_grupo like '" + numradicado.Trim() + "' ";
            }
            if (!string.IsNullOrEmpty(IISQL))
            {
                try
                {
                    show_loading_panel(361, 174, 414, 36);
                    con.Open();
                    OracleCommand command = new OracleCommand(IISQL, con);
                    OracleDataReader reader = command.ExecuteReader();
                    OracleDataAdapter sda = new OracleDataAdapter(command);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    con.Close();
                    con.Dispose();
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    hide_loading_panel();
                }
                catch (Exception ex)
                {
                    hide_loading_panel();
                    con.Close();
                    con.Dispose();
                    MessageBox.Show(ex.ToString());
                }
                System.Windows.Forms.Clipboard.SetText(IISQL);
            }
        }
        private void guardarImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarImagen();
        }
        private void GuardarImagen()
        {
            if (!string.IsNullOrEmpty(labelImagen.Text.Trim()))
            {
                DEPRADICADO = labelImagen.Text.Substring(5, 3);
                string imagenf = "/" + labelImagen.Text.Substring(0, 4) + "/" + DEPRADICADO + "/" + labelImagen.Text + ".tif";
                string dirserver = "/" + labelImagen.Text.Substring(0, 4) + "/" + DEPRADICADO + "/";
                string ImagenLocal = DirTraB + imagenf;
            }
        }
        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            configuracion frm = new configuracion();
            frm.Show();
        }
        private void imprimirImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show print dialog
            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += Doc_PrintPage;
            pd.Document = doc;
            if (pd.ShowDialog() == DialogResult.OK)
                doc.Print();
        }
        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            //Print image
            Bitmap bm = new Bitmap(816, 1056);
            PageEdit.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, PageEdit.Width, PageEdit.Height));
            e.Graphics.DrawImage(bm, 0, 0);
            bm.Dispose();
        }
        private void preparar_pagina(PictureBox picture)
        {
            //PageEdit.Image = null;
            picture.Image = null;
        }
        private void cargar_pagina(string path, PictureBox picture)
        {
            //picture.Image =;
        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            var imagen = RotateImage(TiffCarga[0], 90, true, true, System.Drawing.Color.White);
            TiffCarga[0] = imagen;
            PageEdit.Image = imagen;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var imagen = RotateImage(TiffCarga[0], -90, true, true, System.Drawing.Color.White);
            TiffCarga[0] = imagen;
            PageEdit.Image = imagen;
        }

        private void btn_enviar_2_Click(object sender, EventArgs e)
        {

        }

        private void btn_enviar_1_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Imagen (*.tif, *.tiff) | *.tif; *.tiff";
            dialog.InitialDirectory = @"C:\";
            dialog.Title = "Abrir Imagen";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                var img = Bitmap.FromFile(fileName);
                var pages = img.GetFrameCount(FrameDimension.Page);
                List<System.Drawing.Image> cargar = new List<System.Drawing.Image>();
                cargarImagen0(pageRange, true, fileName);
            }
        }
        private List<System.Drawing.Image> Split(string pstrInputFilePath)
        {
            List<System.Drawing.Image> cargar = new List<System.Drawing.Image>();
            System.Drawing.Image tiffImage = System.Drawing.Image.FromFile(pstrInputFilePath);
            Guid objGuid = tiffImage.FrameDimensionsList[0];
            FrameDimension dimension = new FrameDimension(objGuid);
            int noOfPages = tiffImage.GetFrameCount(dimension);

            ImageCodecInfo encodeInfo = null;
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < imageEncoders.Length; j++)
            {
                if (imageEncoders[j].MimeType == "image/tiff")
                {
                    encodeInfo = imageEncoders[j];
                    break;
                }
            }

            foreach (Guid guid in tiffImage.FrameDimensionsList)
            {
                for (int index = 0; index < noOfPages; index++)
                {
                    FrameDimension currentFrame = new FrameDimension(guid);
                    tiffImage.SelectActiveFrame(currentFrame, index);
                    Bitmap nextFrame = new Bitmap(tiffImage);
                    cargar.Add(nextFrame);
                }
            }
            return cargar;
        }
        private void cargarImagen(List<System.Drawing.Image> imagenes, int[] range, bool inicial)
        {
            TiffCarga.Clear();
            foreach (var imagen in imagenes)
            {
                Bitmap nextFrame = new Bitmap(imagen);
                TiffCarga.Add(nextFrame);
            }
            paginaActual = 1;

            bool pageEmpty1 = true;
            bool pageEmpty2 = true;
            bool pageEmpty3 = true;

            for (int i = range[0]; i < range[1] + 1; i++)
            {
                bool asigno = false;
                if (pageEmpty1 && !asigno)
                {
                    if (TiffCarga.Count >= i + 1)
                    {
                        PageScreen1.Image = TiffCarga[i];
                        lblScreen1.Text = pref_pag + (i + 1).ToString();
                        pageEmpty1 = false;
                        asigno = true;
                    }
                }
                if (pageEmpty2 && !asigno)
                {
                    if (TiffCarga.Count >= i + 1)
                    {
                        PageScreen2.Image = TiffCarga[i];
                        lblScreen2.Text = pref_pag + (i + 1).ToString();
                        pageEmpty2 = false;
                        asigno = true;
                    }
                }
                if (pageEmpty3 && !asigno)
                {
                    if (TiffCarga.Count >= i + 1)
                    {
                        PageScreen3.Image = TiffCarga[i];
                        lblScreen3.Text = pref_pag + (i + 1).ToString();
                        pageEmpty3 = false;
                        asigno = true;
                    }
                }
            }
            if (pageEmpty1)
            {
                PageScreen1.Image = null;
                lblScreen1.Text = "";
            }

            if (pageEmpty2)
            {
                PageScreen2.Image = null;
                lblScreen2.Text = "";
            }

            if (pageEmpty3)
            {
                PageScreen3.Image = null;
                lblScreen3.Text = "";
            }

            if (inicial && TiffCarga.Count >= 1)
                PageEdit.Image = TiffCarga[0];
        }
        private void cargarImagen1(int[] range)
        {

            bool pageEmpty1 = true;
            bool pageEmpty2 = true;
            bool pageEmpty3 = true;

            for (int i = range[0]; i < range[1] + 1; i++)
            {
                bool asigno = false;
                if (pageEmpty1 && !asigno)
                {
                    if (TiffCarga.Count >= i + 1)
                    {
                        PageScreen1.Image = TiffCarga[i];
                        lblScreen1.Text = pref_pag + (i + 1).ToString();
                        pageEmpty1 = false;
                        asigno = true;
                    }
                }
                if (pageEmpty2 && !asigno)
                {
                    if (TiffCarga.Count >= i + 1)
                    {
                        PageScreen2.Image = TiffCarga[i];
                        lblScreen2.Text = pref_pag + (i + 1).ToString();
                        pageEmpty2 = false;
                        asigno = true;
                    }
                }
                if (pageEmpty3 && !asigno)
                {
                    if (TiffCarga.Count >= i + 1)
                    {
                        PageScreen3.Image = TiffCarga[i];
                        lblScreen3.Text = pref_pag + (i + 1).ToString();
                        pageEmpty3 = false;
                        asigno = true;
                    }
                }
            }
            if (pageEmpty1)
            {
                PageScreen1.Image = null;
                lblScreen1.Text = "";
            }

            if (pageEmpty2)
            {
                PageScreen2.Image = null;
                lblScreen2.Text = "";
            }

            if (pageEmpty3)
            {
                PageScreen3.Image = null;
                lblScreen3.Text = "";
            }
        }

        private void cargarImagen0(int[] range, bool inicial, string path)
        {
            List<System.Drawing.Image> cargue = new List<System.Drawing.Image>();
            System.Drawing.Image tiffImage = System.Drawing.Image.FromFile(path);
            Guid objGuid = tiffImage.FrameDimensionsList[0];
            FrameDimension dimension = new FrameDimension(objGuid);
            int noOfPages = tiffImage.GetFrameCount(dimension);
            ImageCodecInfo encodeInfo = null;
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < imageEncoders.Length; j++)
            {
                if (imageEncoders[j].MimeType == "image/tiff")
                {
                    encodeInfo = imageEncoders[j];
                    break;
                }
            }
            foreach (Guid guid in tiffImage.FrameDimensionsList)
            {
                for (int index = range[0]; index < range[1] + 1; index++)
                {
                    FrameDimension currentFrame = new FrameDimension(guid);
                    tiffImage.SelectActiveFrame(currentFrame, index);
                    Bitmap nextFrame = new Bitmap(tiffImage);
                    cargue.Add(nextFrame);
                }
            }

            bool pageEmpty1 = true;
            bool pageEmpty2 = true;
            bool pageEmpty3 = true;

            for (int i = range[0]; i < range[1] + 1; i++)
            {
                bool asigno = false;
                if (pageEmpty1 && !asigno)
                {
                    if (cargue.Count >= i + 1)
                    {
                        PageScreen1.Image = cargue[i];
                        lblScreen1.Text = pref_pag + (i + 1).ToString();
                        pageEmpty1 = false;
                        asigno = true;
                    }
                }
                if (pageEmpty2 && !asigno)
                {
                    if (cargue.Count >= i + 1)
                    {
                        PageScreen2.Image = cargue[i];
                        lblScreen2.Text = pref_pag + (i + 1).ToString();
                        pageEmpty2 = false;
                        asigno = true;
                    }
                }
                if (pageEmpty3 && !asigno)
                {
                    if (cargue.Count >= i + 1)
                    {
                        PageScreen3.Image = cargue[i];
                        lblScreen3.Text = pref_pag + (i + 1).ToString();
                        pageEmpty3 = false;
                        asigno = true;
                    }
                }
            }
            if (pageEmpty1)
            {
                PageScreen1.Image = null;
                lblScreen1.Text = "";
            }

            if (pageEmpty2)
            {
                PageScreen2.Image = null;
                lblScreen2.Text = "";
            }

            if (pageEmpty3)
            {
                PageScreen3.Image = null;
                lblScreen3.Text = "";
            }

            if (inicial && cargue.Count >= 1)
                PageEdit.Image = cargue[0];
        }
        private void cargarImagen2(int[] range, string path)
        {

            bool pageEmpty1 = true;
            bool pageEmpty2 = true;
            bool pageEmpty3 = true;

            for (int i = range[0]; i < range[1] + 1; i++)
            {
                bool asigno = false;
                if (pageEmpty1 && !asigno)
                {
                    if (TiffCarga.Count >= i + 1)
                    {
                        PageScreen1.Image = TiffCarga[i];
                        lblScreen1.Text = pref_pag + (i + 1).ToString();
                        pageEmpty1 = false;
                        asigno = true;
                    }
                }
                if (pageEmpty2 && !asigno)
                {
                    if (TiffCarga.Count >= i + 1)
                    {
                        PageScreen2.Image = TiffCarga[i];
                        lblScreen2.Text = pref_pag + (i + 1).ToString();
                        pageEmpty2 = false;
                        asigno = true;
                    }
                }
                if (pageEmpty3 && !asigno)
                {
                    if (TiffCarga.Count >= i + 1)
                    {
                        PageScreen3.Image = TiffCarga[i];
                        lblScreen3.Text = pref_pag + (i + 1).ToString();
                        pageEmpty3 = false;
                        asigno = true;
                    }
                }
            }
            if (pageEmpty1)
            {
                PageScreen1.Image = null;
                lblScreen1.Text = "";
            }

            if (pageEmpty2)
            {
                PageScreen2.Image = null;
                lblScreen2.Text = "";
            }

            if (pageEmpty3)
            {
                PageScreen3.Image = null;
                lblScreen3.Text = "";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(digitalizador_user, digitalizador);
                    client.UploadFile("ftp://fs04cc01/bodega_dev_of01/hello_A1_b_cs.pdf", WebRequestMethods.Ftp.UploadFile, @"D:\hello_A1_b_cs.pdf");
                    MessageBox.Show("El archivo se subió correctamente");
                }
                var request = (FtpWebRequest)WebRequest.Create("ftp://fs04cc01/bodega_dev_of01/hello_A1_b_cs.pdf");
                request.Credentials = new NetworkCredential(digitalizador_user, digitalizador);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                try
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    MessageBox.Show("El archivo se subió correctamente");
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode ==
                        FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        MessageBox.Show("El archivo no se subió correctamente, por favor vuelva a intentar");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        private void show_loading_panel(int size_x, int size_y, int loc_x, int loc_y)
        {
            pan_loading.Location = new Point(loc_x, loc_y);
            pan_loading.Size = new Size(size_x, size_y);
            pan_loading.Visible = true;
            pan_loading.Enabled = true;
            pan_loading.Refresh();
        }
        private void hide_loading_panel()
        {
            pan_loading.Location = new Point(0, 0);
            pan_loading.Size = new Size(1, 1);
            pan_loading.Visible = false;
            pan_loading.Enabled = false;
            pan_loading.Refresh();
        }





        //Metodos y funciones para Escaner
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetupTwain();
        }
        private void SetupTwain()
        {
            var appId = TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetEntryAssembly());
            _twain = new TwainSession(appId);
            _twain.StateChanged += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("State changed to " + _twain.State + " on thread " + Thread.CurrentThread.ManagedThreadId);
            };
            _twain.TransferError += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Got xfer error on thread " + Thread.CurrentThread.ManagedThreadId);
            };
            _twain.DataTransferred += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Transferred data event on thread " + Thread.CurrentThread.ManagedThreadId);

                // example on getting ext image info
                var infos = e.GetExtImageInfo(ExtendedImageInfo.Camera).Where(it => it.ReturnCode == ReturnCode.Success);
                foreach (var it in infos)
                {
                    var values = it.ReadValues();
                    PlatformInfo.Current.Log.Info(string.Format("{0} = {1}", it.InfoID, values.FirstOrDefault()));
                    break;
                }

                // handle image data
                System.Drawing.Image img = null;
                if (e.NativeData != IntPtr.Zero)
                {
                    var stream = e.GetNativeImageStream();
                    if (stream != null)
                    {
                        img = System.Drawing.Image.FromStream(stream);
                    }
                }
                else if (!string.IsNullOrEmpty(e.FileDataPath))
                {
                    img = new Bitmap(e.FileDataPath);
                }
                if (img != null)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        if (PageEdit.Image != null)
                        {
                            PageEdit.Image.Dispose();
                            PageEdit.Image = null;
                        }
                        PageEdit.Image = img;
                        var stream = new System.IO.MemoryStream();
                        img.Save(stream, System.Drawing.Imaging.ImageFormat.Tiff);
                        stream.Position = 0;
                        var image = System.Drawing.Image.FromStream(stream);
                        imagenes.Add(image);
                        pageRange[0] = 0;
                        pageRange[1] = 2;
                        cargarImagen(imagenes, pageRange, true);
                    }));
                }
            };
            _twain.SourceDisabled += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Source disabled event on thread " + Thread.CurrentThread.ManagedThreadId);
                this.BeginInvoke(new Action(() =>
                {
                    btnStopScan.Enabled = false;
                    btnStopScan.Visible = false;
                    btnStartCapture.Enabled = true;
                    btnStartCapture.Visible = true;
                    //panelOptions.Enabled = true;
                    LoadSourceCaps();
                    //clean_picturebox();
                    //cargar_picturebox();
                }));
            };
            _twain.TransferReady += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Transferr ready event on thread " + Thread.CurrentThread.ManagedThreadId);
                e.CancelAll = _stopScan;
            };

            // either set sync context and don't worry about threads during events,
            // or don't and use control.invoke during the events yourself
            PlatformInfo.Current.Log.Info("Setup thread = " + Thread.CurrentThread.ManagedThreadId);
            _twain.SynchronizationContext = SynchronizationContext.Current;
            if (_twain.State < 3)
            {
                // use this for internal msg loop
                _twain.Open();
                // use this to hook into current app loop
                //_twain.Open(new WindowsFormsMessageLoopHook(this.Handle));
            }
        }
        void SourceMenuItem_Click(object sender, EventArgs e)
        {
            // do nothing if source is enabled
            if (_twain.State > 4) { return; }

            if (_twain.State == 4) { _twain.CurrentSource.Close(); }

            foreach (var btn in seleccionarEscanerToolStripMenuItem.DropDownItems)
            {
                var srcBtn = btn as ToolStripMenuItem;
                if (srcBtn != null) { srcBtn.Checked = false; }
            }

            var curBtn = (sender as ToolStripMenuItem);
            var src = curBtn.Tag as DataSource;
            if (src.Open() == ReturnCode.Success)
            {
                curBtn.Checked = true;
                btnStartCapture.Enabled = true;
                LoadSourceCaps();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_twain != null)
            {
                if (e.CloseReason == CloseReason.UserClosing && _twain.State > 4)
                {
                    e.Cancel = true;
                }
                else
                {
                    CleanupTwain();
                }
            }
            base.OnFormClosing(e);
        }
        private void CleanupTwain()
        {
            if (_twain.State == 4)
            {
                _twain.CurrentSource.Close();
            }
            if (_twain.State == 3)
            {
                _twain.Close();
            }

            if (_twain.State > 2)
            {
                // normal close down didn't work, do hard kill
                _twain.ForceStepDown(2);
            }
        }

        private void configurarEscanerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _twain.CurrentSource.Enable(SourceEnableMode.ShowUIOnly, true, this.Handle);
        }
        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            imagenes.Clear();
            PageEdit.Image = null;
            if (_twain.State == 4)
            {
                //_twain.CurrentSource.CapXferCount.Set(4);

                _stopScan = false;

                if (_twain.CurrentSource.Capabilities.CapUIControllable.IsSupported)//.SupportedCaps.Contains(CapabilityId.CapUIControllable))
                {
                    // hide scanner ui if possible
                    if (_twain.CurrentSource.Enable(SourceEnableMode.NoUI, false, this.Handle) == ReturnCode.Success)
                    {
                        btnStopScan.Enabled = true;
                        btnStopScan.Visible = true;
                        btnStartCapture.Enabled = false;
                        btnStartCapture.Visible = false;
                    }
                }
                else
                {
                    if (_twain.CurrentSource.Enable(SourceEnableMode.ShowUI, true, this.Handle) == ReturnCode.Success)
                    {
                        btnStopScan.Enabled = true;
                        btnStopScan.Visible = true;
                        btnStartCapture.Enabled = false;
                        btnStartCapture.Visible = false;
                    }
                }
            }
        }

        private void btnStopScan_Click(object sender, EventArgs e)
        {
            _stopScan = true;
        }
        private void LoadSourceCaps()
        {
            var src = _twain.CurrentSource;
            _loadingCaps = true;

            //var test = src.SupportedCaps;

            if (groupDepth.Enabled = src.Capabilities.ICapPixelType.IsSupported)
            {
                LoadDepth(src.Capabilities.ICapPixelType);
            }
            if (groupDuplex.Enabled = src.Capabilities.ICapAutomaticRotate.IsSupported)
            {
                LoadAutoRotation(src.Capabilities.ICapAutomaticRotate);
            }
            if (groupDPI.Enabled = src.Capabilities.ICapXResolution.IsSupported && src.Capabilities.ICapYResolution.IsSupported)
            {
                LoadDPI(src.Capabilities.ICapXResolution);
            }
            // TODO: find out if this is how duplex works or also needs the other option
            if (groupDuplex.Enabled = src.Capabilities.CapDuplexEnabled.IsSupported)
            {
                LoadDuplex(src.Capabilities.CapDuplexEnabled);
            }
            if (groupSize.Enabled = src.Capabilities.ICapSupportedSizes.IsSupported)
            {
                LoadPaperSize(src.Capabilities.ICapSupportedSizes);
            }
            configurarEscanerToolStripMenuItem.Enabled = src.Capabilities.CapEnableDSUIOnly.IsSupported;
            _loadingCaps = false;
        }
        //Capacidad e tamaño de papel
        private void LoadPaperSize(ICapWrapper<SupportedSize> cap)
        {
            var list = cap.GetValues().ToList();
            comboSize.DataSource = list;
            var cur = cap.GetCurrent();
            if (list.Contains(cur))
            {
                comboSize.SelectedItem = cur;
            }
            var labelTest = cap.GetLabel();
            if (!string.IsNullOrEmpty(labelTest))
            {
                groupSize.Text = labelTest;
            }
        }
        //capacidad de dos caras
        private void LoadDuplex(ICapWrapper<BoolType> cap)
        {
            ckDuplex.Checked = cap.GetCurrent() == BoolType.True;
        }
        private void LoadAutoRotation(ICapWrapper<BoolType> cap)
        {
            ckDuplex.Checked = cap.GetCurrent() == BoolType.False;
        }
        //capacidad de numero de DPI
        private void LoadDPI(ICapWrapper<TWFix32> cap)
        {
            // only allow dpi of certain values for those source that lists everything
            var list = cap.GetValues().Where(dpi => (dpi % 50) == 0).ToList();
            comboDPI.DataSource = list;
            var cur = cap.GetCurrent();
            if (list.Contains(cur))
            {
                comboDPI.SelectedItem = cur;
            }
        }
        //Capacidad Calidad de Color
        private void LoadDepth(ICapWrapper<PixelType> cap)
        {
            var list = cap.GetValues().ToList();
            comboDepth.DataSource = list;
            var cur = cap.GetCurrent();
            if (list.Contains(cur))
            {
                comboDepth.SelectedItem = cur;
            }
            var labelTest = cap.GetLabel();
            if (!string.IsNullOrEmpty(labelTest))
            {
                groupDepth.Text = labelTest;
            }
        }

        private void comboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCaps && _twain.State == 4)
            {
                var sel = (SupportedSize)comboSize.SelectedItem;
                _twain.CurrentSource.Capabilities.ICapSupportedSizes.SetValue(sel);
            }
        }
        private void comboDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCaps && _twain.State == 4)
            {
                var sel = (PixelType)comboDepth.SelectedItem;
                _twain.CurrentSource.Capabilities.ICapPixelType.SetValue(sel);
            }
        }
        private void comboDPI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCaps && _twain.State == 4)
            {
                var sel = (TWFix32)comboDPI.SelectedItem;
                _twain.CurrentSource.Capabilities.ICapXResolution.SetValue(sel);
                _twain.CurrentSource.Capabilities.ICapYResolution.SetValue(sel);
            }
        }
        private void ckDuplex_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCaps && _twain.State == 4)
            {
                _twain.CurrentSource.Capabilities.CapDuplexEnabled.SetValue(ckDuplex.Checked ? BoolType.True : BoolType.False);
                _twain.CurrentSource.Capabilities.ICapAutomaticRotate.SetValue(BoolType.False);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {

        }

        private Point RectStartPoint;
        private System.Drawing.Rectangle Rect = new System.Drawing.Rectangle();
        private Brush selectionBrush = new SolidBrush(System.Drawing.Color.FromArgb(128, 72, 145, 220));
        private Brush selectionBrush1 = new SolidBrush(System.Drawing.Color.FromArgb(1, 130, 48, 211));
        private Point _StartPoint;
        //imagen
        private void button9_Click(object sender, EventArgs e)
        {
            if (PageEdit.Image != null && Rect.X > 0 && Rect.Y > 0)
            {
                using (Bitmap bitmap = new Bitmap(Width, Height))
                using (Graphics graphics = Graphics.FromImage(TiffCarga[0]))
                {
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, Width, Height);
                    graphics.FillRectangle(new SolidBrush(System.Drawing.Color.White), Rect);
                    Invalidate();
                    PageEdit.Refresh();
                }
            }
        }
        private void PageEdit_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            Point tempEndPoint = e.Location;
            textBox1.Text = tempEndPoint.X.ToString();
            textBox2.Text = tempEndPoint.Y.ToString();

            _StartPoint = e.Location;
            textBox5.Text = _StartPoint.X.ToString();
            textBox4.Text = _StartPoint.Y.ToString();
            textBox5.Text = panel2.AutoScrollPosition.X.ToString();
            textBox4.Text = panel2.AutoScrollPosition.Y.ToString();


            if (PageEdit.Image != null)
            {
                if (tempEndPoint.X <= PageEdit.Image.Width && tempEndPoint.Y <= PageEdit.Image.Height)
                {
                    Rect.Location = new Point(
                        Math.Min(RectStartPoint.X, tempEndPoint.X),
                        Math.Min(RectStartPoint.Y, tempEndPoint.Y));
                    Rect.Size = new Size(
                        Math.Abs(RectStartPoint.X - tempEndPoint.X),
                        Math.Abs(RectStartPoint.Y - tempEndPoint.Y));

                    Point changePoint = new Point(e.Location.X - RectStartPoint.X,
                                  e.Location.Y - RectStartPoint.Y);
                    bool cambio = false;

                    //if (_StartPoint.Y >= 1320 && !cambio)
                    //{
                    //    panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 810);
                    //    cambio = true;
                    //}
                    if (_StartPoint.Y >= 1400 && !cambio)
                    {
                        panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 720);
                        cambio = true;
                    }
                    //if (_StartPoint.Y >= 1320 && !cambio)
                    //{
                    //    panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 810);
                    //    cambio = true;
                    //}
                    if (_StartPoint.Y >= 1200 && !cambio)
                    {
                        panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 600);
                        cambio = true;
                    }
                    //if (_StartPoint.Y >= 1080 && !cambio)
                    //{
                    //    panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 690);
                    //    cambio = true;
                    //}
                    if (_StartPoint.Y >= 960 && !cambio)
                    {
                        panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 480);
                        cambio = true;
                    }

                    //if (_StartPoint.Y >= 840 && !cambio)
                    //{
                    //    panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 330);
                    //    cambio = true;
                    //}
                    if (_StartPoint.Y >= 720 && !cambio)
                    {
                        panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 240);
                        cambio = true;
                    }
                    //if (_StartPoint.Y >= 600 && !cambio)
                    //{
                    //    panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 210);
                    //    cambio = true;
                    //}
                    if (_StartPoint.Y > 480 && !cambio)
                    {
                        panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 120);
                        cambio = true;
                    }
                    if (_StartPoint.Y > 360 && !cambio)
                    {
                        panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 0);
                        cambio = true;
                    }
                    //if (_StartPoint.Y >= 360 && !cambio)
                    //{
                    //    panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 360);
                    //    cambio = true;
                    //}




                    //if (_StartPoint.Y <= 1200 && _StartPoint.Y > 960 && !cambio)
                    //{
                    //    panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, 1440);
                    //    cambio = true;
                    //}

                    //if (_StartPoint.Y > 600)
                    //{
                    //    panel2.AutoScrollPosition = new Point(-panel2.AutoScrollPosition.X, -600);
                    //}




                    //New Drawing.Point((DeltaX - Panel1.AutoScrollPosition.X), (DeltaY - Panel1.AutoScrollPosition.Y));
                }
            }
            PageEdit.Invalidate();
        }
        private void PageEdit_MouseDown(object sender, MouseEventArgs e)
        {
            // Determine the initial rectangle coordinates...
            RectStartPoint = e.Location;

            Invalidate();
        }
        private void PageEdit_Paint(object sender, PaintEventArgs e)
        {
            // Draw the rectangle...
            if (PageEdit.Image != null)
            {
                if (Rect != null && Rect.Width > 0 && Rect.Height > 0)
                {
                    if (Rect.Width <= PageEdit.Image.Width)
                    {
                        Pen blackPen = new Pen(System.Drawing.Color.Black, 1);
                        e.Graphics.DrawRectangle(blackPen, Rect);
                        e.Graphics.FillRectangle(selectionBrush, Rect);
                    }
                    else
                    {

                    }
                }
            }
        }
        private void PageEdit_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Rect.Contains(e.Location))
                {
                    Debug.WriteLine("Right click");
                }
            }
        }
        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {

            //if (e.Button == MouseButtons.Left)
            //{
            //    Point changePoint = new Point(e.Location.X - _StartPoint.X,
            //                                  e.Location.Y - _StartPoint.Y);
            //    panel1.AutoScrollPosition = new Point(-panel1.AutoScrollPosition.X - changePoint.X,
            //                                          -panel1.AutoScrollPosition.Y - changePoint.Y);
            //}
        }
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //    _StartPoint = e.Location;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            var rectangulo = Rect;
            var imagen = PageEdit.Image;
        }
        private System.Drawing.Image generarCodigoBarras(string numero_documento)
        {
            Barcode39 b = new Barcode39();
            System.Drawing.Image img;
            b.ShowString = true;
            b.IncludeCheckSumDigit = true;
            b.TextFont = new System.Drawing.Font("Courier New", 10);
            img = b.GenerateBarcodeImage(codbarras.Width, codbarras.Height, numero_documento);
            return img;
        }
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            limpiar_informacion_radicado();
            if (dataGridView1.Rows.Count > 0)
            {
                string tipo_rem = "";
                string tipo = "";
                string numero_documento = "";
                string paginas = "";
                string fecha = "";
                string asunto = "";
                string dependencia = "";
                string path = "";
                string nombre_documento = "";
                int OEM = 0;
                int ESP = 0;
                int CIU = 0;
                string FUN = "";
                string remitente = "";

                if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
                    tipo = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                if (dataGridView1.Rows[e.RowIndex].Cells[2].Value != null)
                    numero_documento = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

                if (!string.IsNullOrEmpty(tipo) && !string.IsNullOrEmpty(numero_documento))
                {
                    string IISQL = "Select a.SGD_CIU_CODIGO,a.SGD_ESP_CODI,a.SGD_OEM_CODIGO,a.SGD_DOC_FUN from sgd_dir_drecciones a where a.radi_nume_radi=" + numero_documento;
                    IISQL = IISQL + " ORDER BY SGD_DIR_TIPO  ";
                    OracleConnection con = new OracleConnection(funciones.conni);
                    try
                    {
                        con.Open();
                        OracleCommand command = new OracleCommand(IISQL, con);
                        OracleDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            if (int.TryParse(reader[0].ToString(), out CIU))
                                FUN = "";
                            if (int.TryParse(reader[1].ToString(), out ESP))
                                FUN = "";
                            if (int.TryParse(reader[2].ToString(), out OEM))
                                FUN = "";
                            if (reader[3] != null)
                            {
                                FUN = reader[3].ToString();
                            }
                            funciones.desconectar(con);
                        }
                        else
                            funciones.desconectar(con);
                    }
                    catch (Exception)
                    {
                        funciones.desconectar(con);
                    }

                    if (tipo == "RADICADO")
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
                            paginas = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[e.RowIndex].Cells[3].Value != null)
                            fecha = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                        if (dataGridView1.Rows[e.RowIndex].Cells[4].Value != null)
                            asunto = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                        if (dataGridView1.Rows[e.RowIndex].Cells[5].Value != null)
                            dependencia = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                        if (dataGridView1.Rows[e.RowIndex].Cells[6].Value != null)
                            path = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                        lbl_InfoRadicado1.Text = tipo + " No." + numero_documento.Substring(0, 4) + "-" + numero_documento.Substring(4, 3) + "-" + numero_documento.Substring(7, 6) + "-" + numero_documento.Substring(13, 1);
                        lbl_InfoRadicado2.Text = "Fecha" + fecha + "->" + dependencia;

                        if (OEM > 0)
                        {
                            IISQL = "Select a.sgd_oem_oempresa REMITENTE from SGD_OEM_OEMPRESAS a where a.SGD_OEM_CODIGO=" + OEM.ToString();
                            tipo_rem += "(OEM) ";
                        }
                        if (CIU > 0)
                        {
                            IISQL = "Select a.sgd_ciu_nombre || a.sgd_ciu_apell1 || sgd_ciu_apell2 REMITENTE from SGD_CIU_CIUDADANO a where a.sgd_ciu_codigo=" + CIU.ToString();
                            tipo_rem += "(CIU) ";
                        }
                        if (ESP > 0)
                        {
                            IISQL = "Select a.SIGLA_DE_LA_EMPRESA || '-' || a.NOMBRE_DE_LA_EMPRESA REMITENTE from BODEGA_EMPRESAS a where a.IDENTIFICADOR_EMPRESA=" + ESP.ToString();
                            tipo_rem += "(ESP) ";
                        }
                        if (FUN != string.Empty)
                        {
                            IISQL = "Select substr(USUA_NOMB,0,20) || '-' || DEPE_CODI REMITENTE from USUARIO a where a.usua_doc='" + FUN + "'";
                            tipo_rem += "(FUN) ";
                        }

                        con = new OracleConnection(funciones.conni);
                        try
                        {
                            con.Open();
                            OracleCommand command = new OracleCommand(IISQL, con);
                            OracleDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                if (reader[0] != null)
                                    remitente = reader[0].ToString();
                                funciones.desconectar(con);
                            }
                            else
                                funciones.desconectar(con);
                        }
                        catch (Exception ex)
                        {
                            funciones.desconectar(con);
                        }

                        lbl_InfoRadicado3.Text += tipo_rem;
                        lbl_InfoRadicado3.Text += remitente;

                        if (numero_documento.Length == 14)
                            codbarras.Image = generarCodigoBarras(numero_documento);

                        lbl_num_doc.Text = "RADICADO " + numero_documento.Substring(0, 4) + "-" + numero_documento.Substring(4, 3) + "-" + numero_documento.Substring(7, 6) + "-" + numero_documento.Substring(numero_documento.Length - 1);

                        if (path.Contains("pdf") || path.Contains("tif") || path.Contains("tiff"))
                            MessageBox.Show("El radicado seleccionado ya tiene un archivo asociado. Si continua, el pdf será remplazado.");
                    }
                    else if (tipo == "EXPEDIENTE")
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[4].Value != null)
                            asunto = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                        if (dataGridView1.Rows[e.RowIndex].Cells[5].Value != null)
                            nombre_documento = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

                        lbl_InfoRadicado1.Text = tipo + " No." + numero_documento;
                        lbl_InfoRadicado2.Text = "Título:";
                        lbl_InfoRadicado3.Text = asunto;
                        lbl_InfoRadicado4.Text = "Nombre-CC-Nit:";
                        lbl_InfoRadicado5.Text = nombre_documento;
                        lbl_num_doc.Text = "EXPEDIENTE " + numero_documento;

                        if (numero_documento.Length > 14)
                            codbarras.Image = generarCodigoBarras(numero_documento);
                    }
                }
            }
        }
        private void buscarTodosRadicadosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void limpiar_informacion_radicado()
        {
            lbl_InfoRadicado1.Text = "";
            lbl_InfoRadicado2.Text = "";
            lbl_InfoRadicado3.Text = "";
            lbl_InfoRadicado4.Text = "";
            lbl_InfoRadicado5.Text = "";
            lbl_num_doc.Text = "";
            codbarras.Image = null;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (pageRange[1] < TiffCarga.Count)
            {
                pageRange[0]++;
                pageRange[1]++;
                cargarImagen1(pageRange);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (pageRange[0] != 0)
            {
                pageRange[0]--;
                pageRange[1]--;
                cargarImagen1(pageRange);
            }
        }
        public static Bitmap RotateImage(System.Drawing.Image inputImage, float angleDegrees, bool upsizeOk,
                                         bool clipOk, System.Drawing.Color backgroundColor)
        {
            if (angleDegrees == 0f)
                return (Bitmap)inputImage.Clone();

            int oldWidth = inputImage.Width;
            int oldHeight = inputImage.Height;
            int newWidth = oldWidth;
            int newHeight = oldHeight;
            float scaleFactor = 1f;

            if (upsizeOk || !clipOk)
            {
                double angleRadians = angleDegrees * Math.PI / 180d;

                double cos = Math.Abs(Math.Cos(angleRadians));
                double sin = Math.Abs(Math.Sin(angleRadians));
                newWidth = (int)Math.Round(oldWidth * cos + oldHeight * sin);
                newHeight = (int)Math.Round(oldWidth * sin + oldHeight * cos);
            }

            if (!upsizeOk && !clipOk)
            {
                scaleFactor = Math.Min((float)oldWidth / newWidth, (float)oldHeight / newHeight);
                newWidth = oldWidth;
                newHeight = oldHeight;
            }

            Bitmap newBitmap = new Bitmap(newWidth, newHeight, backgroundColor == System.Drawing.Color.Transparent ?
                                             PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);
            newBitmap.SetResolution(inputImage.HorizontalResolution, inputImage.VerticalResolution);


            using (Graphics graphicsObject = Graphics.FromImage(newBitmap))
            {
                graphicsObject.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsObject.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphicsObject.SmoothingMode = SmoothingMode.HighQuality;

                if (backgroundColor != System.Drawing.Color.Transparent)
                    graphicsObject.Clear(backgroundColor);

                graphicsObject.TranslateTransform(newWidth / 2f, newHeight / 2f);

                if (scaleFactor != 1f)
                    graphicsObject.ScaleTransform(scaleFactor, scaleFactor);

                graphicsObject.RotateTransform(angleDegrees);
                graphicsObject.TranslateTransform(-oldWidth / 2f, -oldHeight / 2f);

                graphicsObject.DrawImage(inputImage, 0, 0);
            }

            return newBitmap;
        }

        private void btnAbrirImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Imagen (*.tif, *.tiff) | *.tif; *.tiff";
            if (Directory.Exists(@"D:\imgidu\"))
            {
                dialog.InitialDirectory = @"D:\imgidu\";
            }
            else
            {
                if (Directory.Exists(@"D:\"))
                {
                    dialog.InitialDirectory = @"D:\";
                }
                else
                {
                    if (Directory.Exists(@"C:\"))
                    {
                        dialog.InitialDirectory = @"C:\";
                    }
                }
            }

            dialog.Title = "Abrir Imagen";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                var img = Bitmap.FromFile(fileName);
                var pages = img.GetFrameCount(FrameDimension.Page);
                List<System.Drawing.Image> cargar = new List<System.Drawing.Image>();
                cargar = Split(fileName);
                if (cargar.Count > 0)
                    cargarImagen(cargar, pageRange, true);
            }
        }

        private void btnEnviarPDF1_Click(object sender, EventArgs e)
        {

            //Recorrer pagina por pagina e identificar el tamaño ideal
            
            Document doc = new Document(PageSize.LETTER);
            doc.SetMargins(0, 0, 0, 0);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream("D:\\hello_A1-b_cs.pdf", FileMode.Create));
            writer.PDFXConformance = PdfWriter.PDFA1B;
            doc.Open();

            PdfDictionary outi = new PdfDictionary(PdfName.OUTPUTINTENT);
            outi.Put(PdfName.OUTPUTCONDITIONIDENTIFIER, new PdfString("sRGB IEC61966-2.1"));
            outi.Put(PdfName.INFO, new PdfString("sRGB IEC61966-2.1"));
            outi.Put(PdfName.S, PdfName.GTS_PDFA1);

            //Perfiles icc
            ICC_Profile icc = ICC_Profile.GetInstance("D:\\sRGB_v4.icc");
            PdfICCBased ib = new PdfICCBased(icc);
            ib.Remove(PdfName.ALTERNATE);
            outi.Put(PdfName.DESTOUTPUTPROFILE, writer.AddToBody(ib).IndirectReference);

            writer.ExtraCatalog.Put(PdfName.OUTPUTINTENTS, new PdfArray(outi));

            BaseFont bf = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.WINANSI, true);
            iTextSharp.text.Font f = new iTextSharp.text.Font(bf, 12);

            //Captar la ruta de imagenes del radicado, si existe: usar sus imagenes
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(@"D:\escaner.tif");
            int total = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);

            //Crear las paginas
            for (int i = 0; i < total; ++i)
            {
                doc.NewPage();
                float subtrahend = doc.PageSize.Height - 10;
                bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, i);
                iTextSharp.text.Image pool = iTextSharp.text.Image.GetInstance(bm, ImageFormat.Tiff);
                pool.Alignment = 3;
                pool.ScaleToFit(doc.PageSize.Width - (doc.RightMargin * 2), subtrahend);
                doc.Add(pool);
            }

            writer.CreateXmpMetadata();
            doc.Close();

            //Enviar archivo si existe
            try
            {
                show_loading_panel(361, 174, 414, 36);
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(digitalizador_user, digitalizador);
                    client.UploadFile("ftp://fs04cc01/bodega_dev_of01/hello_A1_b_cs.pdf", WebRequestMethods.Ftp.UploadFile, @"D:\hello_A1-b_cs.pdf");
                    MessageBox.Show("El archivo se subió correctamente");
                }
                var request = (FtpWebRequest)WebRequest.Create("ftp://fs04cc01/bodega_dev_of01/hello_A1_b_cs.pdf");
                request.Credentials = new NetworkCredential(digitalizador_user, digitalizador);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                try
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    MessageBox.Show("El archivo se subió correctamente");
                }
                catch (WebException ex)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode ==
                        FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        MessageBox.Show("El archivo no se subió correctamente, por favor vuelva a intentar");
                    }
                }
                hide_loading_panel();
            }
            catch (Exception ex)
            {
                hide_loading_panel();
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnBorrarSeleccion_Click(object sender, EventArgs e)
        {
            if (PageEdit.Image != null && Rect.X > 0 && Rect.Y > 0)
            {
                using (Bitmap bitmap = new Bitmap(Width, Height))
                using (Graphics graphics = Graphics.FromImage(TiffCarga[0]))
                {
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, Width, Height);
                    graphics.FillRectangle(new SolidBrush(System.Drawing.Color.White), Rect);
                    Invalidate();
                    PageEdit.Refresh();
                }
            }
        }

        private void PageScreen1_Click(object sender, EventArgs e)
        {
            PageEdit.Image = PageScreen1.Image;
        }

        private void PageScreen2_Click(object sender, EventArgs e)
        {
            PageEdit.Image = PageScreen2.Image;
        }

        private void PageScreen3_Click(object sender, EventArgs e)
        {
            PageEdit.Image = PageScreen3.Image;
        }

        private void ScanOrfeo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                btnPrevScreen_Click(null, null);
            if (e.KeyCode == Keys.Right)
                btnNextScreen_Click(null, null);


        }

        private void btnPrevScreen_Click(object sender, EventArgs e)
        {
            if (pageRange[0] != 0)
            {
                pageRange[0]--;
                pageRange[1]--;
                txtPage.Text = (pageRange[0] + 1).ToString();
                cargarImagen1(pageRange);
            }
        }

        private void btnNextScreen_Click(object sender, EventArgs e)
        {
            if (pageRange[1] < TiffCarga.Count)
            {
                pageRange[0]++;
                pageRange[1]++;
                txtPage.Text = (pageRange[0] + 1).ToString();
                cargarImagen1(pageRange);
            }
        }

        private void txtPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtPage_KeyUp(object sender, KeyEventArgs e)
        {
            int page;
            if (int.TryParse(txtPage.Text.Replace(" ", ""), out page))
            {
                if (TiffCarga.Count >= page && TiffCarga.Count > 0)
                {
                    pageRange[0] = page - 1;
                    pageRange[1] += 2;
                    cargarImagen1(pageRange);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //text1
            //int numeroCopias = (int)numCopias.Value;
            int numeroCopias = 1;
            if (!imprimirSoloStickerToolStripMenuItem.Checked)
            {
                //RADI_NUME_RADI
                //RADI_DEPE_ACTU
                //RA_ASUN
                //Recorrer todas las filas del gridview
                if (dataGridView1.Rows.Count > 0)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {

                        string tipo = "";
                        string numero_documento = "";
                        string tipo_rem = "";
                        string paginas = "";
                        string fecha = "";
                        string asunto = "";
                        string dependencia = "";
                        string path = "";
                        string nombre_documento = "";
                        int OEM = 0;
                        int ESP = 0;
                        int CIU = 0;
                        string FUN = "";
                        string remitente = "";
                        string nombre_dep = "";
                        string anex_desc = "";


                        if (dataGridView1.SelectedRows[0].Cells[0].Value != null)
                            tipo = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                        if (dataGridView1.Rows[0].Cells[2].Value != null)
                            numero_documento = dataGridView1.Rows[0].Cells[2].Value.ToString();

                        if (!string.IsNullOrEmpty(tipo) && !string.IsNullOrEmpty(numero_documento))
                        {
                            string IISQL = "Select a.SGD_CIU_CODIGO,a.SGD_ESP_CODI,a.SGD_OEM_CODIGO,a.SGD_DOC_FUN, r.RADI_DESC_ANEX from sgd_dir_drecciones a, radicado r where a.radi_nume_radi=r.radi_nume_radi and a.radi_nume_radi=" + numero_documento;
                            IISQL = IISQL + " ORDER BY SGD_DIR_TIPO  ";
                            OracleConnection con = new OracleConnection(funciones.conni);
                            try
                            {
                                con.Open();
                                OracleCommand command = new OracleCommand(IISQL, con);
                                OracleDataReader reader = command.ExecuteReader();
                                if (reader.Read())
                                {
                                    if (int.TryParse(reader[0].ToString(), out CIU))
                                        FUN = "";
                                    if (int.TryParse(reader[1].ToString(), out ESP))
                                        FUN = "";
                                    if (int.TryParse(reader[2].ToString(), out OEM))
                                        FUN = "";
                                    if (reader[3] != null)
                                    {
                                        FUN = reader[3].ToString();
                                    }
                                    if (reader[4] != null)
                                    {
                                        anex_desc = reader[4].ToString();
                                    }
                                    funciones.desconectar(con);
                                }
                                else
                                    funciones.desconectar(con);
                            }
                            catch (Exception ex)
                            {
                                funciones.desconectar(con);
                            }
                            if (tipo == "RADICADO")
                            {
                                paginas = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                                if (dataGridView1.SelectedRows[0].Cells[3].Value != null)
                                    fecha = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                                if (dataGridView1.SelectedRows[0].Cells[4].Value != null)
                                    asunto = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                                if (dataGridView1.SelectedRows[0].Cells[5].Value != null)
                                    dependencia = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                                if (dataGridView1.SelectedRows[0].Cells[6].Value != null)
                                    path = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();


                                if (OEM > 0)
                                {
                                    IISQL = "Select a.sgd_oem_oempresa REMITENTE from SGD_OEM_OEMPRESAS a where a.SGD_OEM_CODIGO=" + OEM.ToString();
                                    tipo_rem += "(OEM) ";
                                }
                                if (CIU > 0)
                                {
                                    IISQL = "Select a.sgd_ciu_nombre || a.sgd_ciu_apell1 || sgd_ciu_apell2 REMITENTE from SGD_CIU_CIUDADANO a where a.sgd_ciu_codigo=" + CIU.ToString();
                                    tipo_rem += "(CIU) ";
                                }
                                if (ESP > 0)
                                {
                                    IISQL = "Select a.SIGLA_DE_LA_EMPRESA || '-' || a.NOMBRE_DE_LA_EMPRESA REMITENTE from BODEGA_EMPRESAS a where a.IDENTIFICADOR_EMPRESA=" + ESP.ToString();
                                    tipo_rem += "(ESP) ";
                                }
                                if (FUN != string.Empty)
                                {
                                    IISQL = "Select substr(USUA_NOMB,0,20) || '-' || DEPE_CODI REMITENTE from USUARIO a where a.usua_doc='" + FUN + "'";
                                    tipo_rem += "(FUN) ";
                                }

                                con = new OracleConnection(funciones.conni);
                                try
                                {
                                    con.Open();
                                    OracleCommand command = new OracleCommand(IISQL, con);
                                    OracleDataReader reader = command.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        if (reader[0] != null)
                                            remitente = reader[0].ToString();
                                        funciones.desconectar(con);
                                    }
                                    else
                                        funciones.desconectar(con);
                                }
                                catch (Exception ex)
                                {
                                    funciones.desconectar(con);
                                }


                                IISQL = "SELECT DEPE_NOMB FROM dependencia where DEPE_CODI=" + dependencia;
                                con = new OracleConnection(funciones.conni);
                                try
                                {
                                    con.Open();
                                    OracleCommand command = new OracleCommand(IISQL, con);
                                    OracleDataReader reader = command.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        if (reader[0] != null)
                                            nombre_dep = reader[0].ToString();
                                        funciones.desconectar(con);
                                    }
                                    else
                                        funciones.desconectar(con);
                                }
                                catch (Exception ex)
                                {
                                    funciones.desconectar(con);
                                }

                                string numRad = tipo + " No." + numero_documento.Substring(0, 4) + "-" + numero_documento.Substring(4, 3) + "-" + numero_documento.Substring(7, 6) + "-" + numero_documento.Substring(13, 1);
                                string fechaRad = string.Format(fecha, "dd/mm/yyyy hh:mm AMPM");
                                string depeRad = dependencia;
                                tipo_rem += remitente;
                                string año = numero_documento.Substring(0, 4);
                                string dependencia_str = numero_documento.Substring(4, 3);
                                string consecutivo = numero_documento.Substring(7, 6);
                                string tipo_rad = numero_documento.Substring(numero_documento.Length - 1);
                                System.Drawing.Image CodigoBarras = generarCodigoBarras(numero_documento);

                                tipo_rem = tipo_rem.Replace("DIRECCION", "DIR");
                                tipo_rem = tipo_rem.Replace("DEPENDENCIA", "DEP");
                                tipo_rem = tipo_rem.Replace("OFICINA", "OFIC");
                                tipo_rem = tipo_rem.Replace("SUPERINTENDENCIA", "SUP");

                                nombre_dep = nombre_dep.Replace("DIRECCION", "DIR");
                                nombre_dep = nombre_dep.Replace("DEPENDENCIA", "DEP");
                                nombre_dep = nombre_dep.Replace("OFICINA", "OFIC");
                                nombre_dep = nombre_dep.Replace("SUPERINTENDENCIA", "SUP");

                                if (anex_desc.Length>=58)
                                    anex_desc = anex_desc.Substring(0, 58);
                                else
                                    anex_desc = anex_desc.Substring(0, anex_desc.Length);

                                anex_desc = anex_desc.Replace(System.Environment.NewLine, " ");
                            }
                            else if (tipo == "EXPEDIENTE")
                            {
                                if (dataGridView1.SelectedRows[0].Cells[4].Value != null)
                                    asunto = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                                if (dataGridView1.SelectedRows[0].Cells[5].Value != null)
                                    nombre_documento = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();

                                //lbl_InfoRadicado1.Text = tipo + " No." + numero_documento;
                                //lbl_InfoRadicado2.Text = "Título:";
                                //lbl_InfoRadicado3.Text = asunto;
                                //lbl_InfoRadicado4.Text = "Nombre-CC-Nit:";
                                //lbl_InfoRadicado5.Text = nombre_documento;
                                //lbl_num_doc.Text = "EXPEDIENTE " + numero_documento;

                                //if (numero_documento.Length > 14)
                                //    codbarras.Image = generarCodigoBarras(numero_documento);
                            }

                            //IMPRIMIR
                            for (int i = 0; i < numeroCopias; i++)
                            {
                                PrintDocument pd = new PrintDocument();
                                System.Drawing.Printing.PaperSize tamañoPapel = new PaperSize("Custom Paper Size", 1200, 1200);
                                pd.PrinterSettings.DefaultPageSettings.PaperSize = tamañoPapel;
                                pd.DefaultPageSettings.PaperSize = tamañoPapel;
                                pd.PrintPage += PrintPage;
                                //here to select the printer attached to user PC
                                PrintDialog printDialog1 = new PrintDialog();
                                printDialog1.Document = pd;
                                DialogResult result = printDialog1.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    pd.Print();//this will trigger the Print Event handeler PrintPage
                                }
                            }
                            PrintDocument p = new PrintDocument();

                            //imprimir logo
                            //Imprime el codigo de barras
                            //                Printer.Print numRadicado &" de " & fechRadicado
                            //Printer.Font.Size = 7
                            //Printer.Print "Remite: " & RemitenteT
                            //Printer.Print "Dep.: " & DEPERADI
                            //Printer.Print "Anexos: " & Replace(Left(RADIANEX, 58), vbCrLf, " ")
                            //Printer.Print "Novedad: " & NOVEDAD



                            //                p.
                            //                Printer.PrintQuality = 3000
                            //Printer.Font.Name = "Arial"
                            //Printer.Font.Size = 4
                            //Printer.Font.Bold = True
                            //Printer.Print vbCrLf &vbCrLf


                        }
                    }
                }
            }
            else
            {

            }
        }
        //The Print Event handeler
        private void PrintPage(object o, PrintPageEventArgs e)
        {
            try
            {
                if (File.Exists(@"D:\logoEntidad.bmp"))
                {
                    //Load the image from the file
                    System.Drawing.Image img = System.Drawing.Image.FromFile(@"D:\logoEntidad.bmp");

                    //Adjust the size of the image to the page to print the full image without loosing any part of it
                    //System.Drawing.Rectangle m = e.MarginBounds;

                    //if ((double)img.Width / (double)img.Height > (double)m.Width / (double)m.Height) // image is wider
                    //{
                    //    m.Height = (int)((double)img.Height / (double)img.Width * (double)m.Width);
                    //}
                    //else
                    //{
                    //    m.Width = (int)((double)img.Width / (double)img.Height * (double)m.Height);
                    //}
                    e.Graphics.DrawImage(img, new System.Drawing.Rectangle(0, 0, 1000, 400));

                }
            }
            catch (Exception)
            {

            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }
    }

    public class InputBoxResult
    {
        public DialogResult ReturnCode;
        public string Text;
    }
    public class InputBox
    {

        #region Private Windows Contols and Constructor

        // Create a new instance of the form.
        private static Form frmInputDialog;
        private static Label lblPrompt;
        private static Button btnOK;
        private static Button btnCancel;
        private static TextBox txtInput;

        public InputBox()
        {
        }

        #endregion

        #region Private Variables

        private static string _formCaption = string.Empty;
        private static string _formPrompt = string.Empty;
        private static InputBoxResult _outputResponse = new InputBoxResult();
        private static string _defaultValue = string.Empty;
        private static int _xPos = -1;
        private static int _yPos = -1;

        #endregion

        #region Windows Form code

        private static void InitializeComponent()
        {
            // Create a new instance of the form.
            frmInputDialog = new Form();
            lblPrompt = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            txtInput = new TextBox();
            frmInputDialog.SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            lblPrompt.BackColor = SystemColors.Control;
            lblPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(0)));
            lblPrompt.Location = new Point(12, 9);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(302, 82);
            lblPrompt.TabIndex = 3;
            // 
            // btnOK
            // 
            btnOK.DialogResult = DialogResult.OK;
            btnOK.FlatStyle = FlatStyle.Popup;
            btnOK.Location = new Point(326, 8);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(64, 24);
            btnOK.TabIndex = 1;
            btnOK.Text = "&OK";
            btnOK.Click += new EventHandler(btnOK_Click);
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Location = new Point(326, 40);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(64, 24);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // txtInput
            // 
            txtInput.Location = new Point(8, 100);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(379, 20);
            txtInput.TabIndex = 0;
            txtInput.Text = "";
            // 
            // InputBoxDialog
            // 
            frmInputDialog.AutoScaleBaseSize = new Size(5, 13);
            frmInputDialog.ClientSize = new Size(398, 128);
            frmInputDialog.Controls.Add(txtInput);
            frmInputDialog.Controls.Add(btnCancel);
            frmInputDialog.Controls.Add(btnOK);
            frmInputDialog.Controls.Add(lblPrompt);
            frmInputDialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            frmInputDialog.MaximizeBox = false;
            frmInputDialog.MinimizeBox = false;
            frmInputDialog.Name = "InputBoxDialog";
            frmInputDialog.ResumeLayout(false);
        }

        #endregion

        #region Private function, InputBox Form move and change size

        static private void LoadForm()
        {
            OutputResponse.ReturnCode = DialogResult.Ignore;
            OutputResponse.Text = string.Empty;

            txtInput.Text = _defaultValue;
            lblPrompt.Text = _formPrompt;
            frmInputDialog.Text = _formCaption;

            // Retrieve the working rectangle from the Screen class
            // using the PrimaryScreen and the WorkingArea properties.
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;

            if ((_xPos >= 0 && _xPos < workingRectangle.Width - 100) && (_yPos >= 0 && _yPos < workingRectangle.Height - 100))
            {
                frmInputDialog.StartPosition = FormStartPosition.Manual;
                frmInputDialog.Location = new System.Drawing.Point(_xPos, _yPos);
            }
            else
                frmInputDialog.StartPosition = FormStartPosition.CenterScreen;


            string PrompText = lblPrompt.Text;

            int n = 0;
            int Index = 0;
            while (PrompText.IndexOf("\n", Index) > -1)
            {
                Index = PrompText.IndexOf("\n", Index) + 1;
                n++;
            }

            if (n == 0)
                n = 1;

            System.Drawing.Point Txt = txtInput.Location;
            Txt.Y = Txt.Y + (n * 4);
            txtInput.Location = Txt;
            System.Drawing.Size form = frmInputDialog.Size;
            form.Height = form.Height + (n * 4);
            frmInputDialog.Size = form;

            txtInput.SelectionStart = 0;
            txtInput.SelectionLength = txtInput.Text.Length;
            txtInput.Focus();
        }

        #endregion

        #region Button control click event

        static private void btnOK_Click(object sender, System.EventArgs e)
        {
            OutputResponse.ReturnCode = DialogResult.OK;
            OutputResponse.Text = txtInput.Text;
            frmInputDialog.Dispose();
        }

        static private void btnCancel_Click(object sender, System.EventArgs e)
        {
            OutputResponse.ReturnCode = DialogResult.Cancel;
            OutputResponse.Text = string.Empty; //Clean output response
            frmInputDialog.Dispose();
        }

        #endregion

        #region Public Static Show functions

        static public InputBoxResult Show(string Prompt)
        {
            InitializeComponent();
            FormPrompt = Prompt;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        static public InputBoxResult Show(string Prompt, string Title)
        {
            InitializeComponent();

            FormCaption = Title;
            FormPrompt = Prompt;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        static public InputBoxResult Show(string Prompt, string Title, string Default)
        {
            InitializeComponent();

            FormCaption = Title;
            FormPrompt = Prompt;
            DefaultValue = Default;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        static public InputBoxResult Show(string Prompt, string Title, string Default, int XPos, int YPos)
        {
            InitializeComponent();
            FormCaption = Title;
            FormPrompt = Prompt;
            DefaultValue = Default;
            XPosition = XPos;
            YPosition = YPos;

            // Display the form as a modal dialog box.
            LoadForm();
            frmInputDialog.ShowDialog();
            return OutputResponse;
        }

        #endregion

        #region Private Properties

        static private string FormCaption
        {
            set
            {
                _formCaption = value;
            }
        } // property FormCaption

        static private string FormPrompt
        {
            set
            {
                _formPrompt = value;
            }
        } // property FormPrompt

        static private InputBoxResult OutputResponse
        {
            get
            {
                return _outputResponse;
            }
            set
            {
                _outputResponse = value;
            }
        } // property InputResponse

        static private string DefaultValue
        {
            set
            {
                _defaultValue = value;
            }
        } // property DefaultValue

        static private int XPosition
        {
            set
            {
                if (value >= 0)
                    _xPos = value;
            }
        } // property XPos

        static private int YPosition
        {
            set
            {
                if (value >= 0)
                    _yPos = value;
            }
        } // property YPos

        #endregion
    }
}
