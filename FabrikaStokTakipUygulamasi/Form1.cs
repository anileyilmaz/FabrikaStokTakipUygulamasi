using System;
using System.Drawing;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    public partial class Form1 : Form
    {
        private Button aktifButon;

        public Form1()
        {
            InitializeComponent();

            btnDashboard.Paint   += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.AnaSayfa, btnDashboard.ClientRectangle, Color.White);
            btnUrunler.Paint     += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kutu,      btnUrunler.ClientRectangle,   Color.White);
            btnUrunEkle.Paint    += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ekle,       btnUrunEkle.ClientRectangle,  Color.White);
            btnArama.Paint       += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ara,        btnArama.ClientRectangle,     Color.White);
            btnLowStock.Paint    += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Uyarim,     btnLowStock.ClientRectangle, Color.White);
            btnAdmin.Paint       += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ayarlar,    btnAdmin.ClientRectangle,    btnAdmin.ForeColor);
            btnOturumKapat.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kilit,      btnOturumKapat.ClientRectangle, Color.White);

            if (KullaniciYonetici.AktifKullanici != null)
            {
                lblKullaniciAdi.Text = KullaniciYonetici.AktifKullanici.KullaniciAdi;
                lblKullaniciRol.Text = KullaniciYonetici.AktifKullanici.RolAdi;
            }

            btnAdmin.Visible = KullaniciYonetici.AktifKullanici?.IsAdmin ?? false;

            LangManager.DilDegisti += NavMetinleriGuncelle;
            NavMetinleriGuncelle();

            ButonuAktifYap(btnDashboard);
            SayfaYukle(new FormDashboard());
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            LangManager.DilDegisti -= NavMetinleriGuncelle;
            // Uygulama herhangi bir şekilde kapanırsa (X tuşu dahil) çevrimdışı yap
            KullaniciYonetici.Cikis();
            base.OnFormClosed(e);
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void NavMetinleriGuncelle()
        {
            btnDashboard.Text   = LangManager.T("nav.dashboard");
            btnUrunler.Text     = LangManager.T("nav.urunler");
            btnUrunEkle.Text    = LangManager.T("nav.urunekle");
            btnArama.Text       = LangManager.T("nav.arama");
            btnLowStock.Text    = LangManager.T("nav.lowstock");
            btnAdmin.Text       = LangManager.T("nav.admin");
            btnOturumKapat.Text = LangManager.T("nav.cikis");
            label1.Text         = LangManager.T("nav.baslik");

            try
            {
                if (panelMain.Tag is Form aktifForm && !aktifForm.IsDisposed)
                {
                    var tip = aktifForm.GetType();
                    Form yeniForm = null;
                    if      (tip == typeof(FormDashboard)) yeniForm = new FormDashboard();
                    else if (tip == typeof(FormUrunler))   yeniForm = new FormUrunler();
                    else if (tip == typeof(FormUrunEkle))  yeniForm = new FormUrunEkle();
                    else if (tip == typeof(FormArama))     yeniForm = new FormArama();
                    else if (tip == typeof(FormLowStock))  yeniForm = new FormLowStock();
                    else if (tip == typeof(FormAdmin))     yeniForm = new FormAdmin();
                    if (yeniForm != null) SayfaYukle(yeniForm);
                }
            }
            catch { }
        }

        public void SayfaYukle(Form sayfa)
        {
            panelMain.Controls.Clear();
            sayfa.TopLevel        = false;
            sayfa.FormBorderStyle = FormBorderStyle.None;
            sayfa.Dock            = DockStyle.Fill;
            panelMain.Controls.Add(sayfa);
            panelMain.Tag = sayfa;
            sayfa.BringToFront();
            sayfa.Show();
        }

        public void FormYukle(Form frm) => SayfaYukle(frm);

        private void ButonuAktifYap(Button btn)
        {
            if (aktifButon != null) aktifButon.BackColor = Color.FromArgb(45, 62, 80);
            aktifButon = btn;
            aktifButon.BackColor = Color.FromArgb(41, 128, 185);
        }

        private void btnDashboard_Click(object sender, EventArgs e) { ButonuAktifYap(btnDashboard); SayfaYukle(new FormDashboard()); }
        private void btnUrunler_Click(object sender, EventArgs e)    { ButonuAktifYap(btnUrunler);   SayfaYukle(new FormUrunler()); }
        private void btnUrunEkle_Click(object sender, EventArgs e)   { ButonuAktifYap(btnUrunEkle);  SayfaYukle(new FormUrunEkle()); }
        private void btnArama_Click(object sender, EventArgs e)      { ButonuAktifYap(btnArama);     SayfaYukle(new FormArama()); }
        private void btnLowStock_Click(object sender, EventArgs e)   { ButonuAktifYap(btnLowStock);  SayfaYukle(new FormLowStock()); }
        private void btnAdmin_Click(object sender, EventArgs e)      { ButonuAktifYap(btnAdmin);     SayfaYukle(new FormAdmin()); }

        private void btnOturumKapat_Click(object sender, EventArgs e)
        {
            string mesaj  = LangManager.T("oturum.kapat.mesaj");
            string baslik = LangManager.T("oturum.kapat.baslik");

            if (MessageBox.Show(mesaj, baslik, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                != DialogResult.Yes) return;

            // Bilinçli çıkış → "oturumu açık tut" ayarını sil
            OturumAyarlari.Kaydet(false, string.Empty, string.Empty);

            KullaniciYonetici.Cikis();

            var yeniLogin = new FormLogin();
            Program.AppContext.MainForm = yeniLogin;
            yeniLogin.Show();
            this.Close();
        }
    }
}
