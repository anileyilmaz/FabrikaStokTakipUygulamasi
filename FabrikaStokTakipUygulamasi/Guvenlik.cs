using System;
using System.Security.Cryptography;

namespace FabrikaStokTakipUygulamasi
{
    /// <summary>
    /// PBKDF2 tabanlı şifre hash'leme. Saklanan format: "{iterasyon}.{tuzBase64}.{hashBase64}".
    /// </summary>
    public static class Guvenlik
    {
        private const int TuzBoyutuBayt = 16;
        private const int HashBoyutuBayt = 32;
        private const int Iterasyon = 100_000;

        public static string SifreyiHashle(string duzMetinSifre)
        {
            byte[] tuz = RandomNumberGenerator.GetBytes(TuzBoyutuBayt);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                duzMetinSifre, tuz, Iterasyon, HashAlgorithmName.SHA256, HashBoyutuBayt);
            return $"{Iterasyon}.{Convert.ToBase64String(tuz)}.{Convert.ToBase64String(hash)}";
        }

        public static bool SifreDogrula(string duzMetinSifre, string saklananHash)
        {
            if (string.IsNullOrEmpty(saklananHash)) return false;

            string[] parcalar = saklananHash.Split('.');
            if (parcalar.Length != 3) return false;
            if (!int.TryParse(parcalar[0], out int iterasyon)) return false;

            byte[] tuz, beklenenHash;
            try
            {
                tuz = Convert.FromBase64String(parcalar[1]);
                beklenenHash = Convert.FromBase64String(parcalar[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] hesaplananHash = Rfc2898DeriveBytes.Pbkdf2(
                duzMetinSifre, tuz, iterasyon, HashAlgorithmName.SHA256, beklenenHash.Length);

            return CryptographicOperations.FixedTimeEquals(hesaplananHash, beklenenHash);
        }
    }
}
