# Fabrika Stok Takip Uygulaması – Kurulum (.NET 8 Sürümü)

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
