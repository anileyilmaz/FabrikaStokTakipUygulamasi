using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    public partial class FormUrunler : Form
    {
        public FormUrunler()
        {
            InitializeComponent();

            btnDetail.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Dokuman, btnDetail.ClientRectangle, System.Drawing.Color.White);
            btnEdit.Paint   += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Duzenle, btnEdit.ClientRectangle,   System.Drawing.Color.Black);
            btnDelete.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Sil,      btnDelete.ClientRectangle, System.Drawing.Color.White);
            btnExcel.Paint  += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.DisaAktar, btnExcel.ClientRectangle, System.Drawing.Color.White);
        }

        private void FormUrunler_Load(object sender, EventArgs e)
        {
            UrunleriYukle();
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
            btnDetail.Text = LangManager.T("btn.detay");
            btnEdit.Text   = LangManager.T("btn.duzenle");
            btnDelete.Text = LangManager.T("btn.sil");
            btnExcel.Text  = LangManager.T("btn.excel");
            UrunKolonlariGuncelle();
        }

        private void UrunKolonlariGuncelle()
        {
            foreach (DataGridViewColumn col in dgvProducts.Columns)
            {
                switch (col.Name)
                {
                    case "colFirma":       col.HeaderText = "Customer";        break;
                    case "colCertificate": col.HeaderText = "Certificate No";  break;
                    case "colMaterial":    col.HeaderText = "Material Sp.";    break;
                    case "colBatch":       col.HeaderText = "Batch No";        break;
                    case "colParent":      col.HeaderText = "Parent Coil No";  break;
                    case "colHeat":        col.HeaderText = "Heat No";         break;
                    case "colGrade":       col.HeaderText = "Grade";           break;
                    case "colThickness":   col.HeaderText = "THK(mm)";         break;
                    case "colWidth":       col.HeaderText = "Width(mm)";       break;
                    case "colLength":      col.HeaderText = "Length(mm)";      break;
                    case "colStok":        col.HeaderText = LangManager.T("urunler.col.stok"); break;
                    case "colTarih":       col.HeaderText = LangManager.T("urunler.col.tarih");break;
                }
            }
        }

        private void UrunleriYukle()
        {
            dgvProducts.DataSource = null;
            dgvProducts.Rows.Clear();

            foreach (var u in StokVeritabani.TumUrunler())
            {
                dgvProducts.Rows.Add(
                    u.Customer,
                    u.Certificate,
                    u.Material,
                    u.Batch,
                    u.Parent,
                    u.Heat,
                    u.Grade,
                    u.Thickness,
                    u.Width,
                    u.Length,
                    u.Stok,
                    u.EklenmeTarihi
                );

                // Id'yi Tag olarak sakla (son eklenen satıra)
                dgvProducts.Rows[dgvProducts.Rows.Count - 1].Tag = u.Id;
            }

            lblTotalProduct.Text = LangManager.T("urunler.toplam") + dgvProducts.Rows.Count;
            dgvProducts.ClearSelection();
            dgvProducts.CurrentCell = null;
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvProducts.ClearSelection();
            dgvProducts.CurrentCell = null;
        }

        // ─── Seçili ürünü Urun nesnesine dönüştür ───────────────────────────
        private Urun SecilenUruneGetir()
        {
            if (dgvProducts.SelectedRows.Count == 0) return null;

            var row = dgvProducts.SelectedRows[0];
            int id = row.Tag is int tagId ? tagId : 0;

            foreach (var u in StokVeritabani.TumUrunler())
                if (u.Id == id) return u;

            return null;
        }

        // ─── Ürün Detay ──────────────────────────────────────────────────────
        private void btnDetail_Click(object sender, EventArgs e)
        {
            var urun = SecilenUruneGetir();
            if (urun == null)
            {
                MessageBox.Show(LangManager.T("urunler.sec"), LangManager.T("genel.uyari"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var frm = new FormUrunDetay(urun))
                frm.ShowDialog(this);
        }

        // ─── Düzenle ─────────────────────────────────────────────────────────
        private void btnEdit_Click(object sender, EventArgs e)
        {
            var urun = SecilenUruneGetir();
            if (urun == null)
            {
                MessageBox.Show(LangManager.T("urunler.sec"), LangManager.T("genel.uyari"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var frm = new FormUrunDuzenle(urun))
            {
                frm.ShowDialog(this);
                if (frm.Guncellendi)
                    UrunleriYukle();
            }
        }

        // ─── Sil ─────────────────────────────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var urun = SecilenUruneGetir();
            if (urun == null)
            {
                MessageBox.Show(LangManager.T("urunler.sec"), LangManager.T("genel.uyari"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var frm = new FormSilOnay(urun.UrunCinsi))
            {
                frm.ShowDialog(this);
                if (frm.Onaylandi)
                {
                    // Silme hareketini kaydet (önce — ürün silinmeden)
                    StokVeritabani.HareketKaydet(
                        urun.Id,
                        string.IsNullOrEmpty(urun.UrunCinsi) ? urun.Material : urun.UrunCinsi,
                        urun.Stok,
                        0,
                        LangManager.Ingilizce ? "Product Deleted" : "Ürün Silindi");
                    StokVeritabani.UrunSil(urun.Id);
                    UrunleriYukle();
                }
            }
        }

        // ─── Excel Aktar ─────────────────────────────────────────────────────
        private void btnExcel_Click(object sender, EventArgs e)
        {
            var urunler = StokVeritabani.TumUrunler();
            if (urunler.Count == 0)
            {
                MessageBox.Show(LangManager.Ingilizce ? "No products to export." : "Aktarılacak ürün bulunamadı.", LangManager.Ingilizce ? "Info" : "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title    = LangManager.Ingilizce ? "Save as Excel" : "Excel Olarak Kaydet";
                sfd.Filter   = "Excel Dosyası (*.csv)|*.csv|Tüm Dosyalar (*.*)|*.*";
                sfd.FileName = "Urunler_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var sb = new StringBuilder();

                    // Başlık satırı
                    sb.AppendLine("Customer;Certificate No;Material Sp.;Batch No;Parent Coil No;Heat No;Grade;THK(mm);Width(mm);Length(mm);Ürün Adedi;Eklenme Tarihi");

                    foreach (var u in urunler)
                    {
                        sb.AppendLine(string.Join(";", new[]
                        {
                            MetneSarla(u.Customer),
                            MetneSarla(u.Certificate),
                            MetneSarla(u.Material),
                            MetneSarla(u.Batch),
                            MetneSarla(u.Parent),
                            MetneSarla(u.Heat),
                            MetneSarla(u.Grade),
                            MetneSarla(u.Thickness),
                            MetneSarla(u.Width),
                            MetneSarla(u.Length),
                            u.Stok.ToString(),
                            MetneSarla(u.EklenmeTarihi)
                        }));
                    }

                    // UTF-8 BOM ile yaz (Excel'de Türkçe karakter desteği)
                    File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));

                    MessageBox.Show(
                        $"{urunler.Count} ürün başarıyla aktarıldı.\n\n{sfd.FileName}",
                        "Excel Aktarım Başarılı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya kaydedilirken hata oluştu:\n" + ex.Message,
                        LangManager.T("genel.hata"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string CsvKelimesi(string deger)
        {
            if (string.IsNullOrEmpty(deger)) return "";
            if (deger.Contains(";") || deger.Contains("\"") || deger.Contains("\n"))
                return "\"" + deger.Replace("\"", "\"\"") + "\"";
            return deger;
        }

        /// <summary>
        /// Değeri çift tırnak içine alarak Excel'e metin olarak gönderir.
        /// Sayısal ve tarih formatındaki değerlerin otomatik parse edilmesini engeller.
        /// </summary>
        private static string MetneSarla(string deger)
        {
            if (string.IsNullOrEmpty(deger)) return "\"\"";
            // İçindeki çift tırnak varsa iki tırnak yap (CSV standardı)
            return "\"" + deger.Replace("\"", "\"\"") + "\"";
        }
    }
}
