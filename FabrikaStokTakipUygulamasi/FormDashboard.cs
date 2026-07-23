using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FabrikaStokTakipUygulamasi
{
    public partial class FormDashboard : Form
    {
        public FormDashboard()
        {
            InitializeComponent();

            foreach (var panel in new[] { panelTotal, panelCritical, panelCompany })
                UIStil.YuvarlakBolgeUygula(panel, 14);

            UIStil.YuvarlakBolgeUygula(chartStokDagilimi, 10);
            UIStil.YuvarlakBolgeUygula(chartSonUrunler, 10);

            this.Paint += (s, e) =>
            {
                UIStil.YumusakGolgeCiz(e.Graphics, panelTotal.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, panelCritical.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, panelCompany.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, chartStokDagilimi.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, chartSonUrunler.Bounds);
            };
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
            int toplam = 0, kritik = 0;
            try
            {
                toplam = StokVeritabani.ToplamUrun();
                kritik = StokVeritabani.KritikStokSayisi();
                label1.Text = toplam.ToString();
                label4.Text = kritik.ToString();
                label6.Text = StokVeritabani.FirmaSayisi().ToString();
            }
            catch
            {
                label1.Text = "—";
                label4.Text = "—";
                label6.Text = "—";
            }

            GrafikGuncelle(toplam, kritik);
            SonUrunlerGrafiguGuncelle();

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

        private void GrafikGuncelle(int toplam, int kritik)
        {
            int normal = System.Math.Max(0, toplam - kritik);

            var seri = chartStokDagilimi.Series["Seri1"];
            seri.Points.Clear();
            seri.ChartType = SeriesChartType.Doughnut;

            if (toplam <= 0)
            {
                seri.Points.AddXY(LangManager.Ingilizce ? "No data" : "Veri yok", 1);
                seri.Points[0].Color = System.Drawing.Color.FromArgb(220, 220, 220);
                seri.Points[0].LegendText = LangManager.Ingilizce ? "No data" : "Veri yok";
                return;
            }

            int kritikIdx = seri.Points.AddXY(LangManager.Ingilizce ? "Critical" : "Kritik", kritik);
            seri.Points[kritikIdx].Color = System.Drawing.Color.FromArgb(192, 57, 43);
            seri.Points[kritikIdx].LegendText = (LangManager.Ingilizce ? "Critical: " : "Kritik: ") + kritik;

            int normalIdx = seri.Points.AddXY(LangManager.Ingilizce ? "Normal" : "Normal", normal);
            seri.Points[normalIdx].Color = System.Drawing.Color.FromArgb(41, 128, 185);
            seri.Points[normalIdx].LegendText = (LangManager.Ingilizce ? "Normal: " : "Normal: ") + normal;
        }

        private void SonUrunlerGrafiguGuncelle()
        {
            var seri = chartSonUrunler.Series["Seri2"];
            seri.Points.Clear();

            List<Urun> urunler;
            try { urunler = StokVeritabani.TumUrunler(); }
            catch { return; }

            foreach (var u in urunler.Take(10))
            {
                string etiket = string.IsNullOrWhiteSpace(u.UrunCinsi) ? ("#" + u.Id) : u.UrunCinsi;
                if (etiket.Length > 10) etiket = etiket.Substring(0, 10) + "…";
                seri.Points.AddXY(etiket, u.Stok);
            }
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
