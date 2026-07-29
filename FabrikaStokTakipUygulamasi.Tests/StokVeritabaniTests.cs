using System;
using Npgsql;
using Xunit;

namespace FabrikaStokTakipUygulamasi.Tests
{
    [Collection("StokTakip Sıralı")]
    public class StokVeritabaniTests
    {
        private static void OrtamDegiskenleriniTemizle()
        {
            Environment.SetEnvironmentVariable("STOK_DB_URL", null);
            Environment.SetEnvironmentVariable("DATABASE_PUBLIC_URL", null);
            Environment.SetEnvironmentVariable("DATABASE_URL", null);
        }

        [Fact]
        public void Baglanti_bilgisi_hicbir_degisken_yoksa_anlamli_hata_firlatir()
        {
            OrtamDegiskenleriniTemizle();
            try
            {
                var ex = Assert.Throws<InvalidOperationException>(
                    () => StokVeritabani.PostgreSqlBaglantiStringiOlustur());
                Assert.Contains("STOK_DB_URL", ex.Message);
            }
            finally
            {
                OrtamDegiskenleriniTemizle();
            }
        }

        [Fact]
        public void Postgres_url_formati_dogru_ayristirilir()
        {
            OrtamDegiskenleriniTemizle();
            try
            {
                Environment.SetEnvironmentVariable(
                    "STOK_DB_URL", "postgresql://kullanici:gizli@ornek-host:6543/stokdb");

                var connStr = StokVeritabani.PostgreSqlBaglantiStringiOlustur();
                var builder = new NpgsqlConnectionStringBuilder(connStr);

                Assert.Equal("ornek-host", builder.Host);
                Assert.Equal(6543, builder.Port);
                Assert.Equal("kullanici", builder.Username);
                Assert.Equal("gizli", builder.Password);
                Assert.Equal("stokdb", builder.Database);
                Assert.Equal(SslMode.Require, builder.SslMode);
            }
            finally
            {
                OrtamDegiskenleriniTemizle();
            }
        }

        [Fact]
        public void Url_icindeki_ozel_karakterler_dogru_decode_edilir()
        {
            OrtamDegiskenleriniTemizle();
            try
            {
                // Şifrede '@' işareti gibi URL-encode edilmesi gereken bir karakter olsun.
                Environment.SetEnvironmentVariable(
                    "STOK_DB_URL", "postgresql://kullanici:s%40ifre@ornek-host:5432/stokdb");

                var connStr = StokVeritabani.PostgreSqlBaglantiStringiOlustur();
                var builder = new NpgsqlConnectionStringBuilder(connStr);

                Assert.Equal("s@ifre", builder.Password);
            }
            finally
            {
                OrtamDegiskenleriniTemizle();
            }
        }

        [Fact]
        public void Npgsql_formatinda_verilen_string_oldugu_gibi_kullanilir()
        {
            OrtamDegiskenleriniTemizle();
            try
            {
                const string dogrudanFormat = "Host=ornek-host;Port=5432;Database=stokdb;Username=u;Password=p";
                Environment.SetEnvironmentVariable("STOK_DB_URL", dogrudanFormat);

                var connStr = StokVeritabani.PostgreSqlBaglantiStringiOlustur();

                Assert.Equal(dogrudanFormat, connStr);
            }
            finally
            {
                OrtamDegiskenleriniTemizle();
            }
        }

        [Fact]
        public void Oncelik_sirasi_STOK_DB_URL_digerlerinden_once_gelir()
        {
            OrtamDegiskenleriniTemizle();
            try
            {
                Environment.SetEnvironmentVariable(
                    "DATABASE_URL", "postgresql://x:y@digerhost:5432/digerdb");
                Environment.SetEnvironmentVariable(
                    "STOK_DB_URL", "postgresql://x:y@dogruhost:5432/dogrudb");

                var connStr = StokVeritabani.PostgreSqlBaglantiStringiOlustur();
                var builder = new NpgsqlConnectionStringBuilder(connStr);

                Assert.Equal("dogruhost", builder.Host);
            }
            finally
            {
                OrtamDegiskenleriniTemizle();
            }
        }
    }
}
