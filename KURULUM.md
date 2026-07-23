# Fabrika Stok Takip Uygulaması – Kurulum (.NET 8 Sürümü)

## Mimari

Bu uygulama artık **fabrika içi paylaşımlı bir PostgreSQL sunucusu** kullanır — tüm istasyonlar (depo, kalite kontrol, mühendislik) aynı veritabanına yerel ağ (LAN) üzerinden bağlanır. İnternet bağlantısı gerekmez.

## 1) Sunucu kurulumu (SADECE BİR KEZ, bir bilgisayarda/sunucuda yapılır)

1. Fabrikada sürekli açık kalacak bir bilgisayar seçin (bu, veritabanı sunucusu olacak).
2. [postgresql.org/download/windows](https://www.postgresql.org/download/windows/) adresinden PostgreSQL'i indirip kurun (kurulum sırasında bir "postgres" kullanıcı şifresi belirlemeniz istenecek — bunu not edin).
3. Kurulum sırasında gelen **pgAdmin** aracını açın, yeni bir veritabanı oluşturun (örn. adı: `fabrikastok`) ve uygulamanın kullanacağı ayrı bir kullanıcı oluşturun (örn. kullanıcı adı: `stokuygulamasi`, güçlü bir şifre belirleyin).
4. **Bu yeni kullanıcıya `fabrikastok` veritabanı üzerinde yetki verin** — PostgreSQL 15 ve üzerinde bu adım atlanırsa uygulama ilk açılışta tablo oluşturamaz ve "Veritabanı başlatılamadı" hatasıyla kapanır. `fabrikastok` veritabanını seçili haldeyken pgAdmin'in Query Tool'unda şunu çalıştırın:
   ```sql
   GRANT ALL PRIVILEGES ON DATABASE fabrikastok TO stokuygulamasi;
   GRANT ALL ON SCHEMA public TO stokuygulamasi;
   ```
   (Kullanıcı adını 3. adımda seçtiğiniz isimle değiştirin.)
5. PostgreSQL'in yerel ağdan bağlantı kabul etmesi için:
   - `C:\Program Files\PostgreSQL\<sürüm>\data\postgresql.conf` dosyasında `listen_addresses = '*'` satırını etkinleştirin.
   - Aynı klasördeki `pg_hba.conf` dosyasına, fabrika ağınızın IP aralığına izin veren bir satır ekleyin, örneğin:
     ```
     host    all             all             192.168.1.0/24          scram-sha-256
     ```
     (Kendi ağınızın adresine göre uyarlayın — IT ekibinize danışın.)
   - PostgreSQL servisini yeniden başlatın (Hizmetler / Services içinden).
6. Windows Güvenlik Duvarı'nda 5432 portuna gelen bağlantılara izin verin (Gelen Kurallar / Inbound Rules).
7. Bu bilgisayarın yerel ağ IP adresini not edin (`ipconfig` ile görülebilir, örn. `192.168.1.50`) — her istasyonun bağlantı ayarlarında bu adres kullanılacak.

## 2) Her istasyonda (uygulamanın çalışacağı her bilgisayarda)

1. `FabrikaStokTakipUygulamasi.sln` dosyasını **Visual Studio 2022** ile açın.
2. **Ctrl + Shift + B** ile derleyin (NuGet paketleri otomatik indirilir).
3. **F5** ile çalıştırın.
4. İlk çalıştırmada "Sunucu Bağlantı Ayarları" ekranı açılır — sunucu adımında not ettiğiniz IP adresini, portu (5432), veritabanı adını ve uygulama kullanıcısının bilgilerini girin, **Kaydet ve Devam Et**'e basın. Bu bilgi bu bilgisayara özel, şifrelenmiş olarak saklanır (`%AppData%\FabrikaStokTakipUygulamasi\baglanti.json`) — bir daha sorulmaz.
5. Varsayılan kullanıcılarla giriş yapabilirsiniz: `emir/1234`, `barkan/1234` (Depo Personeli), `anil/1234`, `goksu/1234` (Mühendis), `admin/admin` (Admin) — şifreler veritabanında hash'lenerek saklanır.

## Notlar

- Sunucu bilgisayarı kapalıyken hiçbir istasyon uygulamayı kullanamaz — sunucunun sürekli açık kalması operasyonel bir gerekliliktir.
- Bağlantı bilgileri yanlışsa veya sunucu IP'si değiştiyse, uygulama açılışta bağlanamadığında "Sunucu bağlantı bilgilerini yeniden girmek ister misiniz?" diye sorar — **Evet** derseniz kurulum ekranı otomatik olarak tekrar açılır, dosyayı elle silmenize gerek yoktur.
