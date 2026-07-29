using Xunit;

namespace FabrikaStokTakipUygulamasi.Tests
{
    // LangManager.AktifDil ve ortam değişkenleri process-global durum taşıdığı için
    // (bkz. StokVeritabaniTests, LangManagerTests, KullaniciTests) tüm testler aynı
    // collection'da tutulup paralel çalıştırılmaz.
    [CollectionDefinition("StokTakip Sıralı", DisableParallelization = true)]
    public class TestSirasiCollection { }
}
