using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    public class FormUrunDuzenle : Form
    {
        private static readonly Color CNavy    = UIStil.Lacivert;
        private static readonly Color CLightBg = UIStil.GriAcik;
        private static readonly Color CAccent  = UIStil.Aksan;
        private static readonly Color CWhite   = UIStil.Beyaz;
        private static readonly Color CBorder  = UIStil.GriOrta;

        private readonly Urun _urun;
        public bool Guncellendi { get; private set; } = false;

        private TextBox txtUrunCinsi, txtMaterial, txtGrade, txtThickness,
                        txtWidth, txtLength, txtCustomer, txtCertificate,
                        txtBatch, txtHeat, txtParent;
        private NumericUpDown nudStok;

        // PDF alanı
        private byte[] _yeniPdfData;
        private string _yeniPdfAdi;
        private bool   _pdfSilindi = false;
        private Label  lblPdfDurum;
        private Button btnPdfDegis, btnPdfSil;

        public FormUrunDuzenle(Urun urun)
        {
            _urun = urun;
            _yeniPdfData = urun.SertifikaPdf;
            _yeniPdfAdi  = urun.SertifikaDosyaAdi;
            BuildUI();
            DoldurAlanlar();
        }

        private void BuildUI()
        {
            this.Text            = LangManager.T("duzenle.baslik");
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = CLightBg;
            this.ClientSize      = new Size(660, 980);
            this.Font            = new Font("Segoe UI", 10f);

            // ── Başlık ────────────────────────────────────────────────────────
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 75, BackColor = CNavy };
            panelHeader.Controls.Add(new Label
            {
                Text = LangManager.T("duzenle.baslik"),
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = CWhite, AutoSize = true, Location = new Point(22, 16)
            });
            panelHeader.Controls.Add(new Label
            {
                Text = _urun.UrunCinsi, Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(189, 195, 199), AutoSize = true, Location = new Point(24, 48)
            });

            // ── Form Kartı ────────────────────────────────────────────────────
            var panelCard = new Panel
            {
                BackColor = CWhite, Location = new Point(22, 90),
                Size = new Size(616, 720), BorderStyle = BorderStyle.None
            };
            panelCard.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(CBorder),
                    new Rectangle(0, 0, panelCard.Width - 1, panelCard.Height - 1));

            var scrollPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = CWhite };
            panelCard.Controls.Add(scrollPanel);

            int y = 16;
            txtUrunCinsi   = AlanEkle(scrollPanel, "Ürün Cinsi",  ref y, 598);
            txtMaterial    = AlanEkle(scrollPanel, "Material",    ref y, 598);
            txtGrade       = AlanEkle(scrollPanel, "Grade",       ref y, 598);
            txtThickness   = AlanEkle(scrollPanel, "Thickness",   ref y, 598);
            txtWidth       = AlanEkle(scrollPanel, "Width",       ref y, 598);
            txtLength      = AlanEkle(scrollPanel, "Length",      ref y, 598);
            nudStok        = StokAlanEkle(scrollPanel, "Stok",    ref y, 598);
            txtCustomer    = AlanEkle(scrollPanel, "Customer",    ref y, 598);
            txtCertificate = AlanEkle(scrollPanel, "Certificate", ref y, 598);
            txtBatch       = AlanEkle(scrollPanel, "Batch",       ref y, 598);
            txtHeat        = AlanEkle(scrollPanel, "Heat",        ref y, 598);
            txtParent      = AlanEkle(scrollPanel, "Parent",      ref y, 598);

            // ── PDF Paneli ────────────────────────────────────────────────────
            var panelPdf = new Panel
            {
                BackColor = CWhite, Location = new Point(22, 824),
                Size = new Size(616, 100), BorderStyle = BorderStyle.None
            };
            panelPdf.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(CBorder),
                    new Rectangle(0, 0, panelPdf.Width - 1, panelPdf.Height - 1));

            var lblPdfBaslik = new Label
            {
                Text = "Sertifika PDF",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(14, 8), AutoSize = true
            };

            btnPdfDegis = new Button
            {
                Text = LangManager.T("duzenle.pdfdegis"),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = CWhite, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Padding = new Padding(24, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(14, 30), Size = new Size(185, 34), Cursor = Cursors.Hand
            };
            btnPdfDegis.FlatAppearance.BorderSize = 0;
            btnPdfDegis.Click += BtnPdfDegis_Click;

            btnPdfSil = new Button
            {
                Text = LangManager.T("duzenle.pdfsil"),
                BackColor = Color.FromArgb(192, 57, 43),
                ForeColor = CWhite, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                Padding = new Padding(24, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(208, 30), Size = new Size(185, 34), Cursor = Cursors.Hand
            };
            btnPdfSil.FlatAppearance.BorderSize = 0;
            btnPdfSil.Click += BtnPdfSil_Click;

            btnPdfDegis.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Dokuman, btnPdfDegis.ClientRectangle, CWhite, 11f);
            btnPdfSil.Paint   += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Sil,      btnPdfSil.ClientRectangle,   CWhite, 11f);

            lblPdfDurum = new Label
            {
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(14, 70), Size = new Size(580, 20)
            };

            panelPdf.Controls.Add(lblPdfBaslik);
            panelPdf.Controls.Add(btnPdfDegis);
            panelPdf.Controls.Add(btnPdfSil);
            panelPdf.Controls.Add(lblPdfDurum);
            PdfDurumGuncelle();

            // ── Güncelle / İptal ──────────────────────────────────────────────
            var btnGuncelle = new Button
            {
                Text = LangManager.T("duzenle.guncelle"),
                BackColor = CAccent, ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                Size = new Size(302, 48), Location = new Point(22, 936), Cursor = Cursors.Hand
            };
            btnGuncelle.FlatAppearance.BorderSize = 0;
            btnGuncelle.Click += BtnGuncelle_Click;

            var btnIptal = new Button
            {
                Text = LangManager.T("duzenle.iptal"),
                BackColor = Color.FromArgb(149, 165, 166), ForeColor = CWhite,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                Size = new Size(302, 48), Location = new Point(342, 936), Cursor = Cursors.Hand
            };
            btnIptal.FlatAppearance.BorderSize = 0;
            btnIptal.Click += (s, e) => this.Close();

            this.Controls.Add(panelHeader);
            this.Controls.Add(panelCard);
            this.Controls.Add(panelPdf);
            this.Controls.Add(btnGuncelle);
            this.Controls.Add(btnIptal);
        }

        private void BtnPdfDegis_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title  = LangManager.T("duzenle.pdfdegis");
                ofd.Filter = "PDF Dosyası (*.pdf)|*.pdf";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                var bilgi = new FileInfo(ofd.FileName);
                if (bilgi.Length > 20 * 1024 * 1024)
                {
                    MessageBox.Show(LangManager.Ingilizce
                        ? "PDF file is too large. Max 20 MB."
                        : "PDF dosyası çok büyük. Maksimum 20 MB.",
                        LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _yeniPdfData = File.ReadAllBytes(ofd.FileName);
                _yeniPdfAdi  = Path.GetFileName(ofd.FileName);
                _pdfSilindi  = false;
                PdfDurumGuncelle();
            }
        }

        private void BtnPdfSil_Click(object sender, EventArgs e)
        {
            _yeniPdfData = null;
            _yeniPdfAdi  = null;
            _pdfSilindi  = true;
            PdfDurumGuncelle();
        }

        private void PdfDurumGuncelle()
        {
            if (_yeniPdfData != null)
            {
                lblPdfDurum.Text      = _yeniPdfAdi;
                lblPdfDurum.ForeColor = Color.FromArgb(39, 174, 96);
                btnPdfSil.Visible     = true;
            }
            else if (_pdfSilindi)
            {
                lblPdfDurum.Text      = LangManager.Ingilizce ? "PDF will be removed on save." : "Kayıtta PDF kaldırılacak.";
                lblPdfDurum.ForeColor = Color.FromArgb(192, 57, 43);
                btnPdfSil.Visible     = false;
            }
            else
            {
                lblPdfDurum.Text      = LangManager.Ingilizce ? "No PDF attached." : "PDF eklenmemiş.";
                lblPdfDurum.ForeColor = Color.FromArgb(149, 165, 166);
                btnPdfSil.Visible     = false;
            }
        }

        private TextBox AlanEkle(Panel parent, string etiket, ref int y, int panelWidth)
        {
            parent.Controls.Add(new Label
            {
                Text = etiket, Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(18, y + 2), Size = new Size(140, 22),
                TextAlign = ContentAlignment.MiddleLeft
            });
            var txt = new TextBox
            {
                Location = new Point(165, y), Size = new Size(panelWidth - 185, 26),
                Font = new Font("Segoe UI", 10f), BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            parent.Controls.Add(txt);
            parent.Controls.Add(new Panel
            {
                BackColor = Color.FromArgb(236, 240, 241),
                Location = new Point(0, y + 36), Size = new Size(panelWidth - 20, 1)
            });
            y += 52;
            return txt;
        }

        private NumericUpDown StokAlanEkle(Panel parent, string etiket, ref int y, int panelWidth)
        {
            parent.Controls.Add(new Label
            {
                Text = etiket, Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(18, y + 2), Size = new Size(140, 22),
                TextAlign = ContentAlignment.MiddleLeft
            });
            var nud = new NumericUpDown
            {
                Location = new Point(165, y), Size = new Size(120, 26),
                Font = new Font("Segoe UI", 10f), Minimum = 0, Maximum = 999999,
                BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(248, 249, 250)
            };
            parent.Controls.Add(nud);
            parent.Controls.Add(new Panel
            {
                BackColor = Color.FromArgb(236, 240, 241),
                Location = new Point(0, y + 36), Size = new Size(panelWidth - 20, 1)
            });
            y += 52;
            return nud;
        }

        private void DoldurAlanlar()
        {
            txtUrunCinsi.Text   = _urun.UrunCinsi   ?? "";
            txtMaterial.Text    = _urun.Material     ?? "";
            txtGrade.Text       = _urun.Grade        ?? "";
            txtThickness.Text   = _urun.Thickness    ?? "";
            txtWidth.Text       = _urun.Width        ?? "";
            txtLength.Text      = _urun.Length       ?? "";
            nudStok.Value       = Math.Max(0, Math.Min(999999, _urun.Stok));
            txtCustomer.Text    = _urun.Customer     ?? "";
            txtCertificate.Text = _urun.Certificate  ?? "";
            txtBatch.Text       = _urun.Batch        ?? "";
            txtHeat.Text        = _urun.Heat         ?? "";
            txtParent.Text      = _urun.Parent       ?? "";
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrunCinsi.Text))
            {
                MessageBox.Show(
                    LangManager.Ingilizce ? "Product Name cannot be empty." : LangManager.T("genel.uyari"),
                    LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _urun.UrunCinsi    = txtUrunCinsi.Text.Trim();
            _urun.Material     = txtMaterial.Text.Trim();
            _urun.Grade        = txtGrade.Text.Trim();
            _urun.Thickness    = txtThickness.Text.Trim();
            _urun.Width        = txtWidth.Text.Trim();
            _urun.Length       = txtLength.Text.Trim();

            int eskiStok = _urun.Stok;
            int yeniStok = (int)nudStok.Value;
            _urun.Stok         = yeniStok;

            _urun.Customer     = txtCustomer.Text.Trim();
            _urun.Certificate  = txtCertificate.Text.Trim();
            _urun.Batch        = txtBatch.Text.Trim();
            _urun.Heat         = txtHeat.Text.Trim();
            _urun.Parent       = txtParent.Text.Trim();

            // PDF: değiştirildiyse veya silindiyse güncelle
            _urun.SertifikaPdf      = _yeniPdfData;
            _urun.SertifikaDosyaAdi = _yeniPdfAdi;

            StokVeritabani.UrunGuncelle(_urun);
            StokVeritabani.HareketKaydet(_urun.Id, _urun.UrunCinsi, eskiStok, yeniStok);
            Guncellendi = true;
            this.Close();
        }
    }
}
