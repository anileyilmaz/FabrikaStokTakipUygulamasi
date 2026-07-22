using System;
using System.Drawing;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    public class FormSilOnay : Form
    {
        private static readonly Color CNavy   = Color.FromArgb(44, 62, 80);
        private static readonly Color CRed    = Color.FromArgb(192, 57, 43);
        private static readonly Color CLightBg= Color.FromArgb(236, 240, 241);
        private static readonly Color CWhite  = Color.White;

        public bool Onaylandi { get; private set; } = false;

        public FormSilOnay(string urunAdi)
        {
            BuildUI(urunAdi);
        }

        private void BuildUI(string urunAdi)
        {
            this.Text = LangManager.Ingilizce ? "Delete Confirmation" : "Silme Onayı";
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = CLightBg;
            this.ClientSize      = new Size(420, 260);
            this.Font            = new Font("Segoe UI", 9.5f);

            // Üst kırmızı şerit
            var panelStripe = new Panel
            {
                BackColor = CRed,
                Location  = new Point(0, 0),
                Size      = new Size(420, 6)
            };

            // Uyarı ikonu
            var lblIcon = new Label
            {
                Text      = "⚠",
                Font      = new Font("Segoe UI", 34f),
                ForeColor = CRed,
                AutoSize  = true
            };
            lblIcon.Location = new Point((this.ClientSize.Width - lblIcon.PreferredWidth) / 2, 18);

            // Ana mesaj
            var lblMesaj = new Label
            {
                Text      = LangManager.T("silonay.mesaj"),
                Font      = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = CNavy,
                TextAlign = ContentAlignment.MiddleCenter,
                Size      = new Size(380, 54),
                Location  = new Point(20, 88)
            };

            // Ürün adı
            var lblUrun = new Label
            {
                Text      = urunAdi,
                Font      = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(127, 140, 141),
                TextAlign = ContentAlignment.MiddleCenter,
                Size      = new Size(380, 22),
                Location  = new Point(20, 144)
            };

            // Evet butonu
            var btnEvet = new Button
            {
                Text      = LangManager.T("silonay.evet"),
                BackColor = CRed,
                ForeColor = CWhite,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Size      = new Size(180, 46),
                Location  = new Point(20, 192),
                Cursor    = Cursors.Hand
            };
            btnEvet.FlatAppearance.BorderSize = 0;
            btnEvet.Click += (s, e) => { Onaylandi = true; this.Close(); };

            // Hayır butonu
            var btnHayir = new Button
            {
                Text      = LangManager.T("silonay.hayir"),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = CWhite,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Size      = new Size(180, 46),
                Location  = new Point(220, 192),
                Cursor    = Cursors.Hand
            };
            btnHayir.FlatAppearance.BorderSize = 0;
            btnHayir.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[]
            {
                panelStripe, lblIcon, lblMesaj, lblUrun, btnEvet, btnHayir
            });
        }
    }
}
