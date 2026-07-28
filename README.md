<h1 align="center">Fabrika Stok Takip Uygulaması</h1>

<p align="center">
Küçük bir fabrika/depo için ürün ve stok hareketlerini takip eden, giriş korumalı
masaüstü uygulaması (WinForms, .NET 8).
</p>

---

## Özellikler

- **Giriş ve kullanıcı yönetimi** — kullanıcı adı/şifre ile giriş, admin panelinden
  kullanıcı ekleme/yönetme.
- **Ürün yönetimi** — ürün ekleme, düzenleme, detay görüntüleme, arama, silme (onay
  penceresiyle).
- **Düşük stok uyarıları** — ürün bazında stok limiti belirleme, limitin altına
  düşen ürünleri ayrı bir listede gösterme.
- **Dashboard** — stok durumunun özet görünümü.
- **TR/EN dil desteği** — uygulama genelinde anlık dil değiştirme.
- **PostgreSQL veya SQLite** — varsayılan olarak yerel SQLite kullanır; Railway
  üzerinde barındırılan bir PostgreSQL veritabanına da bağlanabilir (bkz.
  [RAILWAY_DATABASE_KURULUM.md](RAILWAY_DATABASE_KURULUM.md)).

## Teknolojiler

C# · .NET 8 (WinForms) · Microsoft.Data.Sqlite · Npgsql (PostgreSQL)

## Kurulum ve çalıştırma

1. `FabrikaStokTakipUygulamasi.sln` dosyasını **Visual Studio 2022** ile aç.
2. **Ctrl+Shift+B** ile derle (NuGet paketleri otomatik indirilir, ilk derlemede
   internet bağlantısı gerekir).
3. **F5** ile çalıştır.

Veritabanı varsayılan olarak `%AppData%\FabrikaStokTakipUygulamasi\stok.db` altında
otomatik oluşturulur. PostgreSQL/Railway ile çalıştırmak için
[RAILWAY_DATABASE_KURULUM.md](RAILWAY_DATABASE_KURULUM.md)'deki adımları izle —
bağlantı bilgisi asla koda gömülmez, bir ortam değişkeni (`STOK_DB_URL`) ile verilir.

Daha fazla kurulum detayı için [KURULUM.md](KURULUM.md).
