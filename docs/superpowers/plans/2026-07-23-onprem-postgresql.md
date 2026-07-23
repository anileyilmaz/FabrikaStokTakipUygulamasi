# Fabrika Stok Takip Uygulaması — Faz 0: Fabrika İçi PostgreSQL — Uygulama Planı

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Move the database back from local SQLite to PostgreSQL, hosted on a factory-internal (LAN) server rather than a cloud provider, so multiple workstations (depo, kalite kontrol, mühendislik) can share one database. Connection details are read from a local, DPAPI-encrypted JSON config file instead of Windows environment variables (the earlier Railway setup's pain point).

**Architecture:** `StokVeritabani.cs` and `KullaniciYonetici.cs` are rewritten from `Microsoft.Data.Sqlite` back to `Npgsql`, using the exact same schema shape as the pre-SQLite-migration Postgres version (`SERIAL PRIMARY KEY`, `BYTEA`, etc.) but with `UrunEkle` now returning `int` (via `RETURNING "Id"`) to preserve the contract established by the SQLite-era plan and consumed by `FormUrunEkle.cs`. A new `BaglantiAyarlari.cs` (mirroring `OturumAyarlari.cs`'s DPAPI pattern) stores server/port/database/user/password in `%AppData%\FabrikaStokTakipUygulamasi\baglanti.json`. A new first-run dialog (`FormSunucuAyarlari`) collects these values on a workstation's first launch if the file doesn't exist yet.

**Tech Stack:** .NET 8 (`net8.0-windows`), WinForms, `Npgsql` 8.0.5 (replacing `Microsoft.Data.Sqlite`), `System.Security.Cryptography.ProtectedData` (already present, reused).

**Spec:** `docs/superpowers/specs/2026-07-23-onprem-postgresql-design.md`

## Global Constraints

- Target framework stays `net8.0-windows`.
- Schema stays the same shape as before (`Urunler`, `StokHareketleri`, `Kullanicilar` — same columns) — this plan changes the database ENGINE and CONNECTION SOURCE only, not the data model. New traceability/certification fields are explicitly out of scope (a later phase).
- `UrunEkle` must keep returning `int` (the new row's Id) — `FabrikaStokTakipUygulamasi/FormUrunEkle.cs` already does `int yeniId = StokVeritabani.UrunEkle(urun);` and must not need to change.
- Password hashing (`Guvenlik.cs`, PBKDF2) and the "keep me logged in" DPAPI encryption (`OturumAyarlari.cs`) are UNCHANGED by this plan — they are database-engine-independent and must not be touched.
- No Railway-specific environment-variable reading (`STOK_DB_URL` etc.) anywhere — connection info comes only from `BaglantiAyarlari`.
- This environment has no .NET SDK and no PostgreSQL server — every task's verification step is a self-review (re-read the diff) rather than a compiler/runtime check. GitHub Actions (`windows-latest`, `.github/workflows/build.yml`) builds on every push but cannot test real database connectivity (no Postgres service available there either, per this plan's scope — see Task 7 for real-server verification, which is the human operator's job).
- Tasks 1, 3, and 4 form one atomic provider-swap unit — like the earlier SQLite migration, the project will NOT compile in the commits between Task 1 (removes `Microsoft.Data.Sqlite`) and Task 4 (finishes rewriting `KullaniciYonetici.cs`). This is expected, not a mistake.
- Every task ends with a `git commit`, on the existing branch `feature/sqlite-migration-ui-refresh` (continuation of unreleased work already on that branch — do not create a new branch).

---

## File Structure (changed files)

| File | Change |
|---|---|
| `FabrikaStokTakipUygulamasi/FabrikaStokTakipUygulamasi.csproj` | Remove `Microsoft.Data.Sqlite`, add `Npgsql` 8.0.5 |
| `FabrikaStokTakipUygulamasi/BaglantiAyarlari.cs` | **New.** DPAPI-encrypted local connection-config file (mirrors `OturumAyarlari.cs`) |
| `FabrikaStokTakipUygulamasi/StokVeritabani.cs` | Rewritten: Npgsql/PostgreSQL, connection string built from `BaglantiAyarlari`, `UrunEkle` returns `int` via `RETURNING "Id"` |
| `FabrikaStokTakipUygulamasi/KullaniciYonetici.cs` | Rewritten: Npgsql/PostgreSQL, same PBKDF2/blank-password-keeps-current logic as the SQLite version |
| `FabrikaStokTakipUygulamasi/FormSunucuAyarlari.cs` | **New.** First-run dialog collecting server/port/database/user/password |
| `FabrikaStokTakipUygulamasi/Program.cs` | Show `FormSunucuAyarlari` before `StokVeritabani.Baslat()` if `BaglantiAyarlari.DosyaVarMi()` is false |
| `KURULUM.md` | Add on-premise PostgreSQL server setup section |

---

### Task 1: NuGet paket geçişi (Npgsql'e dönüş)

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FabrikaStokTakipUygulamasi.csproj`

**Interfaces:**
- Produces: `Npgsql` namespace available to Tasks 3–4.

- [ ] **Step 1: Replace the package reference**

Current content includes:

```xml
    <PackageReference Include="Microsoft.Data.Sqlite" Version="8.0.10" />
```

Replace with:

```xml
    <PackageReference Include="Npgsql" Version="8.0.5" />
```

Leave `System.Security.Cryptography.ProtectedData` and `WinForms.DataVisualization` untouched.

- [ ] **Step 2: Self-review**

Confirm no remaining `Microsoft.Data.Sqlite` reference anywhere in the `.csproj`, and the rest of the `<PropertyGroup>`/other `<PackageReference>` entries are untouched.

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FabrikaStokTakipUygulamasi.csproj
git commit -m "build: replace Microsoft.Data.Sqlite with Npgsql for the on-premise PostgreSQL move"
```

---

### Task 2: `BaglantiAyarlari.cs` — DPAPI şifreli bağlantı yapılandırması

**Files:**
- Create: `FabrikaStokTakipUygulamasi/BaglantiAyarlari.cs`

**Interfaces:**
- Produces: `public static class BaglantiAyarlari` with `bool DosyaVarMi()`, `void Kaydet(string sunucu, int port, string veritabaniAdi, string kullaniciAdi, string sifre)`, `(string sunucu, int port, string veritabaniAdi, string kullaniciAdi, string sifre)? Oku()` (nullable tuple — `null` means "not configured yet or unreadable", caller must trigger first-run setup). Consumed by Task 3 (`StokVeritabani.cs`) and Task 5 (`Program.cs`/`FormSunucuAyarlari.cs`).

- [ ] **Step 1: Write the file**

```csharp
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
```

- [ ] **Step 2: Self-review**

Confirm this file has NO reference to `Npgsql`/`Microsoft.Data.Sqlite` (it's purely a config file reader/writer, database-engine-agnostic). Confirm the DPAPI pattern (entropy constant, `CurrentUser` scope, try/catch-returns-null-on-any-failure) matches `OturumAyarlari.cs`'s established approach.

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/BaglantiAyarlari.cs
git commit -m "feat: add DPAPI-encrypted local connection-config file for the shared PostgreSQL server"
```

---

### Task 3: `StokVeritabani.cs` — PostgreSQL'e dönüş

**Files:**
- Modify (full rewrite): `FabrikaStokTakipUygulamasi/StokVeritabani.cs`

**Interfaces:**
- Consumes: `BaglantiAyarlari.Oku()` (Task 2).
- Produces: `public static NpgsqlConnection YeniBaglanti()` (consumed by `KullaniciYonetici.cs`, Task 4), `public static int UrunEkle(Urun urun)` (unchanged `int` contract from the SQLite-era plan — consumed by `FormUrunEkle.cs`, already written to expect this), `public static void Baslat()`, and all other pre-existing method signatures (`UrunSil`, `UrunGuncelle`, `TumUrunler`, `LowStockLimitGuncelle`, `HareketKaydet`, `HareketleriGetir`, `ToplamUrun`, `KritikStokSayisi`, `FirmaSayisi`) unchanged.

- [ ] **Step 1: Replace the entire file**

```csharp
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
        /// <summary>
        /// Fabrika içi paylaşımlı PostgreSQL sunucusuna bağlantı bilgisi
        /// BaglantiAyarlari'ndan (yerel, DPAPI şifreli JSON dosyası) okunur.
        /// </summary>
        private static string BaglantiString
        {
            get
            {
                var ayar = BaglantiAyarlari.Oku();
                if (ayar == null)
                {
                    throw new InvalidOperationException(
                        "Sunucu bağlantı bilgisi bulunamadı. Lütfen önce bağlantı ayarlarını yapılandırın.");
                }

                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host     = ayar.Value.sunucu,
                    Port     = ayar.Value.port > 0 ? ayar.Value.port : 5432,
                    Database = ayar.Value.veritabaniAdi,
                    Username = ayar.Value.kullaniciAdi,
                    Password = ayar.Value.sifre,
                    Timeout        = 15,
                    CommandTimeout = 30
                };
                return builder.ConnectionString;
            }
        }

        /// <summary>
        /// KullaniciYonetici ve diğer sınıfların kullanımı için açık bağlantı nesnesi döndürür.
        /// Çağıran taraf Open() ve Dispose() çağrısından sorumludur.
        /// </summary>
        public static NpgsqlConnection YeniBaglanti() => new NpgsqlConnection(BaglantiString);

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

                try
                {
                    using (var km = new NpgsqlCommand(
                        @"ALTER TABLE ""StokHareketleri"" ADD COLUMN IF NOT EXISTS ""IslemAdi"" TEXT NOT NULL DEFAULT 'Stok Eklendi';",
                        baglanti)) km.ExecuteNonQuery();
                }
                catch { /* IF NOT EXISTS desteklenmiyorsa yoksay */ }

                KullaniciYonetici.TabloyuKur(baglanti);
            }
        }

        private static void MigrationKolonEkle(NpgsqlConnection baglanti, string kolon, string tip)
        {
            using (var k = new NpgsqlCommand(
                $"ALTER TABLE \"Urunler\" ADD COLUMN IF NOT EXISTS \"{kolon}\" {tip};", baglanti))
                k.ExecuteNonQuery();
        }

        /// <summary>Ürünü ekler ve yeni satırın Id'sini döndürür.</summary>
        public static int UrunEkle(Urun urun)
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
                         @EklenmeTarihi, @SertifikaPdf, @SertifikaDosyaAdi)
                    RETURNING ""Id"";";

                using (var k = new NpgsqlCommand(sql, baglanti))
                {
                    ParametreleriEkle(k, urun, false);
                    return Convert.ToInt32(k.ExecuteScalar());
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
                    string islemStr = islemAdi ?? (yeniStok > eskiStok ? "Stok Eklendi" : "Stok Çıkarıldı");
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
            TekSayiSorgu("SELECT COUNT(DISTINCT TRIM(\"Customer\")) FROM \"Urunler\" WHERE \"Customer\" IS NOT NULL AND TRIM(\"Customer\") != '';");

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
```

- [ ] **Step 2: Self-review**

Confirm `UrunEkle` returns `int` (not `void`) via `RETURNING "Id"` + `ExecuteScalar()`, matching the contract `FormUrunEkle.cs` already relies on (`int yeniId = StokVeritabani.UrunEkle(urun);` — do not re-verify or change `FormUrunEkle.cs` itself, it needs no changes). Confirm `BaglantiString` throws a clear error if `BaglantiAyarlari.Oku()` returns null, rather than crashing with a null-reference deeper in `NpgsqlConnectionStringBuilder`. Confirm `Baslat()` still calls `KullaniciYonetici.TabloyuKur(baglanti)` with an `NpgsqlConnection` — note for Task 4: this means `KullaniciYonetici.TabloyuKur`'s parameter type must be `NpgsqlConnection`, matching what it was before the SQLite migration (not what it currently is, which is `SqliteConnection` — Task 4 fixes this).

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/StokVeritabani.cs
git commit -m "feat: migrate StokVeritabani from SQLite back to on-premise PostgreSQL"
```

---

### Task 4: `KullaniciYonetici.cs` — PostgreSQL'e dönüş

**Files:**
- Modify (full rewrite): `FabrikaStokTakipUygulamasi/KullaniciYonetici.cs`

**Interfaces:**
- Consumes: `Guvenlik.SifreyiHashle`/`SifreDogrula` (unchanged, pre-existing), `StokVeritabani.YeniBaglanti()` returning `NpgsqlConnection` (Task 3).
- Produces: `public static void TabloyuKur(NpgsqlConnection baglanti)` (parameter type changed back from `SqliteConnection` — matches Task 3's call site), and all other pre-existing method signatures unchanged (`GirisYap`, `Cikis`, `TumKullanicilar`, `YeniKullaniciEkle`, `KullaniciGuncelle` with its blank-password-keeps-current semantics, `KullaniciSil`).

- [ ] **Step 1: Replace the entire file**

```csharp
using System;
using System.Collections.Generic;
using Npgsql;

namespace FabrikaStokTakipUygulamasi
{
    public enum KullaniciRol { Admin, Muhendis, DepoPersoneli }

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
    /// Veriler fabrika içi paylaşımlı PostgreSQL sunucusundan gelir.
    /// </summary>
    public static class KullaniciYonetici
    {
        public static Kullanici AktifKullanici { get; private set; }

        public static void TabloyuKur(NpgsqlConnection baglanti)
        {
            string sql = @"
                CREATE TABLE IF NOT EXISTS ""Kullanicilar"" (
                    ""Id""           SERIAL PRIMARY KEY,
                    ""KullaniciAdi"" TEXT NOT NULL UNIQUE,
                    ""Sifre""        TEXT NOT NULL,
                    ""Rol""          TEXT NOT NULL DEFAULT 'DepoPersoneli',
                    ""SonGiris""     TIMESTAMPTZ,
                    ""AktifOturum""  BOOLEAN NOT NULL DEFAULT false
                );";
            using (var k = new NpgsqlCommand(sql, baglanti))
                k.ExecuteNonQuery();

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
                using (var k = new NpgsqlCommand(ins, baglanti))
                {
                    k.Parameters.AddWithValue("@Ad",    ad);
                    k.Parameters.AddWithValue("@Sifre", Guvenlik.SifreyiHashle(sifre));
                    k.Parameters.AddWithValue("@Rol",   rol);
                    k.ExecuteNonQuery();
                }
            }
        }

        public static bool GirisYap(string ad, string sifre)
        {
            try
            {
                using (var baglanti = StokVeritabani.YeniBaglanti())
                {
                    baglanti.Open();

                    string sql = @"SELECT * FROM ""Kullanicilar"" WHERE LOWER(""KullaniciAdi"") = LOWER(@Ad);";
                    using (var k = new NpgsqlCommand(sql, baglanti))
                    {
                        k.Parameters.AddWithValue("@Ad", ad);
                        using (var oku = k.ExecuteReader())
                        {
                            if (!oku.Read()) return false;
                            string hashDb = oku["Sifre"].ToString();
                            if (!Guvenlik.SifreDogrula(sifre, hashDb)) return false;

                            AktifKullanici = new Kullanici
                            {
                                Id           = Convert.ToInt32(oku["Id"]),
                                KullaniciAdi = oku["KullaniciAdi"].ToString(),
                                Sifre        = hashDb,
                                Rol          = RolParse(oku["Rol"].ToString()),
                                SonGiris     = oku["SonGiris"] != DBNull.Value
                                               ? (DateTime?)((DateTime)oku["SonGiris"]).ToLocalTime()
                                               : null,
                            };
                        }
                    }

                    string guncelle = @"
                        UPDATE ""Kullanicilar""
                        SET ""SonGiris"" = NOW(), ""AktifOturum"" = true
                        WHERE ""Id"" = @Id;";
                    using (var k = new NpgsqlCommand(guncelle, baglanti))
                    {
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
                    "Sunucu bağlantı hatası:\n" + ex.Message,
                    "Hata",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public static void Cikis()
        {
            if (AktifKullanici == null) return;
            try
            {
                using (var baglanti = StokVeritabani.YeniBaglanti())
                {
                    baglanti.Open();
                    string sql = @"UPDATE ""Kullanicilar"" SET ""AktifOturum"" = false WHERE ""Id"" = @Id;";
                    using (var k = new NpgsqlCommand(sql, baglanti))
                    {
                        k.Parameters.AddWithValue("@Id", AktifKullanici.Id);
                        k.ExecuteNonQuery();
                    }
                }
            }
            catch { /* Çıkışta bağlantı hatası kritik değil */ }
            finally { AktifKullanici = null; }
        }

        public static List<Kullanici> TumKullanicilar()
        {
            var liste = new List<Kullanici>();
            try
            {
                using (var baglanti = StokVeritabani.YeniBaglanti())
                {
                    baglanti.Open();
                    string sql = @"SELECT * FROM ""Kullanicilar"" ORDER BY ""KullaniciAdi"";";
                    using (var k = new NpgsqlCommand(sql, baglanti))
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
                                               ? (DateTime?)((DateTime)oku["SonGiris"]).ToLocalTime()
                                               : null,
                                AktifOturum  = oku["AktifOturum"] != DBNull.Value
                                               && Convert.ToBoolean(oku["AktifOturum"]),
                            });
                        }
                    }
                }
            }
            catch { /* Bağlantı hatası — boş liste döner */ }
            return liste;
        }

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
                        ON CONFLICT (""KullaniciAdi"") DO NOTHING
                        RETURNING ""Id"";";
                    using (var k = new NpgsqlCommand(sql, baglanti))
                    {
                        k.Parameters.AddWithValue("@Ad",    ad);
                        k.Parameters.AddWithValue("@Sifre", Guvenlik.SifreyiHashle(sifre));
                        k.Parameters.AddWithValue("@Rol",   rol.ToString());
                        var sonuc = k.ExecuteScalar();
                        return sonuc != null;
                    }
                }
            }
            catch { return false; }
        }

        /// <summary>
        /// Kullanıcıyı günceller. <paramref name="yeniSifre"/> boş/null ise mevcut şifre hash'i korunur.
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

                    using (var k = new NpgsqlCommand(sql, baglanti))
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
                    using (var k = new NpgsqlCommand(sql, baglanti))
                    {
                        k.Parameters.AddWithValue("@Ad", ad);
                        return k.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch { return false; }
        }

        private static KullaniciRol RolParse(string rol) => rol switch
        {
            "Admin"    => KullaniciRol.Admin,
            "Muhendis" => KullaniciRol.Muhendis,
            _          => KullaniciRol.DepoPersoneli
        };
    }
}
```

- [ ] **Step 2: Self-review**

Cross-check Task 3's `StokVeritabani.Baslat()` calls `KullaniciYonetici.TabloyuKur(baglanti)` with an `NpgsqlConnection` — this file's `TabloyuKur` parameter type now matches. Confirm `KullaniciGuncelle`'s blank-password-keeps-current branch and `YeniKullaniciEkle`'s non-empty-password requirement are preserved exactly as they were in the SQLite version (these are unrelated to the database engine and must not regress).

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/KullaniciYonetici.cs
git commit -m "feat: migrate KullaniciYonetici from SQLite back to on-premise PostgreSQL"
```

---

### Task 5: İlk kurulum ekranı (`FormSunucuAyarlari`) ve `Program.cs` bağlanması

**Files:**
- Create: `FabrikaStokTakipUygulamasi/FormSunucuAyarlari.cs`
- Modify: `FabrikaStokTakipUygulamasi/Program.cs`

**Interfaces:**
- Consumes: `BaglantiAyarlari.DosyaVarMi()`, `BaglantiAyarlari.Kaydet(...)` (Task 2); `UIStil.*` (colors/fonts, pre-existing from earlier plans) for visual consistency with the rest of the app.
- Produces: a modal dialog shown once before `StokVeritabani.Baslat()` on any workstation that hasn't been configured yet.

- [ ] **Step 1: Write `FormSunucuAyarlari.cs`**

This is a small, code-built dialog (no `Designer.cs`, following the same hand-written-C# pattern already used by `FormSilOnay.cs`/`FormLowStockLimit.cs` in this codebase) that collects the five connection fields and calls `BaglantiAyarlari.Kaydet(...)` on success.

```csharp
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    /// <summary>
    /// İlk çalıştırmada (bağlantı yapılandırma dosyası yoksa) gösterilen,
    /// fabrika içi paylaşımlı PostgreSQL sunucusu bilgilerini toplayan diyalog.
    /// </summary>
    public class FormSunucuAyarlari : Form
    {
        private TextBox txtSunucu, txtVeritabani, txtKullaniciAdi;
        private TextBox txtSifre;
        private NumericUpDown nudPort;

        public FormSunucuAyarlari()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text            = "Sunucu Bağlantı Ayarları";
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.BackColor       = UIStil.GriAcik;
            this.ClientSize      = new Size(420, 400);
            this.Font            = new Font("Segoe UI", 9.5f);

            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = UIStil.LacivertKoyu };
            panelHeader.Controls.Add(new Label
            {
                Text = "Fabrika Sunucu Bağlantısı",
                Font = UIStil.Baslik(14f),
                ForeColor = UIStil.Beyaz, AutoSize = true, Location = new Point(20, 14)
            });
            panelHeader.Controls.Add(new Label
            {
                Text = "Bu bilgisayarın hangi PostgreSQL sunucusuna bağlanacağını girin.",
                Font = UIStil.AltBaslik(8.5f),
                ForeColor = Color.FromArgb(189, 195, 199), AutoSize = true, Location = new Point(22, 42)
            });

            int y = 90;
            Label Etiket(string t) => new Label
            {
                Text = t, Location = new Point(20, y), AutoSize = true,
                Font = UIStil.Etiket(9f), ForeColor = UIStil.GriMetin
            };

            this.Controls.Add(Etiket("Sunucu (IP veya ad):"));
            txtSunucu = new TextBox { Location = new Point(20, y + 20), Size = new Size(380, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtSunucu);
            y += 56;

            this.Controls.Add(Etiket("Port:"));
            nudPort = new NumericUpDown { Location = new Point(20, y + 20), Size = new Size(120, 26), Minimum = 1, Maximum = 65535, Value = 5432, BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(nudPort);
            y += 56;

            this.Controls.Add(Etiket("Veritabanı Adı:"));
            txtVeritabani = new TextBox { Location = new Point(20, y + 20), Size = new Size(380, 26), Text = "fabrikastok", BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtVeritabani);
            y += 56;

            this.Controls.Add(Etiket("Kullanıcı Adı:"));
            txtKullaniciAdi = new TextBox { Location = new Point(20, y + 20), Size = new Size(380, 26), BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtKullaniciAdi);
            y += 56;

            this.Controls.Add(Etiket("Şifre:"));
            txtSifre = new TextBox { Location = new Point(20, y + 20), Size = new Size(380, 26), BorderStyle = BorderStyle.FixedSingle, UseSystemPasswordChar = true };
            this.Controls.Add(txtSifre);

            var btnKaydet = new Button
            {
                Text = "Kaydet ve Devam Et", BackColor = UIStil.Mavi, ForeColor = UIStil.Beyaz,
                FlatStyle = FlatStyle.Flat, Font = UIStil.ButonYazi(10f),
                Size = new Size(380, 44), Location = new Point(20, 340), Cursor = Cursors.Hand
            };
            btnKaydet.FlatAppearance.BorderSize = 0;
            btnKaydet.Click += BtnKaydet_Click;
            this.Controls.Add(btnKaydet);

            this.Controls.Add(panelHeader);
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSunucu.Text) ||
                string.IsNullOrWhiteSpace(txtVeritabani.Text) ||
                string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) ||
                string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BaglantiAyarlari.Kaydet(
                txtSunucu.Text.Trim(),
                (int)nudPort.Value,
                txtVeritabani.Text.Trim(),
                txtKullaniciAdi.Text.Trim(),
                txtSifre.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
```

- [ ] **Step 2: Wire it into `Program.cs`**

Find:

```csharp
            try { StokVeritabani.Baslat(); }
            catch (Exception hata)
            {
                MessageBox.Show("Veritabanı başlatılamadı:\n" + hata.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
```

Replace with:

```csharp
            if (!BaglantiAyarlari.DosyaVarMi())
            {
                using (var ayarForm = new FormSunucuAyarlari())
                {
                    if (ayarForm.ShowDialog() != DialogResult.OK)
                        return; // Kullanıcı iptal etti — uygulama açılmaz
                }
            }

            try { StokVeritabani.Baslat(); }
            catch (Exception hata)
            {
                MessageBox.Show("Veritabanı başlatılamadı:\n" + hata.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
```

- [ ] **Step 3: Self-review**

Confirm `FormSunucuAyarlari` has no `Designer.cs` (matches the established hand-written-C# pattern for small dialogs in this codebase — `FormSilOnay`, `FormLowStockLimit`). Confirm `Program.cs`'s `Main()` shows this dialog BEFORE `StokVeritabani.Baslat()` is ever called, and only when `BaglantiAyarlari.DosyaVarMi()` is false — a workstation that's already configured skips straight to `Baslat()` as before. Confirm cancelling the dialog (`DialogResult != OK`) exits `Main()` cleanly via `return;` without calling `Baslat()` with no config.

- [ ] **Step 4: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormSunucuAyarlari.cs FabrikaStokTakipUygulamasi/Program.cs
git commit -m "feat: add first-run server-connection setup dialog"
```

---

### Task 6: `KURULUM.md` — fabrika içi PostgreSQL kurulum kılavuzu

**Files:**
- Modify: `KURULUM.md`

- [ ] **Step 1: Replace the SQLite-specific setup section with an on-premise PostgreSQL section**

Read the current `KURULUM.md` and replace its "Notlar" section (which currently describes the local SQLite file and default users) with a new structure covering both the one-time server setup (done once, on whichever factory PC will host the database) and the per-workstation setup (done on every computer that runs the app):

```markdown
# Fabrika Stok Takip Uygulaması – Kurulum (.NET 8 Sürümü)

## Mimari

Bu uygulama artık **fabrika içi paylaşımlı bir PostgreSQL sunucusu** kullanır — tüm istasyonlar (depo, kalite kontrol, mühendislik) aynı veritabanına yerel ağ (LAN) üzerinden bağlanır. İnternet bağlantısı gerekmez.

## 1) Sunucu kurulumu (SADECE BİR KEZ, bir bilgisayarda/sunucuda yapılır)

1. Fabrikada sürekli açık kalacak bir bilgisayar seçin (bu, veritabanı sunucusu olacak).
2. [postgresql.org/download/windows](https://www.postgresql.org/download/windows/) adresinden PostgreSQL'i indirip kurun (kurulum sırasında bir "postgres" kullanıcı şifresi belirlemeniz istenecek — bunu not edin).
3. Kurulum sırasında gelen **pgAdmin** aracını açın, yeni bir veritabanı oluşturun (örn. adı: `fabrikastok`) ve uygulamanın kullanacağı ayrı bir kullanıcı oluşturun (örn. kullanıcı adı: `stokuygulamasi`, güçlü bir şifre belirleyin).
4. PostgreSQL'in yerel ağdan bağlantı kabul etmesi için:
   - `C:\Program Files\PostgreSQL\<sürüm>\data\postgresql.conf` dosyasında `listen_addresses = '*'` satırını etkinleştirin.
   - Aynı klasördeki `pg_hba.conf` dosyasına, fabrika ağınızın IP aralığına izin veren bir satır ekleyin, örneğin:
     ```
     host    all             all             192.168.1.0/24          scram-sha-256
     ```
     (Kendi ağınızın adresine göre uyarlayın — IT ekibinize danışın.)
   - PostgreSQL servisini yeniden başlatın (Hizmetler / Services içinden).
5. Windows Güvenlik Duvarı'nda 5432 portuna gelen bağlantılara izin verin (Gelen Kurallar / Inbound Rules).
6. Bu bilgisayarın yerel ağ IP adresini not edin (`ipconfig` ile görülebilir, örn. `192.168.1.50`) — her istasyonun bağlantı ayarlarında bu adres kullanılacak.

## 2) Her istasyonda (uygulamanın çalışacağı her bilgisayarda)

1. `FabrikaStokTakipUygulamasi.sln` dosyasını **Visual Studio 2022** ile açın.
2. **Ctrl + Shift + B** ile derleyin (NuGet paketleri otomatik indirilir).
3. **F5** ile çalıştırın.
4. İlk çalıştırmada "Sunucu Bağlantı Ayarları" ekranı açılır — sunucu adımında not ettiğiniz IP adresini, portu (5432), veritabanı adını ve uygulama kullanıcısının bilgilerini girin, **Kaydet ve Devam Et**'e basın. Bu bilgi bu bilgisayara özel, şifrelenmiş olarak saklanır (`%AppData%\FabrikaStokTakipUygulamasi\baglanti.json`) — bir daha sorulmaz.
5. Varsayılan kullanıcılarla giriş yapabilirsiniz: `emir/1234`, `barkan/1234` (Depo Personeli), `anil/1234`, `goksu/1234` (Mühendis), `admin/admin` (Admin) — şifreler veritabanında hash'lenerek saklanır.

## Notlar

- Sunucu bilgisayarı kapalıyken hiçbir istasyon uygulamayı kullanamaz — sunucunun sürekli açık kalması operasyonel bir gerekliliktir.
- Bağlantı bilgilerini yanlış girdiyseniz, `%AppData%\FabrikaStokTakipUygulamasi\baglanti.json` dosyasını silip uygulamayı yeniden başlatarak kurulum ekranını tekrar tetikleyebilirsiniz.
```

- [ ] **Step 2: Self-review**

Confirm no remaining reference to `Microsoft.Data.Sqlite`/local SQLite file paths anywhere in the updated `KURULUM.md`. Confirm the default users table (username/password/role) matches exactly what `KullaniciYonetici.cs`'s `TabloyuKur` seeds (Task 4) — `emir/1234`, `barkan/1234`, `anil/1234`, `goksu/1234`, `admin/admin`.

- [ ] **Step 3: Commit**

```bash
git add KURULUM.md
git commit -m "docs: document on-premise PostgreSQL server setup and per-workstation configuration"
```

---

### Task 7: Manuel doğrulama (insan operatör — gerçek fabrika sunucusu gerekir)

**This task cannot be executed by an agent in this environment** (no .NET SDK, no Windows, no real PostgreSQL server here). Checklist for the human operator, once Tasks 1–6 are committed and CI is green, AND a real on-premise PostgreSQL server has been set up per Task 6's guide.

- [ ] **Step 1: Build**

Open the solution in Visual Studio, `Ctrl+Shift+B`. Expected: builds with zero errors (CI should already confirm this, but double-check locally).

- [ ] **Step 2: First-run setup**

Delete any existing `%AppData%\FabrikaStokTakipUygulamasi\baglanti.json` (from testing) and run the app. Expected: the "Sunucu Bağlantı Ayarları" dialog appears before login. Enter the real server's connection info, save. Expected: `StokVeritabani.Baslat()` succeeds and creates the tables on the real server, the login screen appears.

- [ ] **Step 3: Multi-station simulation**

If possible, run the app from two different computers on the same LAN, both pointed at the same server. Add a product from one, and confirm it appears (after a refresh/navigation) on the other — this proves the shared-database behavior actually works, which is the whole point of this phase.

- [ ] **Step 4: Regression check**

Confirm login, product add/edit/delete, search, low stock, dashboard, and the admin panel all still work exactly as they did on SQLite — this phase changed only the storage layer, not any feature.

- [ ] **Step 5: Report back**

Report which of the above passed/failed, and the exact error text for anything that failed (especially any Npgsql connection error — these usually point to a `pg_hba.conf`/firewall misconfiguration on the server, not an app bug).
