using System;
using System.Data;
using System.Windows.Forms;

namespace StokTakipUI
{
    public partial class FormDashboard : Form
    {
        public FormDashboard()
        {
            InitializeComponent();
        }

        private void FormDashboard_Load(object sender, EventArgs e)
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
            // İstatistik sayıları
            try
            {
                label1.Text = StokVeritabani.ToplamUrun().ToString();
                label4.Text = StokVeritabani.KritikStokSayisi().ToString();
                label6.Text = StokVeritabani.FirmaSayisi().ToString();
            }
            catch
            {
                label1.Text = "—";
                label4.Text = "—";
                label6.Text = "—";
            }

            // Kart başlıkları
            label2.Text = LangManager.T("dash.toplamUrun");
            label3.Text = LangManager.T("dash.kritikStok");
            label5.Text = LangManager.T("dash.firmaSayisi");
            label7.Text = LangManager.Ingilizce ? "Recently Added Products" : "Son Eklenen Ürünler";

            // Kolon başlıkları (DataPropertyName ile eşleşmeli)
            colFirma.HeaderText    = "Customer";
            colMaterial.HeaderText = "Material Sp.";
            colGrade.HeaderText    = "Grade";
            colThk.HeaderText      = "THK(mm)";
            colWidth.HeaderText    = "Width(mm)";
            colLength.HeaderText   = "Length(mm)";
            colStok.HeaderText     = LangManager.T("urunler.col.stok");
            colDate.HeaderText     = LangManager.T("dash.tablo.tarih");

            TabloyuDoldur();
        }

        private void TabloyuDoldur()
        {
            // DataPropertyName bağlantıları — kolon adı ile DataTable kolon adı eşleşmeli
            dgvRecent.AutoGenerateColumns = false;
            colFirma.DataPropertyName    = "Customer";
            colMaterial.DataPropertyName = "Material";
            colGrade.DataPropertyName    = "Grade";
            colThk.DataPropertyName      = "Thickness";    // ÖNEMLİ: eskiden eksikti
            colWidth.DataPropertyName    = "Width";         // ÖNEMLİ: eskiden eksikti
            colLength.DataPropertyName   = "Length";        // ÖNEMLİ: eskiden eksikti
            colStok.DataPropertyName     = "Stok";          // ÖNEMLİ: eskiden eksikti
            colDate.DataPropertyName     = "EklenmeTarihi";

            // DataTable — tüm gerekli kolonları ekle
            var dt = new DataTable();
            dt.Columns.Add("Customer");
            dt.Columns.Add("Material");
            dt.Columns.Add("Grade");
            dt.Columns.Add("Thickness");
            dt.Columns.Add("Width");
            dt.Columns.Add("Length");
            dt.Columns.Add("Stok",          typeof(int));
            dt.Columns.Add("EklenmeTarihi");

            try
            {
                var urunler = StokVeritabani.TumUrunler();
                int sayac = 0;
                foreach (var u in urunler)
                {
                    if (sayac >= 10) break;
                    dt.Rows.Add(
                        u.Customer,
                        u.Material,
                        u.Grade,
                        u.Thickness,
                        u.Width,
                        u.Length,
                        u.Stok,
                        u.EklenmeTarihi
                    );
                    sayac++;
                }
            }
            catch { /* Bağlantı hatası — boş tablo göster */ }

            dgvRecent.DataSource = dt;
        }

        private void dgvRecent_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}
