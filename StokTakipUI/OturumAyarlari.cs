using System;
using System.IO;
using System.Text.Json;

namespace StokTakipUI
{
    /// <summary>
    /// "Oturumu açık tut" tercihini ve kaydedilmiş kullanıcı bilgisini
    /// AppData klasöründe JSON olarak saklar.
    /// Şifre burada plain-text tutulur — üretim ortamında hash kullanılmalıdır.
    /// </summary>
    public static class OturumAyarlari
    {
        private static readonly string DosyaYolu = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "StokTakipUI",
            "oturum.json");

        private class AyarModel
        {
            public bool   AcikTut       { get; set; }
            public string KullaniciAdi  { get; set; }
            public string Sifre         { get; set; }
        }

        // ── Kaydet ────────────────────────────────────────────────────────────
        public static void Kaydet(bool acikTut, string kullaniciAdi, string sifre)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DosyaYolu)!);

            var model = new AyarModel
            {
                AcikTut      = acikTut,
                KullaniciAdi = acikTut ? kullaniciAdi : string.Empty,
                Sifre        = acikTut ? sifre        : string.Empty,
            };

            File.WriteAllText(DosyaYolu,
                JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true }));
        }

        // ── Oku ───────────────────────────────────────────────────────────────
        public static (bool acikTut, string kullaniciAdi, string sifre) Oku()
        {
            if (!File.Exists(DosyaYolu))
                return (false, string.Empty, string.Empty);

            try
            {
                var model = JsonSerializer.Deserialize<AyarModel>(File.ReadAllText(DosyaYolu));
                if (model == null) return (false, string.Empty, string.Empty);
                return (model.AcikTut, model.KullaniciAdi ?? string.Empty, model.Sifre ?? string.Empty);
            }
            catch
            {
                return (false, string.Empty, string.Empty);
            }
        }

        // ── Temizle (çıkış yapılınca "açık tut" işareti kaldırılmadan çıkılırsa) ──
        public static void OturumKapat()
        {
            // "Açık tut" seçili değilse kayıtlı kimlik bilgisini sil
            var (acikTut, ad, sifre) = Oku();
            if (!acikTut) Kaydet(false, string.Empty, string.Empty);
        }
    }
}
