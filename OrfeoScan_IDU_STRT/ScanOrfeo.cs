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


namespace OrfeoScan_IDU_STRT
{
    /// <summary>
	/// Class used to store the result of an InputBox.Show message.
	/// </summary>
	
    public partial class ScanOrfeo : Form
    {
        
        funciones.funciones funciones = new funciones.funciones();
        private string varConcat = "";
        private string varSubstr = "";
        private string varRadi_Fech_radi = "";
        private string varFechaSistema = "";
        private string varIsNull = "";
        private USUARIO usuarioScanOrfeo;
        public ScanOrfeo(USUARIO usuario)
        {
            usuarioScanOrfeo = usuario;
            InitializeComponent();
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
            labelDepen_load();
            varConcat = "||";
            varSubstr = "substr";
            varRadi_Fech_radi = "TO_CHAR(a.RADI_FECH_RADI,'YYYYMM')";
            varFechaSistema = "sysdate";
            varIsNull = "nvl";
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
                            cBoxtRadicado.Items.Add((string)reader[1]);
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
        private void labelDepen_load()
        {
            Login login = new Login();
            labelDepen.Text = string.Empty;
            labelDepen.Text = usuarioScanOrfeo.DEPE_CODI.ToString();
        }
        private void buscarRadicadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Inputbox de busqueda con boton buscar y cancelar
            
            InputBoxResult test = InputBox.Show("Digite el registro de radicado a consultar" 
                  , "Consultar Radicado", string.Empty, 100, 0);

            if (test.ReturnCode == DialogResult.OK)
            {
                if (test.Text.Length > 4)
                {
                    if (!string.IsNullOrEmpty(labelDepen.Text.Trim()))
                    {
                        BuscarRadicado(test.Text);
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar una dependencia a Buscar");
                    }
                }
                else
                {
                    MessageBox.Show("El número de radicado debe tener más de 4 caracteres");
                }
            }



        }
        private void buscarEnTodosLosRadicadoOExpedientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void BuscarRadicado(string numradicado)
        {
            //verificar se se realiza busqueda de expedientes
            //para busqueda de expedientes

            //para busqueda de radicados
            string IISQL;
            if (impresiónDeSobresToolStripMenuItem.Checked)
            {
                IISQL = "Select a.RADI_NUME_HOJA PAGINAS,a.RADI_NUME_RADI,a.RADI_FECH_RADI,a.RA_ASUN, a.RADI_NOMB || a.RADI_PRIM_APEL || a.RADI_SEGU_APEL AS RADI_NOMB,a.RADI_DEPE_ACTU,renv.SGD_RENV_NOMBRE,renv.SGD_RENV_DIR,renv.SGD_RENV_DEPTO,renv.SGD_RENV_MPIO,a.RADI_USUA_ACTU,b.depe_nomb, TO_CHAR(a.RADI_FECH_RADI,'YYYYMM') as anomes_rad,a.RADI_DEPE_RADI from Radicado a,dependencia b, sgd_renv_regenvio renv where a.RADI_DEPE_ACTU=b.DEPE_CODI AND a.RADI_NUME_RADI=renv.RADI_NUME_SAL AND a.RADI_CHAR_RADI LIKE '" + DateTime.Now.Year.ToString() + labelDepen.Text.Substring(0, 3) + "%'";
            }
            else
            {
                IISQL = "Select /*+ INDEX (a IND_RADICADO_RADI_CHAR_RADI)*/ a.RADI_NUME_HOJA PAGINAS, a.RADI_PATH PATH,a.RADI_NUME_RADI,a.RADI_FECH_RADI,a.RA_ASUN, a.RADI_DEPE_ACTU,a.RADI_PATH,a.RADI_USUA_ACTU,a.RADI_DEPE_RADI from Radicado a where a.radi_nume_radi is not null  ";
            }

            IISQL = IISQL + " and a.radi_char_radi like '%" + numradicado.Trim() + "%' ";
            IISQL = IISQL + " and a.radi_path is null ";
            System.Windows.Forms.Clipboard.SetText(IISQL);
            //Consulta para ordenar registros???



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
            lblPrompt.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(0)));
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
