using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    /// <summary>
    /// Kurumsal tema: ortak renkler, tipografi ve Segoe MDL2 Assets glyph sabitleri.
    /// Tüm formlar renk/ikon/font için buradaki sabitleri kullanır — yerel Color.FromArgb
    /// kopyaları yerine bu sınıfa referans verilir.
    /// </summary>
    public static class UIStil
    {
        // ── Renkler ──────────────────────────────────────────────────────────
        public static readonly Color Lacivert     = Color.FromArgb(15, 118, 110);  // Teal — ana marka rengi
        public static readonly Color LacivertKoyu = Color.FromArgb(11, 46, 44);    // TealKoyu — header/sidebar koyu tonu
        public static readonly Color Mavi         = Color.FromArgb(20, 184, 166);  // TealAcik — aktif/vurgu
        public static readonly Color MaviAcik     = Color.FromArgb(45, 212, 191);  // hover/ikincil buton (daha açık teal)
        public static readonly Color GriAcik      = Color.FromArgb(236, 240, 241); // sayfa arkaplanı
        public static readonly Color GriOrta      = Color.FromArgb(189, 195, 199); // ikincil metin/kenarlık
        public static readonly Color GriMetin     = Color.FromArgb(127, 140, 141); // etiket metni
        public static readonly Color GriInput     = Color.FromArgb(248, 249, 250); // input arkaplanı
        public static readonly Color Beyaz        = Color.White;
        public static readonly Color Basarili     = Color.FromArgb(39, 174, 96);   // durum: yeşil
        public static readonly Color Kritik       = Color.FromArgb(192, 57, 43);   // durum: kırmızı
        public static readonly Color Uyari        = Color.FromArgb(211, 84, 0);    // durum: turuncu/koyu
        public static readonly Color Notr         = Color.FromArgb(149, 165, 166); // ikincil/iptal buton
        public static readonly Color Aksan        = Color.FromArgb(217, 119, 6);   // birincil eylem (kaydet/düzenle) — amber

        // ── Fontlar ──────────────────────────────────────────────────────────
        public static Font Baslik(float boyut = 16f)     => new Font("Segoe UI", boyut, FontStyle.Bold);
        public static Font AltBaslik(float boyut = 9f)    => new Font("Segoe UI", boyut);
        public static Font Govde(float boyut = 9.5f)      => new Font("Segoe UI", boyut);
        public static Font Etiket(float boyut = 9f)       => new Font("Segoe UI", boyut, FontStyle.Bold);
        public static Font ButonYazi(float boyut = 9.5f)  => new Font("Segoe UI Semibold", boyut, FontStyle.Bold);
        public static Font GlyphFont(float boyut = 10f)   => new Font("Segoe MDL2 Assets", boyut);

        // ── Segoe MDL2 Assets glyph'leri ────────────────────────────────────
        // Kod noktaları Segoe MDL2 Assets karakter tablosundan alınmıştır (Windows'ta hazır gelir).
        public static class Glyph
        {
            public const string Kisi      = "\uE77B"; // Contact — tekil kullanıcı
            public const string Kisiler   = "\uE716"; // People — kullanıcılar sekmesi
            public const string Dokuman   = "\uE8A5"; // Page2 — PDF/doküman
            public const string Kapat     = "\uE711"; // Cancel — kapat/iptal (X)
            public const string Sil       = "\uE74D"; // Delete — çöp kutusu
            public const string Dunya     = "\uE774"; // Globe — dil seçimi
            public const string Liste     = "\uE8FD"; // BulletedList — hareketler/liste sekmesi
            public const string Duzenle   = "\uE70F"; // Edit — kalem
            public const string Ara       = "\uE721"; // Search — büyüteç
            public const string Ayarlar   = "\uE713"; // Setting — admin paneli
            public const string Ekle      = "\uE710"; // Add — artı (yeni kayıt)
            public const string Uyarim    = "\uE7BA"; // Important — uyarı üçgeni
            public const string Kutu      = "\uE7B8"; // ProductList — ürün/stok kutusu
            public const string DisaAktar = "\uE898"; // Export — Excel aktar
            public const string Onay      = "\uE73E"; // CheckMark — onay
            public const string Kilit     = "\uE72E"; // Lock — oturum kapat / güvenlik
            public const string Goz       = "\uE890"; // View — şifre göster/gizle
            public const string AnaSayfa  = "\uE80F"; // Home — dashboard nav
        }

        // ── Yardımcı uygulama metodları ─────────────────────────────────────

        /// <summary>
        /// Bir butona sol tarafta Segoe MDL2 Assets glyph ikonu çizer.
        /// ÖNEMLİ: Button.Text tek bir Font kullanır, bu yüzden ikon karakterini doğrudan
        /// Text içine eklemek (örn. glyph + " " + etiket) yanlış render olur (kutu/boş karakter) —
        /// çünkü düğmenin Font'u (Segoe UI Semibold) glyph'in Private-Use-Area kod noktasını çizemez.
        /// Bunun yerine: düğmenin Text'i SADECE etiket metnidir, Padding.Left ikon için yer açar,
        /// ve bu metot Paint olayında glyph'i o boşluğa ayrı bir fontla (Segoe MDL2 Assets) çizer.
        /// Kullanım: btn.Padding = new Padding(38, 0, 0, 0); btn.Paint += (s, e) =>
        ///           UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.X, btn.ClientRectangle, btn.ForeColor);
        /// </summary>
        public static void SolIkonCiz(Graphics g, string glyph, Rectangle butonAlani, Color renk, float boyut = 12f)
        {
            using (var font = GlyphFont(boyut))
            using (var brush = new SolidBrush(renk))
            {
                var olcum = g.MeasureString(glyph, font);
                float x = 14f;
                float y = (butonAlani.Height - olcum.Height) / 2f;
                g.DrawString(glyph, font, brush, x, y);
            }
        }

        /// <summary>
        /// Statik (tıklanamayan) ikon + metin ikilisi için: glyph'i kendi fontuyla çizen küçük
        /// bir Label ile asıl metni taşıyan komşu bir Label döndürür. Aynı Font-karışımı sorunu
        /// Label.Text için de geçerli olduğundan, tek bir Label yerine iki ayrı Label kullanılır.
        /// </summary>
        public static Label IkonLabel(string glyph, Color renk, float boyut = 11f)
            => new Label
            {
                Text = glyph,
                Font = GlyphFont(boyut),
                ForeColor = renk,
                AutoSize = true
            };

        // ── Yuvarlak köşe / gölge / hover animasyonu ────────────────────────

        /// <summary>
        /// Bir kontrolün köşelerini DPI'ya göre ölçeklenmiş yarıçapla yuvarlar.
        /// Kontrol yeniden boyutlanınca bölge otomatik yeniden hesaplanır.
        /// SADECE Panel/dialog gibi kart-benzeri kapsayıcılarda kullanılır —
        /// DataGridView'lerde KULLANILMAZ (bkz. plan Global Constraints).
        /// </summary>
        public static void YuvarlakBolgeUygula(Control c, int yaricap)
        {
            void Uygula()
            {
                if (c.Width <= 0 || c.Height <= 0) return;
                float olcek = c.DeviceDpi / 96f;
                int r = Math.Max(2, (int)(yaricap * olcek));
                var eskiBolge = c.Region;
                using (var yol = YuvarlakDikdortgenYolu(new Rectangle(0, 0, c.Width, c.Height), r))
                    c.Region = new Region(yol);
                eskiBolge?.Dispose();
            }
            c.Resize += (s, e) => Uygula();
            Uygula();
        }

        private static GraphicsPath YuvarlakDikdortgenYolu(Rectangle alan, int r)
        {
            var yol = new GraphicsPath();
            int cap = r * 2;
            yol.AddArc(alan.X, alan.Y, cap, cap, 180, 90);
            yol.AddArc(alan.Right - cap, alan.Y, cap, cap, 270, 90);
            yol.AddArc(alan.Right - cap, alan.Bottom - cap, cap, cap, 0, 90);
            yol.AddArc(alan.X, alan.Bottom - cap, cap, cap, 90, 90);
            yol.CloseFigure();
            return yol;
        }

        /// <summary>
        /// Bir kart panelinin sağ ve alt kenarına, azalan opaklıkta ince çizgiler
        /// çizerek yumuşak bir "gölge hissi" simüle eder (gerçek blur DEĞİLDİR —
        /// WinForms'ta native desteklenmiyor, bkz. tasarım dokümanı).
        /// KULLANIM: panelin kendi Paint'inde değil, panelin PARENT kontrolünün
        /// Paint olayında, panelin Bounds'u verilerek çağrılır — böylece gölge
        /// panelin dışına (sağ/alt) taşan kısmı görünür, panelin kendi içeriğinin
        /// üzerini örtmez:
        ///   this.Paint += (s, e) => UIStil.YumusakGolgeCiz(e.Graphics, panelCard.Bounds);
        /// </summary>
        public static void YumusakGolgeCiz(Graphics g, Rectangle alan, int derinlik = 4)
        {
            for (int i = 1; i <= derinlik; i++)
            {
                int alfa = (int)(60 * (1f - (float)i / (derinlik + 1)));
                using (var kalem = new Pen(Color.FromArgb(alfa, 0, 0, 0)))
                {
                    g.DrawLine(kalem, alan.Right + i, alan.Y + i, alan.Right + i, alan.Bottom + i);
                    g.DrawLine(kalem, alan.X + i, alan.Bottom + i, alan.Right + i, alan.Bottom + i);
                }
            }
        }

        /// <summary>
        /// Bir kontrole fare üzerine gelince/ayrılınca iki renk arasında yumuşak
        /// geçiş animasyonu bağlar. Timer SADECE animasyon sürerken çalışır ve
        /// bitince kendini durdurur (sürekli çalışmaz).
        /// KULLANIM: UIStil.HoverAnimasyonuBagla(btnLogin, UIStil.Mavi, UIStil.MaviAcik,
        ///           renk => btnLogin.BackColor = renk);
        /// </summary>
        public static void HoverAnimasyonuBagla(Control c, Color normal, Color hover, Action<Color> uygula)
        {
            float ilerleme = 0f;
            bool hoverAktif = false;
            var zamanlayici = new System.Windows.Forms.Timer { Interval = 15 };

            zamanlayici.Tick += (s, e) =>
            {
                const float adim = 0.12f;
                ilerleme = hoverAktif ? Math.Min(1f, ilerleme + adim) : Math.Max(0f, ilerleme - adim);
                uygula(RenkKaristir(normal, hover, ilerleme));
                if ((hoverAktif && ilerleme >= 1f) || (!hoverAktif && ilerleme <= 0f))
                    zamanlayici.Stop();
            };

            c.MouseEnter += (s, e) => { hoverAktif = true; zamanlayici.Start(); };
            c.MouseLeave += (s, e) => { hoverAktif = false; zamanlayici.Start(); };
            c.Disposed += (s, e) => zamanlayici.Dispose();

            uygula(normal);
        }

        private static Color RenkKaristir(Color a, Color b, float oran)
        {
            oran = Math.Max(0f, Math.Min(1f, oran));
            int r = (int)(a.R + (b.R - a.R) * oran);
            int g = (int)(a.G + (b.G - a.G) * oran);
            int bl = (int)(a.B + (b.B - a.B) * oran);
            return Color.FromArgb(r, g, bl);
        }
    }
}
