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
- **PostgreSQL (Railway)** — veriler Railway üzerinde barındırılan bir PostgreSQL
  veritabanında tutulur (bkz. [RAILWAY_DATABASE_KURULUM.md](RAILWAY_DATABASE_KURULUM.md)).

## Teknolojiler

C# · .NET 8 (WinForms) · Npgsql (PostgreSQL)

## Kurulum ve çalıştırma

1. `FabrikaStokTakipUygulamasi.sln` dosyasını **Visual Studio 2022** ile aç.
2. **Ctrl+Shift+B** ile derle (NuGet paketleri otomatik indirilir, ilk derlemede
   internet bağlantısı gerekir).
3. Bir Windows ortam değişkeni olarak `STOK_DB_URL`'i Railway PostgreSQL bağlantı
   adresine ayarla (bkz. [RAILWAY_DATABASE_KURULUM.md](RAILWAY_DATABASE_KURULUM.md)) —
   bu değişken olmadan uygulama açılmaz, bağlantı bilgisi koda gömülmez.
4. **F5** ile çalıştır.

Daha fazla kurulum detayı için [KURULUM.md](KURULUM.md).
