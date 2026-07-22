using System.Drawing;
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
        public static readonly Color Lacivert     = Color.FromArgb(44, 62, 80);    // ana marka rengi
        public static readonly Color LacivertKoyu = Color.FromArgb(30, 44, 57);    // header/sidebar koyu tonu
        public static readonly Color Mavi         = Color.FromArgb(41, 128, 185);  // aktif/vurgu
        public static readonly Color MaviAcik     = Color.FromArgb(52, 152, 219);  // hover/ikincil buton
        public static readonly Color GriAcik      = Color.FromArgb(236, 240, 241); // sayfa arkaplanı
        public static readonly Color GriOrta      = Color.FromArgb(189, 195, 199); // ikincil metin/kenarlık
        public static readonly Color GriMetin     = Color.FromArgb(127, 140, 141); // etiket metni
        public static readonly Color GriInput     = Color.FromArgb(248, 249, 250); // input arkaplanı
        public static readonly Color Beyaz        = Color.White;
        public static readonly Color Basarili     = Color.FromArgb(39, 174, 96);   // durum: yeşil
        public static readonly Color Kritik       = Color.FromArgb(192, 57, 43);   // durum: kırmızı
        public static readonly Color Uyari        = Color.FromArgb(211, 84, 0);    // durum: turuncu/koyu
        public static readonly Color Notr         = Color.FromArgb(149, 165, 166); // ikincil/iptal buton
        public static readonly Color Aksan        = Color.FromArgb(243, 156, 18);  // birincil eylem (kaydet/düzenle) — turuncu-altın

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
    }
}
