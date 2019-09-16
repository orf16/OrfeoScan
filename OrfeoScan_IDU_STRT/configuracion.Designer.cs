namespace OrfeoScan_IDU_STRT
{
    partial class configuracion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.Moccasin;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "digitalizador01",
            "digitalizador02",
            "digitalizador03",
            "digitalizador04",
            "digitalizador05",
            "digitalizador06",
            "digitalizador07",
            "digitalizador08",
            "digitalizador09",
            "digitalizador10",
            "digitalizador11",
            "digitalizador12",
            "digitalizador13",
            "digitalizador14",
            "digitalizador15",
            "digitalizador16",
            "digitalizador17",
            "digitalizador18",
            "digitalizador19",
            "digitalizador20",
            "digitalizador21",
            "digitalizador22",
            "digitalizador23",
            "digitalizador24",
            "digitalizador25",
            "digitalizador26",
            "digitalizador27",
            "digitalizador28",
            "digitalizador29",
            "digitalizador30",
            "digitalizador31",
            "digitalizador32",
            "digitalizador33",
            "digitalizador34",
            "digitalizador35",
            "digitalizador36",
            "digitalizador37",
            "digitalizador38",
            "digitalizador39",
            "digitalizador40",
            "digitalizador41",
            "digitalizador42",
            "digitalizador43",
            "digitalizador44",
            "digitalizador45",
            "digitalizador46",
            "digitalizador47",
            "digitalizador48",
            "digitalizador49",
            "digitalizador50",
            "digitalizador51",
            "digitalizador52",
            "digitalizador53"});
            this.comboBox1.Location = new System.Drawing.Point(15, 26);
            this.comboBox1.MaxDropDownItems = 10;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(784, 22);
            this.comboBox1.TabIndex = 42;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(323, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 29);
            this.button1.TabIndex = 43;
            this.button1.Text = "Guardar Configuración";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 14);
            this.label2.TabIndex = 45;
            this.label2.Text = "Usuario FTP";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(640, 376);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 32);
            this.button2.TabIndex = 46;
            this.button2.Text = "Crear Carpetas IDU";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // configuracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(811, 420);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "configuracion";
            this.Text = "Configuración";
            this.Load += new System.EventHandler(this.configuracion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
    }
}