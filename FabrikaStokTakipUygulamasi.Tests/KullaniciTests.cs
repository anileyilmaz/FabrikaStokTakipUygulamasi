using Xunit;

namespace FabrikaStokTakipUygulamasi.Tests
{
    [Collection("StokTakip Sıralı")]
    public class KullaniciTests
    {
        public KullaniciTests()
        {
            LangManager.DilAyarla(LangManager.Dil.TR);
        }

        [Theory]
        [InlineData(KullaniciRol.Admin, "Admin")]
        [InlineData(KullaniciRol.Muhendis, "Mühendis")]
        [InlineData(KullaniciRol.DepoPersoneli, "Depo Personeli")]
        public void RolAdi_TR_dilinde_dogru_metni_dondurur(KullaniciRol rol, string beklenen)
        {
            LangManager.DilAyarla(LangManager.Dil.TR);
            var kullanici = new Kullanici { Rol = rol };

            Assert.Equal(beklenen, kullanici.RolAdi);
        }

        [Theory]
        [InlineData(KullaniciRol.Admin, "Admin")]
        [InlineData(KullaniciRol.Muhendis, "Engineer")]
        [InlineData(KullaniciRol.DepoPersoneli, "Warehouse Staff")]
        public void RolAdi_EN_dilinde_dogru_metni_dondurur(KullaniciRol rol, string beklenen)
        {
            LangManager.DilAyarla(LangManager.Dil.EN);
            var kullanici = new Kullanici { Rol = rol };

            Assert.Equal(beklenen, kullanici.RolAdi);
        }

        [Fact]
        public void IsAdmin_yalnizca_Admin_rolunde_true_doner()
        {
            Assert.True(new Kullanici { Rol = KullaniciRol.Admin }.IsAdmin);
            Assert.False(new Kullanici { Rol = KullaniciRol.Muhendis }.IsAdmin);
            Assert.False(new Kullanici { Rol = KullaniciRol.DepoPersoneli }.IsAdmin);
        }
    }
}
