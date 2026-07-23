using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    public partial class FormUrunEkle : Form
    {
        private byte[] _secilenPdfData = null;
        private string _secilenPdfAdi  = null;

        public FormUrunEkle()
        {
            InitializeComponent();
            this.TopLevel        = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock            = DockStyle.Fill;

            btnUrunEkle.Paint  += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ekle,  btnUrunEkle.ClientRectangle, Color.White);
            btnTemizle.Paint   += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kapat, btnTemizle.ClientRectangle,  Color.White);
            btnPdfSec.Paint    += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Dokuman, btnPdfSec.ClientRectangle, Color.White, 11f);
            btnPdfKaldir.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kapat,   btnPdfKaldir.ClientRectangle, Color.White, 11f);

            foreach (var grp in new GroupBox[] { grpGenel, grpOlcu, grpStok, grpSertifika })
                UIStil.YuvarlakBolgeUygula(grp, 10);

            UIStil.HoverAnimasyonuBagla(btnUrunEkle, UIStil.Aksan, Color.FromArgb(245, 180, 90), renk => btnUrunEkle.BackColor = renk);
        }

        private void FormUrunEkle_Load(object sender, EventArgs e)
        {
            DiliUygula();
            LangManager.DilDegisti += DiliUygula;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            LangManager.DilDegisti -= DiliUygula;
            base.OnFormClosed(e);
        }

        private void DiliUygula()
        {
            lblTitle.Text       = LangManager.T("urunekle.baslik");
            btnUrunEkle.Text    = LangManager.T("urunekle.ekle");
            btnTemizle.Text     = LangManager.T("urunekle.temizle");
            btnPdfSec.Text      = LangManager.T("urunekle.pdfsec");
            btnPdfKaldir.Text   = LangManager.T("urunekle.pdfsil");
            grpGenel.Text       = LangManager.Ingilizce ? "General Info" : "Genel Bilgiler";
            grpOlcu.Text        = LangManager.Ingilizce ? "Dimensions" : "Ürün Ölçüleri";
            grpStok.Text        = LangManager.Ingilizce ? "Stock Info" : "Stok Bilgisi";
            grpSertifika.Text   = LangManager.Ingilizce ? "Certificate (PDF)" : "Sertifika (PDF)";
            label1.Text         = LangManager.T("ue.adet");
            // Designer label'ları (GroupBox içindeki)
            Customer.Text    = LangManager.T("ue.customer");
            Certificate.Text = LangManager.T("ue.certificate");
            Material.Text    = LangManager.T("ue.material");
            Batch.Text       = LangManager.T("ue.batch");
            lblParent.Text   = LangManager.T("ue.parent");
            Heat.Text        = LangManager.T("ue.heat");
            Grade.Text       = LangManager.T("ue.grade");
            THK.Text         = LangManager.T("ue.thk");
            lblWidth.Text    = LangManager.T("ue.width");
            Lenght.Text      = LangManager.T("ue.length");
            PdfDurumGuncelle();
        }

        // ── PDF seçme ────────────────────────────────────────────────────────
        private void btnPdfSec_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title  = LangManager.T("urunekle.pdfyukle");
                ofd.Filter = "PDF Dosyası (*.pdf)|*.pdf";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                // Boyut kontrolü: max 20 MB
                var bilgi = new FileInfo(ofd.FileName);
                if (bilgi.Length > 20 * 1024 * 1024)
                {
                    MessageBox.Show(LangManager.Ingilizce
                        ? "PDF file is too large. Max 20 MB."
                        : "PDF dosyası çok büyük. Maksimum 20 MB.",
                        LangManager.T("genel.uyari"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _secilenPdfData = File.ReadAllBytes(ofd.FileName);
                _secilenPdfAdi  = Path.GetFileName(ofd.FileName);
                PdfDurumGuncelle();
            }
        }

        private void btnPdfKaldir_Click(object sender, EventArgs e)
        {
            _secilenPdfData = null;
            _secilenPdfAdi  = null;
            PdfDurumGuncelle();
        }

        private void PdfDurumGuncelle()
        {
            if (_secilenPdfData != null)
            {
                lblPdfDurum.Text      = _secilenPdfAdi;
                lblPdfDurum.ForeColor = Color.FromArgb(39, 174, 96);
                btnPdfKaldir.Visible  = true;
            }
            else
            {
                lblPdfDurum.Text      = LangManager.T("urunekle.pdfyok");
                lblPdfDurum.ForeColor = Color.FromArgb(149, 165, 166);
                btnPdfKaldir.Visible  = false;
            }
        }

        // ── Ürün Ekle ────────────────────────────────────────────────────────
        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustomer.Text))
            {
                MessageBox.Show(
                    LangManager.Ingilizce ? "Please fill the Customer field." : "Lütfen Customer alanını doldurunuz.",
                    LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomer.Focus();
                return;
            }

            if (!int.TryParse(txtAdet.Text.Trim(), out int adet) || adet < 0)
            {
                MessageBox.Show(
                    LangManager.Ingilizce ? "Quantity must be a valid non-negative integer." : "Ürün adedi geçerli bir tam sayı olmalıdır.",
                    LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAdet.Focus();
                return;
            }

            var urun = new Urun
            {
                UrunCinsi         = txtMaterial.Text.Trim(),
                Material          = txtMaterial.Text.Trim(),
                Grade             = txtGrade.Text.Trim(),
                Thickness         = txtTHK.Text.Trim(),
                Width             = txtWidth.Text.Trim(),
                Length            = txtLength.Text.Trim(),
                Stok              = adet,
                Customer          = txtCustomer.Text.Trim(),
                Certificate       = txtCertificate.Text.Trim(),
                Batch             = txtBatch.Text.Trim(),
                Heat              = txtHeat.Text.Trim(),
                Parent            = txtParent.Text.Trim(),
                EklenmeTarihi     = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                SertifikaPdf      = _secilenPdfData,
                SertifikaDosyaAdi = _secilenPdfAdi
            };

            try
            {
                int yeniId = StokVeritabani.UrunEkle(urun);
                StokVeritabani.HareketKaydet(yeniId, urun.UrunCinsi, 0, adet);

                MessageBox.Show(
                    LangManager.Ingilizce ? "Product added successfully!" : "Ürün başarıyla eklendi!",
                    LangManager.T("genel.basarili"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                TemizleFormulari();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    (LangManager.Ingilizce ? "Error adding product:\n" : "Ürün eklenirken hata oluştu:\n") + ex.Message,
                    LangManager.T("genel.hata"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TemizleFormulari()
        {
            txtCustomer.Clear();   txtCertificate.Clear(); txtMaterial.Clear();
            txtBatch.Clear();      txtParent.Clear();      txtHeat.Clear();
            txtGrade.Clear();      txtTHK.Clear();         txtWidth.Clear();
            txtLength.Clear();     txtAdet.Clear();
            _secilenPdfData = null;
            _secilenPdfAdi  = null;
            PdfDurumGuncelle();
        }

        private void btnTemizle_Click(object sender, EventArgs e) { TemizleFormulari(); }
    }
}
