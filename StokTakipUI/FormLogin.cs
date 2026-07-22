using System;
using System.Windows.Forms;

namespace StokTakipUI
{
    public partial class FormLogin : Form
    {
        private bool _sifreGoruntu = false;

        public FormLogin()
        {
            InitializeComponent();
            DiliUygula();
            LangManager.DilDegisti += DiliUygula;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            LangManager.DilDegisti -= DiliUygula;
            base.OnFormClosed(e);
        }

        // ── Load: UI doldur ──────────────────────────────────────────────────
        private void FormLogin_Load(object sender, EventArgs e)
        {
            var (acikTut, kullaniciAdi, _) = OturumAyarlari.Oku();
            chkAcikTut.Checked = acikTut;
            if (acikTut && !string.IsNullOrEmpty(kullaniciAdi))
                txtUsername.Text = kullaniciAdi;
        }

        // ── Shown: AppContext hazır → otomatik giriş ─────────────────────────
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var (acikTut, kullaniciAdi, sifre) = OturumAyarlari.Oku();
            if (!acikTut) return;
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre)) return;

            if (KullaniciYonetici.GirisYap(kullaniciAdi, sifre))
                GirisBasariliOturumAc();
        }

        // ── Dil ─────────────────────────────────────────────────────────────
        private void DiliUygula()
        {
            lblTitle.Text     = LangManager.T("login.baslik");
            lblDesc.Text      = LangManager.T("login.desc");
            label1.Text       = LangManager.T("login.kullanici");
            label2.Text       = LangManager.T("login.sifre");
            btnLogin.Text     = LangManager.T("login.btn");
            btnDilToggle.Text = LangManager.AktifDil == LangManager.Dil.TR ? "🌐 EN" : "🌐 TR";
            chkAcikTut.Text   = LangManager.Ingilizce ? "Keep me logged in" : "Oturumu Açık Tut";
            this.Text         = LangManager.Ingilizce ? "Login" : "Giriş";
        }

        private void btnDilToggle_Click(object sender, EventArgs e)
        {
            LangManager.DilAyarla(LangManager.AktifDil == LangManager.Dil.TR
                ? LangManager.Dil.EN : LangManager.Dil.TR);
        }

        // ── Göz butonu ───────────────────────────────────────────────────────
        private void btnGozToggle_Click(object sender, EventArgs e)
        {
            _sifreGoruntu = !_sifreGoruntu;
            txtPassword.UseSystemPasswordChar = !_sifreGoruntu;
            btnGozToggle.ForeColor = _sifreGoruntu
                ? System.Drawing.Color.FromArgb(46, 134, 193)
                : System.Drawing.Color.FromArgb(100, 110, 120);
        }

        // ── Giriş Yap ────────────────────────────────────────────────────────
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtUsername.Text.Trim();
            string sifre        = txtPassword.Text;

            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show(LangManager.T("login.bos"),
                    LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!KullaniciYonetici.GirisYap(kullaniciAdi, sifre))
            {
                MessageBox.Show(LangManager.T("login.hatali"),
                    LangManager.T("login.basarisiz"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            // Checkbox'a göre kaydet/sil
            OturumAyarlari.Kaydet(chkAcikTut.Checked, kullaniciAdi, sifre);
            GirisBasariliOturumAc();
        }

        // ── Ana formu aç ─────────────────────────────────────────────────────
        private void GirisBasariliOturumAc()
        {
            var anaEkran = new Form1();

            // Form1 kapanınca login ekranını da kapat
            // Oturum temizleme Form1.btnOturumKapat_Click'te yapılıyor
            anaEkran.FormClosed += (s, args) => this.Close();

            if (Program.AppContext != null)
                Program.AppContext.MainForm = anaEkran;

            anaEkran.Show();
            this.Hide();
        }
    }
}
