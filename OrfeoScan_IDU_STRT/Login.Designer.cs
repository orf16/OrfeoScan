namespace OrfeoScan_IDU_STRT
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tSSL1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSSL2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSSL3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtContraseña = new System.Windows.Forms.TextBox();
            this.btn_ingresar = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSSL1,
            this.tSSL2,
            this.tSSL3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 383);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(793, 22);
            this.statusStrip1.TabIndex = 49;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tSSL1
            // 
            this.tSSL1.BackColor = System.Drawing.Color.Transparent;
            this.tSSL1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tSSL1.Name = "tSSL1";
            this.tSSL1.Size = new System.Drawing.Size(98, 17);
            this.tSSL1.Text = "Conexión Orfeo:";
            // 
            // tSSL2
            // 
            this.tSSL2.BackColor = System.Drawing.Color.Transparent;
            this.tSSL2.Name = "tSSL2";
            this.tSSL2.Size = new System.Drawing.Size(0, 17);
            // 
            // tSSL3
            // 
            this.tSSL3.BackColor = System.Drawing.Color.Transparent;
            this.tSSL3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tSSL3.Name = "tSSL3";
            this.tSSL3.Size = new System.Drawing.Size(71, 17);
            this.tSSL3.Text = "Version: 4.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(141, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 25);
            this.label5.TabIndex = 48;
            this.label5.Text = "Contraseña";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(190, 220);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 25);
            this.label4.TabIndex = 47;
            this.label4.Text = "Usuario";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::OrfeoScan_IDU_STRT.Properties.Resources.ORFEO_LOGO;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(770, 180);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 45;
            this.pictureBox1.TabStop = false;
            // 
            // txtUsuario
            // 
            this.txtUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtUsuario.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.ForeColor = System.Drawing.Color.Black;
            this.txtUsuario.Location = new System.Drawing.Point(277, 220);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(300, 26);
            this.txtUsuario.TabIndex = 42;
            this.txtUsuario.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUsuario_KeyUp);
            // 
            // txtContraseña
            // 
            this.txtContraseña.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtContraseña.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContraseña.ForeColor = System.Drawing.Color.Black;
            this.txtContraseña.Location = new System.Drawing.Point(277, 272);
            this.txtContraseña.Name = "txtContraseña";
            this.txtContraseña.PasswordChar = '•';
            this.txtContraseña.Size = new System.Drawing.Size(300, 26);
            this.txtContraseña.TabIndex = 43;
            this.txtContraseña.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtContraseña_KeyUp);
            // 
            // btn_ingresar
            // 
            this.btn_ingresar.BackColor = System.Drawing.Color.Transparent;
            this.btn_ingresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_ingresar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_ingresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ingresar.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ingresar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btn_ingresar.Location = new System.Drawing.Point(359, 323);
            this.btn_ingresar.Name = "btn_ingresar";
            this.btn_ingresar.Size = new System.Drawing.Size(100, 31);
            this.btn_ingresar.TabIndex = 44;
            this.btn_ingresar.Text = "Ingresar";
            this.btn_ingresar.UseVisualStyleBackColor = false;
            this.btn_ingresar.Click += new System.EventHandler(this.btn_ingresar_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(793, 405);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.txtContraseña);
            this.Controls.Add(this.btn_ingresar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Text = "SCANORFEO - IDU";
            this.Load += new System.EventHandler(this.Login_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Login_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tSSL1;
        private System.Windows.Forms.ToolStripStatusLabel tSSL2;
        private System.Windows.Forms.ToolStripStatusLabel tSSL3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtContraseña;
        private System.Windows.Forms.Button btn_ingresar;
    }
}