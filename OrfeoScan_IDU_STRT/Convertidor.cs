using iTextSharp.text;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using System.IO;

namespace OrfeoScan_IDU_STRT
{
    public partial class Convertidor : Form
    {
        private string title = "Mensaje de OrfeoScan";
        private string convertidor_path = @"D:\Users\clgarcia8\Pictures\";
        private string path_load = @"C:\";
        private string path_save = @"C:\";
        private Document doc_pdf;
        private iTextSharp.text.Image page_prop_pdf;
        public Convertidor()
        {
            InitializeComponent();
        }
        private void Convertidor_Load(object sender, EventArgs e)
        {
            this.Icon = OrfeoScan_IDU_STRT.Properties.Resources.icon;
        }
        private void garbage_collector()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private void btn_convertir_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(convertidor_path))
            {
                saveFileDialog1.InitialDirectory = path_load;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Title = "Guardar PDF";
                saveFileDialog1.DefaultExt = "pdf";
                saveFileDialog1.Filter = "Archivos PDF (*.pdf) | *.pdf";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string nombreArchivo = saveFileDialog1.FileName;
                    path_load = Path.GetDirectoryName(saveFileDialog1.FileName);
                    crearPdf_2(nombreArchivo);
                    if (System.IO.File.Exists(nombreArchivo))
                    {
                        if (new System.IO.FileInfo(nombreArchivo).Length > 0)
                        {
                            MessageBox.Show("PDF guardado", title);
                            progressBar1.Value = 0;
                            lbl_pdfconvertir.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Error en la transformación del PDF", title);
                            progressBar1.Value = 0;
                            lbl_pdfconvertir.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error en la transformación del PDF", title);
                        progressBar1.Value = 0;
                        lbl_pdfconvertir.Text = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay imagenes para convertir y guardar como PDF/A. Seleccione un ", title);
                progressBar1.Value = 0;
                lbl_pdfconvertir.Text = "";
            }
        }
        private void crearPdf_2(string rutaFinal)
        {
            try
            {
                lbl_pdfconvertir.Text = "Convirtiendo a PDF/A...";
                lbl_pdfconvertir.Refresh();
                string ruta_archivo = convertidor_path;
                System.Drawing.Image actualBitmap_ = System.Drawing.Image.FromFile(ruta_archivo);
                Guid objGuid = actualBitmap_.FrameDimensionsList[0];
                System.Drawing.Imaging.FrameDimension objDimension = new System.Drawing.Imaging.FrameDimension(objGuid);
                int total_page = actualBitmap_.GetFrameCount(objDimension);
                actualBitmap_.SelectActiveFrame(objDimension, 0);



                garbage_collector();
                var width0 = actualBitmap_.Width;
                var height0 = actualBitmap_.Height;
                iTextSharp.text.Rectangle cero = new iTextSharp.text.Rectangle(width0, height0);

                doc_pdf = new Document(cero, 0, 0, 0, 0);

                doc_pdf.SetMargins(0, 0, 0, 0);
                PdfWriter writer = PdfWriter.GetInstance(doc_pdf, new FileStream(rutaFinal, FileMode.Create));
                writer.PDFXConformance = PdfWriter.PDFA1B;
                doc_pdf.Open();

                PdfDictionary outi = new PdfDictionary(PdfName.OUTPUTINTENT);
                outi.Put(PdfName.OUTPUTCONDITIONIDENTIFIER, new PdfString("sRGB IEC61966-2.1"));
                outi.Put(PdfName.INFO, new PdfString("sRGB IEC61966-2.1"));
                outi.Put(PdfName.S, PdfName.GTS_PDFA1);

                //Perfiles icc
                var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                path = path.Replace("file:\\", "");
                ICC_Profile icc = ICC_Profile.GetInstance(path + @"\sRGB_v4.icc");
                PdfICCBased ib = new PdfICCBased(icc);
                ib.Remove(PdfName.ALTERNATE);
                outi.Put(PdfName.DESTOUTPUTPROFILE, writer.AddToBody(ib).IndirectReference);

                writer.ExtraCatalog.Put(PdfName.OUTPUTINTENTS, new PdfArray(outi));
                BaseFont bf = BaseFont.CreateFont(path + @"\arial.ttf", BaseFont.WINANSI, true);
                iTextSharp.text.Font f = new iTextSharp.text.Font(bf, 12);

                float subtrahend0 = doc_pdf.PageSize.Height - 10;

                iTextSharp.text.Image pool0;
                pool0 = iTextSharp.text.Image.GetInstance(actualBitmap_, System.Drawing.Imaging.ImageFormat.Gif);

                pool0.Alignment = 3;
                pool0.ScaleToFit(doc_pdf.PageSize.Width - (doc_pdf.RightMargin * 2), subtrahend0);
                doc_pdf.Add(pool0);
                garbage_collector();
                //Crear las paginas
                for (int i = 1; i < total_page; ++i)
                {
                    actualBitmap_.SelectActiveFrame(objDimension, i);
                    float Width = actualBitmap_.Width;
                    float Height = actualBitmap_.Height;
                    Task task1 = Task.Factory.StartNew(() => pdf_paralelo_page(actualBitmap_));
                    Task task2 = Task.Factory.StartNew(() => pdf_paralelo_doc(Width, Height));
                    Task.WaitAll(task1, task2);
                    doc_pdf.Add(page_prop_pdf);
                    decimal porcentaje = ((decimal)i / (decimal)total_page);
                    progressBar1.Value = (int)(porcentaje*100);
                    progressBar1.Refresh();
                    garbage_collector();
                }
                progressBar1.Value = 100;
                writer.CreateXmpMetadata();
                doc_pdf.Close();
                actualBitmap_.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falla de sistema en la conversión a PDF/A", title);
                if (ex.ToString().Contains("utilizado en otro proceso"))
                {
                    MessageBox.Show("El PDF esta siendo utilizado en otro proceso", title);

                }
                MessageBox.Show(ex.ToString(), title);
                garbage_collector();
            }
            garbage_collector();
        }
        private void pdf_paralelo_page(System.Drawing.Image bmp1)
        {
            page_prop_pdf = iTextSharp.text.Image.GetInstance(bmp1, System.Drawing.Imaging.ImageFormat.Gif);
            page_prop_pdf.Alignment = 3;
            page_prop_pdf.ScaleToFit(bmp1.Width, bmp1.Height - 10);
        }
        private void pdf_paralelo_doc(float w, float h)
        {
            doc_pdf.SetPageSize(new iTextSharp.text.Rectangle(w, h));
            doc_pdf.NewPage();
        }
        private void button16_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Imagen (*.tif, *.tiff) | *.tif; *.tiff";
            dialog.InitialDirectory = path_save;
            dialog.Title = "Abrir Imagen";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path_save = Path.GetDirectoryName(dialog.FileName);
                rtb_nombrearchivo.Text = string.Empty;
                convertidor_path = string.Empty;
                rtb_nombrearchivo.Text = dialog.FileName;
                convertidor_path= dialog.FileName;
            }
        }
        private void Convertidor_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
