# FabrikaStokTakipUygulamasi — SQLite Geçişi, Hata Düzeltmeleri ve Kurumsal UI Yenilemesi — Uygulama Planı

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Revert `FabrikaStokTakipUygulamasi` (WinForms, .NET 8) from Railway PostgreSQL back to local SQLite, fix the bugs/security issues found during review, and refresh the UI into a consistent corporate look (navy/blue palette, Segoe MDL2 Assets glyph icons, no emoji).

**Architecture:** Data layer (`StokVeritabani.cs`, `KullaniciYonetici.cs`) is rewritten from Npgsql to `Microsoft.Data.Sqlite`, storing a single file at `%AppData%\FabrikaStokTakipUygulamasi\stok.db`. A new `Guvenlik.cs` centralizes PBKDF2 password hashing. A new `UIStil.cs` centralizes the corporate color palette, typography, and Segoe MDL2 Assets glyph constants, applied consistently across every form (both code-built forms and WinForms-Designer-based forms).

**Tech Stack:** .NET 8 (`net8.0-windows`), WinForms, `Microsoft.Data.Sqlite`, `System.Security.Cryptography.ProtectedData` (DPAPI), `System.Windows.Forms.DataVisualization` (Chart control).

**Spec:** `docs/superpowers/specs/2026-07-22-sqlite-migration-ui-refresh-design.md`

## Global Constraints

- Target framework stays `net8.0-windows`; do not change `TargetFramework`, `UseWindowsForms`, or other existing `<PropertyGroup>` settings in `FabrikaStokTakipUygulamasi.csproj` beyond what each task specifies.
- No Railway/Postgres data migration — the new SQLite database starts empty; default users are seeded fresh.
- No dark mode / theme switching.
- No real animations (fade/slide) — only hover/pressed color feedback via `FlatAppearance`.
- Password hashing must use the built-in `System.Security.Cryptography.Rfc2898DeriveBytes` (PBKDF2) — no external hashing NuGet package.
- Icons are Segoe MDL2 Assets glyph-font characters only — no bundled icon image files, no emoji.
- **This development environment has no .NET SDK installed and cannot run `dotnet build` or `dotnet test`.** Every task's "verification" step is therefore a self-review (re-read the diff, check it against the task's stated rules) rather than a compiler/test run. The final task in this plan is a manual QA checklist for the human operator to run in Visual Studio on Windows.
- Every task ends with a `git commit` (the repo was already initialized with a baseline commit).
- All new/changed identifiers, comments, and UI strings follow the existing codebase convention of Turkish method/variable names with English data labels (e.g. `UrunEkle`, `"Customer"` column header) — do not switch the whole codebase to English.

---

## File Structure (new/changed files)

| File | Change |
|---|---|
| `FabrikaStokTakipUygulamasi/FabrikaStokTakipUygulamasi.csproj` | Swap `Npgsql` → `Microsoft.Data.Sqlite`; add `System.Security.Cryptography.ProtectedData`, `System.Windows.Forms.DataVisualization` |
| `FabrikaStokTakipUygulamasi/Guvenlik.cs` | **New.** PBKDF2 password hash/verify helper |
| `FabrikaStokTakipUygulamasi/UIStil.cs` | **New.** Shared corporate colors, fonts, Segoe MDL2 Assets glyph constants, DataGridView/Button styling helpers |
| `FabrikaStokTakipUygulamasi/StokVeritabani.cs` | Rewritten: SQLite connection/schema, `UrunEkle` now returns the new row's `int Id` |
| `FabrikaStokTakipUygulamasi/KullaniciYonetici.cs` | Rewritten: SQLite connection, PBKDF2 hashing, blank-password-keeps-current update semantics |
| `FabrikaStokTakipUygulamasi/OturumAyarlari.cs` | DPAPI-encrypt the stored "keep me logged in" password |
| `FabrikaStokTakipUygulamasi/FormUrunEkle.cs` | Use `UrunEkle`'s returned Id instead of re-querying; delete dead Designer-artifact handlers |
| `FabrikaStokTakipUygulamasi/FormAdmin.cs` | Password field no longer shows plaintext/hash on edit; blank means "keep current"; fixed-length dot mask |
| `FabrikaStokTakipUygulamasi/Form1.cs`, `Form1.Designer.cs` | Corporate glyphs on sidebar nav buttons |
| `FabrikaStokTakipUygulamasi/FormLogin.cs`, `FormLogin.Designer.cs` | Corporate glyphs, minor visual cleanup |
| `FabrikaStokTakipUygulamasi/FormDashboard.cs`, `FormDashboard.Designer.cs` | Full visual rebuild: navy/blue stat cards with icons, chart |
| `FabrikaStokTakipUygulamasi/FormUrunler.cs`, `FormUrunler.Designer.cs` | Corporate colors/icons on toolbar and grid |
| `FabrikaStokTakipUygulamasi/FormArama.cs`, `FormArama.Designer.cs` | Corporate background/icons |
| `FabrikaStokTakipUygulamasi/FormUrunEkle.Designer.cs` | Corporate colors/fonts/icons |
| `FabrikaStokTakipUygulamasi/FormUrunDuzenle.cs`, `FormLowStock.cs`, `FormLowStockSecim.cs`, `FormLowStockLimit.cs`, `FormUrunDetay.cs`, `FormSilOnay.cs`, `FormAdmin.cs` | Swap local color constants / emoji for `UIStil` glyphs and colors |
| `KURULUM.md` | Updated to reflect SQLite-only setup |
| `RAILWAY_DATABASE_KURULUM.md` | Deleted |

**Compile-state note:** Tasks 1, 3, and 4 swap the database provider across interdependent files (`FabrikaStokTakipUygulamasi.csproj`, `StokVeritabani.cs`, `KullaniciYonetici.cs`). The project will **not compile** in the commits between Task 1 and Task 4 — e.g. right after Task 1 removes the `Npgsql` package, `KullaniciYonetici.cs` still references `NpgsqlConnection` until Task 4 rewrites it. This is expected and unavoidable for a single-provider swap touching two mutually-referencing files; it is not a sign of a mistake. Compilation is only expected to succeed again starting at Task 4's commit, and should stay green for every commit from Task 4 onward (verified by the human operator in Task 17, since this environment cannot run a compiler — see Global Constraints).

---

### Task 1: NuGet paket geçişi

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FabrikaStokTakipUygulamasi.csproj`

**Interfaces:**
- Produces: `Microsoft.Data.Sqlite` namespace available to all later tasks; `System.Security.Cryptography.ProtectedData` (DPAPI) available to Task 5; `System.Windows.Forms.DataVisualization.Charting` available to Task 11.

- [ ] **Step 1: Replace the package references**

Current content of `FabrikaStokTakipUygulamasi/FabrikaStokTakipUygulamasi.csproj`:

```xml
  <ItemGroup>
    <PackageReference Include="Npgsql" Version="8.0.5" />
  </ItemGroup>
```

Replace with:

```xml
  <ItemGroup>
    <PackageReference Include="Microsoft.Data.Sqlite" Version="8.0.10" />
    <PackageReference Include="System.Security.Cryptography.ProtectedData" Version="8.0.0" />
    <PackageReference Include="WinForms.DataVisualization" Version="1.10.2" /> <!-- corrected post-CI: see fix commit d5a36bc -->
  </ItemGroup>
```

- [ ] **Step 2: Self-review**

Read the file back and confirm: no remaining `Npgsql` reference anywhere in the `.csproj`, the three new packages are present with those exact versions, and the rest of the `<PropertyGroup>` is untouched.

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FabrikaStokTakipUygulamasi.csproj
git commit -m "build: replace Npgsql with Microsoft.Data.Sqlite, add DPAPI and Chart packages"
```

---

### Task 2: Şifre hash'leme yardımcı sınıfı (`Guvenlik.cs`)

**Files:**
- Create: `FabrikaStokTakipUygulamasi/Guvenlik.cs`

**Interfaces:**
- Produces: `public static class Guvenlik` with `string SifreyiHashle(string duzMetinSifre)` and `bool SifreDogrula(string duzMetinSifre, string saklananHash)` — consumed by Task 4 (`KullaniciYonetici.cs`).

- [ ] **Step 1: Write the file**

```csharp
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
```

- [ ] **Step 2: Self-review**

Confirm the file compiles conceptually: `Rfc2898DeriveBytes.Pbkdf2` is a static method available since .NET 6 (no instance needed), `CryptographicOperations.FixedTimeEquals` takes two `ReadOnlySpan<byte>` (byte arrays implicitly convert). Confirm `SifreDogrula` returns `false` (not throws) for any malformed stored hash — this matters because Task 4 seeds default users and any old/garbage value must fail closed, not crash the login form.

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/Guvenlik.cs
git commit -m "feat: add PBKDF2 password hashing helper"
```

---

### Task 3: `StokVeritabani.cs` — SQLite'a geçiş

**Files:**
- Modify (full rewrite): `FabrikaStokTakipUygulamasi/StokVeritabani.cs`

**Interfaces:**
- Consumes: `Guvenlik` is not used directly here (only by `KullaniciYonetici`).
- Produces: `public static SqliteConnection YeniBaglanti()` (consumed by `KullaniciYonetici.cs`), `public static int UrunEkle(Urun urun)` — **return type changed from `void` to `int`, returns the new row's Id** (consumed by Task 6, `FormUrunEkle.cs`), plus unchanged signatures for `UrunSil`, `UrunGuncelle`, `TumUrunler`, `LowStockLimitGuncelle`, `HareketKaydet`, `HareketleriGetir`, `ToplamUrun`, `KritikStokSayisi`, `FirmaSayisi`, and `public static void Baslat()`.

- [ ] **Step 1: Replace the entire file**

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

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
        private static string _baglantiString;

        /// <summary>%AppData%\FabrikaStokTakipUygulamasi\stok.db — tek makine, dosya tabanlı veritabanı.</summary>
        private static string BaglantiString
        {
            get
            {
                if (_baglantiString != null) return _baglantiString;

                string klasor = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "FabrikaStokTakipUygulamasi");
                Directory.CreateDirectory(klasor);
                string dosyaYolu = Path.Combine(klasor, "stok.db");

                _baglantiString = new SqliteConnectionStringBuilder
                {
                    DataSource = dosyaYolu,
                    Mode = SqliteOpenMode.ReadWriteCreate
                }.ConnectionString;

                return _baglantiString;
            }
        }

        /// <summary>
        /// KullaniciYonetici ve diğer sınıfların kullanımı için açık bağlantı nesnesi döndürür.
        /// Çağıran taraf Open() ve Dispose() çağrısından sorumludur.
        /// </summary>
        public static SqliteConnection YeniBaglanti() => new SqliteConnection(BaglantiString);

        public static void Baslat()
        {
            using (var baglanti = new SqliteConnection(BaglantiString))
            {
                baglanti.Open();

                string urunlerTablosu = @"
                    CREATE TABLE IF NOT EXISTS ""Urunler"" (
                        ""Id""                INTEGER PRIMARY KEY AUTOINCREMENT,
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
                        ""SertifikaPdf""      BLOB,
                        ""SertifikaDosyaAdi"" TEXT
                    );";
                using (var k = new SqliteCommand(urunlerTablosu, baglanti)) k.ExecuteNonQuery();

                MigrationKolonEkleTablo(baglanti, "Urunler", "LowStockLimit",     "INTEGER NOT NULL DEFAULT -1");
                MigrationKolonEkleTablo(baglanti, "Urunler", "SertifikaPdf",      "BLOB");
                MigrationKolonEkleTablo(baglanti, "Urunler", "SertifikaDosyaAdi", "TEXT");

                string hareketTablosu = @"
                    CREATE TABLE IF NOT EXISTS ""StokHareketleri"" (
                        ""Id""           INTEGER PRIMARY KEY AUTOINCREMENT,
                        ""Tarih""        TEXT NOT NULL,
                        ""KullaniciAdi"" TEXT NOT NULL,
                        ""UrunId""       INTEGER NOT NULL,
                        ""UrunAdi""      TEXT NOT NULL,
                        ""EskiStok""     INTEGER NOT NULL,
                        ""YeniStok""     INTEGER NOT NULL,
                        ""Fark""         INTEGER NOT NULL,
                        ""IslemAdi""     TEXT NOT NULL DEFAULT 'Stok Eklendi'
                    );";
                using (var k = new SqliteCommand(hareketTablosu, baglanti)) k.ExecuteNonQuery();

                MigrationKolonEkleTablo(baglanti, "StokHareketleri", "IslemAdi", "TEXT NOT NULL DEFAULT 'Stok Eklendi'");

                // Kullanıcılar tablosunu oluştur ve varsayılan kullanıcıları ekle
                KullaniciYonetici.TabloyuKur(baglanti);
            }
        }

        private static bool KolonVarMi(SqliteConnection baglanti, string tablo, string kolon)
        {
            using (var k = new SqliteCommand($"PRAGMA table_info(\"{tablo}\");", baglanti))
            using (var oku = k.ExecuteReader())
            {
                while (oku.Read())
                {
                    if (string.Equals(oku["name"].ToString(), kolon, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }

        private static void MigrationKolonEkleTablo(SqliteConnection baglanti, string tablo, string kolon, string tip)
        {
            if (KolonVarMi(baglanti, tablo, kolon)) return;
            using (var k = new SqliteCommand($"ALTER TABLE \"{tablo}\" ADD COLUMN \"{kolon}\" {tip};", baglanti))
                k.ExecuteNonQuery();
        }

        /// <summary>Ürünü ekler ve yeni satırın Id'sini döndürür.</summary>
        public static int UrunEkle(Urun urun)
        {
            using (var baglanti = new SqliteConnection(BaglantiString))
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
                         @EklenmeTarihi, @SertifikaPdf, @SertifikaDosyaAdi);
                    SELECT last_insert_rowid();";

                using (var k = new SqliteCommand(sql, baglanti))
                {
                    ParametreleriEkle(k, urun, false);
                    return Convert.ToInt32(k.ExecuteScalar());
                }
            }
        }

        public static void UrunSil(int id)
        {
            using (var baglanti = new SqliteConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new SqliteCommand("DELETE FROM \"Urunler\" WHERE \"Id\"=@Id;", baglanti))
                {
                    k.Parameters.AddWithValue("@Id", id);
                    k.ExecuteNonQuery();
                }
            }
        }

        public static void UrunGuncelle(Urun urun)
        {
            using (var baglanti = new SqliteConnection(BaglantiString))
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

                using (var k = new SqliteCommand(sql, baglanti))
                {
                    ParametreleriEkle(k, urun, true);
                    k.ExecuteNonQuery();
                }
            }
        }

        private static void ParametreleriEkle(SqliteCommand k, Urun urun, bool idEkle)
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
            using (var baglanti = new SqliteConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new SqliteCommand("SELECT * FROM \"Urunler\" ORDER BY \"Id\" DESC;", baglanti))
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
            using (var baglanti = new SqliteConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new SqliteCommand("UPDATE \"Urunler\" SET \"LowStockLimit\"=@Limit WHERE \"Id\"=@Id;", baglanti))
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

            using (var baglanti = new SqliteConnection(BaglantiString))
            {
                baglanti.Open();
                string sql = @"
                    INSERT INTO ""StokHareketleri""
                        (""Tarih"", ""KullaniciAdi"", ""UrunId"", ""UrunAdi"", ""EskiStok"", ""YeniStok"", ""Fark"", ""IslemAdi"")
                    VALUES
                        (@Tarih, @KullaniciAdi, @UrunId, @UrunAdi, @EskiStok, @YeniStok, @Fark, @IslemAdi);";
                using (var k = new SqliteCommand(sql, baglanti))
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
            using (var baglanti = new SqliteConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new SqliteCommand("SELECT * FROM \"StokHareketleri\" ORDER BY \"Id\" DESC;", baglanti))
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
            using (var baglanti = new SqliteConnection(BaglantiString))
            {
                baglanti.Open();
                using (var k = new SqliteCommand(sql, baglanti))
                    return Convert.ToInt32(k.ExecuteScalar());
            }
        }
    }
}
```

- [ ] **Step 2: Self-review**

Check against `KullaniciYonetici.cs` (not yet rewritten — that's Task 4): `KullaniciYonetici.TabloyuKur` is called with a `SqliteConnection` argument here — Task 4 must change `TabloyuKur`'s parameter type from `NpgsqlConnection` to `SqliteConnection` to match, otherwise this won't compile. Note this dependency explicitly when starting Task 4.

Confirm every `BYTEA` → `BLOB`, every `SERIAL PRIMARY KEY` → `INTEGER PRIMARY KEY AUTOINCREMENT`, and that `UrunEkle` is `int` returning (not `void`).

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/StokVeritabani.cs
git commit -m "feat: migrate StokVeritabani from PostgreSQL to local SQLite"
```


---

### Task 4: `KullaniciYonetici.cs` — SQLite + şifre hash'leme

**Files:**
- Modify (full rewrite): `FabrikaStokTakipUygulamasi/KullaniciYonetici.cs`

**Interfaces:**
- Consumes: `Guvenlik.SifreyiHashle(string)`, `Guvenlik.SifreDogrula(string, string)` from Task 2; `StokVeritabani.YeniBaglanti()` returning `SqliteConnection` from Task 3.
- Produces: `public static void TabloyuKur(SqliteConnection baglanti)` (parameter type changed from `NpgsqlConnection` — called by `StokVeritabani.Baslat()` from Task 3), `GirisYap(string, string)`, `Cikis()`, `TumKullanicilar()`, `YeniKullaniciEkle(string, string, KullaniciRol)`, and **changed semantics**: `KullaniciGuncelle(string eskiAd, string yeniAd, string yeniSifre, KullaniciRol rol)` — if `yeniSifre` is empty/null, the stored password hash is left unchanged (consumed by Task 7, `FormAdmin.cs`).

- [ ] **Step 1: Replace the entire file**

```csharp
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
```

- [ ] **Step 2: Self-review**

Cross-check `Task 3`'s `StokVeritabani.Baslat()` calls `KullaniciYonetici.TabloyuKur(baglanti)` with a `SqliteConnection` — the parameter type here now matches. Confirm `KullaniciGuncelle`'s new blank-password branch is what Task 7 (`FormAdmin.cs`) will rely on.

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/KullaniciYonetici.cs
git commit -m "feat: migrate KullaniciYonetici to SQLite with PBKDF2 password hashing"
```

---

### Task 5: "Oturumu açık tut" dosyasını DPAPI ile şifrele

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/OturumAyarlari.cs`

**Interfaces:**
- Produces: same public signatures as before — `Kaydet(bool, string, string)`, `Oku()` returning `(bool acikTut, string kullaniciAdi, string sifre)` — callers in `FormLogin.cs` and `Form1.cs` are unchanged.

- [ ] **Step 1: Replace the entire file**

```csharp
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
```

- [ ] **Step 2: Self-review**

Note the JSON field was renamed from `Sifre` to `SifreSifreliBase64` — this is a breaking change to the on-disk format, but that's fine: it's a small local cache file, not the database; any pre-existing `oturum.json` from before this change will simply fail to deserialize into the new shape's field and `Oku()`'s catch-all will make it fall back to "not logged in", which is safe (user just re-enters credentials once).

Confirm `ProtectedData` and `DataProtectionScope` resolve from the `System.Security.Cryptography.ProtectedData` package added in Task 1.

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/OturumAyarlari.cs
git commit -m "fix: encrypt the \"keep me logged in\" password with Windows DPAPI instead of storing it in plaintext"
```

---

### Task 6: `FormUrunEkle.cs` — kırılgan Id varsayımını düzelt, ölü kodu temizle

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormUrunEkle.cs`

**Interfaces:**
- Consumes: `StokVeritabani.UrunEkle(Urun)` now returns `int` (Task 3).

- [ ] **Step 1: Fix the fragile "last inserted product" assumption**

Current code (`FormUrunEkle.cs`, inside `btnUrunEkle_Click`):

```csharp
            try
            {
                StokVeritabani.UrunEkle(urun);
                var tumUrunler = StokVeritabani.TumUrunler();
                if (tumUrunler.Count > 0)
                    StokVeritabani.HareketKaydet(tumUrunler[0].Id, tumUrunler[0].UrunCinsi, 0, adet);
```

Replace with:

```csharp
            try
            {
                int yeniId = StokVeritabani.UrunEkle(urun);
                StokVeritabani.HareketKaydet(yeniId, urun.UrunCinsi, 0, adet);
```

- [ ] **Step 2: Delete the dead Designer-artifact handlers**

Confirmed via `grep -n "label1_Click\|label2_Click\|label3_Click\|label4_Click\|label5_Click\|textBox1_TextChanged\|btnKaydet_Click" FabrikaStokTakipUygulamasi/FormUrunEkle.Designer.cs` that none of these names appear in the Designer file — they are unwired, unused methods. Delete this block from the end of `FormUrunEkle.cs`:

```csharp
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void btnKaydet_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click_1(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void textBox1_TextChanged_1(object sender, EventArgs e) { }
        private void label1_Click_2(object sender, EventArgs e) { }
        private void textBox1_TextChanged_2(object sender, EventArgs e) { }
```

(Keep the line right before it, `private void btnTemizle_Click(object sender, EventArgs e) { TemizleFormulari(); }`, and the closing `}` braces for the class/namespace.)

- [ ] **Step 3: Self-review**

Re-read the file: `btnUrunEkle_Click` no longer calls `TumUrunler()` at all in its success path; `HareketKaydet` receives the real new Id. No remaining reference to any of the twelve deleted dead methods anywhere else in the file (they had no other callers — they were only invoked via Designer event wiring, which doesn't exist).

- [ ] **Step 4: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormUrunEkle.cs
git commit -m "fix: use UrunEkle's returned Id instead of re-querying, remove dead Designer-artifact handlers"
```

---

### Task 7: `FormAdmin.cs` — şifre alanı artık düz metin göstermiyor

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormAdmin.cs`

**Interfaces:**
- Consumes: `KullaniciYonetici.KullaniciGuncelle` — blank `yeniSifre` now means "keep current" (Task 4).

- [ ] **Step 1: Stop showing the stored password hash when editing a user**

Current code (inside `AcKullaniciFrm`):

```csharp
            var lblSifre = Lbl(LangManager.T("admin.kulyeni.sifre"), 70);
            // Düzenle modunda mevcut şifre açık gösterilir; yeni kullanıcıda maskelenir
            bool sifreMaskeli = (hedef == null);
            var txtSifre = Txt(hedef?.Sifre ?? "", 88, sifreMaskeli);
```

Replace with:

```csharp
            var lblSifre = Lbl(yeni
                ? LangManager.T("admin.kulyeni.sifre")
                : (LangManager.Ingilizce ? "New Password (leave blank to keep current)"
                                         : "Yeni Şifre (boş bırakılırsa değişmez)"), 70);
            // Güvenlik: saklanan şifre hash'i hiçbir zaman ekranda gösterilmez.
            // Alan her zaman boş başlar; düzenleme modunda boş bırakılırsa mevcut şifre korunur.
            var txtSifre = Txt("", 88, pwd: true);
```

- [ ] **Step 2: Allow blank password only in edit mode**

Current code (inside the same method, `btnKaydet.Click` handler):

```csharp
                if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(sifre))
                {
                    MessageBox.Show(LangManager.T("admin.kulyeni.bos"),
                        LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
```

Replace with:

```csharp
                bool sifreZorunlu = yeni; // Yeni kullanıcıda şifre şart; düzenlemede boş = değiştirme
                if (string.IsNullOrWhiteSpace(ad) || (sifreZorunlu && string.IsNullOrWhiteSpace(sifre)))
                {
                    MessageBox.Show(LangManager.T("admin.kulyeni.bos"),
                        LangManager.T("genel.uyari"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
```

- [ ] **Step 3: Fix the password dot-mask length in the users grid**

Current code (inside `KullanicilariYukle`):

```csharp
                string sifre = new string('•', k.Sifre.Length);
```

Replace with:

```csharp
                // Hash uzunluğu gerçek şifre uzunluğunu ele vermesin diye sabit sayıda nokta gösterilir.
                string sifre = new string('•', 8);
```

- [ ] **Step 4: Self-review**

Re-read the full `AcKullaniciFrm` method: for a new user (`yeni == true`), password is still required (unchanged behavior). For editing an existing user, leaving the password field empty now calls `KullaniciYonetici.KullaniciGuncelle(hedef.KullaniciAdi, ad, "", rol)` (empty string, not null — `Txt("", 88, ...)`'s `.Text` on an untouched `TextBox` is `""`), which Task 4's `KullaniciGuncelle` treats as "keep current password" via `string.IsNullOrEmpty(yeniSifre)`. Confirm no other place in `FormAdmin.cs` reads `k.Sifre` for display purposes besides the two spots just changed.

- [ ] **Step 5: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormAdmin.cs
git commit -m "fix: stop displaying stored password hash in the admin user-edit dialog"
```

---

### Task 8: Ortak kurumsal tema yardımcı sınıfı (`UIStil.cs`)

**Files:**
- Create: `FabrikaStokTakipUygulamasi/UIStil.cs`

**Interfaces:**
- Produces: `public static class UIStil` with color constants, font factory methods, a nested `public static class Glyph` (Segoe MDL2 Assets glyph string constants), and helper methods `FlatBuyuk(Button, Color, Color?)`, `SolIkonCiz(Graphics, string, Rectangle, Color, float)` (paints a glyph inside a button's left padding — see the code comment for why this is needed instead of concatenating the glyph into `Button.Text`), `IkonLabel(string, Color, float)` (a small Label showing just a glyph, for pairing next to a text Label), and `GridTemasi(DataGridView)`. Consumed by Tasks 9–15 (every form's UI polish).

- [ ] **Step 1: Write the file**

```csharp
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

        /// <summary>Flat, kenarlıksız, el imleçli buton stilini uygular.</summary>
        public static void FlatBuyuk(Button b, Color arka, Color? yazi = null)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = arka;
            b.ForeColor = yazi ?? Beyaz;
            b.Cursor = Cursors.Hand;
            b.Font = ButonYazi();
        }

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

        /// <summary>Tüm DataGridView'lerde kullanılan tutarlı kurumsal grid görünümü.</summary>
        public static void GridTemasi(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = Color.FromArgb(220, 220, 220);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Lacivert;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Beyaz;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Lacivert;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Beyaz;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 40;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 32;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(174, 214, 241);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
        }
    }
}
```

- [ ] **Step 2: Self-review**

Confirm every glyph constant is a single `\uXXXX` escape (not a surrogate pair — all chosen Segoe MDL2 Assets codepoints are in the Basic Multilingual Plane's Private Use Area, so a single `char`/UTF-16 unit is correct). Confirm `GridTemasi` only sets appearance properties, never `Dock`, `Location`, `Size`, or `Columns` — those stay owned by each form so this task cannot silently break any form's layout.

- [ ] **Step 3: Verify glyphs on Windows (manual, requires Visual Studio machine)**

The codepoints above were chosen from memory against the well-known Segoe MDL2 Assets set and were **not** rendered/confirmed in this development environment (no Windows/Character Map access here — see Global Constraints). Before relying on them: open **Character Map** (`charmap.exe`) on the Windows machine, select font **"Segoe MDL2 Assets"**, and check each codepoint in the table below renders the icon its comment describes. If a codepoint shows the wrong glyph or a blank box, replace just that one `\uXXXX` value in `UIStil.Glyph` with the correct codepoint from Character Map — every other task in this plan only ever references the glyph by name (e.g. `UIStil.Glyph.Kisi`), never by raw codepoint, so a swap here doesn't require touching any other file.

| Glyph name | Codepoint | Expected icon |
|---|---|---|
| `Kisi` | `E77B` | single person / contact |
| `Kisiler` | `E716` | multiple people |
| `Dokuman` | `E8A5` | page / document |
| `Kapat` | `E711` | X / cancel |
| `Sil` | `E74D` | trash can |
| `Dunya` | `E774` | globe |
| `Liste` | `E8FD` | bulleted list |
| `Duzenle` | `E70F` | pencil |
| `Ara` | `E721` | magnifying glass |
| `Ayarlar` | `E713` | gear |
| `Ekle` | `E710` | plus |
| `Uyarim` | `E7BA` | warning triangle |
| `Kutu` | `E7B8` | box/list |
| `DisaAktar` | `E898` | export/share arrow |
| `Onay` | `E73E` | checkmark |
| `Kilit` | `E72E` | padlock |
| `Goz` | `E890` | eye |
| `AnaSayfa` | `E80F` | house |

- [ ] **Step 4: Commit**

```bash
git add FabrikaStokTakipUygulamasi/UIStil.cs
git commit -m "feat: add shared corporate theme helper (colors, fonts, Segoe MDL2 Assets glyphs)"
```

---

### Task 9: Form1 (kenar çubuğu/nav) — kurumsal ikonlar

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/Form1.cs`
- Modify: `FabrikaStokTakipUygulamasi/Form1.Designer.cs`

**Interfaces:**
- Consumes: `UIStil.Glyph.*`, `UIStil.SolIkonCiz` from Task 8.

**Important technical note carried into every remaining UI task:** a `Button`/`Label`'s `Text` uses one single `Font` for the whole string. Segoe MDL2 Assets glyphs only render through that font — concatenating a glyph character into a control's existing `Text` (e.g. `"👤 " + name`, which worked because emoji have automatic font fallback) does **not** work for MDL2 glyphs and will show a blank box. Every icon addition in this and later tasks therefore either (a) uses `UIStil.SolIkonCiz` in a button's `Paint` handler after widening `Padding.Left`, or (b) uses a separate small `UIStil.IkonLabel` control placed next to the text control. Plain emoji that are being *removed* (not replaced by an icon) are simply deleted from the string.

- [ ] **Step 1: Remove the plain emoji from the username label (no icon needed here)**

In `Form1.cs`, inside the constructor:

```csharp
            if (KullaniciYonetici.AktifKullanici != null)
            {
                lblKullaniciAdi.Text = "👤 " + KullaniciYonetici.AktifKullanici.KullaniciAdi;
                lblKullaniciRol.Text = KullaniciYonetici.AktifKullanici.RolAdi;
            }
```

Replace with:

```csharp
            if (KullaniciYonetici.AktifKullanici != null)
            {
                lblKullaniciAdi.Text = KullaniciYonetici.AktifKullanici.KullaniciAdi;
                lblKullaniciRol.Text = KullaniciYonetici.AktifKullanici.RolAdi;
            }
```

- [ ] **Step 2: Wire icon painting for the six nav buttons and the logout button**

In `Form1.cs`, right after the `InitializeComponent();` call in the constructor, insert:

```csharp
            btnDashboard.Paint   += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.AnaSayfa, btnDashboard.ClientRectangle, Color.White);
            btnUrunler.Paint     += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kutu,      btnUrunler.ClientRectangle,   Color.White);
            btnUrunEkle.Paint    += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ekle,       btnUrunEkle.ClientRectangle,  Color.White);
            btnArama.Paint       += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ara,        btnArama.ClientRectangle,     Color.White);
            btnLowStock.Paint    += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Uyarim,     btnLowStock.ClientRectangle, Color.White);
            btnAdmin.Paint       += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ayarlar,    btnAdmin.ClientRectangle,    btnAdmin.ForeColor);
            btnOturumKapat.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kilit,      btnOturumKapat.ClientRectangle, Color.White);
```

(`Form1.cs` already has `using System.Drawing;` at the top — no new `using` needed.)

- [ ] **Step 3: Make room for the icons in `Form1.Designer.cs`**

For each of these five lines (one per nav button), change the `Padding` from `10` to `38` on the left:

```csharp
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnUrunler.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnUrunEkle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnArama.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnLowStock.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAdmin.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
```

become:

```csharp
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnUrunler.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnUrunEkle.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnArama.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnLowStock.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnAdmin.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
```

`btnOturumKapat` has no `Padding`/`TextAlign` set at all today. Find this block:

```csharp
            // btnOturumKapat - kullanıcı bilgisinin altında
            this.btnOturumKapat.Text = "⏻  Oturumu Kapat";
            this.btnOturumKapat.Location = new System.Drawing.Point(12, 60);
```

Replace with:

```csharp
            // btnOturumKapat - kullanıcı bilgisinin altında
            this.btnOturumKapat.Text = "Oturumu Kapat";
            this.btnOturumKapat.Padding = new System.Windows.Forms.Padding(38, 0, 0, 0);
            this.btnOturumKapat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOturumKapat.Location = new System.Drawing.Point(12, 60);
```

- [ ] **Step 4: Drop the leftover emoji from the Admin button's design-time text**

```csharp
            this.btnAdmin.Text = "🔐  Admin Paneli";
```

becomes:

```csharp
            this.btnAdmin.Text = "Admin Paneli";
```

(This static value is overwritten at runtime by `Form1.NavMetinleriGuncelle()`'s `btnAdmin.Text = LangManager.T("nav.admin");` — this change is only so the Designer view doesn't show a dead emoji if someone opens it in Visual Studio.)

- [ ] **Step 5: Self-review**

Re-read `Form1.cs`: confirm the six `Paint` handlers reference the exact button field names declared in `Form1.Designer.cs` (`btnDashboard`, `btnUrunler`, `btnUrunEkle`, `btnArama`, `btnLowStock`, `btnAdmin`, `btnOturumKapat` — all `public` fields per the Designer file's field list, so this compiles from `Form1.cs`). Confirm `NavMetinleriGuncelle()` in `Form1.cs` is untouched — it still sets plain label text (`btnDashboard.Text = LangManager.T("nav.dashboard")`, etc.) with no glyph concatenation, since the icon is now painted separately by the `Paint` handler and stays fixed regardless of language changes.

- [ ] **Step 6: Commit**

```bash
git add FabrikaStokTakipUygulamasi/Form1.cs FabrikaStokTakipUygulamasi/Form1.Designer.cs
git commit -m "style: add corporate Segoe MDL2 Assets icons to the sidebar nav"
```

---

### Task 10: FormLogin — kurumsal ikonlar ve renk hizalaması

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormLogin.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormLogin.Designer.cs`

**Interfaces:**
- Consumes: `UIStil.Glyph.*`, `UIStil.SolIkonCiz`, `UIStil.Mavi`, `UIStil.GriAcik` from Task 8.

- [ ] **Step 1: `btnGozToggle` is icon-only — set its font directly, no mixing needed**

In `FormLogin.Designer.cs`:

```csharp
            btnGozToggle.Text                   = "👁";
            btnGozToggle.Font                   = new System.Drawing.Font("Segoe UI", 9F);
```

Replace with:

```csharp
            btnGozToggle.Text                   = FabrikaStokTakipUygulamasi.UIStil.Glyph.Goz;
            btnGozToggle.Font                   = FabrikaStokTakipUygulamasi.UIStil.GlyphFont(11f);
```

- [ ] **Step 2: `btnDilToggle` mixes an icon with text — widen its padding and paint the globe separately**

In `FormLogin.Designer.cs`, find:

```csharp
            // btnDilToggle
            btnDilToggle.BackColor               = System.Drawing.Color.FromArgb(22, 160, 133);
            btnDilToggle.Cursor                  = System.Windows.Forms.Cursors.Hand;
            btnDilToggle.FlatAppearance.BorderSize = 0;
            btnDilToggle.FlatStyle               = System.Windows.Forms.FlatStyle.Flat;
            btnDilToggle.Font                    = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnDilToggle.ForeColor               = System.Drawing.Color.White;
            btnDilToggle.Location                = new System.Drawing.Point(1261, 703);
            btnDilToggle.Name                    = "btnDilToggle";
            btnDilToggle.Size                    = new System.Drawing.Size(100, 40);
            btnDilToggle.TabIndex                = 6;
            btnDilToggle.Text                    = "🌐 EN";
            btnDilToggle.UseVisualStyleBackColor = false;
            btnDilToggle.Click                  += btnDilToggle_Click;
```

Replace with:

```csharp
            // btnDilToggle
            btnDilToggle.BackColor               = System.Drawing.Color.FromArgb(22, 160, 133);
            btnDilToggle.Cursor                  = System.Windows.Forms.Cursors.Hand;
            btnDilToggle.FlatAppearance.BorderSize = 0;
            btnDilToggle.FlatStyle               = System.Windows.Forms.FlatStyle.Flat;
            btnDilToggle.Font                    = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnDilToggle.ForeColor               = System.Drawing.Color.White;
            btnDilToggle.Location                = new System.Drawing.Point(1261, 703);
            btnDilToggle.Name                    = "btnDilToggle";
            btnDilToggle.Padding                 = new System.Windows.Forms.Padding(26, 0, 0, 0);
            btnDilToggle.Size                    = new System.Drawing.Size(100, 40);
            btnDilToggle.TabIndex                = 6;
            btnDilToggle.Text                    = "EN";
            btnDilToggle.TextAlign               = System.Drawing.ContentAlignment.MiddleLeft;
            btnDilToggle.UseVisualStyleBackColor = false;
            btnDilToggle.Click                  += btnDilToggle_Click;
            btnDilToggle.Paint                  += (s, e) => FabrikaStokTakipUygulamasi.UIStil.SolIkonCiz(e.Graphics, FabrikaStokTakipUygulamasi.UIStil.Glyph.Dunya, btnDilToggle.ClientRectangle, System.Drawing.Color.White, 11f);
```

- [ ] **Step 3: Update the language-toggle text in `FormLogin.cs` (drop the emoji, icon is now painted, not text)**

```csharp
            btnDilToggle.Text = LangManager.AktifDil == LangManager.Dil.TR ? "🌐 EN" : "🌐 TR";
```

Replace with:

```csharp
            btnDilToggle.Text = LangManager.AktifDil == LangManager.Dil.TR ? "EN" : "TR";
```

- [ ] **Step 4: Align two off-palette colors to `UIStil`**

In `FormLogin.Designer.cs`:

```csharp
            BackColor           = System.Drawing.Color.FromArgb(244, 246, 248);
```
→
```csharp
            BackColor           = FabrikaStokTakipUygulamasi.UIStil.GriAcik;
```

and:

```csharp
            btnLogin.BackColor               = System.Drawing.Color.FromArgb(46, 134, 193);
```
→
```csharp
            btnLogin.BackColor               = FabrikaStokTakipUygulamasi.UIStil.Mavi;
```

- [ ] **Step 5: Self-review**

Confirm `btnGozToggle_Click` in `FormLogin.cs` (the show/hide password toggle) still only changes `ForeColor` — it never touches `Font` or `Text`, so it stays compatible with the new MDL2 font/glyph set in step 1. Confirm the panel's dark-navy branding area (`panelLeft`, `FromArgb(30, 42, 56)`) is left untouched — it's already a corporate-consistent navy, no change needed.

- [ ] **Step 6: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormLogin.cs FabrikaStokTakipUygulamasi/FormLogin.Designer.cs
git commit -m "style: replace emoji with Segoe MDL2 Assets icons on the login screen"
```

---

### Task 11: FormDashboard — kurumsal kartlar + stok dağılımı grafiği

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormDashboard.Designer.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormDashboard.cs`

**Interfaces:**
- Consumes: `UIStil.GriAcik`, `UIStil.Lacivert`, `UIStil.Kritik`, `UIStil.Mavi`, `UIStil.Glyph.*` (Task 8); `StokVeritabani.ToplamUrun()`, `StokVeritabani.KritikStokSayisi()` (unchanged signatures from Task 3); `System.Windows.Forms.DataVisualization.Charting` (Task 1's new package).

This form currently has the most off-palette styling in the app (`Color.Gainsboro` background, `Color.Brown` and `Color.OliveDrab` stat panels, `"Microsoft Sans Serif"` grid font) — bring it in line with the rest of the app's navy/blue palette, and add a small pie chart showing the critical-vs-normal stock split using the already-existing `ToplamUrun()`/`KritikStokSayisi()` data (no new DB query needed).

- [ ] **Step 1: Off-palette color fixes in `FormDashboard.Designer.cs`**

```csharp
            this.panelCritical.BackColor = System.Drawing.Color.Brown;
```
→
```csharp
            this.panelCritical.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
```

```csharp
            this.label3.BackColor = System.Drawing.Color.Brown;
```
→
```csharp
            this.label3.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
```

```csharp
            this.panelCompany.BackColor = System.Drawing.Color.OliveDrab;
```
→
```csharp
            this.panelCompany.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
```

```csharp
            this.BackColor = System.Drawing.Color.Gainsboro;
```
→
```csharp
            this.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
```

```csharp
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
```
→
```csharp
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
```

```csharp
            this.lblDashboard.AutoSize = true;
            this.lblDashboard.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblDashboard.Location = new System.Drawing.Point(25, 22);
```
→
```csharp
            this.lblDashboard.AutoSize = true;
            this.lblDashboard.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblDashboard.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblDashboard.Location = new System.Drawing.Point(25, 22);
```

- [ ] **Step 2: Add a faint watermark icon to each of the three stat panels**

Declare three new `Label` fields. In the field declarations at the bottom of the class (after `private System.Windows.Forms.Label label1;`), add:

```csharp
        private System.Windows.Forms.Label lblTotalIkon;
        private System.Windows.Forms.Label lblCriticalIkon;
        private System.Windows.Forms.Label lblCompanyIkon;
```

In `InitializeComponent`, right after the line `this.label1 = new System.Windows.Forms.Label();`, add:

```csharp
            this.lblTotalIkon = new System.Windows.Forms.Label();
```

right after `this.label4 = new System.Windows.Forms.Label();`, add:

```csharp
            this.lblCriticalIkon = new System.Windows.Forms.Label();
```

right after `this.label6 = new System.Windows.Forms.Label();`, add:

```csharp
            this.lblCompanyIkon = new System.Windows.Forms.Label();
```

Then, in each stat panel's `Controls.Add` list, add the new icon label. For example:

```csharp
            this.panelTotal.Controls.Add(this.label2);
            this.panelTotal.Controls.Add(this.label1);
```
→
```csharp
            this.panelTotal.Controls.Add(this.label2);
            this.panelTotal.Controls.Add(this.label1);
            this.panelTotal.Controls.Add(this.lblTotalIkon);
```

Do the same for `panelCritical.Controls.Add(this.label4);` → also add `this.panelCritical.Controls.Add(this.lblCriticalIkon);`, and for `panelCompany.Controls.Add(this.label6);` → also add `this.panelCompany.Controls.Add(this.lblCompanyIkon);`.

Then, right after the `// label6` block (before `// label7`), insert the three icon labels' property setup:

```csharp
            //
            // lblTotalIkon
            //
            this.lblTotalIkon.AutoSize = true;
            this.lblTotalIkon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 22F);
            this.lblTotalIkon.ForeColor = System.Drawing.Color.FromArgb(60, 255, 255, 255);
            this.lblTotalIkon.Location = new System.Drawing.Point(195, 12);
            this.lblTotalIkon.Name = "lblTotalIkon";
            this.lblTotalIkon.Text = FabrikaStokTakipUygulamasi.UIStil.Glyph.Kutu;
            //
            // lblCriticalIkon
            //
            this.lblCriticalIkon.AutoSize = true;
            this.lblCriticalIkon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 22F);
            this.lblCriticalIkon.ForeColor = System.Drawing.Color.FromArgb(60, 255, 255, 255);
            this.lblCriticalIkon.Location = new System.Drawing.Point(195, 12);
            this.lblCriticalIkon.Name = "lblCriticalIkon";
            this.lblCriticalIkon.Text = FabrikaStokTakipUygulamasi.UIStil.Glyph.Uyarim;
            //
            // lblCompanyIkon
            //
            this.lblCompanyIkon.AutoSize = true;
            this.lblCompanyIkon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 22F);
            this.lblCompanyIkon.ForeColor = System.Drawing.Color.FromArgb(60, 255, 255, 255);
            this.lblCompanyIkon.Location = new System.Drawing.Point(195, 12);
            this.lblCompanyIkon.Name = "lblCompanyIkon";
            this.lblCompanyIkon.Text = FabrikaStokTakipUygulamasi.UIStil.Glyph.Kisiler;
```

(These labels use a semi-transparent white `ForeColor` — GDI+ label rendering honors alpha in `SolidBrush`, so this reads as a subtle watermark icon in the corner of each dark stat card, not a solid white icon competing with the big number.)

- [ ] **Step 3: Add the stock-distribution chart**

At the top of `InitializeComponent`, alongside the other local designer variables, add:

```csharp
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
```

Add a field declaration (bottom of the class, alongside the other private fields):

```csharp
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStokDagilimi;
```

After the `// panelCompany` control block's property setup finishes (i.e. right after the last line of the `// label6` property block, and before `// label7`), insert:

```csharp
            //
            // chartStokDagilimi
            //
            this.chartStokDagilimi = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartStokDagilimi)).BeginInit();
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chartStokDagilimi.ChartAreas.Add(chartArea1);
            this.chartStokDagilimi.BackColor = System.Drawing.Color.White;
            this.chartStokDagilimi.BorderlineColor = System.Drawing.Color.FromArgb(189, 195, 199);
            this.chartStokDagilimi.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chartStokDagilimi.BorderlineWidth = 1;
            this.chartStokDagilimi.Location = new System.Drawing.Point(600, 120);
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series1.Name = "Seri1";
            series1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.chartStokDagilimi.Series.Add(series1);
            this.chartStokDagilimi.Name = "chartStokDagilimi";
            this.chartStokDagilimi.Size = new System.Drawing.Size(300, 240);
            this.chartStokDagilimi.TabIndex = 5;
            this.chartStokDagilimi.Text = "chartStokDagilimi";
            ((System.ComponentModel.ISupportInitialize)(this.chartStokDagilimi)).EndInit();
```

Note the `Location`/`Size` places this chart at `(600, 120)`, `300×240` — this overlaps the existing `panelCompany` (`Location (600, 120)`, `Size 250×120`) and `label7`/`dgvRecent`'s vertical space. To make room, first move the three stat panels up and shrink the vertical stat-card row so the chart sits to their right on the same row instead of below them: change `panelCompany.Location` from `new System.Drawing.Point(600, 120)` to `new System.Drawing.Point(600, 120)` is unchanged, but reduce the row width and put the chart at `x = 40` below the cards instead — **simpler placement:** keep all three stat panels exactly where they are (`y = 120..240`), and place the chart in the previously-empty area to their right: change the chart's `Location` above from `(600, 120)` to `(890, 120)` and `Size` from `(300, 240)` to `(254, 120)` so it sits in the same row, to the right of `panelCompany` (which ends at `x = 850`), matching the stat cards' height:

```csharp
            this.chartStokDagilimi.Location = new System.Drawing.Point(890, 120);
            ...
            this.chartStokDagilimi.Size = new System.Drawing.Size(254, 120);
```

(Apply this corrected `Location`/`Size` instead of the `(600,120)`/`(300,240)` values shown earlier in this step — the earlier values were illustrative only, the corrected ones are what actually gets written to the file.)

Finally, add the chart to the form's `Controls` collection. Find:

```csharp
            this.Controls.Add(this.dgvRecent);
            this.Controls.Add(this.label7);
```

and insert the chart before `dgvRecent`:

```csharp
            this.Controls.Add(this.chartStokDagilimi);
            this.Controls.Add(this.dgvRecent);
            this.Controls.Add(this.label7);
```

Also add `((System.ComponentModel.ISupportInitialize)(this.chartStokDagilimi)).BeginInit();` near the top `SuspendLayout` block (alongside the existing `((System.ComponentModel.ISupportInitialize)(this.dgvRecent)).BeginInit();`) — actually this call already happens inline in the `// chartStokDagilimi` block above via its own `BeginInit()`/`EndInit()` pair, so no separate top-level call is needed; just make sure the chart's own `BeginInit()`/`EndInit()` bracket its property assignments as shown.

- [ ] **Step 4: Populate the chart from existing dashboard data (`FormDashboard.cs`)**

Add this `using` at the top of `FormDashboard.cs`:

```csharp
using System.Windows.Forms.DataVisualization.Charting;
```

Find:

```csharp
        private void DiliUygula()
        {
            // İstatistik sayıları
            try
            {
                label1.Text = StokVeritabani.ToplamUrun().ToString();
                label4.Text = StokVeritabani.KritikStokSayisi().ToString();
                label6.Text = StokVeritabani.FirmaSayisi().ToString();
            }
            catch
            {
                label1.Text = "—";
                label4.Text = "—";
                label6.Text = "—";
            }
```

Replace with:

```csharp
        private void DiliUygula()
        {
            // İstatistik sayıları
            int toplam = 0, kritik = 0;
            try
            {
                toplam = StokVeritabani.ToplamUrun();
                kritik = StokVeritabani.KritikStokSayisi();
                label1.Text = toplam.ToString();
                label4.Text = kritik.ToString();
                label6.Text = StokVeritabani.FirmaSayisi().ToString();
            }
            catch
            {
                label1.Text = "—";
                label4.Text = "—";
                label6.Text = "—";
            }

            GrafikGuncelle(toplam, kritik);
```

Then add this new private method right after `DiliUygula()`'s closing brace (before `TabloyuDoldur()`):

```csharp
        private void GrafikGuncelle(int toplam, int kritik)
        {
            int normal = System.Math.Max(0, toplam - kritik);

            var seri = chartStokDagilimi.Series["Seri1"];
            seri.Points.Clear();
            seri.ChartType = SeriesChartType.Doughnut;

            if (toplam <= 0)
            {
                seri.Points.AddXY(LangManager.Ingilizce ? "No data" : "Veri yok", 1);
                seri.Points[0].Color = System.Drawing.Color.FromArgb(220, 220, 220);
                seri.Points[0].LegendText = LangManager.Ingilizce ? "No data" : "Veri yok";
                return;
            }

            int kritikIdx = seri.Points.AddXY(LangManager.Ingilizce ? "Critical" : "Kritik", kritik);
            seri.Points[kritikIdx].Color = System.Drawing.Color.FromArgb(192, 57, 43);
            seri.Points[kritikIdx].LegendText = (LangManager.Ingilizce ? "Critical: " : "Kritik: ") + kritik;

            int normalIdx = seri.Points.AddXY(LangManager.Ingilizce ? "Normal" : "Normal", normal);
            seri.Points[normalIdx].Color = System.Drawing.Color.FromArgb(41, 128, 185);
            seri.Points[normalIdx].LegendText = (LangManager.Ingilizce ? "Normal: " : "Normal: ") + normal;
        }
```

- [ ] **Step 5: Self-review**

Re-read the full `FormDashboard.Designer.cs`: confirm `chartStokDagilimi` is both declared as a field, constructed, added to `ChartAreas`/`Series`, and added to `this.Controls` exactly once each (a `Chart` control missing from `Controls.Add` would compile but never appear on screen). Confirm the three new icon labels' `Location` (`195, 12`) sits inside their 250×120-pixel parent panels without overflowing (label is `AutoSize`, MDL2 glyphs at 22pt are roughly 28×28px, so `(195,12)` to `(223,40)` comfortably fits inside `250×120` with the existing number/caption labels at `x ≤ 94`). Confirm `GrafikGuncelle` guards `toplam <= 0` (a fresh, empty SQLite database) instead of leaving `Points` empty (an empty-but-rendered `Chart` shows a blank white box, which is fine, but the explicit "No data" wedge is a nicer first-run experience) and never divides by anything (only additions/subtraction), so it can't throw `DivideByZeroException`.

- [ ] **Step 6: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormDashboard.Designer.cs FabrikaStokTakipUygulamasi/FormDashboard.cs
git commit -m "style: rebuild Dashboard with corporate colors, stat-card icons, and a stock distribution chart"
```

---

### Task 12: FormUrunler — kurumsal renkler ve buton ikonları

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormUrunler.Designer.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormUrunler.cs`

**Interfaces:**
- Consumes: `UIStil.GriAcik`, `UIStil.Lacivert`, `UIStil.Aksan`, `UIStil.Kritik`, `UIStil.Basarili`, `UIStil.Glyph.*`, `UIStil.SolIkonCiz` (Task 8).

- [ ] **Step 1: Off-palette color fixes in `FormUrunler.Designer.cs`**

```csharp
            panelTop.BackColor = System.Drawing.Color.Gainsboro;
```
→
```csharp
            panelTop.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
```

```csharp
            btnExcel.BackColor = System.Drawing.Color.Green;
```
→
```csharp
            btnExcel.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
```

```csharp
            btnDelete.BackColor = System.Drawing.Color.Maroon;
```
→
```csharp
            btnDelete.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
```

```csharp
            btnDetail.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
```
→
```csharp
            btnDetail.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
```

```csharp
            lblTotalProduct.ForeColor = System.Drawing.Color.DarkGreen;
```
→
```csharp
            lblTotalProduct.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
```

```csharp
            BackColor = System.Drawing.Color.Gainsboro;
```
→
```csharp
            BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
```

(`btnEdit.BackColor = System.Drawing.Color.FromArgb(243, 156, 18);` is already the corporate accent orange — no change needed, it now matches `UIStil.Aksan`.)

- [ ] **Step 2: Make room for and paint icons on the four toolbar buttons**

For each of the four buttons, widen the left padding. Find:

```csharp
            btnExcel.Location = new System.Drawing.Point(443, 23);
            btnExcel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnExcel.Name = "btnExcel";
```
→ insert a `Padding` line:
```csharp
            btnExcel.Location = new System.Drawing.Point(443, 23);
            btnExcel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnExcel.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            btnExcel.Name = "btnExcel";
```

Do the same for the other three — insert `xxx.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);` right after each button's existing `Margin` line:
- `btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);`
- `btnEdit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);`
- `btnDetail.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);`

- [ ] **Step 3: Wire the icon `Paint` handlers in `FormUrunler.cs`**

In the constructor:

```csharp
        public FormUrunler()
        {
            InitializeComponent();
        }
```

Replace with:

```csharp
        public FormUrunler()
        {
            InitializeComponent();

            btnDetail.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Dokuman, btnDetail.ClientRectangle, System.Drawing.Color.White);
            btnEdit.Paint   += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Duzenle, btnEdit.ClientRectangle,   System.Drawing.Color.Black);
            btnDelete.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Sil,      btnDelete.ClientRectangle, System.Drawing.Color.White);
            btnExcel.Paint  += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.DisaAktar, btnExcel.ClientRectangle, System.Drawing.Color.White);
        }
```

(`btnEdit`'s icon is drawn in black because the button's existing `ForeColor` for its text is `System.Drawing.Color.Black` on the light-orange `Aksan` background — matching text/icon color keeps them visually consistent.)

- [ ] **Step 4: Self-review**

Re-read `FormUrunler.Designer.cs`: confirm none of the four buttons' `TextAlign` is set (it isn't in the current file, so the default `MiddleCenter` applies) — with the added left `Padding`, centered text will shift slightly right within the remaining space, leaving room on the left for the painted icon; this is visually fine for these fixed-width (128px) buttons. Confirm `dgvProducts`'s existing `Segoe UI` fonts and navy header (`FromArgb(44, 62, 80)`) were already on-palette and are untouched.

- [ ] **Step 5: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormUrunler.Designer.cs FabrikaStokTakipUygulamasi/FormUrunler.cs
git commit -m "style: bring FormUrunler onto the corporate palette and add toolbar icons"
```

---

### Task 13: FormArama — arka plan hizalaması ve arama/temizle ikonları

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormArama.Designer.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormArama.cs`

**Interfaces:**
- Consumes: `UIStil.GriAcik`, `UIStil.Glyph.Ara`, `UIStil.Glyph.Kapat`, `UIStil.SolIkonCiz` (Task 8).

This form's toolbar and grid (`panelTop`, `dgvUrunler`) were already built on the navy/blue palette — only the filter sidebar and page background use the off-palette `Gainsboro`, and the two top-bar buttons need icons.

- [ ] **Step 1: Off-palette color fixes in `FormArama.Designer.cs`**

```csharp
            panelFilter.BackColor = System.Drawing.Color.Gainsboro;
```
→
```csharp
            panelFilter.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
```

```csharp
            BackColor = System.Drawing.Color.Gainsboro;
```
→
```csharp
            BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
```

- [ ] **Step 2: Make room for icons on the search/clear buttons**

```csharp
            btnTemizle.Location = new System.Drawing.Point(608, 14);
            btnTemizle.Name = "btnTemizle";
```
→
```csharp
            btnTemizle.Location = new System.Drawing.Point(608, 14);
            btnTemizle.Name = "btnTemizle";
            btnTemizle.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
```

```csharp
            btnAra.Location = new System.Drawing.Point(503, 14);
            btnAra.Name = "btnAra";
```
→
```csharp
            btnAra.Location = new System.Drawing.Point(503, 14);
            btnAra.Name = "btnAra";
            btnAra.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
```

- [ ] **Step 3: Wire the icon `Paint` handlers in `FormArama.cs`**

```csharp
        public FormArama()
        {
            InitializeComponent();
        }
```

Replace with:

```csharp
        public FormArama()
        {
            InitializeComponent();

            btnAra.Paint     += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ara,   btnAra.ClientRectangle,     System.Drawing.Color.White, 10f);
            btnTemizle.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kapat, btnTemizle.ClientRectangle, System.Drawing.Color.White, 10f);
        }
```

- [ ] **Step 4: Self-review**

Confirm `FormArama_DilDegisti`/`DildurumUygula` in `FormArama.cs` still only set `btnAra.Text`/`btnTemizle.Text` to the plain localized string (`"SEARCH"`/`"ARA"`, `"CLEAR"`/`"TEMİZLE"`) — unchanged — since the icon is now painted independently of the button's `Text`/language. Confirm the 22px padding leaves the existing text properly centered in the remaining space for these narrow (92–110px wide) buttons; if the text looks cramped when reviewed in the Designer, this is a cosmetic tolerance the human operator can nudge (e.g. `Padding` `18` instead of `22`) without needing to touch any other file.

- [ ] **Step 5: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormArama.Designer.cs FabrikaStokTakipUygulamasi/FormArama.cs
git commit -m "style: align FormArama background with the corporate palette, add search/clear icons"
```

---

### Task 14: FormUrunEkle + FormUrunDuzenle — kurumsal font/renk ve ikonlar

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormUrunEkle.Designer.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormUrunEkle.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormUrunDuzenle.cs`

**Interfaces:**
- Consumes: `UIStil.GriAcik`, `UIStil.Glyph.Ekle`, `UIStil.Glyph.Kapat`, `UIStil.Glyph.Dokuman`, `UIStil.Glyph.Duzenle`, `UIStil.SolIkonCiz`, `UIStil.Lacivert`, `UIStil.GriInput`, `UIStil.GriMetin`, `UIStil.Aksan`, `UIStil.Notr`, `UIStil.Kritik`, `UIStil.Basarili` (Task 8).

- [ ] **Step 1: Fix the off-brand font/background in `FormUrunEkle.Designer.cs`**

This file currently sets `"Microsoft Sans Serif", 8.25F` eleven times (the form's default `Font` plus ten field-label fonts: `lblParent`, `Batch`, `Material`, `Certificate`, `Customer`, `Lenght`, `lblWidth`, `THK`, `Grade`, `Heat`). Replace **every** occurrence of:

```csharp
new System.Drawing.Font("Microsoft Sans Serif", 8.25F)
```

with:

```csharp
new System.Drawing.Font("Segoe UI", 9F)
```

Also replace the one differently-sized occurrence, `label1`'s font:

```csharp
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
```
→
```csharp
            label1.Font = new System.Drawing.Font("Segoe UI", 9F);
```

And the page background:

```csharp
            BackColor = System.Drawing.Color.Gainsboro;
```
→
```csharp
            BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
```

- [ ] **Step 2: Add icons to the two footer buttons**

```csharp
            btnUrunEkle.Location = new System.Drawing.Point(780, 540);
            btnUrunEkle.Name = "btnUrunEkle";
            btnUrunEkle.Size = new System.Drawing.Size(160, 45);
```
→
```csharp
            btnUrunEkle.Location = new System.Drawing.Point(780, 540);
            btnUrunEkle.Name = "btnUrunEkle";
            btnUrunEkle.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            btnUrunEkle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnUrunEkle.Size = new System.Drawing.Size(160, 45);
```

```csharp
            btnTemizle.Location = new System.Drawing.Point(780, 595);
            btnTemizle.Name = "btnTemizle";
            btnTemizle.Size = new System.Drawing.Size(160, 45);
```
→
```csharp
            btnTemizle.Location = new System.Drawing.Point(780, 595);
            btnTemizle.Name = "btnTemizle";
            btnTemizle.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            btnTemizle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnTemizle.Size = new System.Drawing.Size(160, 45);
```

- [ ] **Step 3: Drop the emoji from the two PDF buttons, make room for icons**

```csharp
            btnPdfSec.Location = new System.Drawing.Point(15, 38);
            btnPdfSec.Name = "btnPdfSec";
            btnPdfSec.Size = new System.Drawing.Size(185, 38);
            btnPdfSec.TabIndex = 0;
            btnPdfSec.Text = "📄 Sertifika PDF Seç...";
```
→
```csharp
            btnPdfSec.Location = new System.Drawing.Point(15, 38);
            btnPdfSec.Name = "btnPdfSec";
            btnPdfSec.Padding = new System.Windows.Forms.Padding(26, 0, 0, 0);
            btnPdfSec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnPdfSec.Size = new System.Drawing.Size(185, 38);
            btnPdfSec.TabIndex = 0;
            btnPdfSec.Text = "Sertifika PDF Seç...";
```

```csharp
            btnPdfKaldir.Location = new System.Drawing.Point(210, 38);
            btnPdfKaldir.Name = "btnPdfKaldir";
            btnPdfKaldir.Size = new System.Drawing.Size(185, 38);
            btnPdfKaldir.TabIndex = 1;
            btnPdfKaldir.Text = "✖ PDF Kaldır";
```
→
```csharp
            btnPdfKaldir.Location = new System.Drawing.Point(210, 38);
            btnPdfKaldir.Name = "btnPdfKaldir";
            btnPdfKaldir.Padding = new System.Windows.Forms.Padding(26, 0, 0, 0);
            btnPdfKaldir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnPdfKaldir.Size = new System.Drawing.Size(185, 38);
            btnPdfKaldir.TabIndex = 1;
            btnPdfKaldir.Text = "PDF Kaldır";
```

- [ ] **Step 4: Wire the four `Paint` handlers in `FormUrunEkle.cs`**

```csharp
        public FormUrunEkle()
        {
            InitializeComponent();
            this.TopLevel        = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock            = DockStyle.Fill;
        }
```

Replace with:

```csharp
        public FormUrunEkle()
        {
            InitializeComponent();
            this.TopLevel        = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock            = DockStyle.Fill;

            btnUrunEkle.Paint  += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ekle,  btnUrunEkle.ClientRectangle, Color.White);
            btnTemizle.Paint   += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kapat, btnTemizle.ClientRectangle,  Color.White);
            btnPdfSec.Paint    += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Dokuman, btnPdfSec.ClientRectangle, Color.White, 11f);
            btnPdfKaldir.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Kapat,   btnPdfKaldir.ClientRectangle, Color.White, 11f);
        }
```

- [ ] **Step 5: `FormUrunDuzenle.cs` — reuse `UIStil` instead of local color constants, drop emoji**

Find:

```csharp
        private static readonly Color CNavy    = Color.FromArgb(44, 62, 80);
        private static readonly Color CLightBg = Color.FromArgb(236, 240, 241);
        private static readonly Color CAccent  = Color.FromArgb(243, 156, 18);
        private static readonly Color CWhite   = Color.White;
        private static readonly Color CBorder  = Color.FromArgb(189, 195, 199);
```

Replace with:

```csharp
        private static readonly Color CNavy    = UIStil.Lacivert;
        private static readonly Color CLightBg = UIStil.GriAcik;
        private static readonly Color CAccent  = UIStil.Aksan;
        private static readonly Color CWhite   = UIStil.Beyaz;
        private static readonly Color CBorder  = UIStil.GriOrta;
```

(This keeps every other line in the file — which references `CNavy`, `CLightBg`, etc. — working unchanged, since the field names are preserved; only their values now come from the single shared source of truth.)

Then drop the two emoji in the PDF panel. Find:

```csharp
            var lblPdfBaslik = new Label
            {
                Text = "📄 Sertifika PDF",
```
→
```csharp
            var lblPdfBaslik = new Label
            {
                Text = "Sertifika PDF",
```

Find:

```csharp
            btnPdfDegis = new Button
            {
                Text = "📂 " + LangManager.T("duzenle.pdfdegis"),
```
→
```csharp
            btnPdfDegis = new Button
            {
                Text = LangManager.T("duzenle.pdfdegis"),
```

Find:

```csharp
            btnPdfSil = new Button
            {
                Text = "✖ " + LangManager.T("duzenle.pdfsil"),
```
→
```csharp
            btnPdfSil = new Button
            {
                Text = LangManager.T("duzenle.pdfsil"),
```

Then, right after both buttons are constructed and their `Click` handlers assigned (i.e. right after the line `btnPdfSil.Click += BtnPdfSil_Click;`), add:

```csharp
            btnPdfDegis.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Dokuman, btnPdfDegis.ClientRectangle, CWhite, 11f);
            btnPdfSil.Paint   += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Sil,      btnPdfSil.ClientRectangle,   CWhite, 11f);
```

Their existing `Size = new Size(185, 34)` already leaves enough left margin for the default `SolIkonCiz` `x = 14f` draw position without any `Padding` adjustment needed, since these buttons have no `Text`-vs-`Padding` interaction issue — **however**, confirm this after the edit: if the button's text (which starts flush left with no configured `Padding`) visually overlaps the icon in a Visual Studio design-time preview, add `btnPdfDegis.Padding = new Padding(24, 0, 0, 0);` / `btnPdfSil.Padding = new Padding(24, 0, 0, 0);` right next to their `Font` assignment (same fix pattern as every other button in this plan).

- [ ] **Step 6: Self-review**

Grep `FabrikaStokTakipUygulamasi/FormUrunEkle.Designer.cs` and `FabrikaStokTakipUygulamasi/FormUrunDuzenle.cs` for the literal emoji characters `📄`, `📂`, `✖` to confirm none remain. Confirm `FormUrunDuzenle.cs`'s color-constant rename didn't miss any usage — every `CNavy`/`CLightBg`/`CAccent`/`CWhite`/`CBorder` reference elsewhere in the file still compiles unchanged since only the *initializer* changed, not the field names.

- [ ] **Step 7: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormUrunEkle.Designer.cs FabrikaStokTakipUygulamasi/FormUrunEkle.cs FabrikaStokTakipUygulamasi/FormUrunDuzenle.cs
git commit -m "style: corporate fonts/colors and icons for the product add/edit forms"
```

---

### Task 15: Kalan kod-tabanlı formlar — ortak `UIStil` sabitlerine geçiş ve emoji temizliği

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormLowStockSecim.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormLowStockLimit.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormSilOnay.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormUrunDetay.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormAdmin.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormLowStock.Designer.cs` (rule-based pass, see Step 6)

**Interfaces:**
- Consumes: `UIStil.Lacivert`, `UIStil.LacivertKoyu`, `UIStil.Aksan`, `UIStil.GriAcik`, `UIStil.Beyaz`, `UIStil.GriOrta`, `UIStil.Kritik`, `UIStil.Basarili`, `UIStil.Uyari`, `UIStil.Mavi`, `UIStil.Glyph.*`, `UIStil.SolIkonCiz`, `UIStil.IkonLabel` (Task 8).

These five forms each declare their own local `private static readonly Color CNavy = Color.FromArgb(44, 62, 80);`-style constants (duplicated across files). Point every one of them at the shared `UIStil` values instead — this is a pure substitution, the field *names* used throughout each file's body stay the same, so no other line in these files needs to change.

- [ ] **Step 1: `FormLowStockSecim.cs`**

```csharp
        private static readonly Color CNavy    = Color.FromArgb(44, 62, 80);
        private static readonly Color CWhite   = Color.White;
        private static readonly Color CBg      = Color.FromArgb(236, 240, 241);
```
→
```csharp
        private static readonly Color CNavy    = UIStil.Lacivert;
        private static readonly Color CWhite   = UIStil.Beyaz;
        private static readonly Color CBg      = UIStil.GriAcik;
```

- [ ] **Step 2: `FormLowStockLimit.cs`**

```csharp
        private static readonly Color CNavy   = Color.FromArgb(44, 62, 80);
        private static readonly Color CAccent = Color.FromArgb(243, 156, 18);
        private static readonly Color CLightBg= Color.FromArgb(236, 240, 241);
        private static readonly Color CWhite  = Color.White;
        private static readonly Color CBorder = Color.FromArgb(189, 195, 199);
```
→
```csharp
        private static readonly Color CNavy   = UIStil.Lacivert;
        private static readonly Color CAccent = UIStil.Aksan;
        private static readonly Color CLightBg= UIStil.GriAcik;
        private static readonly Color CWhite  = UIStil.Beyaz;
        private static readonly Color CBorder = UIStil.GriOrta;
```

- [ ] **Step 3: `FormSilOnay.cs`**

```csharp
        private static readonly Color CNavy   = Color.FromArgb(44, 62, 80);
        private static readonly Color CRed    = Color.FromArgb(192, 57, 43);
        private static readonly Color CLightBg= Color.FromArgb(236, 240, 241);
        private static readonly Color CWhite  = Color.White;
```
→
```csharp
        private static readonly Color CNavy   = UIStil.Lacivert;
        private static readonly Color CRed    = UIStil.Kritik;
        private static readonly Color CLightBg= UIStil.GriAcik;
        private static readonly Color CWhite  = UIStil.Beyaz;
```

Also replace the warning-triangle symbol with the corporate glyph (this one is icon-only, no text is concatenated into the same control, so a direct font/text swap is safe — no `Padding`/`Paint` trick needed):

```csharp
            var lblIcon = new Label
            {
                Text      = "⚠",
                Font      = new Font("Segoe UI", 34f),
                ForeColor = CRed,
                AutoSize  = true
            };
```
→
```csharp
            var lblIcon = new Label
            {
                Text      = UIStil.Glyph.Uyarim,
                Font      = UIStil.GlyphFont(34f),
                ForeColor = CRed,
                AutoSize  = true
            };
```

- [ ] **Step 4: `FormUrunDetay.cs`**

```csharp
        private static readonly Color CNavy    = Color.FromArgb(44, 62, 80);
        private static readonly Color CLightBg = Color.FromArgb(236, 240, 241);
        private static readonly Color CWhite   = Color.White;
        private static readonly Color CBorder  = Color.FromArgb(189, 195, 199);
```
→
```csharp
        private static readonly Color CNavy    = UIStil.Lacivert;
        private static readonly Color CLightBg = UIStil.GriAcik;
        private static readonly Color CWhite   = UIStil.Beyaz;
        private static readonly Color CBorder  = UIStil.GriOrta;
```

The PDF button mixes an emoji with other text — apply the Padding+Paint pattern:

```csharp
                var btnPdf = new Button
                {
                    Text      = $"📄 {LangManager.T("detay.sertifika")}  —  {_urun.SertifikaDosyaAdi}",
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = CWhite,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                    Location  = new Point(10, 10),
                    Size      = new Size(574, 46),
                    Cursor    = Cursors.Hand,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                btnPdf.FlatAppearance.BorderSize = 0;
                btnPdf.Click += (s, e) => PdfAc(_urun.SertifikaPdf, _urun.SertifikaDosyaAdi);
                panelPdf.Controls.Add(btnPdf);
```
→
```csharp
                var btnPdf = new Button
                {
                    Text      = $"{LangManager.T("detay.sertifika")}  —  {_urun.SertifikaDosyaAdi}",
                    BackColor = Color.FromArgb(52, 152, 219),
                    ForeColor = CWhite,
                    FlatStyle = FlatStyle.Flat,
                    Font      = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                    Location  = new Point(10, 10),
                    Size      = new Size(574, 46),
                    Padding   = new Padding(34, 0, 0, 0),
                    Cursor    = Cursors.Hand,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                btnPdf.FlatAppearance.BorderSize = 0;
                btnPdf.Click += (s, e) => PdfAc(_urun.SertifikaPdf, _urun.SertifikaDosyaAdi);
                btnPdf.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Dokuman, btnPdf.ClientRectangle, CWhite, 12f);
                panelPdf.Controls.Add(btnPdf);
```

The "no PDF" state is a static (non-clickable) label — use a separate `IkonLabel` next to the text instead:

```csharp
            else
            {
                panelPdf.Controls.Add(new Label
                {
                    Text      = "📄 " + LangManager.T("detay.sertifikayok"),
                    Font      = new Font("Segoe UI", 9f),
                    ForeColor = Color.FromArgb(149, 165, 166),
                    Location  = new Point(14, 12),
                    AutoSize  = true
                });
            }
```
→
```csharp
            else
            {
                var ikon = UIStil.IkonLabel(UIStil.Glyph.Dokuman, Color.FromArgb(149, 165, 166), 10f);
                ikon.Location = new Point(14, 13);
                panelPdf.Controls.Add(ikon);

                panelPdf.Controls.Add(new Label
                {
                    Text      = LangManager.T("detay.sertifikayok"),
                    Font      = new Font("Segoe UI", 9f),
                    ForeColor = Color.FromArgb(149, 165, 166),
                    Location  = new Point(36, 12),
                    AutoSize  = true
                });
            }
```

- [ ] **Step 5: `FormAdmin.cs`**

```csharp
        static readonly Color CNavy   = Color.FromArgb(44, 62, 80);
        static readonly Color CDark   = Color.FromArgb(30, 44, 57);
        static readonly Color CGray   = Color.FromArgb(236, 240, 241);
        static readonly Color CWhite  = Color.White;
        static readonly Color CGreen  = Color.FromArgb(39, 174, 96);
        static readonly Color CRed    = Color.FromArgb(192, 57, 43);
        static readonly Color COrange = Color.FromArgb(211, 84, 0);
        static readonly Color CBlue   = Color.FromArgb(41, 128, 185);
```
→
```csharp
        static readonly Color CNavy   = UIStil.Lacivert;
        static readonly Color CDark   = UIStil.LacivertKoyu;
        static readonly Color CGray   = UIStil.GriAcik;
        static readonly Color CWhite  = UIStil.Beyaz;
        static readonly Color CGreen  = UIStil.Basarili;
        static readonly Color CRed    = UIStil.Kritik;
        static readonly Color COrange = UIStil.Uyari;
        static readonly Color CBlue   = UIStil.Mavi;
```

Drop the emoji from the tab headers (plain text — a `TabPage` header would need `TabControl.DrawMode = OwnerDrawFixed` plus a `DrawItem` handler to paint a separate-font icon, which is disproportionate effort for a tab caption; removing the emoji outright still satisfies "no emoji" without that complexity):

```csharp
            tabKullanicilar.Text = LangManager.Ingilizce ? "👥  Users"            : "👥  Kullanıcılar";
            tabHareketler.Text   = LangManager.Ingilizce ? "📋  Stock Movements"  : "📋  Stok Hareketleri";
```
→
```csharp
            tabKullanicilar.Text = LangManager.Ingilizce ? "Users"            : "Kullanıcılar";
            tabHareketler.Text   = LangManager.Ingilizce ? "Stock Movements"  : "Stok Hareketleri";
```

The three toolbar buttons are regular `Button`s — apply the icon `Paint` pattern. Find:

```csharp
            btnYeniKul = MkBtn("", CGreen,   new Point(14, 11),  new Size(155, 30));
            btnDuzKul  = MkBtn("", COrange,  new Point(179, 11), new Size(130, 30));
            btnSilKul  = MkBtn("", CRed,     new Point(319, 11), new Size(110, 30));

            btnYeniKul.Click += (s, e) => AcKullaniciFrm(null);
            btnDuzKul.Click  += BtnDuzenle_Click;
            btnSilKul.Click  += BtnSil_Click;
```
→
```csharp
            btnYeniKul = MkBtn("", CGreen,   new Point(14, 11),  new Size(155, 30));
            btnDuzKul  = MkBtn("", COrange,  new Point(179, 11), new Size(130, 30));
            btnSilKul  = MkBtn("", CRed,     new Point(319, 11), new Size(110, 30));

            btnYeniKul.Padding = new Padding(26, 0, 0, 0);
            btnDuzKul.Padding  = new Padding(26, 0, 0, 0);
            btnSilKul.Padding  = new Padding(26, 0, 0, 0);
            btnYeniKul.Paint += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Ekle,    btnYeniKul.ClientRectangle, CWhite, 10f);
            btnDuzKul.Paint  += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Duzenle, btnDuzKul.ClientRectangle,  CWhite, 10f);
            btnSilKul.Paint  += (s, e) => UIStil.SolIkonCiz(e.Graphics, UIStil.Glyph.Sil,     btnSilKul.ClientRectangle,  CWhite, 10f);

            btnYeniKul.Click += (s, e) => AcKullaniciFrm(null);
            btnDuzKul.Click  += BtnDuzenle_Click;
            btnSilKul.Click  += BtnSil_Click;
```

And the two remaining emoji-prefixed button label strings (these lose the emoji prefix — the icon is now painted separately, and `MkBtn`'s `Text` parameter above was already `""`, so `GuncelleKulKolonlar()` is what actually sets the visible label):

```csharp
            btnYeniKul.Text = LangManager.Ingilizce ? "👤  New User"  : "👤  Yeni Kullanıcı";
            btnDuzKul.Text  = LangManager.Ingilizce ? "✎  Edit"       : "✎  Düzenle";
            btnSilKul.Text  = LangManager.Ingilizce ? "✖  Delete"     : "✖  Sil";
```
→
```csharp
            btnYeniKul.Text = LangManager.Ingilizce ? "New User"  : "Yeni Kullanıcı";
            btnDuzKul.Text  = LangManager.Ingilizce ? "Edit"       : "Düzenle";
            btnSilKul.Text  = LangManager.Ingilizce ? "Delete"     : "Sil";
```

- [ ] **Step 6: `FormLowStock.Designer.cs` — generic corporate-palette sweep (this file has not been read in full during planning)**

Open `FabrikaStokTakipUygulamasi/FormLowStock.Designer.cs` and:
1. `grep -n "Gainsboro\|Color.Brown\|Color.OliveDrab\|Color.Maroon\|Color.DarkGreen\|Color.Green\b\|Microsoft Sans Serif"` — for every match, replace with the equivalent `UIStil` color/font following the same mapping used in Tasks 11–14 (`Gainsboro`→`UIStil.GriAcik`'s RGB `FromArgb(236, 240, 241)`, `Microsoft Sans Serif`→`"Segoe UI"`, any red/green/orange system color name → the matching `UIStil.Kritik`/`UIStil.Basarili`/`UIStil.Uyari` RGB values).
2. `grep -n "📄\|📂\|✖\|👤\|👥\|📋\|🔐\|⏻\|👁\|🌐"` in both `FormLowStock.Designer.cs` and `FormLowStock.cs` — if any emoji shows up (none were seen in the code-behind read earlier in this project, but the Designer file was never opened), remove it from the string and, if the button/label pairs an icon with other text in the same control, apply the same `Padding` + `UIStil.SolIkonCiz` `Paint`-handler pattern used throughout Tasks 9–14; if it's an icon-only control, swap `Font` to `UIStil.GlyphFont(...)` and `Text` to the matching `UIStil.Glyph.*` constant directly (no `Paint` handler needed).
3. If neither grep finds anything, this file was already on-palette — note that in the commit message and move on; do not invent changes where none are needed.

- [ ] **Step 7: Self-review**

For each of the five fully-specified files, grep for the literal emoji characters (`📄`, `📂`, `✖`, `⚠`, `👤`, `👥`, `📋`) to confirm none remain. Confirm every `CNavy`/`CWhite`/etc. constant substitution kept the exact same field name so no other line in these files needed edits — spot-check by re-reading each file's `BuildUI`/`Build` method end-to-end once.

- [ ] **Step 8: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormLowStockSecim.cs FabrikaStokTakipUygulamasi/FormLowStockLimit.cs FabrikaStokTakipUygulamasi/FormSilOnay.cs FabrikaStokTakipUygulamasi/FormUrunDetay.cs FabrikaStokTakipUygulamasi/FormAdmin.cs FabrikaStokTakipUygulamasi/FormLowStock.Designer.cs
git commit -m "style: point remaining forms at the shared UIStil palette and remove leftover emoji"
```

---

### Task 16: Belgeleri güncelle, Railway dokümanını kaldır

**Files:**
- Modify: `KURULUM.md`
- Delete: `RAILWAY_DATABASE_KURULUM.md`

- [ ] **Step 1: Delete the Railway setup doc**

```bash
git rm RAILWAY_DATABASE_KURULUM.md
```

- [ ] **Step 2: Update `KURULUM.md` to match the actual package version from Task 1**

Current content:

```markdown
# FabrikaStokTakipUygulamasi – Kurulum (NET 8 Sürümü)

## Neden bu sürüm?
.NET Framework 4.7.2 targeting pack bilgisayarınızda yüklü değildi.
Bu sürüm .NET 8 (Windows) kullanıyor — modern Visual Studio'larda her zaman hazır gelir.

## Adımlar

1. `FabrikaStokTakipUygulamasi.sln` dosyasını **Visual Studio 2022** ile açın
2. **Ctrl + Shift + B** ile derleyin (NuGet paketi otomatik indirilir)
3. **F5** ile çalıştırın

## Notlar
- SQLite paketi: `Microsoft.Data.Sqlite 8.0.0` (otomatik indirilir)
- Veritabanı: `%AppData%\FabrikaStokTakipUygulamasi\stok.db` (ilk açılışta otomatik oluşur)
- İnternet bağlantısı ilk derlemede gereklidir (NuGet indirmesi için)
```

Replace with:

```markdown
# FabrikaStokTakipUygulamasi – Kurulum (.NET 8 Sürümü)

## Neden bu sürüm?
.NET Framework 4.7.2 targeting pack bilgisayarınızda yüklü değildi.
Bu sürüm .NET 8 (Windows) kullanıyor — modern Visual Studio'larda her zaman hazır gelir.
Veritabanı yerel SQLite'tır — internet bağlantısı veya ayrı bir sunucu kurulumu gerekmez
(sadece ilk derlemede NuGet paketlerinin indirilmesi için internet gerekir).

## Adımlar

1. `FabrikaStokTakipUygulamasi.sln` dosyasını **Visual Studio 2022** ile açın
2. **Ctrl + Shift + B** ile derleyin (NuGet paketleri otomatik indirilir)
3. **F5** ile çalıştırın

## Notlar
- SQLite paketi: `Microsoft.Data.Sqlite 8.0.10` (otomatik indirilir)
- Veritabanı: `%AppData%\FabrikaStokTakipUygulamasi\stok.db` (ilk açılışta otomatik oluşur, boş başlar)
- Varsayılan kullanıcılar (ilk açılışta otomatik eklenir): `emir/1234`, `barkan/1234` (Depo Personeli),
  `anil/1234`, `goksu/1234` (Mühendis), `admin/admin` (Admin) — şifreler veritabanında hash'lenerek saklanır
- Bilgisayar değiştirilirse `%AppData%\FabrikaStokTakipUygulamasi\stok.db` dosyası elle yeni bilgisayara kopyalanmalıdır;
  otomatik senkronizasyon yoktur (tek makine, dosya tabanlı veritabanı)
- İnternet bağlantısı yalnızca ilk derlemede gereklidir (NuGet indirmesi için)
```

- [ ] **Step 3: Self-review**

Confirm no other file in the repository references `RAILWAY_DATABASE_KURULUM.md` (`grep -rn "RAILWAY_DATABASE_KURULUM" .` from the repo root should return nothing after the deletion) or the environment variables `STOK_DB_URL` / `DATABASE_PUBLIC_URL` / `DATABASE_URL` (`grep -rn "STOK_DB_URL\|DATABASE_PUBLIC_URL\|DATABASE_URL" FabrikaStokTakipUygulamasi/` should return nothing — Task 3 already removed all reads of these from `StokVeritabani.cs`).

- [ ] **Step 4: Commit**

```bash
git add KURULUM.md
git commit -m "docs: update setup instructions for the local SQLite version, remove Railway doc"
```

---

### Task 17: Manuel QA (insan operatör — Windows + Visual Studio gerekir)

**This task cannot be executed by an agent in this environment** (no .NET SDK, no Windows, no WinForms runtime — see Global Constraints). It is a checklist for the human operator to run once all 16 previous tasks are committed, on a Windows machine with Visual Studio 2022.

- [ ] **Step 1: Build**

Open `FabrikaStokTakipUygulamasi.sln` in Visual Studio 2022, `Ctrl+Shift+B`. Expected: builds with zero errors. If there are errors, report the exact error text/file/line back — each is almost certainly a small typo introduced by one of the mechanical text replacements in Tasks 9–15 (e.g. a missed closing brace, a `UIStil.` reference before Task 8 was applied) and can be fixed with a single targeted edit.

- [ ] **Step 2: First run / database creation**

Press `F5`. Expected: no crash on startup, the login screen appears, and `%AppData%\FabrikaStokTakipUygulamasi\stok.db` now exists on disk (check via `dir %AppData%\FabrikaStokTakipUygulamasi` in a terminal).

- [ ] **Step 3: Login**

Log in as `admin` / `admin`. Expected: succeeds, main window opens with the sidebar nav showing icons (not blank boxes — if any nav icon shows a tofu/box character, that glyph's codepoint needs correcting per Task 8 Step 3's Character Map verification). Try a wrong password once — expected: rejected with the existing error dialog.

- [ ] **Step 4: "Keep me logged in"**

Check "Oturumu Açık Tut", log in, close the app fully, relaunch. Expected: automatically logged back in without re-entering credentials. Then open `%AppData%\FabrikaStokTakipUygulamasi\oturum.json` in a text editor — expected: the password field is a long opaque Base64 blob, not the plaintext password.

- [ ] **Step 5: Product CRUD**

Add a new product (Ürün Ekle) with a quantity — expected: success message, and it appears at the top of Ürünler. Edit it (change stock quantity) — expected: saves, Admin → Stok Hareketleri shows the movement with the correct old/new stock values (this exercises Task 6's fixed `UrunEkle` Id-return path and Task 3's rewritten `HareketKaydet`). Delete it — expected: confirmation dialog, then it disappears from the list and a "Product Deleted" movement is logged.

- [ ] **Step 6: Search, Low Stock, Dashboard**

Open Gelişmiş Arama and confirm filters narrow the grid. Open Low Stock and confirm the color-coded rows (red/orange/green) render correctly. Open Dashboard and confirm the three stat cards show correct numbers and the doughnut chart renders with a red "Kritik" and blue "Normal" wedge (or the "No data" placeholder wedge if the database is empty).

- [ ] **Step 7: Admin panel — password field**

Admin panel → Kullanıcılar → select a user → Düzenle. Expected: the password field is now **blank** (not showing the old password or a hash). Save without touching the password field — expected: the user can still log in with their old password afterward (proves "blank = keep current" works). Then edit again and type a new password, save, log out, log back in with the new password — expected: succeeds.

- [ ] **Step 8: Report back**

Report which of the above passed/failed. Any failure is expected to be a small, isolated fix (wrong glyph codepoint, a missed brace, a copy-paste typo) rather than a structural problem, since every task's logic was written against the actual current file contents read during planning.
