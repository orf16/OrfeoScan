﻿namespace OrfeoScan_IDU_STRT
{
    partial class Contenedor
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssl_label_dependencia = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_valor_dependencia = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_label_usuario = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_valor_usuario = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.consultarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_label_dependencia,
            this.tssl_valor_dependencia,
            this.tssl_label_usuario,
            this.tssl_valor_usuario});
            this.statusStrip1.Location = new System.Drawing.Point(0, 659);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1410, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssl_label_dependencia
            // 
            this.tssl_label_dependencia.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssl_label_dependencia.Name = "tssl_label_dependencia";
            this.tssl_label_dependencia.Size = new System.Drawing.Size(80, 17);
            this.tssl_label_dependencia.Text = "Dependencia";
            // 
            // tssl_valor_dependencia
            // 
            this.tssl_valor_dependencia.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssl_valor_dependencia.Name = "tssl_valor_dependencia";
            this.tssl_valor_dependencia.Size = new System.Drawing.Size(0, 17);
            // 
            // tssl_label_usuario
            // 
            this.tssl_label_usuario.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssl_label_usuario.Name = "tssl_label_usuario";
            this.tssl_label_usuario.Size = new System.Drawing.Size(50, 17);
            this.tssl_label_usuario.Text = "Usuario";
            // 
            // tssl_valor_usuario
            // 
            this.tssl_valor_usuario.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tssl_valor_usuario.Name = "tssl_valor_usuario";
            this.tssl_valor_usuario.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consultarToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1410, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // consultarToolStripMenuItem
            // 
            this.consultarToolStripMenuItem.Name = "consultarToolStripMenuItem";
            this.consultarToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.consultarToolStripMenuItem.Text = "consultar";
            this.consultarToolStripMenuItem.Click += new System.EventHandler(this.consultarToolStripMenuItem_Click);
            // 
            // Contenedor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1410, 681);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1300, 720);
            this.Name = "Contenedor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCANORFEO - IDU";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Contenedor_FormClosed);
            this.Load += new System.EventHandler(this.Contenedor_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssl_label_dependencia;
        private System.Windows.Forms.ToolStripStatusLabel tssl_valor_dependencia;
        private System.Windows.Forms.ToolStripStatusLabel tssl_label_usuario;
        private System.Windows.Forms.ToolStripStatusLabel tssl_valor_usuario;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem consultarToolStripMenuItem;
    }
}