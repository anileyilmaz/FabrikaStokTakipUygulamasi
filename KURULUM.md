# Fabrika Stok Takip Uygulaması – Kurulum (NET 8 Sürümü)

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
