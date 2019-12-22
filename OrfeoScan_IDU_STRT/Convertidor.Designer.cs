namespace OrfeoScan_IDU_STRT
{
    partial class Convertidor
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
            this.rtb_nombrearchivo = new System.Windows.Forms.RichTextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.button16 = new System.Windows.Forms.Button();
            this.btn_convertir = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lbl_pdfconvertir = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rtb_nombrearchivo
            // 
            this.rtb_nombrearchivo.Enabled = false;
            this.rtb_nombrearchivo.Location = new System.Drawing.Point(129, 9);
            this.rtb_nombrearchivo.Name = "rtb_nombrearchivo";
            this.rtb_nombrearchivo.Size = new System.Drawing.Size(796, 29);
            this.rtb_nombrearchivo.TabIndex = 103;
            this.rtb_nombrearchivo.Text = "";
            // 
            // label38
            // 
            this.label38.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.White;
            this.label38.Location = new System.Drawing.Point(27, 9);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(98, 41);
            this.label38.TabIndex = 102;
            this.label38.Text = "Seleccionar Archivo";
            this.label38.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.White;
            this.button16.BackgroundImage = global::OrfeoScan_IDU_STRT.Properties.Resources.icons8_folder_48;
            this.button16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button16.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button16.FlatAppearance.BorderSize = 0;
            this.button16.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Location = new System.Drawing.Point(933, 9);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(28, 29);
            this.button16.TabIndex = 101;
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // btn_convertir
            // 
            this.btn_convertir.BackColor = System.Drawing.Color.White;
            this.btn_convertir.BackgroundImage = global::OrfeoScan_IDU_STRT.Properties.Resources.icons8_pdf_64;
            this.btn_convertir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_convertir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_convertir.FlatAppearance.BorderSize = 0;
            this.btn_convertir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_convertir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_convertir.Location = new System.Drawing.Point(979, 9);
            this.btn_convertir.Name = "btn_convertir";
            this.btn_convertir.Size = new System.Drawing.Size(28, 29);
            this.btn_convertir.TabIndex = 104;
            this.btn_convertir.UseVisualStyleBackColor = false;
            this.btn_convertir.Click += new System.EventHandler(this.btn_convertir_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(129, 44);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(796, 23);
            this.progressBar1.TabIndex = 107;
            // 
            // lbl_pdfconvertir
            // 
            this.lbl_pdfconvertir.AutoSize = true;
            this.lbl_pdfconvertir.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_pdfconvertir.ForeColor = System.Drawing.Color.White;
            this.lbl_pdfconvertir.Location = new System.Drawing.Point(457, 73);
            this.lbl_pdfconvertir.Name = "lbl_pdfconvertir";
            this.lbl_pdfconvertir.Size = new System.Drawing.Size(0, 16);
            this.lbl_pdfconvertir.TabIndex = 106;
            // 
            // Convertidor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1020, 93);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbl_pdfconvertir);
            this.Controls.Add(this.btn_convertir);
            this.Controls.Add(this.rtb_nombrearchivo);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.button16);
            this.Name = "Convertidor";
            this.Text = "Convertidor TIFF/PDF";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Convertidor_FormClosed);
            this.Load += new System.EventHandler(this.Convertidor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox rtb_nombrearchivo;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button btn_convertir;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lbl_pdfconvertir;
    }
}