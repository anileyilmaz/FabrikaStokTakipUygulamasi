using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace FabrikaStokTakipUygulamasi
{
    public enum KullaniciRol { Admin, Muhendis, DepoPersoneli }

    /// <summary>
    /// Tek bir kullanıcıyı temsil eder.
    /// Veriler SQLite'dan gelir — bellekteki liste sadece cache görevi görür.
    /// </summary>
    public class Kullanici
    {
        public int           Id            { get; set; }
        public string        KullaniciAdi  { get; set; }
        public string        Sifre         { get; set; }
        public KullaniciRol  Rol           { get; set; }
        public DateTime?     SonGiris      { get; set; }
        public bool          AktifOturum   { get; set; }

        public string RolAdi => Rol switch
        {
            KullaniciRol.Admin         => "Admin",
            KullaniciRol.Muhendis      => LangManager.Ingilizce ? "Engineer"       : "Mühendis",
            KullaniciRol.DepoPersoneli => LangManager.Ingilizce ? "Warehouse Staff" : "Depo Personeli",
            _                          => "?"
        };

        public bool IsAdmin => Rol == KullaniciRol.Admin;
    }

    /// <summary>
    /// Kullanıcı oturum yönetimi. Şifreler PBKDF2 ile hash'lenerek saklanır (bkz. Guvenlik.cs).
    /// </summary>
    public static class KullaniciYonetici
    {
        public static Kullanici AktifKullanici { get; private set; }

        // ── Tablo kurulumu (StokVeritabani.Baslat() tarafından çağrılır) ─────
        public static void TabloyuKur(SqliteConnection baglanti)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS ""Kullanicilar"" (
                    ""Id""           INTEGER PRIMARY KEY AUTOINCREMENT,
                    ""KullaniciAdi"" TEXT NOT NULL UNIQUE,
                    ""Sifre""        TEXT NOT NULL,
                    ""Rol""          TEXT NOT NULL DEFAULT 'DepoPersoneli',
                    ""SonGiris""     TEXT,
                    ""AktifOturum""  INTEGER NOT NULL DEFAULT 0
                );";
            using (var k = new SqliteCommand(sql, baglanti))
                k.ExecuteNonQuery();

            // Varsayılan kullanıcıları ekle (yoksa) — şifreler hash'lenmiş olarak yazılır
            var varsayilanlar = new[]
            {
                ("emir",   "1234",  "DepoPersoneli"),
                ("barkan", "1234",  "DepoPersoneli"),
                ("anil",   "1234",  "Muhendis"),
                ("goksu",  "1234",  "Muhendis"),
                ("admin",  "admin", "Admin"),
            };

            foreach (var (ad, sifre, rol) in varsayilanlar)
            {
                string ins = @"
                    INSERT INTO ""Kullanicilar"" (""KullaniciAdi"", ""Sifre"", ""Rol"")
                    VALUES (@Ad, @Sifre, @Rol)
                    ON CONFLICT (""KullaniciAdi"") DO NOTHING;";
                using (var k = new SqliteCommand(ins, baglanti))
                {
                    k.Parameters.AddWithValue("@Ad",    ad);
                    k.Parameters.AddWithValue("@Sifre", Guvenlik.SifreyiHashle(sifre));
                    k.Parameters.AddWithValue("@Rol",   rol);
                    k.ExecuteNonQuery();
                }
            }
        }

        // ── Giriş ─────────────────────────────────────────────────────────────
        public static bool GirisYap(string ad, string sifre)
        {
            try
            {
                using (var baglanti = StokVeritabani.YeniBaglanti())
                {
                    baglanti.Open();

                    string sql = @"SELECT * FROM ""Kullanicilar"" WHERE LOWER(""KullaniciAdi"") = LOWER(@Ad);";
                    using (var k = new SqliteCommand(sql, baglanti))
                    {
                        k.Parameters.AddWithValue("@Ad", ad);
                        using (var oku = k.ExecuteReader())
                        {
                            if (!oku.Read()) return false;                              // kullanıcı yok
                            string hashDb = oku["Sifre"].ToString();
                            if (!Guvenlik.SifreDogrula(sifre, hashDb)) return false;     // şifre yanlış

                            AktifKullanici = new Kullanici
                            {
                                Id           = Convert.ToInt32(oku["Id"]),
                                KullaniciAdi = oku["KullaniciAdi"].ToString(),
                                Sifre        = hashDb,
                                Rol          = RolParse(oku["Rol"].ToString()),
                                SonGiris     = oku["SonGiris"] != DBNull.Value
                                               ? DateTime.Parse(oku["SonGiris"].ToString())
                                               : null,
                            };
                        }
                    }

                    string guncelle = @"
                        UPDATE ""Kullanicilar""
                        SET ""SonGiris"" = @SonGiris, ""AktifOturum"" = 1
                        WHERE ""Id"" = @Id;";
                    using (var k = new SqliteCommand(guncelle, baglanti))
                    {
                        k.Parameters.AddWithValue("@SonGiris", DateTime.Now.ToString("O"));
                        k.Parameters.AddWithValue("@Id", AktifKullanici.Id);
                        k.ExecuteNonQuery();
                    }

                    AktifKullanici.SonGiris    = DateTime.Now;
                    AktifKullanici.AktifOturum = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Veritabanı bağlantı hatası:\n" + ex.Message,
                    "Hata",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        // ── Çıkış ─────────────────────────────────────────────────────────────
        public static void Cikis()
        {
            if (AktifKullanici == null) return;
            try
            {
                using (var baglanti = StokVeritabani.YeniBaglanti())
                {
                    baglanti.Open();
                    string sql = @"UPDATE ""Kullanicilar"" SET ""AktifOturum"" = 0 WHERE ""Id"" = @Id;";
                    using (var k = new SqliteCommand(sql, baglanti))
                    {
                        k.Parameters.AddWithValue("@Id", AktifKullanici.Id);
                        k.ExecuteNonQuery();
                    }
                }
            }
            catch { /* Çıkışta bağlantı hatası kritik değil */ }
            finally { AktifKullanici = null; }
        }

        // ── Tüm kullanıcıları getir (Admin paneli için) ───────────────────────
        public static List<Kullanici> TumKullanicilar()
        {
            var liste = new List<Kullanici>();
            try
            {
                using (var baglanti = StokVeritabani.YeniBaglanti())
                {
                    baglanti.Open();
                    string sql = @"SELECT * FROM ""Kullanicilar"" ORDER BY ""KullaniciAdi"";";
                    using (var k = new SqliteCommand(sql, baglanti))
                    using (var oku = k.ExecuteReader())
                    {
                        while (oku.Read())
                        {
                            liste.Add(new Kullanici
                            {
                                Id           = Convert.ToInt32(oku["Id"]),
                                KullaniciAdi = oku["KullaniciAdi"].ToString(),
                                Sifre        = oku["Sifre"].ToString(),
                                Rol          = RolParse(oku["Rol"].ToString()),
                                SonGiris     = oku["SonGiris"] != DBNull.Value
                                               ? DateTime.Parse(oku["SonGiris"].ToString())
                                               : null,
                                AktifOturum  = oku["AktifOturum"] != DBNull.Value
                                               && Convert.ToInt32(oku["AktifOturum"]) != 0,
                            });
                        }
                    }
                }
            }
            catch { /* Bağlantı hatası — boş liste döner */ }
            return liste;
        }

        // ── Kullanıcı yönetimi ────────────────────────────────────────────────
        public static bool YeniKullaniciEkle(string ad, string sifre, KullaniciRol rol)
        {
            if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(sifre)) return false;
            try
            {
                using (var baglanti = StokVeritabani.YeniBaglanti())
                {
                    baglanti.Open();
                    string sql = @"
                        INSERT INTO ""Kullanicilar"" (""KullaniciAdi"", ""Sifre"", ""Rol"")
                        VALUES (@Ad, @Sifre, @Rol)
                        ON CONFLICT (""KullaniciAdi"") DO NOTHING;";
                    using (var k = new SqliteCommand(sql, baglanti))
                    {
                        k.Parameters.AddWithValue("@Ad",    ad);
                        k.Parameters.AddWithValue("@Sifre", Guvenlik.SifreyiHashle(sifre));
                        k.Parameters.AddWithValue("@Rol",   rol.ToString());
                        return k.ExecuteNonQuery() > 0;   // 0 satır → çakışma var, eklenmedi
                    }
                }
            }
            catch { return false; }
        }

        /// <summary>
        /// Kullanıcıyı günceller. <paramref name="yeniSifre"/> boş/null ise mevcut şifre hash'i korunur
        /// (Admin panelinde şifre alanı artık düz metin göstermediği için varsayılan davranış budur).
        /// </summary>
        public static bool KullaniciGuncelle(string eskiAd, string yeniAd, string yeniSifre, KullaniciRol rol)
        {
            if (string.IsNullOrWhiteSpace(yeniAd)) return false;
            try
            {
                using (var baglanti = StokVeritabani.YeniBaglanti())
                {
                    baglanti.Open();

                    string sql = string.IsNullOrEmpty(yeniSifre)
                        ? @"UPDATE ""Kullanicilar""
                            SET ""KullaniciAdi"" = @YeniAd, ""Rol"" = @Rol
                            WHERE LOWER(""KullaniciAdi"") = LOWER(@EskiAd);"
                        : @"UPDATE ""Kullanicilar""
                            SET ""KullaniciAdi"" = @YeniAd, ""Sifre"" = @Sifre, ""Rol"" = @Rol
                            WHERE LOWER(""KullaniciAdi"") = LOWER(@EskiAd);";

                    using (var k = new SqliteCommand(sql, baglanti))
                    {
                        k.Parameters.AddWithValue("@EskiAd", eskiAd);
                        k.Parameters.AddWithValue("@YeniAd", yeniAd);
                        k.Parameters.AddWithValue("@Rol",    rol.ToString());
                        if (!string.IsNullOrEmpty(yeniSifre))
                            k.Parameters.AddWithValue("@Sifre", Guvenlik.SifreyiHashle(yeniSifre));
                        return k.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        public static bool KullaniciSil(string ad)
        {
            if (AktifKullanici?.KullaniciAdi.ToLower() == ad.ToLower()) return false;
            try
            {
                using (var baglanti = StokVeritabani.YeniBaglanti())
                {
                    baglanti.Open();
                    string sql = @"DELETE FROM ""Kullanicilar"" WHERE LOWER(""KullaniciAdi"") = LOWER(@Ad);";
                    using (var k = new SqliteCommand(sql, baglanti))
                    {
                        k.Parameters.AddWithValue("@Ad", ad);
                        return k.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        // ── Yardımcı ─────────────────────────────────────────────────────────
        private static KullaniciRol RolParse(string rol) => rol switch
        {
            "Admin"         => KullaniciRol.Admin,
            "Muhendis"      => KullaniciRol.Muhendis,
            _               => KullaniciRol.DepoPersoneli
        };
    }
}
