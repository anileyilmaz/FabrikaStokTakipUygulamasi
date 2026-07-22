using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    public class FormAdmin : Form
    {
        // ── Renkler ──────────────────────────────────────────────────────────
        static readonly Color CNavy   = UIStil.Lacivert;
        static readonly Color CDark   = UIStil.LacivertKoyu;
        static readonly Color CGray   = UIStil.GriAcik;
        static readonly Color CWhite  = UIStil.Beyaz;
        static readonly Color CGreen  = UIStil.Basarili;
        static readonly Color CRed    = UIStil.Kritik;
        static readonly Color COrange = UIStil.Uyari;
        static readonly Color CBlue   = UIStil.Mavi;

        // ── Kontroller ───────────────────────────────────────────────────────
        private TabControl   tabMain;
        private TabPage      tabKullanicilar, tabHareketler;

        // Kullanıcılar sekmesi
        private DataGridView dgvKul;
        private Button       btnYeniKul, btnDuzKul, btnSilKul;
        private Label        lblKulSayisi;

        // Hareketler sekmesi
        private DataGridView dgvHar;
        private ComboBox     cmbFiltre;
        private Label        lblHarSayisi;
        private bool         yukleniyor;

        // ── Kurucu ───────────────────────────────────────────────────────────
        public FormAdmin()
        {
            this.Text            = LangManager.T("admin.paneli");
            this.BackColor       = CGray;
            this.Font            = new Font("Segoe UI", 9.5f);
            this.TopLevel        = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock            = DockStyle.Fill;

            Build();
            Reload();

            LangManager.DilDegisti += OnDilDegisti;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            LangManager.DilDegisti -= OnDilDegisti;
            base.OnFormClosed(e);
        }

        // ── UI İnşası ────────────────────────────────────────────────────────
        private void Build()
        {
            // 1) TabControl — ÖNCE ekle (Fill olduğu için Top'tan önce gelmeliy)
            tabMain = new TabControl { Dock = DockStyle.Fill };
            tabMain.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);

            tabKullanicilar = new TabPage { BackColor = CGray, Padding = new Padding(0) };
            tabHareketler   = new TabPage { BackColor = CGray, Padding = new Padding(0) };
            tabMain.TabPages.AddRange(new TabPage[] { tabKullanicilar, tabHareketler });

            BuildKullanicilarTab();
            BuildHareketlerTab();

            // 2) Başlık paneli — SONRA ekle (DockStyle.Top)
            var pHeader = new Panel { Dock = DockStyle.Top, Height = 65, BackColor = CDark };
            var lblBas  = new Label { AutoSize = true, Location = new Point(18, 10),
                Font = new Font("Segoe UI", 13f, FontStyle.Bold), ForeColor = CWhite };
            var lblAlt  = new Label { AutoSize = true, Location = new Point(20, 38),
                Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(149, 165, 166) };
            pHeader.Controls.AddRange(new Control[] { lblBas, lblAlt });

            // Dil güncellemeleri için referans tut
            _lblBaslik = lblBas;
            _lblAltBaslik = lblAlt;

            // Controls.Add sırası: Fill önce, Top sonra
            this.Controls.Add(tabMain);
            this.Controls.Add(pHeader);

            UygulaTabBasliklari();
        }

        private Label _lblBaslik, _lblAltBaslik;

        private void UygulaTabBasliklari()
        {
            _lblBaslik.Text      = LangManager.T("admin.baslik");
            _lblAltBaslik.Text   = LangManager.T("admin.altbaslik");
            tabKullanicilar.Text = LangManager.Ingilizce ? "Users"            : "Kullanıcılar";
            tabHareketler.Text   = LangManager.Ingilizce ? "Stock Movements"  : "Stok Hareketleri";
        }

        // ════════════════════════════════════════════════════════════════════
        // KULLANICILAR SEKMESİ
        // ════════════════════════════════════════════════════════════════════
        private void BuildKullanicilarTab()
        {
            // Araç çubuğu
            var toolbar = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = CNavy };

            btnYeniKul = MkBtn("", CGreen,   new Point(14, 11),  new Size(155, 30));
            btnDuzKul  = MkBtn("", COrange,  new Point(179, 11), new Size(130, 30));
            btnSilKul  = MkBtn("", CRed,     new Point(319, 11), new Size(110, 30));

            btnYeniKul.Padding = new Padding(26, 0, 0, 0);
            btnDuzKul.Padding  = new Padding(26, 0, 0, 0);
            btnSilKul.Padding  = new Padding(26, 0, 0, 0);
            btnYeniKul.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ekle,    btnYeniKul.ClientRectangle, CWhite, 10f);
            btnDuzKul.Paint  += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Duzenle, btnDuzKul.ClientRectangle,  CWhite, 10f);
            btnSilKul.Paint  += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Sil,     btnSilKul.ClientRectangle,  CWhite, 10f);

            btnYeniKul.Click += (s, e) => AcKullaniciFrm(null);
            btnDuzKul.Click  += BtnDuzenle_Click;
            btnSilKul.Click  += BtnSil_Click;

            lblKulSayisi = new Label
            {
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 8.5f),
                Location = new Point(444, 17), AutoSize = true
            };

            toolbar.Controls.AddRange(new Control[]
                { btnYeniKul, btnDuzKul, btnSilKul, lblKulSayisi });

            // Grid — Fill, toolbar — Top  →  önce Fill ekle
            dgvKul = MkGrid();
            dgvKul.Dock = DockStyle.Fill;
            dgvKul.DefaultCellStyle.SelectionBackColor = Color.FromArgb(174, 214, 241);
            dgvKul.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvKul.CellFormatting += DgvKul_CellFormatting;
            dgvKul.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0) BtnDuzenle_Click(s, e);
            };

            dgvKul.Columns.AddRange(new DataGridViewColumn[]
            {
                MkCol("cKulAdi",   "", 24),
                MkCol("cRol",      "", 20),
                MkCol("cSonGiris", "", 28),
                MkCol("cDurum",    "", 16),
                MkCol("cSifre",    "", 12),
            });

            tabKullanicilar.Controls.Add(dgvKul);     // Fill önce
            tabKullanicilar.Controls.Add(toolbar);    // Top sonra

            GuncelleKulKolonlar();
        }

        private void GuncelleKulKolonlar()
        {
            btnYeniKul.Text = LangManager.Ingilizce ? "New User"  : "Yeni Kullanıcı";
            btnDuzKul.Text  = LangManager.Ingilizce ? "Edit"       : "Düzenle";
            btnSilKul.Text  = LangManager.Ingilizce ? "Delete"     : "Sil";
            dgvKul.Columns["cKulAdi"].HeaderText   = LangManager.T("admin.kul.ad");
            dgvKul.Columns["cRol"].HeaderText       = LangManager.T("admin.kul.rol");
            dgvKul.Columns["cSonGiris"].HeaderText  = LangManager.T("admin.kul.giris");
            dgvKul.Columns["cDurum"].HeaderText     = LangManager.T("admin.kul.durum");
            dgvKul.Columns["cSifre"].HeaderText     = LangManager.T("admin.kul.sifre");
        }

        private void DgvKul_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            switch (dgvKul.Columns[e.ColumnIndex].Name)
            {
                case "cDurum":
                    bool aktif = e.Value?.ToString() == LangManager.T("admin.aktif");
                    e.CellStyle.ForeColor = aktif ? CGreen : Color.FromArgb(127, 140, 141);
                    e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                    break;
                case "cRol":
                    if (e.Value?.ToString() == "Admin")
                        e.CellStyle.ForeColor = CBlue;
                    e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                    break;
            }
        }

        private void KullanicilariYukle()
        {
            dgvKul.Rows.Clear();
            var liste = KullaniciYonetici.TumKullanicilar();
            foreach (var k in liste)
            {
                // AktifOturum değeri sunucudan gelir — tüm istemciler gerçek durumu görür
                bool aktif = k.AktifOturum;
                string durum = aktif
                    ? LangManager.T("admin.aktif")
                    : LangManager.T("admin.offline");
                string sonGiris = k.SonGiris.HasValue
                    ? k.SonGiris.Value.ToString("dd.MM.yyyy HH:mm")
                    : (LangManager.T("admin.hic"));
                // Hash uzunluğu gerçek şifre uzunluğunu ele vermesin diye sabit sayıda nokta gösterilir.
                string sifre = new string('•', 8);

                int idx = dgvKul.Rows.Add(k.KullaniciAdi, k.RolAdi, sonGiris, durum, sifre);
                dgvKul.Rows[idx].Tag = k.KullaniciAdi;
            }
            lblKulSayisi.Text = liste.Count + LangManager.T("admin.kul.sayi");
            dgvKul.ClearSelection();
        }

        // ── Kullanıcı seç ───────────────────────────────────────────────────
        private Kullanici SecilenKul()
        {
            if (dgvKul.SelectedRows.Count == 0) return null;
            string ad = dgvKul.SelectedRows[0].Tag?.ToString();
            if (string.IsNullOrEmpty(ad)) return null;
            foreach (var k in KullaniciYonetici.TumKullanicilar())
                if (k.KullaniciAdi == ad) return k;
            return null;
        }

        // ── Düzenle butonu ──────────────────────────────────────────────────
        private void BtnDuzenle_Click(object sender, EventArgs e)
        {
            var k = SecilenKul();
            if (k == null)
            {
                MessageBox.Show(
                    LangManager.Ingilizce ? "Please select a user." : "Lütfen bir kullanıcı seçin.",
                    LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AcKullaniciFrm(k);
        }

        // ── Sil butonu ──────────────────────────────────────────────────────
        private void BtnSil_Click(object sender, EventArgs e)
        {
            var k = SecilenKul();
            if (k == null)
            {
                MessageBox.Show(
                    LangManager.Ingilizce ? "Please select a user." : "Lütfen bir kullanıcı seçin.",
                    LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (KullaniciYonetici.AktifKullanici?.KullaniciAdi == k.KullaniciAdi)
            {
                MessageBox.Show(
                    LangManager.Ingilizce ? "You cannot delete your own account."
                                          : "Kendi hesabınızı silemezsiniz.",
                    LangManager.T("genel.hata"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string msg = $"\"{k.KullaniciAdi}\" " + LangManager.T("admin.sil.onay");
            if (MessageBox.Show(msg, LangManager.T("admin.sil.baslik"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                KullaniciYonetici.KullaniciSil(k.KullaniciAdi);
                KullanicilariYukle();
            }
        }

        // ── Kullanıcı Ekle/Düzenle Formu ────────────────────────────────────
        private void AcKullaniciFrm(Kullanici hedef)
        {
            bool yeni = hedef == null;

            var frm = new Form
            {
                Text            = yeni
                    ? LangManager.T("admin.kulyeni.baslik")
                    : (LangManager.Ingilizce ? "Edit User: " : "Düzenle: ") + hedef.KullaniciAdi,
                Size            = new Size(400, 340),
                StartPosition   = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox     = false, MinimizeBox = false,
                BackColor       = CGray,
                Font            = new Font("Segoe UI", 9.5f)
            };

            // Header
            var pHead = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = CNavy };
            pHead.Controls.Add(new Label
            {
                Text = frm.Text, Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = CWhite, AutoSize = true, Location = new Point(14, 14)
            });
            frm.Controls.Add(pHead);

            // İçerik paneli
            var pBody = new Panel { Location = new Point(0, 55), Size = new Size(400, 200), BackColor = CWhite };

            Label Lbl(string t, int y) => new Label
                { Text = t, Location = new Point(20, y), AutoSize = true,
                  Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                  ForeColor = Color.FromArgb(100, 100, 100) };

            TextBox Txt(string v, int y, bool pwd = false) => new TextBox
                { Location = new Point(20, y), Size = new Size(348, 26), Text = v,
                  BorderStyle = BorderStyle.FixedSingle, PasswordChar = pwd ? '*' : '\0',
                  BackColor = Color.FromArgb(248, 249, 250) };

            var lblAd    = Lbl(LangManager.T("admin.kulyeni.ad"),    16);
            var txtAd    = Txt(hedef?.KullaniciAdi ?? "",             34);
            var lblSifre = Lbl(yeni
                ? LangManager.T("admin.kulyeni.sifre")
                : (LangManager.Ingilizce ? "New Password (leave blank to keep current)"
                                         : "Yeni Şifre (boş bırakılırsa değişmez)"), 70);
            // Güvenlik: saklanan şifre hash'i hiçbir zaman ekranda gösterilmez.
            // Alan her zaman boş başlar; düzenleme modunda boş bırakılırsa mevcut şifre korunur.
            var txtSifre = Txt("", 88, pwd: true);
            var lblRol   = Lbl(LangManager.T("admin.kulyeni.rol"),  124);

            var cmbRol = new ComboBox
            {
                Location = new Point(20, 142), Size = new Size(348, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(248, 249, 250)
            };
            cmbRol.Items.Add(LangManager.T("admin.kulyeni.rol1"));
            cmbRol.Items.Add(LangManager.T("admin.kulyeni.rol2"));
            cmbRol.Items.Add(LangManager.T("admin.kulyeni.rol3"));
            cmbRol.SelectedIndex = hedef == null ? 0
                : hedef.Rol == KullaniciRol.Admin ? 2
                : hedef.Rol == KullaniciRol.Muhendis ? 1 : 0;

            pBody.Controls.AddRange(new Control[]
                { lblAd, txtAd, lblSifre, txtSifre, lblRol, cmbRol });
            frm.Controls.Add(pBody);

            // Butonlar
            var btnKaydet = new Button
            {
                Text = yeni ? LangManager.T("admin.kulyeni.kaydet")
                            : LangManager.T("duzenle.guncelle"),
                Location = new Point(20, 268), Size = new Size(168, 38),
                BackColor = CGreen, ForeColor = CWhite,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold)
            };
            btnKaydet.FlatAppearance.BorderSize = 0;

            var btnIptal = new Button
            {
                Text = LangManager.T("genel.iptal"),
                Location = new Point(200, 268), Size = new Size(168, 38),
                BackColor = Color.FromArgb(149, 165, 166), ForeColor = CWhite,
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold)
            };
            btnIptal.FlatAppearance.BorderSize = 0;
            btnIptal.Click += (s, ev) => frm.Close();

            btnKaydet.Click += (s, ev) =>
            {
                string ad    = txtAd.Text.Trim();
                string sifre = txtSifre.Text;
                var    rol   = cmbRol.SelectedIndex == 2 ? KullaniciRol.Admin
                             : cmbRol.SelectedIndex == 1 ? KullaniciRol.Muhendis
                             : KullaniciRol.DepoPersoneli;

                bool sifreZorunlu = yeni; // Yeni kullanıcıda şifre şart; düzenlemede boş = değiştirme
                if (string.IsNullOrWhiteSpace(ad) || (sifreZorunlu && string.IsNullOrWhiteSpace(sifre)))
                {
                    MessageBox.Show(LangManager.T("admin.kulyeni.bos"),
                        LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool ok = yeni
                    ? KullaniciYonetici.YeniKullaniciEkle(ad, sifre, rol)
                    : KullaniciYonetici.KullaniciGuncelle(hedef.KullaniciAdi, ad, sifre, rol);

                if (ok)
                {
                    MessageBox.Show(
                        yeni ? LangManager.T("admin.kulyeni.ok")
                             : (LangManager.T("admin.guncellendi")),
                        LangManager.T("genel.basarili"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frm.Close();
                    KullanicilariYukle();
                }
                else
                {
                    MessageBox.Show(LangManager.T("admin.kulyeni.var"),
                        LangManager.T("genel.hata"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            frm.Controls.AddRange(new Control[] { btnKaydet, btnIptal });
            frm.ShowDialog();
        }

        // ════════════════════════════════════════════════════════════════════
        // STOK HAREKETLERİ SEKMESİ
        // ════════════════════════════════════════════════════════════════════
        private void BuildHareketlerTab()
        {
            var toolbar = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = CNavy };

            toolbar.Controls.Add(new Label
            {
                Text = LangManager.T("admin.filtre"), ForeColor = CWhite,
                Font = new Font("Segoe UI", 9f), Location = new Point(14, 18), AutoSize = true
            });

            cmbFiltre = new ComboBox
            {
                Location = new Point(140, 13), Size = new Size(180, 26),
                DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9f)
            };
            cmbFiltre.SelectedIndexChanged += (s, e) => { if (!yukleniyor) HareketleriYukle(); };

            var btnYenile = MkBtn(LangManager.T("admin.yenile"), CBlue, new Point(330, 13), new Size(95, 28));
            btnYenile.Click += (s, e) => HareketleriYukle();

            lblHarSayisi = new Label
            {
                ForeColor = Color.FromArgb(189, 195, 199),
                Font = new Font("Segoe UI", 8.5f), Location = new Point(440, 18), AutoSize = true
            };

            toolbar.Controls.AddRange(new Control[]
                { cmbFiltre, btnYenile, lblHarSayisi });

            dgvHar = MkGrid();
            dgvHar.Dock = DockStyle.Fill;
            dgvHar.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgvHar.DefaultCellStyle.SelectionBackColor = Color.FromArgb(174, 214, 241);
            dgvHar.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvHar.CellFormatting += DgvHar_CellFormatting;

            dgvHar.Columns.AddRange(new DataGridViewColumn[]
            {
                MkCol("hTarih", LangManager.T("admin.col.tarih"), 18),
                MkCol("hKul",   LangManager.T("admin.col.kul"),   12),
                MkCol("hUrun",  LangManager.T("admin.col.urun"),  28),
                MkCol("hEski",  LangManager.T("admin.col.eski"),  12),
                MkCol("hYeni",  LangManager.T("admin.col.yeni"),  12),
                MkCol("hFark",  LangManager.T("admin.col.fark"),  10),
                MkCol("hIslem", LangManager.T("admin.col.islem"), 18),
            });
            dgvHar.Columns["hUrun"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            tabHareketler.Controls.Add(dgvHar);      // Fill önce
            tabHareketler.Controls.Add(toolbar);     // Top sonra
        }

        private void DgvHar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvHar.Rows[e.RowIndex].Tag == null) return;
            int fark = (int)dgvHar.Rows[e.RowIndex].Tag;
            string col = dgvHar.Columns[e.ColumnIndex].Name;
            if (col == "hFark" || col == "hIslem")
            {
                // Silinme → turuncu, ekleme → yeşil, çıkarma → kırmızı
                string islemVal = dgvHar.Rows[e.RowIndex].Cells["hIslem"].Value?.ToString() ?? "";
                if (islemVal.Contains("Silindi") || islemVal.Contains("Deleted"))
                    e.CellStyle.ForeColor = Color.FromArgb(211, 84, 0);
                else
                    e.CellStyle.ForeColor = fark > 0 ? CGreen : CRed;
                e.CellStyle.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
            }
        }

        private void HareketleriYukle()
        {
            yukleniyor = true;
            try
            {
                List<StokHareket> liste;
                try { liste = StokVeritabani.HareketleriGetir(); }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        (LangManager.Ingilizce ? "DB Error: " : "Veritabanı hatası: ") + ex.Message,
                        LangManager.T("genel.hata"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Filtre combobox yenile
                string eski = cmbFiltre.SelectedItem?.ToString();
                string tumKey = LangManager.Ingilizce ? "All Users" : "Tüm Kullanıcılar";
                cmbFiltre.Items.Clear();
                cmbFiltre.Items.Add(tumKey);
                var eklenmiş = new HashSet<string>();
                foreach (var h in liste)
                    if (eklenmiş.Add(h.KullaniciAdi)) cmbFiltre.Items.Add(h.KullaniciAdi);
                int idx = cmbFiltre.Items.IndexOf(eski ?? tumKey);
                cmbFiltre.SelectedIndex = idx >= 0 ? idx : 0;

                // Tabloyu doldur
                dgvHar.Rows.Clear();
                string filtre = cmbFiltre.SelectedItem?.ToString();
                bool tumHepsi = filtre == tumKey || string.IsNullOrEmpty(filtre);
                int sayac = 0;

                foreach (var h in liste)
                {
                    if (!tumHepsi && h.KullaniciAdi != filtre) continue;
                    // İşlem adını DB'den al; dil çevirisi uygula
                    string islem;
                    string islemDb = h.IslemAdi ?? "";
                    if (islemDb.Contains("Silindi") || islemDb.Contains("Deleted"))
                        islem = LangManager.Ingilizce ? "Product Deleted" : "Ürün Silindi";
                    else if (h.Fark > 0)
                        islem = LangManager.Ingilizce ? "Stock Added"   : "Stok Eklendi";
                    else
                        islem = LangManager.Ingilizce ? "Stock Removed" : "Stok Çıkarıldı";
                    string farkStr = h.Fark > 0 ? "+" + h.Fark : h.Fark.ToString();
                    int ri = dgvHar.Rows.Add(h.Tarih, h.KullaniciAdi, h.UrunAdi,
                                              h.EskiStok, h.YeniStok, farkStr, islem);
                    dgvHar.Rows[ri].Tag = h.Fark;
                    sayac++;
                }

                lblHarSayisi.Text = sayac + (LangManager.Ingilizce ? " records" : " kayıt");
                dgvHar.ClearSelection();
            }
            finally { yukleniyor = false; }
        }

        // ── Dil güncellemesi ────────────────────────────────────────────────
        private void OnDilDegisti()
        {
            UygulaTabBasliklari();
            GuncelleKulKolonlar();

            // Hareketler kolon başlıkları
            dgvHar.Columns["hTarih"].HeaderText = LangManager.T("admin.col.tarih");
            dgvHar.Columns["hKul"].HeaderText   = LangManager.T("admin.col.kul");
            dgvHar.Columns["hUrun"].HeaderText  = LangManager.T("admin.col.urun");
            dgvHar.Columns["hEski"].HeaderText  = LangManager.T("admin.col.eski");
            dgvHar.Columns["hYeni"].HeaderText  = LangManager.T("admin.col.yeni");
            dgvHar.Columns["hFark"].HeaderText  = LangManager.T("admin.col.fark");
            dgvHar.Columns["hIslem"].HeaderText = LangManager.T("admin.col.islem");

            Reload();
        }

        private void Reload()
        {
            KullanicilariYukle();
            HareketleriYukle();
        }

        // ── Yardımcılar ──────────────────────────────────────────────────────
        private DataGridView MkGrid() => new DataGridView
        {
            ReadOnly              = true,
            AllowUserToAddRows    = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible     = false,
            SelectionMode         = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect           = false,
            BackgroundColor       = CWhite,
            BorderStyle           = BorderStyle.None,
            GridColor             = Color.FromArgb(220, 220, 220),
            AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.Fill,
            CellBorderStyle       = DataGridViewCellBorderStyle.SingleHorizontal,
            EnableHeadersVisualStyles = false,
            ColumnHeadersHeight   = 52,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            RowTemplate           = { Height = 34 },
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor  = CNavy,
                ForeColor  = CWhite,
                Font       = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                Alignment  = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = CNavy,
                SelectionForeColor = CWhite,
            },
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Font      = new Font("Segoe UI", 9.5f),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
            }
        };

        private DataGridViewTextBoxColumn MkCol(string name, string header, int weight)
            => new DataGridViewTextBoxColumn
            {
                Name = name, HeaderText = header, FillWeight = weight, ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };

        private Button MkBtn(string text, Color back, Point loc, Size size)
        {
            var b = new Button
            {
                Text = text, Location = loc, Size = size, BackColor = back,
                ForeColor = CWhite, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }
    }
}
