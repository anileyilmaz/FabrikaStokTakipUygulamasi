using System;
using System.Drawing;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    /// <summary>
    /// İlk çalıştırmada (bağlantı yapılandırma dosyası yoksa) gösterilen,
    /// fabrika içi paylaşımlı PostgreSQL sunucusu bilgilerini toplayan diyalog.
    /// </summary>
    public class FormSunucuAyarlari : Form
    {
        private TextBox txtSunucu, txtVeritabani, txtKullaniciAdi;
        private TextBox txtSifre;
        private NumericUpDown nudPort;

        public FormSunucuAyarlari()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text            = "Sunucu Bağlantı Ayarları";
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = UIStil.GriAcik;
            this.ClientSize      = new Size(420, 444);
            this.Font            = new Font("Segoe UI", 9.5f);
            this.AutoScaleMode   = AutoScaleMode.Font;

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = UIStil.LacivertKoyu };
            panelHeader.Controls.Add(new Label
            {
                Text = "Fabrika Sunucu Bağlantısı",
                Font = UIStil.Baslik(14f),
                ForeColor = UIStil.Beyaz, AutoSize = true, Location = new Point(20, 14)
            });
            panelHeader.Controls.Add(new Label
            {
                Text = "Bu bilgisayarın hangi PostgreSQL sunucusuna bağlanacağını girin.",
                Font = UIStil.AltBaslik(8.5f),
                ForeColor = Color.FromArgb(189, 195, 199), AutoSize = true, Location = new Point(22, 42)
            });

            int y = 90;
            Label Etiket(string t) => new Label
            {
                Text = t, Location = new Point(20, y), AutoSize = true,
                Font = UIStil.Etiket(9f), ForeColor = UIStil.GriMetin
            };

            this.Controls.Add(Etiket("Sunucu (IP veya ad):"));
            txtSunucu = new TextBox { Location = new Point(20, y + 20), Size = new Size(380, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtSunucu);
            y += 56;

            this.Controls.Add(Etiket("Port:"));
            nudPort = new NumericUpDown { Location = new Point(20, y + 20), Size = new Size(120, 26), Minimum = 1, Maximum = 65535, Value = 5432, BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(nudPort);
            y += 56;

            this.Controls.Add(Etiket("Veritabanı Adı:"));
            txtVeritabani = new TextBox { Location = new Point(20, y + 20), Size = new Size(380, 26), Text = "fabrikastok", BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtVeritabani);
            y += 56;

            this.Controls.Add(Etiket("Kullanıcı Adı:"));
            txtKullaniciAdi = new TextBox { Location = new Point(20, y + 20), Size = new Size(380, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtKullaniciAdi);
            y += 56;

            this.Controls.Add(Etiket("Şifre:"));
            txtSifre = new TextBox { Location = new Point(20, y + 20), Size = new Size(380, 26), BorderStyle = BorderStyle.FixedSingle, UseSystemPasswordChar = true };
            this.Controls.Add(txtSifre);

            var btnKaydet = new Button
            {
                Text = "Kaydet ve Devam Et", BackColor = UIStil.Mavi, ForeColor = UIStil.Beyaz,
                FlatStyle = FlatStyle.Flat, Font = UIStil.ButonYazi(10f),
                Size = new Size(380, 44), Location = new Point(20, 380), Cursor = Cursors.Hand
            };
            btnKaydet.FlatAppearance.BorderSize = 0;
            btnKaydet.Click += BtnKaydet_Click;
            this.Controls.Add(btnKaydet);

            this.Controls.Add(panelHeader);
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSunucu.Text) ||
                string.IsNullOrWhiteSpace(txtVeritabani.Text) ||
                string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) ||
                string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BaglantiAyarlari.Kaydet(
                txtSunucu.Text.Trim(),
                (int)nudPort.Value,
                txtVeritabani.Text.Trim(),
                txtKullaniciAdi.Text.Trim(),
                txtSifre.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
