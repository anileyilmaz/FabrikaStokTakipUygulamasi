using System;
using System.Drawing;
using System.Windows.Forms;

namespace StokTakipUI
{
    public partial class FormLowStock : Form
    {
        public FormLowStock()
        {
            InitializeComponent();
        }

        private void FormLowStock_Load(object sender, EventArgs e)
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
            btnYeni.Text          = LangManager.T("lowstock.yeni");
            btnDuzenle.Text       = LangManager.T("lowstock.duzenle");
            lblcriticalstock.Text = LangManager.Ingilizce ? "Critical Stock Tracking" : "Kritik Stok Takibi";
            lblCriticalCount.Text = LangManager.T("ls.kritik") + "0";
            colCustomer.HeaderText   = "Customer";
            colCert.HeaderText       = "Certificate No";
            colMaterial.HeaderText   = "Material Sp.";
            colBatch.HeaderText      = "Batch No";
            colParent.HeaderText     = "Parent Coil No";
            colHeat.HeaderText       = "Heat No";
            colGrade.HeaderText      = "Grade";
            colThickness.HeaderText  = "THK(mm)";
            colWidth.HeaderText      = "Width(mm)";
            colLength.HeaderText     = "Length(mm)";
            colStock.HeaderText      = LangManager.T("ls.col.stok");
            colLowLimit.HeaderText   = LangManager.T("ls.col.limit");
            colStatus.HeaderText     = LangManager.T("ls.col.durum");
            YukleVeRenklendir();
        }

        public void YukleVeRenklendir()
        {
            dgvLowStock.DataSource = null;
            dgvLowStock.Rows.Clear();

            foreach (var u in StokVeritabani.TumUrunler())
            {
                int limit = u.LowStockLimit >= 0 ? u.LowStockLimit : 5;
                string durum;
                if      (u.Stok <= limit)       durum = LangManager.T("ls.durum.kritik");
                else if (u.Stok <= limit + 10)  durum = LangManager.T("ls.durum.azaliyor");
                else                            durum = LangManager.T("ls.durum.normal");

                string limitGoster = u.LowStockLimit >= 0 ? u.LowStockLimit.ToString() : "—";

                int idx = dgvLowStock.Rows.Add(
                    u.Customer,    // Customer
                    u.Certificate, // Certificate No
                    u.Material,    // Material Sp.
                    u.Batch,       // Batch No
                    u.Parent,      // Parent Coil No
                    u.Heat,        // Heat No
                    u.Grade,       // Grade
                    u.Thickness,   // THK(mm)
                    u.Width,       // Width(mm)
                    u.Length,      // Length(mm)
                    u.Stok,        // Ürün Adedi
                    limitGoster,   // Limit
                    durum          // Durum
                );
                dgvLowStock.Rows[idx].Tag = u.Id;
            }

            Renklendir();
            dgvLowStock.ClearSelection();
            dgvLowStock.CurrentCell = null;

            int kritikAdet = StokVeritabani.KritikStokSayisi();
            lblCriticalCount.Text = LangManager.T("ls.kritik") + kritikAdet;
        }

        private void Renklendir()
        {
            Color kirmizi     = Color.FromArgb(255, 205, 210);
            Color turuncu     = Color.FromArgb(255, 236, 179);
            Color yesil       = Color.FromArgb(200, 230, 201);
            Color kirmizYazi  = Color.FromArgb(183, 28, 28);
            Color turuncuYazi = Color.FromArgb(230, 81, 0);
            Color yesilYazi   = Color.FromArgb(27, 94, 32);

            foreach (DataGridViewRow row in dgvLowStock.Rows)
            {
                string durum = row.Cells["colStatus"].Value?.ToString() ?? "";
                if (durum == LangManager.T("ls.durum.kritik"))
                {
                    row.DefaultCellStyle.BackColor          = kirmizi;
                    row.DefaultCellStyle.ForeColor          = kirmizYazi;
                    row.DefaultCellStyle.SelectionBackColor = kirmizi;
                    row.DefaultCellStyle.SelectionForeColor = kirmizYazi;
                }
                else if (durum == LangManager.T("ls.durum.azaliyor"))
                {
                    row.DefaultCellStyle.BackColor          = turuncu;
                    row.DefaultCellStyle.ForeColor          = turuncuYazi;
                    row.DefaultCellStyle.SelectionBackColor = turuncu;
                    row.DefaultCellStyle.SelectionForeColor = turuncuYazi;
                }
                else
                {
                    row.DefaultCellStyle.BackColor          = yesil;
                    row.DefaultCellStyle.ForeColor          = yesilYazi;
                    row.DefaultCellStyle.SelectionBackColor = yesil;
                    row.DefaultCellStyle.SelectionForeColor = yesilYazi;
                }
            }
        }

        // Yeni butonu → popup aç
        private void btnYeni_Click(object sender, EventArgs e)
        {
            using (var frm = new FormLowStockSecim("yeni"))
            {
                frm.ShowDialog(this);
                if (frm.Degisiklik)
                    YukleVeRenklendir();
            }
        }

        // Düzenle butonu → popup aç
        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            using (var frm = new FormLowStockSecim("duzenle"))
            {
                frm.ShowDialog(this);
                if (frm.Degisiklik)
                    YukleVeRenklendir();
            }
        }

        // Ana listede tıklama → seçimi hemen temizle (sadece görüntüleme)
        private void dgvLowStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvLowStock.ClearSelection();
            dgvLowStock.CurrentCell = null;
        }

        // Ana listede çift tıklama da etkisiz
        private void dgvLowStock_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kasıtlı boş — ana liste sadece görüntüleme
        }
    }
}
