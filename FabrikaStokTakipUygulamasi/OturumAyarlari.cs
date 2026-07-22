using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FabrikaStokTakipUygulamasi
{
    /// <summary>
    /// "Oturumu açık tut" tercihini ve kaydedilmiş kullanıcı bilgisini
    /// AppData klasöründe JSON olarak saklar.
    /// Şifre, Windows DPAPI (ProtectedData, CurrentUser scope) ile şifrelenip
    /// Base64 olarak yazılır — sadece aynı Windows kullanıcı hesabı çözebilir.
    /// </summary>
    public static class OturumAyarlari
    {
        private static readonly string DosyaYolu = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FabrikaStokTakipUygulamasi",
            "oturum.json");

        // DPAPI'nin ek doğrulama verisi (entropy) — dosya başka bir amaçla kullanılmaya çalışılırsa çözülemesin diye.
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("FabrikaStokTakipUygulamasi.OturumAyarlari.v1");

        private class AyarModel
        {
            public bool   AcikTut          { get; set; }
            public string KullaniciAdi     { get; set; }
            public string SifreSifreliBase64 { get; set; }
        }

        // ── Kaydet ────────────────────────────────────────────────────────────
        public static void Kaydet(bool acikTut, string kullaniciAdi, string sifre)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DosyaYolu)!);

            string sifreliBase64 = "";
            if (acikTut && !string.IsNullOrEmpty(sifre))
            {
                byte[] duzMetin = Encoding.UTF8.GetBytes(sifre);
                byte[] sifreli  = ProtectedData.Protect(duzMetin, Entropy, DataProtectionScope.CurrentUser);
                sifreliBase64   = Convert.ToBase64String(sifreli);
            }

            var model = new AyarModel
            {
                AcikTut            = acikTut,
                KullaniciAdi       = acikTut ? kullaniciAdi : string.Empty,
                SifreSifreliBase64 = acikTut ? sifreliBase64 : string.Empty,
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
                if (model == null || !model.AcikTut || string.IsNullOrEmpty(model.SifreSifreliBase64))
                    return (false, string.Empty, string.Empty);

                byte[] sifreli  = Convert.FromBase64String(model.SifreSifreliBase64);
                byte[] duzMetin = ProtectedData.Unprotect(sifreli, Entropy, DataProtectionScope.CurrentUser);
                string sifre    = Encoding.UTF8.GetString(duzMetin);

                return (model.AcikTut, model.KullaniciAdi ?? string.Empty, sifre);
            }
            catch
            {
                // Bozuk dosya, farklı Windows hesabından kopyalanmış dosya, vb. → güvenli taraf: giriş isteme
                return (false, string.Empty, string.Empty);
            }
        }

        // ── Temizle (çıkış yapılınca "açık tut" işareti kaldırılmadan çıkılırsa) ──
        public static void OturumKapat()
        {
            var (acikTut, _, _) = Oku();
            if (!acikTut) Kaydet(false, string.Empty, string.Empty);
        }
    }
}
