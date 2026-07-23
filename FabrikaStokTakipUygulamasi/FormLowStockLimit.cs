using System;
using System.Drawing;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    public class FormLowStockLimit : Form
    {
        private static readonly Color CNavy   = UIStil.Lacivert;
        private static readonly Color CAccent = UIStil.Aksan;
        private static readonly Color CLightBg= UIStil.GriAcik;
        private static readonly Color CWhite  = UIStil.Beyaz;
        private static readonly Color CBorder = UIStil.GriOrta;

        private readonly Urun _urun;
        private readonly bool _duzenle;
        private NumericUpDown nudLimit;

        public bool Kaydedildi { get; private set; } = false;

        public FormLowStockLimit(Urun urun, bool duzenle)
        {
            _urun    = urun;
            _duzenle = duzenle;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text            = LangManager.T(_duzenle ? "ls.limit.form.duz" : "ls.limit.form.yeni");
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = CLightBg;
            this.Size            = new Size(400, 310);
            this.Font            = new Font("Segoe UI", 9.5f);

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = CNavy };
            panelHeader.Controls.Add(new Label
            {
                Text      = LangManager.T(_duzenle ? "ls.limit.baslik.duz" : "ls.limit.baslik.yeni"),
                Font      = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = CWhite, AutoSize = true, Location = new Point(16, 14)
            });
            panelHeader.Controls.Add(new Label
            {
                Text      = _urun.UrunCinsi + " — " + LangManager.T("ls.gstok") + _urun.Stok,
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(189, 195, 199),
                AutoSize  = true, Location = new Point(18, 40)
            });

            var panelCard = new Panel
            {
                BackColor = CWhite, Location = new Point(20, 80), Size = new Size(342, 120)
            };
            panelCard.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(CBorder),
                    new Rectangle(0, 0, panelCard.Width - 1, panelCard.Height - 1));

            panelCard.Controls.Add(new Label
            {
                Text      = LangManager.T("ls.limit.aciklama"),
                Font      = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location  = new Point(16, 16), AutoSize = true
            });

            nudLimit = new NumericUpDown
            {
                Location    = new Point(16, 44), Size = new Size(120, 28),
                Font        = new Font("Segoe UI", 11f, FontStyle.Bold),
                Minimum     = 0, Maximum = 999999,
                Value       = _duzenle && _urun.LowStockLimit >= 0 ? _urun.LowStockLimit : 5,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor   = Color.FromArgb(248, 249, 250)
            };
            panelCard.Controls.Add(nudLimit);

            panelCard.Controls.Add(new Label
            {
                Text      = LangManager.T("ls.limit.adet"),
                Font      = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location  = new Point(145, 50), AutoSize = true
            });

            panelCard.Controls.Add(new Label
            {
                Text      = LangManager.T("ls.limit.bilgi"),
                Font      = new Font("Segoe UI", 8f),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location  = new Point(16, 82), Size = new Size(310, 34)
            });

            var btnKaydet = new Button
            {
                Text      = LangManager.T(_duzenle ? "ls.limit.guncelle" : "ls.limit.onayla"),
                BackColor = _duzenle ? CAccent : CNavy,
                ForeColor = _duzenle ? Color.Black : CWhite,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Size      = new Size(160, 42), Location = new Point(20, 218), Cursor = Cursors.Hand
            };
            btnKaydet.FlatAppearance.BorderSize = 0;
            btnKaydet.Click += (s, e) =>
            {
                StokVeritabani.LowStockLimitGuncelle(_urun.Id, (int)nudLimit.Value);
                Kaydedildi = true;
                this.Close();
            };

            var btnIptal = new Button
            {
                Text      = LangManager.T("genel.iptal"),
                BackColor = Color.FromArgb(149, 165, 166), ForeColor = CWhite,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Size      = new Size(160, 42), Location = new Point(202, 218), Cursor = Cursors.Hand
            };
            btnIptal.FlatAppearance.BorderSize = 0;
            btnIptal.Click += (s, e) => this.Close();

            this.Controls.Add(panelHeader);
            this.Controls.Add(panelCard);
            this.Controls.Add(btnKaydet);
            this.Controls.Add(btnIptal);

            UIStil.YuvarlakBolgeUygula(panelCard, 10);
            this.Paint += (s, e) => UIStil.YumusakGolgeCiz(e.Graphics, panelCard.Bounds);

            Color btnKaydetNormalRenk = _duzenle ? CAccent : CNavy;
            UIStil.HoverAnimasyonuBagla(btnKaydet, btnKaydetNormalRenk, Color.FromArgb(245, 180, 90), renk => btnKaydet.BackColor = renk);
        }
    }
}
