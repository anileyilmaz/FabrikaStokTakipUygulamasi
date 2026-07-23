using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    public partial class FormArama : Form
    {
        public FormArama()
        {
            InitializeComponent();

            btnAra.Paint     += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ara,   btnAra.ClientRectangle,     System.Drawing.Color.White, 10f);
            btnTemizle.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kapat, btnTemizle.ClientRectangle, System.Drawing.Color.White, 10f);

            UIStil.HoverAnimasyonuBagla(btnAra, UIStil.Mavi, UIStil.MaviAcik, renk => btnAra.BackColor = renk);
        }

        private void FormArama_Load(object sender, EventArgs e)
        {
            DildurumUygula();
            DoldurComboBoxlar();
            BaglaOlaylar();
            Ara();
            LangManager.DilDegisti += FormArama_DilDegisti;
        }

        private void BaglaOlaylar()
        {
            txtSearch.KeyDown += (s, ke) => { if (ke.KeyCode == Keys.Enter) Ara(); };
            cmbUrun.SelectedIndexChanged      += (s, ev) => Ara();
            cmbMaterial.SelectedIndexChanged  += (s, ev) => Ara();
            cmbGrade.SelectedIndexChanged     += (s, ev) => Ara();
            cmbThickness.SelectedIndexChanged += (s, ev) => Ara();
            cmbDiameter.SelectedIndexChanged  += (s, ev) => Ara();
            // Aralık inputları: Enter veya odak kaybında ara
            foreach (var txt in new[] { txtThicknessMin, txtThicknessMax,
                                         txtWidthMin, txtWidthMax,
                                         txtLengthMin, txtLengthMax,
                                         txtStockMin, txtStockMax })
            {
                txt.Leave += (s, ev) => Ara();
                txt.KeyDown += (s, ke) => { if (ke.KeyCode == Keys.Enter) Ara(); };
            }
        }

        private void FormArama_DilDegisti()
        {
            DildurumUygula();
            DoldurComboBoxlar();
            Ara();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            LangManager.DilDegisti -= FormArama_DilDegisti;
            base.OnFormClosed(e);
        }

        private void DildurumUygula()
        {
            btnAra.Text     = LangManager.Ingilizce ? "SEARCH" : "ARA";
            btnTemizle.Text = LangManager.Ingilizce ? "CLEAR"  : "TEMİZLE";

            // SearchType combobox
            int idx = cmbSearchType.SelectedIndex < 0 ? 0 : cmbSearchType.SelectedIndex;
            cmbSearchType.Items.Clear();
            cmbSearchType.Items.AddRange(LangManager.Ingilizce
                ? new object[] { "All", "Material", "Grade", "Customer", "Certificate" }
                : new object[] { "Tümü", "Material", "Grade", "Customer", "Certificate" });
            cmbSearchType.SelectedIndex = Math.Min(idx, cmbSearchType.Items.Count - 1);

            // Kolon başlıkları
            colFirma.HeaderText       = "Customer";
            colCertificate.HeaderText = "Certificate No";
            colMaterial.HeaderText    = "Material Sp.";
            colBatch.HeaderText       = "Batch No";
            colParent.HeaderText      = "Parent Coil No";
            colHeat.HeaderText        = "Heat No";
            colGrade.HeaderText       = "Grade";
            colThickness.HeaderText   = "THK(mm)";
            colWidth.HeaderText       = "Width(mm)";
            colLength.HeaderText      = "Length(mm)";
            colStock.HeaderText       = LangManager.Ingilizce ? "Quantity" : "Ürün Adedi";

            // Filtre etiketleri
            lblFilterCustomer.Text  = LangManager.Ingilizce ? "Customer"      : "Customer";
            lblFilterParent.Text    = LangManager.Ingilizce ? "Parent Coil No": "Parent Coil No";
            lblFilterBatch.Text     = LangManager.Ingilizce ? "Batch No"       : "Batch No";
            lblFilterGrade.Text     = LangManager.Ingilizce ? "Grade"          : "Grade";
            lblFilterHeat.Text      = LangManager.Ingilizce ? "Heat No"        : "Heat No";
            lblThicknessRange.Text  = "Thickness(mm)";
            lblWidth.Text           = "Width(mm)";
            lblLength.Text          = "Length(mm)";
            lblStock.Text           = LangManager.Ingilizce ? "Current Stock" : "Güncel Stok";
        }

        private void DoldurComboBoxlar()
        {
            var urunler = StokVeritabani.TumUrunler();

            // customer → Customer
            DoldurCmb(cmbUrun,      urunler.Select(u => u.Customer).Distinct().OrderBy(x => x));
            // parent coil no → Parent
            DoldurCmb(cmbDiameter,  urunler.Select(u => u.Parent).Distinct().OrderBy(x => x));
            // batch no → Batch
            DoldurCmb(cmbMaterial,  urunler.Select(u => u.Batch).Distinct().OrderBy(x => x));
            // grade → Grade
            DoldurCmb(cmbGrade,     urunler.Select(u => u.Grade).Distinct().OrderBy(x => x));
            // heat no → Heat
            DoldurCmb(cmbThickness, urunler.Select(u => u.Heat).Distinct().OrderBy(x => x));
        }

        private void DoldurCmb(ComboBox cmb, IEnumerable<string> degerler)
        {
            cmb.Items.Clear();
            cmb.Items.Add(LangManager.Ingilizce ? "All" : "Tümü");
            foreach (var d in degerler)
                if (!string.IsNullOrWhiteSpace(d)) cmb.Items.Add(d);
            cmb.SelectedIndex = 0;
        }

        private void Ara()
        {
            var urunler  = StokVeritabani.TumUrunler();
            var sonuclar = new List<Urun>(urunler);

            // ── Üst bar arama ────────────────────────────────────────────────
            string aramaMetni = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(aramaMetni))
            {
                string alan   = cmbSearchType.SelectedItem?.ToString() ?? "";
                string tumKey = LangManager.Ingilizce ? "All" : "Tümü";
                sonuclar = sonuclar.Where(u =>
                {
                    return alan switch
                    {
                        "Material"    => u.Material?.ToLower().Contains(aramaMetni) == true,
                        "Grade"       => u.Grade?.ToLower().Contains(aramaMetni) == true,
                        "Customer"    => u.Customer?.ToLower().Contains(aramaMetni) == true,
                        "Certificate" => u.Certificate?.ToLower().Contains(aramaMetni) == true,
                        _ => // Tümü
                            (u.Material?.ToLower().Contains(aramaMetni) == true)    ||
                            (u.Grade?.ToLower().Contains(aramaMetni) == true)       ||
                            (u.Customer?.ToLower().Contains(aramaMetni) == true)    ||
                            (u.Certificate?.ToLower().Contains(aramaMetni) == true) ||
                            (u.Batch?.ToLower().Contains(aramaMetni) == true)       ||
                            (u.Heat?.ToLower().Contains(aramaMetni) == true)        ||
                            (u.Parent?.ToLower().Contains(aramaMetni) == true)
                    };
                }).ToList();
            }

            // ── Sol filtre comboboxları ───────────────────────────────────────
            // customer
            if (cmbUrun.SelectedIndex > 0)
                sonuclar = sonuclar.Where(u => u.Customer == cmbUrun.SelectedItem.ToString()).ToList();

            // parent coil no
            if (cmbDiameter.SelectedIndex > 0)
                sonuclar = sonuclar.Where(u => u.Parent == cmbDiameter.SelectedItem.ToString()).ToList();

            // batch no
            if (cmbMaterial.SelectedIndex > 0)
                sonuclar = sonuclar.Where(u => u.Batch == cmbMaterial.SelectedItem.ToString()).ToList();

            // grade
            if (cmbGrade.SelectedIndex > 0)
                sonuclar = sonuclar.Where(u => u.Grade == cmbGrade.SelectedItem.ToString()).ToList();

            // heat no
            if (cmbThickness.SelectedIndex > 0)
                sonuclar = sonuclar.Where(u => u.Heat == cmbThickness.SelectedItem.ToString()).ToList();

            // ── Sayısal aralık filtreleri ─────────────────────────────────────
            // Thickness aralık
            if (double.TryParse(txtThicknessMin.Text, out double tMin))
                sonuclar = sonuclar.Where(u => double.TryParse(u.Thickness, out double t) && t >= tMin).ToList();
            if (double.TryParse(txtThicknessMax.Text, out double tMax))
                sonuclar = sonuclar.Where(u => double.TryParse(u.Thickness, out double t) && t <= tMax).ToList();

            // Width aralık
            if (double.TryParse(txtWidthMin.Text, out double wMin))
                sonuclar = sonuclar.Where(u => double.TryParse(u.Width, out double w) && w >= wMin).ToList();
            if (double.TryParse(txtWidthMax.Text, out double wMax))
                sonuclar = sonuclar.Where(u => double.TryParse(u.Width, out double w) && w <= wMax).ToList();

            // Length aralık
            if (double.TryParse(txtLengthMin.Text, out double lMin))
                sonuclar = sonuclar.Where(u => double.TryParse(u.Length, out double l) && l >= lMin).ToList();
            if (double.TryParse(txtLengthMax.Text, out double lMax))
                sonuclar = sonuclar.Where(u => double.TryParse(u.Length, out double l) && l <= lMax).ToList();

            // Stok aralık
            if (int.TryParse(txtStockMin.Text, out int sMin))
                sonuclar = sonuclar.Where(u => u.Stok >= sMin).ToList();
            if (int.TryParse(txtStockMax.Text, out int sMax))
                sonuclar = sonuclar.Where(u => u.Stok <= sMax).ToList();

            // ── Grid'e yükle (Firma başta, UrunCinsi yok) ────────────────────
            dgvUrunler.Rows.Clear();

            foreach (var u in sonuclar)
            {
                int rowIdx = dgvUrunler.Rows.Add(
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
                    u.Stok         // Ürün Adedi
                );

            }

            lblKayitSayisi.Text = LangManager.T("arama.bulunan") + sonuclar.Count;
            dgvUrunler.ClearSelection();
            dgvUrunler.CurrentCell = null;
        }

        private void btnAra_Click(object sender, EventArgs e) => Ara();

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            txtThicknessMin.Clear(); txtThicknessMax.Clear();
            txtWidthMin.Clear();  txtWidthMax.Clear();
            txtLengthMin.Clear(); txtLengthMax.Clear();
            txtStockMin.Clear();  txtStockMax.Clear();
            if (cmbSearchType.Items.Count > 0) cmbSearchType.SelectedIndex = 0;
            if (cmbUrun.Items.Count > 0)       cmbUrun.SelectedIndex = 0;
            if (cmbMaterial.Items.Count > 0)   cmbMaterial.SelectedIndex = 0;
            if (cmbGrade.Items.Count > 0)      cmbGrade.SelectedIndex = 0;
            if (cmbThickness.Items.Count > 0)  cmbThickness.SelectedIndex = 0;
            if (cmbDiameter.Items.Count > 0)   cmbDiameter.SelectedIndex = 0;
            Ara();
        }

        private void panelTop_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
    }
}
