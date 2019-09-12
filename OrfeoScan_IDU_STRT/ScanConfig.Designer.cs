namespace OrfeoScan_IDU_STRT
{
    partial class ScanConfig
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
            this.groupDPI = new System.Windows.Forms.GroupBox();
            this.comboDPI = new System.Windows.Forms.ComboBox();
            this.groupSize = new System.Windows.Forms.GroupBox();
            this.comboSize = new System.Windows.Forms.ComboBox();
            this.groupDuplex = new System.Windows.Forms.GroupBox();
            this.ckDuplex = new System.Windows.Forms.CheckBox();
            this.groupDepth = new System.Windows.Forms.GroupBox();
            this.comboDepth = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupDPI.SuspendLayout();
            this.groupSize.SuspendLayout();
            this.groupDuplex.SuspendLayout();
            this.groupDepth.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupDPI
            // 
            this.groupDPI.Controls.Add(this.comboDPI);
            this.groupDPI.Location = new System.Drawing.Point(69, 91);
            this.groupDPI.Name = "groupDPI";
            this.groupDPI.Size = new System.Drawing.Size(200, 61);
            this.groupDPI.TabIndex = 66;
            this.groupDPI.TabStop = false;
            this.groupDPI.Text = "DPI";
            // 
            // comboDPI
            // 
            this.comboDPI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboDPI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDPI.FormattingEnabled = true;
            this.comboDPI.Location = new System.Drawing.Point(11, 21);
            this.comboDPI.Name = "comboDPI";
            this.comboDPI.Size = new System.Drawing.Size(169, 21);
            this.comboDPI.TabIndex = 1;
            // 
            // groupSize
            // 
            this.groupSize.Controls.Add(this.comboSize);
            this.groupSize.Location = new System.Drawing.Point(297, 167);
            this.groupSize.Name = "groupSize";
            this.groupSize.Size = new System.Drawing.Size(200, 61);
            this.groupSize.TabIndex = 65;
            this.groupSize.TabStop = false;
            this.groupSize.Text = "Tamaño Imagen";
            // 
            // comboSize
            // 
            this.comboSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSize.FormattingEnabled = true;
            this.comboSize.Location = new System.Drawing.Point(19, 26);
            this.comboSize.Name = "comboSize";
            this.comboSize.Size = new System.Drawing.Size(169, 21);
            this.comboSize.TabIndex = 1;
            // 
            // groupDuplex
            // 
            this.groupDuplex.Controls.Add(this.ckDuplex);
            this.groupDuplex.Location = new System.Drawing.Point(297, 91);
            this.groupDuplex.Name = "groupDuplex";
            this.groupDuplex.Size = new System.Drawing.Size(200, 60);
            this.groupDuplex.TabIndex = 64;
            this.groupDuplex.TabStop = false;
            this.groupDuplex.Text = "Doble Cara";
            // 
            // ckDuplex
            // 
            this.ckDuplex.AutoSize = true;
            this.ckDuplex.Location = new System.Drawing.Point(62, 26);
            this.ckDuplex.Name = "ckDuplex";
            this.ckDuplex.Size = new System.Drawing.Size(79, 17);
            this.ckDuplex.TabIndex = 63;
            this.ckDuplex.Text = "Doble Cara";
            this.ckDuplex.UseVisualStyleBackColor = true;
            // 
            // groupDepth
            // 
            this.groupDepth.Controls.Add(this.comboDepth);
            this.groupDepth.Location = new System.Drawing.Point(69, 167);
            this.groupDepth.Name = "groupDepth";
            this.groupDepth.Size = new System.Drawing.Size(200, 61);
            this.groupDepth.TabIndex = 63;
            this.groupDepth.TabStop = false;
            this.groupDepth.Text = "Tipo Color";
            // 
            // comboDepth
            // 
            this.comboDepth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDepth.FormattingEnabled = true;
            this.comboDepth.Location = new System.Drawing.Point(16, 26);
            this.comboDepth.Name = "comboDepth";
            this.comboDepth.Size = new System.Drawing.Size(169, 21);
            this.comboDepth.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(108, 357);
            this.panel1.TabIndex = 69;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.groupDepth);
            this.panel2.Controls.Add(this.groupDuplex);
            this.panel2.Controls.Add(this.groupSize);
            this.panel2.Controls.Add(this.groupDPI);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(108, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(621, 357);
            this.panel2.TabIndex = 70;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(129, 252);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 30);
            this.button4.TabIndex = 69;
            this.button4.Text = "Guardar y Escanear";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(318, 252);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 30);
            this.button1.TabIndex = 70;
            this.button1.Text = "Guardar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // ScanConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 357);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ScanConfig";
            this.Text = "Perfil de Escaneo";
            this.groupDPI.ResumeLayout(false);
            this.groupSize.ResumeLayout(false);
            this.groupDuplex.ResumeLayout(false);
            this.groupDuplex.PerformLayout();
            this.groupDepth.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupDPI;
        private System.Windows.Forms.ComboBox comboDPI;
        private System.Windows.Forms.GroupBox groupSize;
        private System.Windows.Forms.ComboBox comboSize;
        private System.Windows.Forms.GroupBox groupDuplex;
        private System.Windows.Forms.CheckBox ckDuplex;
        private System.Windows.Forms.GroupBox groupDepth;
        private System.Windows.Forms.ComboBox comboDepth;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
    }
}