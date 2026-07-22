namespace FabrikaStokTakipUygulamasi
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelLeft    = new System.Windows.Forms.Panel();
            lblDesc      = new System.Windows.Forms.Label();
            lblTitle     = new System.Windows.Forms.Label();
            label1       = new System.Windows.Forms.Label();
            txtUsername  = new System.Windows.Forms.TextBox();
            label2       = new System.Windows.Forms.Label();
            // Şifre alanı için panel (textbox + göz butonu yan yana)
            panelSifre   = new System.Windows.Forms.Panel();
            txtPassword  = new System.Windows.Forms.TextBox();
            btnGozToggle = new System.Windows.Forms.Button();
            chkAcikTut   = new System.Windows.Forms.CheckBox();
            btnLogin     = new System.Windows.Forms.Button();
            btnDilToggle = new System.Windows.Forms.Button();

            panelLeft.SuspendLayout();
            panelSifre.SuspendLayout();
            SuspendLayout();

            // panelLeft
            panelLeft.BackColor = System.Drawing.Color.FromArgb(30, 42, 56);
            panelLeft.Controls.Add(lblDesc);
            panelLeft.Controls.Add(lblTitle);
            panelLeft.Dock     = System.Windows.Forms.DockStyle.Left;
            panelLeft.Name     = "panelLeft";
            panelLeft.Size     = new System.Drawing.Size(408, 763);
            panelLeft.TabIndex = 0;

            lblDesc.AutoSize  = true;
            lblDesc.Font      = new System.Drawing.Font("Segoe UI", 11.25F);
            lblDesc.ForeColor = System.Drawing.Color.White;
            lblDesc.Location  = new System.Drawing.Point(58, 369);
            lblDesc.Name      = "lblDesc";
            lblDesc.Text      = "Üretim ve depo yönetimi\r\niçin profesyonel stok sistemi";

            lblTitle.Font      = new System.Drawing.Font("Segoe UI", 27.75F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location  = new System.Drawing.Point(52, 185);
            lblTitle.Name      = "lblTitle";
            lblTitle.Size      = new System.Drawing.Size(303, 138);
            lblTitle.Text      = "FABRİKA STOK TAKİP UYGULAMASI";

            // Kullanıcı Adı
            label1.AutoSize  = true;
            label1.Font      = new System.Drawing.Font("Segoe UI", 11.25F);
            label1.Location  = new System.Drawing.Point(607, 231);
            label1.Name      = "label1";
            label1.Text      = "Kullanıcı Adı";

            txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtUsername.Font        = new System.Drawing.Font("Segoe UI", 11.25F);
            txtUsername.Location    = new System.Drawing.Point(607, 263);
            txtUsername.Name        = "txtUsername";
            txtUsername.Size        = new System.Drawing.Size(326, 27);
            txtUsername.TabIndex    = 2;

            // Şifre etiketi
            label2.AutoSize  = true;
            label2.Font      = new System.Drawing.Font("Segoe UI", 11.25F);
            label2.Location  = new System.Drawing.Point(607, 317);
            label2.Name      = "label2";
            label2.Text      = "Şifre";

            // panelSifre — TextBox + göz butonu için kapsayıcı
            panelSifre.Location    = new System.Drawing.Point(607, 346);
            panelSifre.Name        = "panelSifre";
            panelSifre.Size        = new System.Drawing.Size(326, 29);
            panelSifre.TabIndex    = 3;
            panelSifre.BackColor   = System.Drawing.Color.Transparent;
            panelSifre.Controls.Add(txtPassword);
            panelSifre.Controls.Add(btnGozToggle);

            // txtPassword
            txtPassword.BorderStyle          = System.Windows.Forms.BorderStyle.FixedSingle;
            txtPassword.Font                 = new System.Drawing.Font("Segoe UI", 11.25F);
            txtPassword.Location             = new System.Drawing.Point(0, 0);
            txtPassword.Name                 = "txtPassword";
            txtPassword.Size                 = new System.Drawing.Size(296, 27);
            txtPassword.TabIndex             = 0;
            txtPassword.UseSystemPasswordChar= true;

            // btnGozToggle — göz ikonu
            btnGozToggle.Location               = new System.Drawing.Point(298, 0);
            btnGozToggle.Name                   = "btnGozToggle";
            btnGozToggle.Size                   = new System.Drawing.Size(28, 27);
            btnGozToggle.TabIndex               = 1;
            btnGozToggle.TabStop                = false;
            btnGozToggle.Text                   = "👁";
            btnGozToggle.Font                   = new System.Drawing.Font("Segoe UI", 9F);
            btnGozToggle.FlatStyle              = System.Windows.Forms.FlatStyle.Flat;
            btnGozToggle.FlatAppearance.BorderSize = 0;
            btnGozToggle.BackColor              = System.Drawing.Color.FromArgb(240, 242, 245);
            btnGozToggle.ForeColor              = System.Drawing.Color.FromArgb(100, 110, 120);
            btnGozToggle.Cursor                 = System.Windows.Forms.Cursors.Hand;
            btnGozToggle.Click                 += btnGozToggle_Click;

            // chkAcikTut — "Oturumu Açık Tut"
            chkAcikTut.AutoSize  = true;
            chkAcikTut.Font      = new System.Drawing.Font("Segoe UI", 9.75F);
            chkAcikTut.ForeColor = System.Drawing.Color.FromArgb(60, 70, 80);
            chkAcikTut.Location  = new System.Drawing.Point(607, 388);
            chkAcikTut.Name      = "chkAcikTut";
            chkAcikTut.Text      = "Oturumu Açık Tut";
            chkAcikTut.TabIndex  = 4;
            chkAcikTut.Cursor    = System.Windows.Forms.Cursors.Hand;

            // btnLogin
            btnLogin.BackColor               = System.Drawing.Color.FromArgb(46, 134, 193);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle               = System.Windows.Forms.FlatStyle.Flat;
            btnLogin.Font                    = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            btnLogin.ForeColor               = System.Drawing.Color.White;
            btnLogin.Location                = new System.Drawing.Point(607, 420);
            btnLogin.Name                    = "btnLogin";
            btnLogin.Size                    = new System.Drawing.Size(327, 52);
            btnLogin.TabIndex                = 5;
            btnLogin.Text                    = "GİRİŞ YAP";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click                  += btnLogin_Click;

            // btnDilToggle
            btnDilToggle.BackColor               = System.Drawing.Color.FromArgb(22, 160, 133);
            btnDilToggle.Cursor                  = System.Windows.Forms.Cursors.Hand;
            btnDilToggle.FlatAppearance.BorderSize = 0;
            btnDilToggle.FlatStyle               = System.Windows.Forms.FlatStyle.Flat;
            btnDilToggle.Font                    = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnDilToggle.ForeColor               = System.Drawing.Color.White;
            btnDilToggle.Location                = new System.Drawing.Point(1261, 703);
            btnDilToggle.Name                    = "btnDilToggle";
            btnDilToggle.Size                    = new System.Drawing.Size(100, 40);
            btnDilToggle.TabIndex                = 6;
            btnDilToggle.Text                    = "🌐 EN";
            btnDilToggle.UseVisualStyleBackColor = false;
            btnDilToggle.Click                  += btnDilToggle_Click;

            // FormLogin
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            BackColor           = System.Drawing.Color.FromArgb(244, 246, 248);
            ClientSize          = new System.Drawing.Size(1381, 763);
            Controls.Add(btnDilToggle);
            Controls.Add(btnLogin);
            Controls.Add(chkAcikTut);
            Controls.Add(panelSifre);
            Controls.Add(label2);
            Controls.Add(txtUsername);
            Controls.Add(label1);
            Controls.Add(panelLeft);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox     = false;
            Name            = "FormLogin";
            StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text            = "Giriş";
            Load           += FormLogin_Load;

            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelSifre.ResumeLayout(false);
            panelSifre.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel    panelLeft;
        private System.Windows.Forms.Label    lblDesc;
        private System.Windows.Forms.Label    lblTitle;
        private System.Windows.Forms.Label    label1;
        private System.Windows.Forms.TextBox  txtUsername;
        private System.Windows.Forms.Label    label2;
        private System.Windows.Forms.Panel    panelSifre;
        private System.Windows.Forms.TextBox  txtPassword;
        private System.Windows.Forms.Button   btnGozToggle;
        private System.Windows.Forms.CheckBox chkAcikTut;
        private System.Windows.Forms.Button   btnLogin;
        private System.Windows.Forms.Button   btnDilToggle;
    }
}
