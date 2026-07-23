using System;
using System.Windows.Forms;

namespace FabrikaStokTakipUygulamasi
{
    internal static class Program
    {
        public static StokAppContext AppContext { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                if (!BaglantiAyarlari.DosyaVarMi())
                {
                    using (var ayarForm = new FormSunucuAyarlari())
                    {
                        if (ayarForm.ShowDialog() != DialogResult.OK)
                            return; // Kullanıcı iptal etti — uygulama açılmaz
                    }
                }

                try
                {
                    StokVeritabani.Baslat();
                    break; // Bağlantı başarılı — kuruluma devam et
                }
                catch (Exception hata)
                {
                    var secim = MessageBox.Show(
                        "Veritabanına bağlanılamadı:\n" + hata.Message +
                        "\n\nSunucu bağlantı bilgilerini yeniden girmek ister misiniz?",
                        "Bağlantı Hatası", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                    if (secim != DialogResult.Yes) return;

                    BaglantiAyarlari.Sil(); // Yeniden sorulması için mevcut (muhtemelen hatalı) ayarı temizle
                }
            }

            AppContext = new StokAppContext();
            Application.Run(AppContext);
        }
    }

    /// <summary>
    /// Oturum açma/kapama döngüsünü yönetir.
    /// FormLogin kapansa bile program, yeni FormLogin gösterene kadar yaşar.
    /// </summary>
    public class StokAppContext : ApplicationContext
    {
        public StokAppContext()
        {
            LoginGoster();
        }

        public void LoginGoster()
        {
            var login = new FormLogin();
            login.FormClosed += (s, e) =>
            {
                // Login kapatıldı ama bir Form1 zaten açık olabilir —
                // Form1 kendi FormClosed'unda ExitApplication çağırır.
                // Eğer hiç form açık kalmadıysa uygulama zaten kapanır.
            };
            MainForm = login;
            login.Show();
        }

        public void CikisYapVeLoginGoster()
        {
            LoginGoster();
        }

        public void UygulamaKapat()
        {
            ExitThread();
        }
    }
}
