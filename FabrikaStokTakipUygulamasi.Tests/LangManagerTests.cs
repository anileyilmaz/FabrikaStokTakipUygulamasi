using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace FabrikaStokTakipUygulamasi.Tests
{
    [Collection("StokTakip Sıralı")]
    public class LangManagerTests
    {
        public LangManagerTests()
        {
            // Her test varsayılan dille (TR) başlasın; testler arası sızıntıyı önler.
            LangManager.DilAyarla(LangManager.Dil.TR);
        }

        [Fact]
        public void TR_aktifken_bilinen_anahtar_turkce_metni_dondurur()
        {
            LangManager.DilAyarla(LangManager.Dil.TR);
            Assert.Equal("Ürünler", LangManager.T("nav.urunler"));
        }

        [Fact]
        public void EN_aktifken_ayni_anahtar_ingilizce_metni_dondurur()
        {
            LangManager.DilAyarla(LangManager.Dil.EN);
            Assert.Equal("Products", LangManager.T("nav.urunler"));
        }

        [Fact]
        public void DilAyarla_Ingilizce_bayragini_gunceller()
        {
            Assert.False(LangManager.Ingilizce);
            LangManager.DilAyarla(LangManager.Dil.EN);
            Assert.True(LangManager.Ingilizce);
        }

        [Fact]
        public void Bilinmeyen_anahtar_icin_anahtarin_kendisi_doner()
        {
            Assert.Equal("hic.olmayan.anahtar", LangManager.T("hic.olmayan.anahtar"));
        }

        [Fact]
        public void DilAyarla_degisince_DilDegisti_eventi_tetiklenir()
        {
            var tetiklendi = false;
            void Handler() => tetiklendi = true;

            LangManager.DilDegisti += Handler;
            try
            {
                LangManager.DilAyarla(LangManager.Dil.EN);
                Assert.True(tetiklendi);
            }
            finally
            {
                LangManager.DilDegisti -= Handler;
            }
        }

        [Fact]
        public void TR_ve_EN_sozlukleri_ayni_anahtar_kumesine_sahiptir()
        {
            // Reflection: iki sözlük de private static readonly, üretim API'sini
            // genişletmeden eksik/fazla çeviri anahtarlarını yakalamak için.
            var flags = BindingFlags.NonPublic | BindingFlags.Static;
            var trField = typeof(LangManager).GetField("_tr", flags);
            var enField = typeof(LangManager).GetField("_en", flags);

            Assert.NotNull(trField);
            Assert.NotNull(enField);

            var tr = (Dictionary<string, string>)trField.GetValue(null);
            var en = (Dictionary<string, string>)enField.GetValue(null);

            var trOlupEnOlmayan = new List<string>();
            foreach (var anahtar in tr.Keys)
            {
                if (!en.ContainsKey(anahtar)) trOlupEnOlmayan.Add(anahtar);
            }

            var enOlupTrOlmayan = new List<string>();
            foreach (var anahtar in en.Keys)
            {
                if (!tr.ContainsKey(anahtar)) enOlupTrOlmayan.Add(anahtar);
            }

            Assert.True(
                trOlupEnOlmayan.Count == 0 && enOlupTrOlmayan.Count == 0,
                $"TR'de olup EN'de olmayan: [{string.Join(", ", trOlupEnOlmayan)}] | " +
                $"EN'de olup TR'de olmayan: [{string.Join(", ", enOlupTrOlmayan)}]");
        }
    }
}
