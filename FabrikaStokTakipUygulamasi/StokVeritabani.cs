using System;
using System.Collections.Generic;
using Npgsql;

namespace FabrikaStokTakipUygulamasi
{
    public class Urun
    {
        public int    Id             { get; set; }
        public string UrunCinsi      { get; set; }
        public string Material       { get; set; }
        public string Grade          { get; set; }
        public string Thickness      { get; set; }
        public string Width          { get; set; }
        public string Length         { get; set; }
        public int    Stok           { get; set; }
        public string Customer       { get; set; }
        public string Certificate    { get; set; }
        public string Batch          { get; set; }
        public string Heat           { get; set; }
        public string Parent         { get; set; }
        public string EklenmeTarihi  { get; set; }
        public int    LowStockLimit  { get; set; } = -1;
        public byte[] SertifikaPdf   { get; set; }
        public string SertifikaDosyaAdi { get; set; }
    }

    public class StokHareket
    {
        public int    Id           { get; set; }
        public string Tarih        { get; set; }
        public string KullaniciAdi { get; set; }
        public int    UrunId       { get; set; }
        public string UrunAdi      { get; set; }
        public int    EskiStok     { get; set; }
        public int    YeniStok     { get; set; }
        public int    Fark         { get; set; }
        public string IslemAdi     { get; set; }
    }

    public static class StokVeritabani
    {
        // Railway PostgreSQL bağlantısı için Windows ortam değişkeni kullanılır.
        // Öncelik sırası: STOK_DB_URL > DATABASE_PUBLIC_URL > DATABASE_URL
        private static string BaglantiString => PostgreSqlBaglantiStringiOlustur();

        /// <summary>
        /// KullaniciYonetici ve diğer sınıfların kullanımı için açık bağlantı nesnesi döndürür.
        /// Çağıran taraf Open() ve Dispose() çağrısından sorumludur.
        /// </summary>
        public static NpgsqlConnection YeniBaglanti() => new NpgsqlConnection(BaglantiString);

        private static string PostgreSqlBaglantiStringiOlustur()
        {
            string url = Environment.GetEnvironmentVariable("STOK_DB_URL")
                      ?? Environment.GetEnvironmentVariable("DATABASE_PUBLIC_URL")
                      ?? Environment.GetEnvironmentVariable("DATABASE_URL");

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException(
                    "Railway PostgreSQL bağlantısı bulunamadı. Windows ortam değişkenlerine STOK_DB_URL veya DATABASE_PUBLIC_URL ekleyin.");
            }

            // Railway genelde postgresql://user:password@host:port/database formatı verir.
            if (url.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
            {
                var uri = new Uri(url);
                string[] userInfo = uri.UserInfo.Split(new[] { ':' }, 2);

                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = uri.Port > 0 ? uri.Port : 5432,
                    Username = Uri.UnescapeDataString(userInfo[0]),
                    Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "",
                    Database = uri.AbsolutePath.TrimStart('/'),
                    SslMode = SslMode.Require,
                    TrustServerCertificate = true,
                    Timeout = 15,
                    CommandTimeout = 30
                };

                return builder.ConnectionString;
            }

            // Npgsql formatında verilirse direkt kullanılır:
            // Host=...;Port=...;Database=...;Username=...;Password=...;Ssl Mode=Require;Trust Server Certificate=true
            return url;
        }

        public static void Baslat()
        {
            using (var baglanti = new NpgsqlConnection(BaglantiString))
            {
                baglanti.Open();

                string urunlerTablosu = @"
                    CREATE TABLE IF NOT EXISTS ""Urunler"" (
                        ""Id""                SERIAL PRIMARY KEY,
                        ""UrunCinsi""         TEXT,
                        ""Material""          TEXT,
                        ""Grade""             TEXT,
                        ""Thickness""         TEXT,
                        ""Width""             TEXT,
                        ""Length""            TEXT,
                        ""Stok""              INTEGER NOT NULL DEFAULT 0,
                        ""Customer""          TEXT,
                        ""Certificate""       TEXT,
                        ""Batch""             TEXT,
                        ""Heat""              TEXT,
                        ""Parent""            TEXT,
                        ""EklenmeTarihi""     TEXT,
                        ""LowStockLimit""     INTEGER NOT NULL DEFAULT -1,
                        ""SertifikaPdf""      BYTEA,
                        ""SertifikaDosyaAdi"" TEXT
                    );";
                using (var k = new NpgsqlCommand(urunlerTablosu, baglanti)) k.ExecuteNonQuery();

                MigrationKolonEkle(baglanti, "LowStockLimit",     "INTEGER NOT NULL DEFAULT -1");
                MigrationKolonEkle(baglanti, "SertifikaPdf",      "BYTEA");
                MigrationKolonEkle(baglanti, "SertifikaDosyaAdi", "TEXT");

                string hareketTablosu = @"
                    CREATE TABLE IF NOT EXISTS ""StokHareketleri"" (
                        ""Id""           SERIAL PRIMARY KEY,
                        ""Tarih""        TEXT NOT NULL,
                        ""KullaniciAdi"" TEXT NOT NULL,
                        ""UrunId""       INTEGER NOT NULL,
                        ""UrunAdi""      TEXT NOT NULL,
                        ""EskiStok""     INTEGER NOT NULL,
                        ""YeniStok""     INTEGER NOT NULL,
                        ""Fark""         INTEGER NOT NULL
                    );";
                using (var k = new NpgsqlCommand(hareketTablosu, baglanti)) k.ExecuteNonQuery();

                // IslemAdi sütunu migration (eski DB'ler için)
                try
                {
                    using (var km = new NpgsqlCommand(
                        @"ALTER TABLE ""StokHareketleri"" ADD COLUMN IF NOT EXISTS ""IslemAdi"" TEXT NOT NULL DEFAULT 'Stok Eklendi';",
                        baglanti)) km.ExecuteNonQuery();
                }
                catch { /* IF NOT EXISTS desteklenmiyorsa yoksay */ }

                // Kullanıcılar tablosunu oluştur ve varsayılan kullanıcıları ekle
                KullaniciYonetici.TabloyuKur(baglanti);
            }
        }

        private static void MigrationKolonEkle(NpgsqlConnection baglanti, string kolon, string tip)
        {
            using (var k = new NpgsqlCommand(
                $"ALTER TABLE \"Urunler\" ADD COLUMN IF NOT EXISTS \"{kolon}\" {tip};", baglanti))
                k.ExecuteNonQuery();
        }

        public static void UrunEkle(Urun urun)
        {
            using (var baglanti = new NpgsqlConnection(BaglantiString))
            {
                baglanti.Open();
                string sql = @"
                    INSERT INTO ""Urunler""
                        (""UrunCinsi"", ""Material"", ""Grade"", ""Thickness"", ""Width"", ""Length"",
                         ""Stok"", ""Customer"", ""Certificate"", ""Batch"", ""Heat"", ""Parent"",
                         ""EklenmeTarihi"", ""SertifikaPdf"", ""SertifikaDosyaAdi"")
                    VALUES
                        (@UrunCinsi, @Material, @Grade, @Thickness, @Width, @Length,
                         @Stok, @Customer, @Certificate, @Batch, @Heat, @Parent,
                         @EklenmeTarihi, @SertifikaPdf, @SertifikaDosyaAdi);";

                using (var k = new NpgsqlCommand(sql, baglanti))
                {
                    ParametreleriEkle(k, urun, false);
                    k.ExecuteNonQuery();
                }
            }
        }

        public static void UrunSil(int id)
        {
            using (var baglanti = new NpgsqlConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new NpgsqlCommand("DELETE FROM \"Urunler\" WHERE \"Id\"=@Id;", baglanti))
                {
                    k.Parameters.AddWithValue("@Id", id);
                    k.ExecuteNonQuery();
                }
            }
        }

        public static void UrunGuncelle(Urun urun)
        {
            using (var baglanti = new NpgsqlConnection(BaglantiString))
            {
                baglanti.Open();
                string sql = @"
                    UPDATE ""Urunler"" SET
                        ""UrunCinsi""         = @UrunCinsi,
                        ""Material""          = @Material,
                        ""Grade""             = @Grade,
                        ""Thickness""         = @Thickness,
                        ""Width""             = @Width,
                        ""Length""            = @Length,
                        ""Stok""              = @Stok,
                        ""Customer""          = @Customer,
                        ""Certificate""       = @Certificate,
                        ""Batch""             = @Batch,
                        ""Heat""              = @Heat,
                        ""Parent""            = @Parent,
                        ""SertifikaPdf""      = @SertifikaPdf,
                        ""SertifikaDosyaAdi"" = @SertifikaDosyaAdi
                    WHERE ""Id"" = @Id;";

                using (var k = new NpgsqlCommand(sql, baglanti))
                {
                    ParametreleriEkle(k, urun, true);
                    k.ExecuteNonQuery();
                }
            }
        }

        private static void ParametreleriEkle(NpgsqlCommand k, Urun urun, bool idEkle)
        {
            k.Parameters.AddWithValue("@UrunCinsi",         (object)(urun.UrunCinsi ?? "") );
            k.Parameters.AddWithValue("@Material",          (object)(urun.Material ?? "") );
            k.Parameters.AddWithValue("@Grade",             (object)(urun.Grade ?? "") );
            k.Parameters.AddWithValue("@Thickness",         (object)(urun.Thickness ?? "") );
            k.Parameters.AddWithValue("@Width",             (object)(urun.Width ?? "") );
            k.Parameters.AddWithValue("@Length",            (object)(urun.Length ?? "") );
            k.Parameters.AddWithValue("@Stok",              urun.Stok);
            k.Parameters.AddWithValue("@Customer",          (object)(urun.Customer ?? "") );
            k.Parameters.AddWithValue("@Certificate",       (object)(urun.Certificate ?? "") );
            k.Parameters.AddWithValue("@Batch",             (object)(urun.Batch ?? "") );
            k.Parameters.AddWithValue("@Heat",              (object)(urun.Heat ?? "") );
            k.Parameters.AddWithValue("@Parent",            (object)(urun.Parent ?? "") );
            k.Parameters.AddWithValue("@EklenmeTarihi",     (object)(urun.EklenmeTarihi ?? "") );
            k.Parameters.AddWithValue("@SertifikaPdf",      (object)urun.SertifikaPdf ?? DBNull.Value);
            k.Parameters.AddWithValue("@SertifikaDosyaAdi", (object)urun.SertifikaDosyaAdi ?? DBNull.Value);
            if (idEkle) k.Parameters.AddWithValue("@Id", urun.Id);
        }

        public static List<Urun> TumUrunler()
        {
            var liste = new List<Urun>();
            using (var baglanti = new NpgsqlConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new NpgsqlCommand("SELECT * FROM \"Urunler\" ORDER BY \"Id\" DESC;", baglanti))
                using (var oku = k.ExecuteReader())
                {
                    while (oku.Read())
                    {
                        liste.Add(new Urun
                        {
                            Id            = Convert.ToInt32(oku["Id"]),
                            UrunCinsi     = oku["UrunCinsi"].ToString(),
                            Material      = oku["Material"].ToString(),
                            Grade         = oku["Grade"].ToString(),
                            Thickness     = oku["Thickness"].ToString(),
                            Width         = oku["Width"].ToString(),
                            Length        = oku["Length"].ToString(),
                            Stok          = Convert.ToInt32(oku["Stok"]),
                            Customer      = oku["Customer"].ToString(),
                            Certificate   = oku["Certificate"].ToString(),
                            Batch         = oku["Batch"].ToString(),
                            Heat          = oku["Heat"].ToString(),
                            Parent        = oku["Parent"].ToString(),
                            EklenmeTarihi = oku["EklenmeTarihi"].ToString(),
                            LowStockLimit = oku["LowStockLimit"] != DBNull.Value ? Convert.ToInt32(oku["LowStockLimit"]) : -1,
                            SertifikaPdf  = oku["SertifikaPdf"] != DBNull.Value ? (byte[])oku["SertifikaPdf"] : null,
                            SertifikaDosyaAdi = oku["SertifikaDosyaAdi"] != DBNull.Value ? oku["SertifikaDosyaAdi"].ToString() : null
                        });
                    }
                }
            }
            return liste;
        }

        public static void LowStockLimitGuncelle(int id, int limit)
        {
            using (var baglanti = new NpgsqlConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new NpgsqlCommand("UPDATE \"Urunler\" SET \"LowStockLimit\"=@Limit WHERE \"Id\"=@Id;", baglanti))
                {
                    k.Parameters.AddWithValue("@Limit", limit);
                    k.Parameters.AddWithValue("@Id", id);
                    k.ExecuteNonQuery();
                }
            }
        }

        public static void HareketKaydet(int urunId, string urunAdi, int eskiStok, int yeniStok, string islemAdi = null)
        {
            if (KullaniciYonetici.AktifKullanici == null) return;
            if (eskiStok == yeniStok && islemAdi == null) return;

            using (var baglanti = new NpgsqlConnection(BaglantiString))
            {
                baglanti.Open();
                string sql = @"
                    INSERT INTO ""StokHareketleri""
                        (""Tarih"", ""KullaniciAdi"", ""UrunId"", ""UrunAdi"", ""EskiStok"", ""YeniStok"", ""Fark"", ""IslemAdi"")
                    VALUES
                        (@Tarih, @KullaniciAdi, @UrunId, @UrunAdi, @EskiStok, @YeniStok, @Fark, @IslemAdi);";
                using (var k = new NpgsqlCommand(sql, baglanti))
                {
                    k.Parameters.AddWithValue("@Tarih",        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    k.Parameters.AddWithValue("@KullaniciAdi", KullaniciYonetici.AktifKullanici.KullaniciAdi);
                    k.Parameters.AddWithValue("@UrunId",       urunId);
                    k.Parameters.AddWithValue("@UrunAdi",      urunAdi ?? "");
                    k.Parameters.AddWithValue("@EskiStok",     eskiStok);
                    k.Parameters.AddWithValue("@YeniStok",     yeniStok);
                    k.Parameters.AddWithValue("@Fark",         yeniStok - eskiStok);
                    string islemStr = islemAdi ?? (yeniStok > eskiStok ? "Stok Eklendi" : "Stok \u00c7\u0131kar\u0131ld\u0131");
                    k.Parameters.AddWithValue("@IslemAdi", islemStr);
                    k.ExecuteNonQuery();
                }
            }
        }

        public static List<StokHareket> HareketleriGetir()
        {
            var liste = new List<StokHareket>();
            using (var baglanti = new NpgsqlConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new NpgsqlCommand("SELECT * FROM \"StokHareketleri\" ORDER BY \"Id\" DESC;", baglanti))
                using (var oku = k.ExecuteReader())
                {
                    while (oku.Read())
                    {
                        liste.Add(new StokHareket
                        {
                            Id           = Convert.ToInt32(oku["Id"]),
                            Tarih        = oku["Tarih"].ToString(),
                            KullaniciAdi = oku["KullaniciAdi"].ToString(),
                            UrunId       = Convert.ToInt32(oku["UrunId"]),
                            UrunAdi      = oku["UrunAdi"].ToString(),
                            EskiStok     = Convert.ToInt32(oku["EskiStok"]),
                            YeniStok     = Convert.ToInt32(oku["YeniStok"]),
                            Fark         = Convert.ToInt32(oku["Fark"]),
                            IslemAdi     = oku["IslemAdi"] != DBNull.Value ? oku["IslemAdi"].ToString() : ""
                        });
                    }
                }
            }
            return liste;
        }

        public static int ToplamUrun() => TekSayiSorgu("SELECT COUNT(*) FROM \"Urunler\";");

        public static int KritikStokSayisi()
        {
            int sayac = 0;
            foreach (var u in TumUrunler())
            {
                int limit = u.LowStockLimit >= 0 ? u.LowStockLimit : 5;
                if (u.Stok <= limit) sayac++;
            }
            return sayac;
        }

        public static int FirmaSayisi() =>
            TekSayiSorgu("SELECT COUNT(DISTINCT TRIM(\"Customer\")) FROM \"Urunler\" WHERE \"Customer\" IS NOT NULL AND TRIM(\"Customer\") != ''; ");

        private static int TekSayiSorgu(string sql)
        {
            using (var baglanti = new NpgsqlConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new NpgsqlCommand(sql, baglanti))
                    return Convert.ToInt32(k.ExecuteScalar());
            }
        }
    }
}
