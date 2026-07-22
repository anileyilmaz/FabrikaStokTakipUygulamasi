namespace FabrikaStokTakipUygulamasi
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        private void InitializeComponent()
        {
            this.panelSidebar     = new System.Windows.Forms.Panel();
            this.btnLowStock      = new System.Windows.Forms.Button();
            this.btnArama         = new System.Windows.Forms.Button();
            this.btnUrunEkle      = new System.Windows.Forms.Button();
            this.btnUrunler       = new System.Windows.Forms.Button();
            this.btnDashboard     = new System.Windows.Forms.Button();
            this.btnAdmin         = new System.Windows.Forms.Button();
            this.panelKullanici   = new System.Windows.Forms.Panel();
            this.lblKullaniciAdi  = new System.Windows.Forms.Label();
            this.lblKullaniciRol  = new System.Windows.Forms.Label();
            this.btnOturumKapat   = new System.Windows.Forms.Button();
            this.panelMain        = new System.Windows.Forms.Panel();
            this.panelTop         = new System.Windows.Forms.Panel();
            this.label1           = new System.Windows.Forms.Label();
            this.panelSidebar.SuspendLayout();
            this.panelKullanici.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();

            // panelSidebar
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.panelSidebar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSidebar.Controls.Add(this.btnLowStock);
            this.panelSidebar.Controls.Add(this.btnArama);
            this.panelSidebar.Controls.Add(this.btnUrunEkle);
            this.panelSidebar.Controls.Add(this.btnUrunler);
            this.panelSidebar.Controls.Add(this.btnDashboard);
            this.panelSidebar.Controls.Add(this.btnAdmin);
            this.panelSidebar.Controls.Add(this.panelKullanici);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(200, 661);
            this.panelSidebar.TabIndex = 0;

            // btnDashboard
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnDashboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(0, 0);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnDashboard.Size = new System.Drawing.Size(198, 55);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);

            // btnUrunler
            this.btnUrunler.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUrunler.FlatAppearance.BorderSize = 0;
            this.btnUrunler.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnUrunler.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnUrunler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUrunler.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnUrunler.ForeColor = System.Drawing.Color.White;
            this.btnUrunler.Location = new System.Drawing.Point(0, 55);
            this.btnUrunler.Name = "btnUrunler";
            this.btnUrunler.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnUrunler.Size = new System.Drawing.Size(198, 55);
            this.btnUrunler.TabIndex = 1;
            this.btnUrunler.Text = "Ürünler";
            this.btnUrunler.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUrunler.UseVisualStyleBackColor = true;
            this.btnUrunler.Click += new System.EventHandler(this.btnUrunler_Click);

            // btnUrunEkle
            this.btnUrunEkle.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUrunEkle.FlatAppearance.BorderSize = 0;
            this.btnUrunEkle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnUrunEkle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnUrunEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUrunEkle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnUrunEkle.ForeColor = System.Drawing.Color.White;
            this.btnUrunEkle.Location = new System.Drawing.Point(0, 110);
            this.btnUrunEkle.Name = "btnUrunEkle";
            this.btnUrunEkle.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnUrunEkle.Size = new System.Drawing.Size(198, 55);
            this.btnUrunEkle.TabIndex = 2;
            this.btnUrunEkle.Text = "Ürün Ekle";
            this.btnUrunEkle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUrunEkle.UseVisualStyleBackColor = true;
            this.btnUrunEkle.Click += new System.EventHandler(this.btnUrunEkle_Click);

            // btnArama
            this.btnArama.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnArama.FlatAppearance.BorderSize = 0;
            this.btnArama.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnArama.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnArama.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArama.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnArama.ForeColor = System.Drawing.Color.White;
            this.btnArama.Location = new System.Drawing.Point(0, 165);
            this.btnArama.Name = "btnArama";
            this.btnArama.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnArama.Size = new System.Drawing.Size(198, 55);
            this.btnArama.TabIndex = 3;
            this.btnArama.Text = "Gelişmiş Arama";
            this.btnArama.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnArama.UseVisualStyleBackColor = true;
            this.btnArama.Click += new System.EventHandler(this.btnArama_Click);

            // btnLowStock
            this.btnLowStock.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLowStock.FlatAppearance.BorderSize = 0;
            this.btnLowStock.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnLowStock.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnLowStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLowStock.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLowStock.ForeColor = System.Drawing.Color.White;
            this.btnLowStock.Location = new System.Drawing.Point(0, 220);
            this.btnLowStock.Name = "btnLowStock";
            this.btnLowStock.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnLowStock.Size = new System.Drawing.Size(198, 55);
            this.btnLowStock.TabIndex = 4;
            this.btnLowStock.Text = "Low Stock";
            this.btnLowStock.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLowStock.UseVisualStyleBackColor = true;
            this.btnLowStock.Click += new System.EventHandler(this.btnLowStock_Click);

            // btnAdmin
            this.btnAdmin.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdmin.FlatAppearance.BorderSize = 0;
            this.btnAdmin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnAdmin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.btnAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdmin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnAdmin.ForeColor = System.Drawing.Color.FromArgb(243, 156, 18);
            this.btnAdmin.BackColor = System.Drawing.Color.FromArgb(30, 44, 57);
            this.btnAdmin.Location = new System.Drawing.Point(0, 275);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnAdmin.Size = new System.Drawing.Size(198, 55);
            this.btnAdmin.TabIndex = 5;
            this.btnAdmin.Text = "Admin Paneli";
            this.btnAdmin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdmin.UseVisualStyleBackColor = false;
            this.btnAdmin.Visible = false;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);

            // panelKullanici - sol alttaki kullanıcı bilgisi + oturum kapat butonu
            this.panelKullanici.BackColor = System.Drawing.Color.FromArgb(30, 44, 57);
            this.panelKullanici.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelKullanici.Height = 110;
            this.panelKullanici.Name = "panelKullanici";
            this.panelKullanici.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelKullanici.Controls.Add(this.lblKullaniciRol);
            this.panelKullanici.Controls.Add(this.lblKullaniciAdi);
            this.panelKullanici.Controls.Add(this.btnOturumKapat);

            // lblKullaniciAdi
            this.lblKullaniciAdi.AutoSize = true;
            this.lblKullaniciAdi.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblKullaniciAdi.ForeColor = System.Drawing.Color.White;
            this.lblKullaniciAdi.Location = new System.Drawing.Point(12, 10);
            this.lblKullaniciAdi.Name = "lblKullaniciAdi";
            this.lblKullaniciAdi.Text = "";

            // lblKullaniciRol
            this.lblKullaniciRol.AutoSize = true;
            this.lblKullaniciRol.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblKullaniciRol.ForeColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.lblKullaniciRol.Location = new System.Drawing.Point(12, 34);
            this.lblKullaniciRol.Name = "lblKullaniciRol";
            this.lblKullaniciRol.Text = "";

            // btnOturumKapat - kullanıcı bilgisinin altında
            this.btnOturumKapat.Text = "Oturumu Kapat";
            this.btnOturumKapat.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnOturumKapat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOturumKapat.Location = new System.Drawing.Point(12, 60);
            this.btnOturumKapat.Size = new System.Drawing.Size(168, 36);
            this.btnOturumKapat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOturumKapat.FlatAppearance.BorderSize = 0;
            this.btnOturumKapat.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.btnOturumKapat.ForeColor = System.Drawing.Color.White;
            this.btnOturumKapat.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnOturumKapat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOturumKapat.Name = "btnOturumKapat";
            this.btnOturumKapat.TabIndex = 6;
            this.btnOturumKapat.Click += new System.EventHandler(this.btnOturumKapat_Click);

            // panelMain
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Controls.Add(this.panelTop);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(200, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(984, 661);
            this.panelMain.TabIndex = 1;

            // panelTop - üst başlık çubuğu
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(45, 62, 80);
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(982, 60);
            this.panelTop.TabIndex = 0;

            // label1 - başlık yazısı
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(25, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "FABRİKA STOK TAKİP UYGULAMASI";

            // Form1 ayarları
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelSidebar);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fabrika Stok Takip Uygulaması";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelSidebar.ResumeLayout(false);
            this.panelKullanici.ResumeLayout(false);
            this.panelKullanici.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label1;
        public  System.Windows.Forms.Button btnDashboard;
        public  System.Windows.Forms.Button btnUrunler;
        public  System.Windows.Forms.Button btnUrunEkle;
        public  System.Windows.Forms.Button btnArama;
        public  System.Windows.Forms.Button btnLowStock;
        public  System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.Panel panelKullanici;
        public  System.Windows.Forms.Label lblKullaniciAdi;
        public  System.Windows.Forms.Label lblKullaniciRol;
        private System.Windows.Forms.Button btnOturumKapat;
    }
}
