# FabrikaStokTakipUygulamasi — SQLite Geçişi, Hata Düzeltmeleri ve Kurumsal UI Yenilemesi

**Tarih:** 2026-07-22
**Durum:** Onay bekliyor

## Amaç

`FabrikaStokTakipUygulamasi` (WinForms, .NET 8) uygulaması şu anda Railway PostgreSQL'e bağlı çalışıyor; bu internet bağlantısı ve ortam değişkeni kurulumu gerektiriyor. Bu iş üç hedefi kapsar:

1. Veritabanını tekrar yerel SQLite'a çevirmek (tek makine, dosya tabanlı, kurulumsuz).
2. İnceleme sırasında bulunan hataları ve bir güvenlik açığını (düz metin şifre) düzeltmek.
3. UI'ı kurumsal/profesyonel bir görünüme kavuşturmak.

## Kapsam Dışı (bilinçli olarak yapılmayacak)

- Railway'deki mevcut verinin taşınması (sıfırdan başlanacak).
- Ağ üzerinden paylaşılan çok kullanıcılı veritabanı senaryosu (tek makine SQLite yeterli).
- Karanlık mod / tema değiştirme.
- Gerçek animasyonlar (fade/slide) — WinForms'ta güvenilir şekilde yapılamıyor, sadece hover/pressed renk geri bildirimi olacak.
- `FormArama` içindeki yanıltıcı ama işlevsel olarak doğru değişken adları (`cmbUrun`→Customer vb.) — kozmetik teknik borç, davranışı etkilemiyor.

---

## 1) Veritabanı: PostgreSQL → SQLite

### Paket değişikliği
- `FabrikaStokTakipUygulamasi.csproj`: `Npgsql` referansı kaldırılır, `Microsoft.Data.Sqlite` (8.x) eklenir.

### Bağlantı ve konum
- Veritabanı dosyası: `%AppData%\FabrikaStokTakipUygulamasi\stok.db` (klasör yoksa oluşturulur).
- `StokVeritabani.YeniBaglanti()` artık `SqliteConnection` döndürür; Railway'e özgü `STOK_DB_URL`/`DATABASE_PUBLIC_URL`/`DATABASE_URL` ortam değişkeni okuma mantığı tamamen kaldırılır.

### Şema dönüşümü (`Baslat()`)
| PostgreSQL | SQLite |
|---|---|
| `SERIAL PRIMARY KEY` | `INTEGER PRIMARY KEY AUTOINCREMENT` |
| `BYTEA` | `BLOB` |
| `TIMESTAMPTZ` | `TEXT` (ISO-8601 string, `DateTime` C# tarafında parse/format edilir) |
| `ALTER TABLE ... ADD COLUMN IF NOT EXISTS` | `PRAGMA table_info("Urunler")` ile kolon varlığı kontrol edilip yoksa `ALTER TABLE ... ADD COLUMN` çalıştırılır |
| `ON CONFLICT (...) DO NOTHING` | Aynı sözdizimi SQLite'da da destekleniyor, değişmeden kalır |
| `NOW()` | C# tarafında `DateTime.Now` / `DateTime.UtcNow` zaten kullanılıyor, DB tarafında gerekmiyor |

Tablolar (`Urunler`, `StokHareketleri`, `Kullanicilar`) aynı kolon isimleriyle SQLite'da yeniden oluşturulur. Çift tırnaklı tanımlayıcılar (`"Urunler"`) SQLite'da da geçerli olduğu için sorgu metinleri büyük ölçüde aynı kalır, sadece tip/PRAGMA farkları uygulanır.

### `KullaniciYonetici.cs`
Tüm `NpgsqlConnection`/`NpgsqlCommand` kullanımları `SqliteConnection`/`SqliteCommand` ile değiştirilir. `TabloyuKur` artık varsayılan kullanıcıları **hash'li şifreyle** ekler (bkz. Bölüm 2).

### Belgeler
- `RAILWAY_DATABASE_KURULUM.md` silinir.
- `KURULUM.md` gözden geçirilip güncel SQLite akışını (paket adı, veritabanı yolu) doğru yansıtacak şekilde küçük düzeltmeler yapılır.

---

## 2) Hata ve güvenlik düzeltmeleri

### 2.1 Şifre hash'leme (kritik güvenlik düzeltmesi)
- `Kullanicilar` tablosundaki `Sifre` kolonu artık **tuzlu PBKDF2 hash** tutar (`System.Security.Cryptography.Rfc2898DeriveBytes`, .NET yerleşik — ek NuGet paketi gerekmez). Saklanan format: `{iterasyon}.{tuzBase64}.{hashBase64}`.
- `KullaniciYonetici.GirisYap`: DB'den okunan hash ile girilen şifre yeniden hesaplanan hash karşılaştırılarak doğrulanır (sabit zamanlı karşılaştırma, `CryptographicOperations.FixedTimeEquals`).
- `TabloyuKur` varsayılan kullanıcıları (`emir/1234`, `barkan/1234`, `anil/1234`, `goksu/1234`, `admin/admin`) artık hash'lenmiş şifreyle ekler.
- `YeniKullaniciEkle` / `KullaniciGuncelle`: girilen düz metin şifre kaydetmeden önce hash'lenir.
- **Admin panelinde şifre görünürlüğü değişiyor**: `FormAdmin.AcKullaniciFrm` düzenleme modunda artık mevcut şifreyi göstermez. Şifre alanı boş gelir; boş bırakılırsa mevcut hash korunur, bir değer girilirse yeni şifre olarak hash'lenip kaydedilir. Kullanıcılar listesindeki `cSifre` kolonu zaten nokta (`•`) maskeli gösteriliyordu, hash uzunluğuna göre sabit sayıda nokta gösterecek şekilde küçük bir düzeltme yapılır (hash uzunluğu şifre uzunluğunu ele vermesin diye).

### 2.2 "Oturumu açık tut" — düz metin şifre dosyası
- `OturumAyarlari.cs`: Diskte saklanan şifre artık `System.Security.Cryptography.ProtectedData` (Windows DPAPI, `DataProtectionScope.CurrentUser`) ile şifrelenip Base64 olarak yazılır. Sadece aynı Windows kullanıcı hesabı çözebilir — dosya kopyalanıp başka bir bilgisayarda okunamaz.
- Bu, giriş formundaki otomatik-giriş akışını bozmadan (aynı Windows oturumunda çözülüp okunabildiği için) düz metin sorununu ortadan kaldırır.

### 2.3 FormUrunEkle — kırılgan "son eklenen ürün" varsayımı
- Şu an: `UrunEkle()` sonrası `TumUrunler()` tekrar çekilip `[0]` (Id DESC ilk satır) yeni eklenen ürün varsayılıyor.
- Düzeltme: `StokVeritabani.UrunEkle` metodu `INSERT` sonrası `SELECT last_insert_rowid();` ile yeni satırın gerçek Id'sini döndürecek şekilde `int` dönüş tipine çevrilir. `FormUrunEkle.btnUrunEkle_Click` bu Id'yi doğrudan `HareketKaydet` çağrısında kullanır; ekstra sorgu ve varsayım ortadan kalkar.

### 2.4 Kullanılmayan Designer artıkları
- `FormUrunEkle.cs` içindeki hiçbir kontrola bağlı olmayan boş event handler'lar (`label1_Click`, `label2_Click_1`, `textBox1_TextChanged`, `btnKaydet_Click` vb. — Designer dosyasıyla çapraz kontrol edilip gerçekten ölü olduğu doğrulanan) silinir.

### 2.5 Genel tarama
- Diğer formlar (Dashboard, Urunler, Arama, LowStock*, Admin) SQLite'a geçiş sırasında baştan sona okunacak; ortaya çıkan başka küçük mantık hataları (varsa) aynı kapsamda düzeltilir. Bunlar önceden listelenemeyecek kadar küçük olası bulgulardır — planı bozacak büyüklükte bir şey çıkarsa uygulama öncesi ayrıca bildirilir.

---

## 3) Kurumsal UI yenilemesi

### Tasarım yönü
Mevcut lacivert/mavi flat-design temel iskeleti korunur (zaten kurumsal bir başlangıç noktası), ama şu eksenlerde sadeleştirilip profesyonelleştirilir:

- **Renk kullanımı**: Lacivert (`#2C3E50` civarı) ve mavi (`#2980B9`/`#3498DB`) birincil paletler olarak kalır. Turuncu/kırmızı/yeşil vurgular yalnızca **durum bildirimi** (kritik stok, hata, başarı, silme onayı) için kullanılır — dekoratif amaçla kullanılmaz. Nötr gri/beyaz içerik alanları sadeliğini korur.
- **İkonlar (emoji → Segoe MDL2 Assets)**: Şu anki renkli emoji ikonlar kaldırılıp, Windows'ta hazır gelen **Segoe MDL2 Assets** glyph fontuyla tek renkli/aksan renkli ikonlara çevrilir. Kullanılacak glyph'ler standart Segoe MDL2 Assets karakter tablosundan seçilecek, örnek eşleşmeler:

  | Anlam | Glyph adı | Kod noktası (yaklaşık) |
  |---|---|---|
  | Kullanıcı (tekil) | Contact | U+E77B |
  | Kullanıcılar (çoğul) | People | U+E716 |
  | Doküman / PDF | Page2 | U+E7C3 |
  | Kapat (X) | Cancel | U+E711 |
  | Çöp kutusu (sil) | Delete | U+E74D |
  | Dil / Dünya | Globe | U+E774 |
  | Liste | BulletedList | U+E8FD |
  | Düzenle (kalem) | Edit | U+E70F |
  | Ara (büyüteç) | Search | U+E721 |
  | Panel/Ayarlar | Settings | U+E713 |

  Kesin kod noktaları implementasyon sırasında Windows karakter eşlem aracıyla (charmap) teyit edilip küçük bir `Icons` sabit sınıfında toplanacak. Bu yaklaşım kurumsal Windows uygulamalarında (Office, Azure Portal masaüstü araçları) yaygındır; ek dosya veya lisans gerekmez, DPI'da bozulmadan ölçeklenir.
- **Tipografi**: Segoe UI ailesi korunur; başlıklarda Semibold/Bold, gövde metninde Regular ile tutarlı bir hiyerarşi netleştirilir (bazı formlarda tutarsız font boyutları düzeltilir).
- **Butonlar**: Flat, keskin/minimal köşe, ince kenarlık; hover'da hafif koyulaşma (aşırı doygun renk geçişi yok), `Cursor.Hand`. Bu stil zaten bazı formlarda var; Designer tabanlı formlara (Form1, FormLogin, FormDashboard, FormUrunler, FormArama, FormUrunEkle) tutarlı şekilde uygulanır.
- **DataGridView'ler**: Tüm tablolarda tutarlı başlık stili (lacivert zemin, beyaz yazı, ortalanmış), ince zebra satır rengi, sade seçim rengi, gereksiz kenarlıkların kaldırılması.
- **Dashboard**: Sayısal istatistik kartlarına (Toplam Ürün, Kritik Stok, Firma Sayısı) sade tek renkli ikon + ince renk aksanı eklenir. `System.Windows.Forms.DataVisualization.Charting` paketiyle sade, kurumsal renklerde (lacivert/mavi/gri tonları, gereksiz gridline'sız) bir grafik eklenir — kritik/normal stok dağılımı için basit bir çubuk veya pasta grafik.
- **Login ekranı**: Sade bir başlık/marka alanı, mevcut kart görünümü küçük hizalama/boşluk düzeltmeleriyle daha temiz hale getirilir.

### Kapsanan formlar
Form1 (sidebar/nav), FormLogin, FormDashboard, FormUrunler, FormUrunEkle, FormUrunDuzenle, FormArama, FormLowStock, FormLowStockSecim, FormLowStockLimit, FormUrunDetay, FormAdmin, FormSilOnay — hepsi aynı ikon/renk/tipografi kurallarına çekilir.

---

## Test / Doğrulama Planı

- **Önemli kısıt**: Bu ortam macOS'tur ve üzerinde .NET SDK / Windows Desktop çalışma zamanı yoktur, dolayısıyla WinForms + net8.0-windows projesi burada **derlenip çalıştırılamaz**. Kod satır satır gözden geçirilerek (statik analiz) doğru yazılacak, ama gerçek derleme ve çalıştırma testi Windows + Visual Studio 2022 üzerinde sizin tarafınızdan yapılmalı.
- Sizin yapmanız gereken doğrulamalar: `Ctrl+Shift+B` ile derleme hatasız geçiyor mu, ilk açılışta `%AppData%\FabrikaStokTakipUygulamasi\stok.db` otomatik oluşuyor mu, varsayılan kullanıcılarla giriş yapılabiliyor mu.
- Ürün ekle/düzenle/sil/ara/excel aktar/low-stock akışlarının manuel olarak denenmesi (WinForms için otomatik UI testi kapsam dışı).
- Giriş: doğru/yanlış şifre, "oturumu açık tut" ile yeniden başlatma senaryosu.
- Admin panelinde kullanıcı ekleme/düzenleme/silme, şifre alanının artık boş geldiğinin doğrulanması.
- Derleme sırasında bir hata çıkarsa, hata mesajını paylaşırsanız hemen düzeltirim.

## Riskler

- SQLite dosyası tek makineye bağlı olduğu için bilgisayar değişirse veritabanı da elle taşınmalı (KURULUM.md'de belirtilecek).
- PBKDF2'ye geçiş, varsayılan kullanıcıların şifrelerini sıfırdan hash'lediği için önceki (Postgres'teki) kullanıcı kayıtlarıyla uyumsuz olacak — zaten veri taşınmayacağı için sorun değil.
- Segoe MDL2 Assets fontu çok eski Windows sürümlerinde eksik glyph gösterebilir; hedef kitle güncel Windows 10/11 olduğu için düşük risk.
- **Derleme doğrulaması bu ortamda yapılamadığı için** (yukarıdaki kısıt), gözden kaçan bir sözdizimi hatası ilk denemede Visual Studio'da çıkabilir; bu durumda küçük bir düzeltme turu gerekebilir.

## Uygulama Notu

Proje şu an bir git deposu değil. Çok sayıda dosyanın değişeceği bu iş için, uygulamaya başlamadan önce mevcut hâli bir git deposuna alıp (yerel, push edilmeyecek) ilk commit'i atacağım — böylece istenirse değişiklikler kolayca karşılaştırılabilir/geri alınabilir.
