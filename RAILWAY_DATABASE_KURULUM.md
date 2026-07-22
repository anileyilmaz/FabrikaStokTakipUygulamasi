# Railway PostgreSQL Kurulumu

Bu sürümde proje lokal SQLite yerine Railway PostgreSQL kullanacak şekilde güncellendi.

## 1) Railway'de PostgreSQL oluştur

1. Railway hesabına gir.
2. New Project oluştur.
3. Add Service / Database kısmından PostgreSQL ekle.
4. PostgreSQL servisinin Variables bölümünden dış bağlantı için `DATABASE_PUBLIC_URL` değerini kopyala.

> Uygulama bilgisayardan çalışacağı için Railway tarafında public TCP bağlantısı gerekir. Railway dokümanına göre PostgreSQL servisi `PGHOST`, `PGPORT`, `PGUSER`, `PGPASSWORD`, `PGDATABASE` ve `DATABASE_URL` değişkenlerini sağlar; dış bağlantı için TCP Proxy kullanılabilir.

## 2) Windows ortam değişkeni ekle

Windows aramasına `Ortam değişkenleri` yaz → Sistem ortam değişkenlerini düzenle → Environment Variables.

Yeni kullanıcı değişkeni ekle:

- Değişken adı: `STOK_DB_URL`
- Değişken değeri: Railway'den aldığın `DATABASE_PUBLIC_URL`

Örnek format:

```txt
postgresql://postgres:şifre@host.railway.app:12345/railway
```

Visual Studio açıksa kapatıp tekrar aç. Çünkü ortam değişkenini yeni okumalı.

## 3) Projeyi çalıştır

Projeyi Visual Studio'da açıp NuGet paketlerini restore et. İlk çalıştırmada tablolar otomatik oluşur:

- `Urunler`
- `StokHareketleri`

## 4) Eski SQLite verileri ne olacak?

Bu paket bağlantıyı PostgreSQL'e çevirdi. Eski `stok.db` içindeki veriler otomatik taşınmaz. Gerekirse ayrıca SQLite → PostgreSQL veri taşıma scripti hazırlanmalı.

## Değişen dosyalar

- `StokTakipUI/StokVeritabani.cs`
- `StokTakipUI/StokTakipUI.csproj`

## Not

Railway bağlantı bilgisini koda yazmadım. Şifreli database adresi GitHub'a veya projeye gömülmemeli; ortam değişkeniyle kullanılmalı.
