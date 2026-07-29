using System.Runtime.CompilerServices;

// Test projesinin internal üyelere (ör. StokVeritabani'nin bağlantı string
// ayrıştırma mantığı) veritabanına dokunmadan erişebilmesi için.
[assembly: InternalsVisibleTo("FabrikaStokTakipUygulamasi.Tests")]
