# Fabrika Stok Takip Uygulaması – Kurulum (NET 8 Sürümü)

## Neden bu sürüm?
.NET Framework 4.7.2 targeting pack bilgisayarınızda yüklü değildi.
Bu sürüm .NET 8 (Windows) kullanıyor — modern Visual Studio'larda her zaman hazır gelir.

## Adımlar

1. `FabrikaStokTakipUygulamasi.sln` dosyasını **Visual Studio 2022** ile açın
2. **Ctrl + Shift + B** ile derleyin (NuGet paketi otomatik indirilir)
3. **F5** ile çalıştırın

## Notlar
- Veritabanı: Railway üzerinde barındırılan PostgreSQL (Npgsql paketi, otomatik indirilir) —
  detaylı kurulum için [RAILWAY_DATABASE_KURULUM.md](RAILWAY_DATABASE_KURULUM.md)'ye bakın.
  `STOK_DB_URL` ortam değişkeni ayarlanmadan uygulama açılmaz.
- İnternet bağlantısı ilk derlemede (NuGet indirmesi için) ve çalışma zamanında
  (Railway'e bağlanmak için) gereklidir.
