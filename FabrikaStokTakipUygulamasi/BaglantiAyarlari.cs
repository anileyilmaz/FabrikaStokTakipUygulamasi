using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FabrikaStokTakipUygulamasi
{
    /// <summary>
    /// Fabrika içi paylaşımlı PostgreSQL sunucusuna bağlanmak için gereken bilgileri
    /// AppData klasöründe saklar. Şifre, OturumAyarlari.cs'teki ile aynı desenle
    /// Windows DPAPI (ProtectedData, CurrentUser scope) ile şifrelenip diske öyle yazılır.
    /// </summary>
    public static class BaglantiAyarlari
    {
        private static readonly string DosyaYolu = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FabrikaStokTakipUygulamasi",
            "baglanti.json");

        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("FabrikaStokTakipUygulamasi.BaglantiAyarlari.v1");

        private class AyarModel
        {
            public string Sunucu             { get; set; }
            public int    Port               { get; set; }
            public string VeritabaniAdi       { get; set; }
            public string KullaniciAdi        { get; set; }
            public string SifreSifreliBase64  { get; set; }
        }

        public static bool DosyaVarMi() => File.Exists(DosyaYolu);

        public static void Kaydet(string sunucu, int port, string veritabaniAdi, string kullaniciAdi, string sifre)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DosyaYolu)!);

            byte[] duzMetin = Encoding.UTF8.GetBytes(sifre ?? "");
            byte[] sifreli  = ProtectedData.Protect(duzMetin, Entropy, DataProtectionScope.CurrentUser);

            var model = new AyarModel
            {
                Sunucu            = sunucu,
                Port              = port,
                VeritabaniAdi     = veritabaniAdi,
                KullaniciAdi      = kullaniciAdi,
                SifreSifreliBase64 = Convert.ToBase64String(sifreli),
            };

            File.WriteAllText(DosyaYolu,
                JsonSerializer.Serialize(model, new JsonSerializerOptions { WriteIndented = true }));
        }

        /// <summary>Dosya yoksa veya okunamazsa/bozuksa null döner — çağıran taraf ilk kurulum akışını başlatmalı.</summary>
        public static (string sunucu, int port, string veritabaniAdi, string kullaniciAdi, string sifre)? Oku()
        {
            if (!File.Exists(DosyaYolu)) return null;

            try
            {
                var model = JsonSerializer.Deserialize<AyarModel>(File.ReadAllText(DosyaYolu));
                if (model == null || string.IsNullOrWhiteSpace(model.Sunucu)) return null;

                byte[] sifreli  = Convert.FromBase64String(model.SifreSifreliBase64 ?? "");
                byte[] duzMetin = ProtectedData.Unprotect(sifreli, Entropy, DataProtectionScope.CurrentUser);
                string sifre    = Encoding.UTF8.GetString(duzMetin);

                return (model.Sunucu, model.Port, model.VeritabaniAdi, model.KullaniciAdi, sifre);
            }
            catch
            {
                return null;
            }
        }
    }
}
