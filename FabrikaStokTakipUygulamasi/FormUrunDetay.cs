using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    public class FormUrunDetay : Form
    {
        private static readonly Color CNavy    = UIStil.Lacivert;
        private static readonly Color CLightBg = UIStil.GriAcik;
        private static readonly Color CWhite   = UIStil.Beyaz;
        private static readonly Color CBorder  = UIStil.GriOrta;

        private readonly Urun _urun;

        public FormUrunDetay(Urun urun)
        {
            _urun = urun;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text            = LangManager.T("detay.baslik");
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = CLightBg;
            this.Font            = new Font("Segoe UI", 10f);

            // PDF var mı? Varsa PDF butonu için ekstra yükseklik
            bool pdfVar = _urun.SertifikaPdf != null && _urun.SertifikaPdf.Length > 0;
            this.ClientSize = new Size(640, pdfVar ? 900 : 840);

            // ── Başlık ────────────────────────────────────────────────────────
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 75, BackColor = CNavy };
            panelHeader.Controls.Add(new Label
            {
                Text = LangManager.T("detay.baslik"),
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = CWhite, AutoSize = true, Location = new Point(22, 16)
            });
            panelHeader.Controls.Add(new Label
            {
                Text = _urun.UrunCinsi,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(189, 195, 199),
                AutoSize = true, Location = new Point(24, 48)
            });

            // ── Bilgi Kartı ───────────────────────────────────────────────────
            var panelCard = new Panel
            {
                BackColor = CWhite,
                Location  = new Point(22, 90),
                Size      = new Size(596, 630)
            };
            panelCard.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(CBorder),
                    new Rectangle(0, 0, panelCard.Width - 1, panelCard.Height - 1));

            var fields = new[]
            {
                ("Ürün Cinsi",    _urun.UrunCinsi),
                ("Material",      _urun.Material),
                ("Grade",         _urun.Grade),
                ("Thickness",     _urun.Thickness),
                ("Width",         _urun.Width),
                ("Length",        _urun.Length),
                ("Stok",          _urun.Stok.ToString()),
                ("Customer",      _urun.Customer),
                ("Certificate",   _urun.Certificate),
                ("Batch",         _urun.Batch),
                ("Heat",          _urun.Heat),
                ("Parent",        _urun.Parent),
                ("Eklenme Tarihi",_urun.EklenmeTarihi),
            };

            int y = 18;
            foreach (var (label, value) in fields)
            {
                panelCard.Controls.Add(new Label
                {
                    Text = label, Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(127, 140, 141),
                    Location = new Point(18, y), Size = new Size(140, 22),
                    TextAlign = ContentAlignment.MiddleLeft
                });

                if (label == "Stok")
                {
                    int stok  = _urun.Stok;
                    int limit = _urun.LowStockLimit >= 0 ? _urun.LowStockLimit : 5;
                    Color badgeColor = stok <= limit
                        ? Color.FromArgb(231, 76, 60)
                        : stok <= limit + 10
                            ? Color.FromArgb(243, 156, 18)
                            : Color.FromArgb(39, 174, 96);
                    var badge = new Panel { Location = new Point(170, y - 1), Size = new Size(70, 24), BackColor = badgeColor };
                    badge.Controls.Add(new Label
                    {
                        Text = stok.ToString(), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                        ForeColor = CWhite, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter
                    });
                    panelCard.Controls.Add(badge);
                }
                else
                {
                    panelCard.Controls.Add(new Label
                    {
                        Text = string.IsNullOrEmpty(value) ? "—" : value,
                        Font = new Font("Segoe UI", 10f),
                        ForeColor = string.IsNullOrEmpty(value) ? Color.FromArgb(189, 195, 199) : CNavy,
                        Location = new Point(170, y), Size = new Size(410, 22),
                        TextAlign = ContentAlignment.MiddleLeft
                    });
                }

                if (label != "Eklenme Tarihi")
                    panelCard.Controls.Add(new Panel
                    {
                        BackColor = Color.FromArgb(236, 240, 241),
                        Location  = new Point(0, y + 29), Size = new Size(596, 1)
                    });

                y += 46;
            }

            // ── PDF Sertifika Paneli ──────────────────────────────────────────
            var panelPdf = new Panel
            {
                BackColor = CWhite,
                Location  = new Point(22, 734),
                Size      = new Size(596, pdfVar ? 66 : 46),
                BorderStyle = BorderStyle.None
            };
            panelPdf.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(CBorder),
                    new Rectangle(0, 0, panelPdf.Width - 1, panelPdf.Height - 1));

            if (pdfVar)
            {
                var btnPdf = new Button
                {
                    Text      = $"{LangManager.T("detay.sertifika")}  —  {_urun.SertifikaDosyaAdi}",
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = CWhite,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                    Location  = new Point(10, 10),
                    Size      = new Size(574, 46),
                    Padding   = new Padding(34, 0, 0, 0),
                    Cursor    = Cursors.Hand,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                btnPdf.FlatAppearance.BorderSize = 0;
                btnPdf.Click += (s, e) => PdfAc(_urun.SertifikaPdf, _urun.SertifikaDosyaAdi);
                btnPdf.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Dokuman, btnPdf.ClientRectangle, CWhite, 12f);
                panelPdf.Controls.Add(btnPdf);
            }
            else
            {
                var ikon = UIStil.IkonLabel(UIStil.Glyph.Dokuman, Color.FromArgb(149, 165, 166), 10f);
                ikon.Location = new Point(14, 13);
                panelPdf.Controls.Add(ikon);

                panelPdf.Controls.Add(new Label
                {
                    Text      = LangManager.T("detay.sertifikayok"),
                    Font      = new Font("Segoe UI", 9f),
                    ForeColor = Color.FromArgb(149, 165, 166),
                    Location  = new Point(36, 12),
                    AutoSize  = true
                });
            }

            // ── Kapat Butonu ──────────────────────────────────────────────────
            int kapatY = pdfVar ? 820 : 790;
            var btnKapat = new Button
            {
                Text      = LangManager.T("detay.kapat"),
                BackColor = CNavy,
                ForeColor = CWhite,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                Size      = new Size(596, 46),
                Location  = new Point(22, kapatY),
                Cursor    = Cursors.Hand
            };
            btnKapat.FlatAppearance.BorderSize = 0;
            btnKapat.Click += (s, e) => this.Close();

            this.Controls.Add(panelHeader);
            this.Controls.Add(panelCard);
            this.Controls.Add(panelPdf);
            this.Controls.Add(btnKapat);
        }

        /// <summary>PDF'i geçici klasöre kaydedip sistemin varsayılan görüntüleyicisiyle açar.</summary>
        private void PdfAc(byte[] pdfData, string dosyaAdi)
        {
            try
            {
                string geciciDizin = Path.Combine(Path.GetTempPath(), "StokTakipPDF");
                Directory.CreateDirectory(geciciDizin);

                // Güvenli dosya adı
                string guvenliAd = string.IsNullOrWhiteSpace(dosyaAdi) ? "sertifika.pdf"
                    : Path.GetFileName(dosyaAdi);
                string dosyaYolu = Path.Combine(geciciDizin, guvenliAd);

                File.WriteAllBytes(dosyaYolu, pdfData);

                // Sistemin varsayılan PDF okuyucusuyla aç (Adobe, Edge, vb.)
                Process.Start(new ProcessStartInfo(dosyaYolu) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(LangManager.T("detay.pdfhata") + ":\n" + ex.Message,
                    LangManager.T("genel.hata"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
