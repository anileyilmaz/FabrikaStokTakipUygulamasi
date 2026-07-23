# Fabrika Stok Takip Uygulaması — Faz 0: Fabrika İçi (On-Premise) PostgreSQL Mimarisi

**Tarih:** 2026-07-23
**Durum:** Onaylandı

## Amaç

Bu uygulama bir **basınçlı tank fabrikasında** birden fazla istasyondan (depo, kalite kontrol, mühendislik) eş zamanlı kullanılacak. Mevcut "tek makine, yerel SQLite" mimarisi bunu desteklemiyor. Bu faz, veritabanını fabrika içi yerel ağda (LAN) barınan paylaşımlı bir PostgreSQL sunucusuna taşır — Railway'deki gibi internet bağımlılığı olmadan.

Bu, ileride planlanan malzeme izlenebilirliği / sertifikasyon / kalite kontrol iş akışı gibi fazların üzerine inşa edileceği temel katmandır. Bu fazın kapsamı **sadece veritabanı motoru ve bağlantı mimarisidir** — şemaya yeni alan eklenmez.

## Kapsam Dışı (bilinçli olarak yapılmayacak)

- Yeni şema alanları (malzeme izlenebilirliği, sertifika tipi, iş emri no vb.) — bunlar Faz 1'de bu paylaşımlı veritabanının üzerine eklenecek.
- Railway/bulut verisinin taşınması — sıfırdan başlanacak (önceki SQLite dönüşümünde de aynı karar verilmişti).
- PostgreSQL sunucusunun fiili kurulumu — bu ortamda (macOS, .NET SDK yok) yapılamaz; kullanıcı fabrikadaki bir bilgisayara/sunucuya kendisi kuracak, bu iş için adım adım bir kılavuz hazırlanacak.
- Gerçek bağlantı/entegrasyon testi — bu ortamda gerçek bir PostgreSQL sunucusu çalıştırılamıyor; CI yalnızca derlemeyi doğrular, gerçek bağlantı testi kullanıcı tarafından fabrika sunucusu kurulduktan sonra Windows'ta yapılacak.

## Mimarî

### Bağlantı yapılandırması

Önceki Railway sürümü, bağlantı bilgisini Windows ortam değişkenlerinden (`STOK_DB_URL` vb.) okuyordu — bu, her istasyonda ayrı ayrı ortam değişkeni tanımlamayı gerektiriyordu ve kurulumu zorlaştırıyordu (KURULUM sürtünmesi, bu yüzden SQLite'a geçilmişti). Bu kez, bağlantı bilgisi basit bir **yerel JSON yapılandırma dosyasından** okunacak:

```
%AppData%\FabrikaStokTakipUygulamasi\baglanti.json
```

İçeriği:

```json
{
  "Sunucu": "192.168.1.50",
  "Port": 5432,
  "VeritabaniAdi": "fabrikastok",
  "KullaniciAdi": "stokuygulamasi",
  "SifreSifreliBase64": "..."
}
```

Şifre, `OturumAyarlari.cs`'te zaten kurulu olan Windows DPAPI deseniyle (`ProtectedData`, `DataProtectionScope.CurrentUser`, ayrı bir entropy sabiti) şifrelenip diske öyle yazılır — düz metin olarak hiçbir zaman diske yazılmaz. Bu, ek bir maliyet getirmiyor çünkü aynı yardımcı desen zaten projede var, sadece yeni bir kullanım alanı.

Uygulama ilk açılışta bu dosya yoksa, kullanıcıya (ilk kurulum sırasında) bu bilgileri girebileceği basit bir form gösterir (bkz. Task listesi) ve dosyayı oluşturur. Dosya varsa doğrudan okunup bağlanılır. Bu, her istasyonda tek seferlik bir kurulum adımı olur; hepsi aynı sunucu bilgisini gösterir.

### Veritabanı şeması

Şema, önceki (Railway öncesi) PostgreSQL sürümüyle bire bir aynı kalır (`Urunler`, `StokHareketleri`, `Kullanicilar` tabloları, `SERIAL PRIMARY KEY`, `BYTEA` vb.) — sadece bağlantı katmanı değişir. `StokVeritabani.cs` ve `KullaniciYonetici.cs`, mevcut SQLite sürümünün üzerine tekrar Npgsql/PostgreSQL'e döndürülür; şifre hash'leme (Guvenlik.cs, PBKDF2) ve DPAPI oturum şifrelemesi (OturumAyarlari.cs) **aynen korunur** — onlar veritabanı motorundan bağımsız.

### .csproj

`Microsoft.Data.Sqlite` paketi kaldırılır, `Npgsql` (8.0.x) tekrar eklenir. `WinForms.DataVisualization` ve `System.Security.Cryptography.ProtectedData` paketleri dokunulmadan kalır.

## Test / Doğrulama Planı

- Bu ortamda derleme/çalıştırma yapılamaz — GitHub Actions (`windows-latest`) her push'ta otomatik derleme kontrolü yapar.
- Gerçek PostgreSQL bağlantısı, sunucu kurulumu Windows tarafında tamamlandıktan sonra insan tarafından test edilir (KURULUM.md'ye eklenecek adım adım kılavuzla).

## Riskler

- Fabrika içi sunucu bilgisayarının her zaman açık/erişilebilir olması operasyonel bir gereklilik haline gelir (SQLite'ın "her istasyon kendi dosyasına yazar" basitliği kaybolur). Bu, çoklu istasyon desteğinin doğal bedeli olarak kullanıcı tarafından kabul edildi.
- DPAPI `CurrentUser` kapsamı kullanıldığı için, bağlantı dosyası aynı Windows kullanıcı hesabıyla oluşturulmalıdır; farklı bir Windows hesabından kopyalanırsa şifre çözülemez ve yeniden girilmesi gerekir (aynı davranış `OturumAyarlari.cs` için de zaten geçerli).
