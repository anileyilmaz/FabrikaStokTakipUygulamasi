using System;
using System.Windows.Forms;

namespace StokTakipUI
{
    internal static class Program
    {
        public static StokAppContext AppContext { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try { StokVeritabani.Baslat(); }
            catch (Exception hata)
            {
                MessageBox.Show("Veritabanı başlatılamadı:\n" + hata.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
