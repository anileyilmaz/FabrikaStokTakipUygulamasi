using System;
using System.Collections.Generic;

namespace FabrikaStokTakipUygulamasi
{
    /// <summary>
    /// Uygulama genelinde TR/EN dil yönetimi.
    /// Dil değişince DilDegisti eventi fırlar; formlar buna abone olur.
    /// </summary>
    public static class LangManager
    {
        public enum Dil { TR, EN }

        public static Dil AktifDil { get; private set; } = Dil.TR;

        /// <summary>Dil değişince tüm abone formlar bu eventi alır.</summary>
        public static event Action DilDegisti;

        public static void DilAyarla(Dil dil)
        {
            AktifDil = dil;
            DilDegisti?.Invoke();
        }

        public static bool Ingilizce => AktifDil == Dil.EN;

        // ─── Çeviri sözlüğü ─────────────────────────────────────────────────
        private static readonly Dictionary<string, string> _tr = new()
        {
            // Form başlıkları
            ["nav.dashboard"]     = "Dashboard",
            ["nav.urunler"]       = "Ürünler",
            ["nav.urunekle"]      = "Ürün Ekle",
            ["nav.arama"]         = "Gelişmiş Arama",
            ["nav.lowstock"]      = "Kritik Stok",
            ["nav.admin"]         = "Admin Paneli",
            ["nav.cikis"]         = "Oturumu Kapat",
            ["nav.baslik"]        = "FABRİKA STOK TAKİP UYGULAMASI",

            // Ürünler sayfası butonları
            ["btn.detay"]         = "Ürün Detay",
            ["btn.duzenle"]       = "Düzenle",
            ["btn.sil"]           = "Sil",
            ["btn.excel"]         = "Excel Aktar",

            // Ürün Ekle sayfası
            ["urunekle.baslik"]   = "ÜRÜN EKLE",
            ["urunekle.ekle"]     = "Ürün Ekle",
            ["urunekle.temizle"]  = "Temizle",
            ["urunekle.pdfyukle"] = "PDF Yükle",
            ["urunekle.pdfsec"]   = "Sertifika PDF Seç...",
            ["urunekle.pdfsil"]   = "PDF Kaldır",
            ["urunekle.pdfonay"]  = "PDF Seçildi",
            ["urunekle.pdfyok"]   = "PDF seçilmedi",

            // Ürün Detay
            ["detay.baslik"]      = "ÜRÜN DETAY",
            ["detay.kapat"]       = "Kapat",
            ["detay.sertifika"]   = "Sertifikayı Görüntüle (PDF)",
            ["detay.sertifikayok"]= "Bu ürüne ait sertifika PDF'i bulunmuyor.",
            ["detay.pdfhata"]     = "PDF açılırken hata oluştu",
            ["detay.pdfindir"]    = "PDF'i İndir / Aç",

            // Ürün Düzenle
            ["duzenle.baslik"]    = "ÜRÜN DÜZENLE",
            ["duzenle.guncelle"]  = "Güncelle",
            ["duzenle.iptal"]     = "İptal",
            ["duzenle.pdfmevcut"] = "Mevcut PDF var",
            ["duzenle.pdfdegis"]  = "PDF Değiştir",
            ["duzenle.pdfsil"]    = "PDF Kaldır",

            // Sil Onayı
            ["silonay.mesaj"]     = "Bu ürünü silmek istediğinize\nemin misiniz?",
            ["silonay.evet"]      = "Evet",
            ["silonay.hayir"]     = "Hayır",

            // LowStock
            ["lowstock.yeni"]     = "Yeni",
            ["lowstock.duzenle"]  = "Düzenle",
            ["lowstock.kritik"]   = "Kritik Ürün: ",

            // Genel
            ["genel.tamam"]       = "Tamam",
            ["genel.iptal"]       = "İptal",
            ["genel.uyari"]       = "Uyarı",
            ["genel.hata"]        = "Hata",
            ["genel.basarili"]    = "Başarılı",
            // Login
            ["login.baslik"]          = "FABRİKA STOK TAKİP UYGULAMASI",
            ["login.desc"]            = "Üretim ve depo yönetimi\r\niçin profesyonel stok sistemi",
            ["login.kullanici"]       = "Kullanıcı Adı",
            ["login.sifre"]           = "Şifre",
            ["login.btn"]             = "GİRİŞ YAP",
            ["login.bos"]             = "Kullanıcı adı ve şifre giriniz.",
            ["login.hatali"]          = "Kullanıcı adı veya şifre hatalı.",
            ["login.basarisiz"]       = "Giriş Başarısız",
            // Dashboard
            ["dash.toplamUrun"]       = "Toplam Ürün",
            ["dash.kritikStok"]       = "Kritik Stok",
            ["dash.firmaSayisi"]      = "Firma Sayısı",
            // Arama
            ["arama.bulunan"]         = "Bulunan Kayıt: ",
            // LowStock
            ["ls.kritik"]             = "Kritik Ürün: ",
            ["ls.limit.baslik.yeni"]  = "LOW STOCK LİMİT ATA",
            ["ls.limit.baslik.duz"]   = "LOW STOCK LİMİT DÜZENLE",
            ["ls.limit.form.yeni"]    = "Low Stock Limiti Ata",
            ["ls.limit.form.duz"]     = "Low Stock Limiti Düzenle",
            ["ls.limit.aciklama"]     = "Low Stock uyarısı çıkacak stok sayısı:",
            ["ls.limit.adet"]         = "adet",
            ["ls.limit.bilgi"]        = "Bu değere ulaşıldığında KRİTİK uyarısı verilir.\n   10 stok üstü yaklaşımda AZALIYOR görünür.",
            ["ls.limit.onayla"]       = "Onayla",
            ["ls.limit.guncelle"]     = "Güncelle",
            ["ls.gstok"]              = "Güncel Stok: ",
            ["ls.sec.baslik.yeni"]    = "YENİ LOW STOCK LİMİTİ ATA",
            ["ls.sec.baslik.duz"]     = "LOW STOCK LİMİTİ DÜZENLE",
            ["ls.sec.form.yeni"]      = "Yeni Low Stock Limiti — Ürün Seç",
            ["ls.sec.form.duz"]       = "Low Stock Düzenle — Ürün Seç",
            ["ls.sec.alt.yeni"]       = "Low stock limiti atamak istediğiniz ürüne çift tıklayın.",
            ["ls.sec.alt.duz"]        = "Limiti düzenlemek istediğiniz ürüne çift tıklayın.",
            // Admin
            ["admin.baslik"]          = "ADMİN — STOK HAREKET LOGU",
            ["admin.altbaslik"]       = "Tüm kullanıcıların stok giriş/çıkış hareketleri",
            ["admin.filtre"]          = "Kullanıcı Filtrele:",
            ["admin.yenile"]          = "Yenile",
            ["admin.yenikul"]         = "Yeni Kullanıcı",
            ["admin.col.tarih"]       = "Tarih / Saat",
            ["admin.col.kul"]         = "Kullanıcı",
            ["admin.col.urun"]        = "Ürün",
            ["admin.col.eski"]        = "Önceki Stok",
            ["admin.col.yeni"]        = "Yeni Stok",
            ["admin.col.fark"]        = "Değişim",
            ["admin.col.islem"]       = "İşlem",
            ["admin.kayit"]           = " kayıt",
            ["admin.kulyeni.baslik"]  = "Yeni Kullanıcı Oluştur",
            ["admin.kulyeni.ad"]      = "Kullanıcı Adı:",
            ["admin.kulyeni.sifre"]   = "Şifre:",
            ["admin.kulyeni.rol"]     = "Kullanıcı Rolü:",
            ["admin.kulyeni.rol1"]    = "Depo Personeli",
            ["admin.kulyeni.rol2"]    = "Mühendis",
            ["admin.kulyeni.rol3"]    = "Admin",
            ["admin.kulyeni.kaydet"]  = "Kullanıcı Oluştur",
            ["admin.kulyeni.bos"]     = "Kullanıcı adı ve şifre boş olamaz.",
            ["admin.kulyeni.var"]     = "Bu kullanıcı adı zaten mevcut.",
            ["admin.kulyeni.ok"]      = "Kullanıcı başarıyla oluşturuldu.",
            ["admin.paneli"]          = "Admin Paneli",
            // FormUrunler
            ["urunler.toplam"]        = "Toplam Ürün: ",
            ["urunler.sec"]           = "Lütfen önce listeden bir ürün seçin.",
            ["urunler.silindi"]       = "Ürün başarıyla silindi.",
            // Oturum
            ["oturum.kapat.mesaj"]    = "Oturumu kapatmak istediğinizden emin misiniz?",
            ["oturum.kapat.baslik"]   = "Oturumu Kapat",
            // Ürün Ekle - form field labels
            ["ue.customer"]       = "Müşteri",
            ["ue.certificate"]    = "Sertifika No",
            ["ue.material"]       = "Malzeme",
            ["ue.batch"]          = "Parti No",
            ["ue.parent"]         = "Parent Coil No",
            ["ue.heat"]           = "Isı No",
            ["ue.grade"]          = "Derece",
            ["ue.thk"]            = "Kalınlık (mm)",
            ["ue.width"]          = "Genişlik (mm)",
            ["ue.length"]         = "Uzunluk (mm)",
            ["ue.adet"]           = "Ürün Adedi",
            // Dashboard son ürünler tablo
            ["dash.tablo.urun"]   = "Ürün",
            ["dash.tablo.firma"]  = "Firma",
            ["dash.tablo.tarih"]  = "Tarih",
            // Ürünler tablo
            ["urunler.col.urun"]  = "Ürün",
            ["urunler.col.mat"]   = "Material",
            ["urunler.col.grade"] = "Grade",
            ["urunler.col.thk"]   = "Thickness",
            ["urunler.col.width"] = "Width",
            ["urunler.col.length"]= "Length",
            ["urunler.col.stok"]  = "Stok",
            ["urunler.col.firma"] = "Firma",
            ["urunler.col.tarih"] = "Tarih",
            // LowStock tablo
            ["ls.col.urun"]       = "Ürün",
            ["ls.col.mat"]        = "Material",
            ["ls.col.grade"]      = "Grade",
            ["ls.col.width"]      = "Width",
            ["ls.col.length"]     = "Length",
            ["ls.col.stok"]       = "Güncel Stok",
            ["ls.col.limit"]      = "Limit",
            ["ls.col.durum"]      = "Durum",
            // LowStock durum
            ["ls.durum.kritik"]   = "KRİTİK",
            ["ls.durum.azaliyor"] = "AZALIYOR",
            ["ls.durum.normal"]   = "NORMAL",
            // Admin kullanıcılar tablo
            ["admin.kul.ad"]      = "Kullanıcı Adı",
            ["admin.kul.rol"]     = "Rol",
            ["admin.kul.giris"]   = "Son Giriş",
            ["admin.kul.durum"]   = "Durum",
            ["admin.kul.sifre"]   = "Şifre",
            ["admin.kul.sayi"]    = " kullanıcı",
            // Admin durum
            ["admin.aktif"]       = "Aktif",
            ["admin.offline"]     = "Çevrimdışı",
            ["admin.hic"]         = "Hiç giriş yapmadı",
            // Admin sil onay
            ["admin.sil.onay"]    = "silinecek. Bu işlem geri alınamaz. Emin misiniz?",
            ["admin.sil.baslik"]  = "Silme Onayı",
            ["admin.guncellendi"] = "Kullanıcı güncellendi.",
            // Genel
            ["genel.bilgi"]       = "Bilgi",

        };

        private static readonly Dictionary<string, string> _en = new()
        {
            ["nav.dashboard"]     = "Dashboard",
            ["nav.urunler"]       = "Products",
            ["nav.urunekle"]      = "Add Product",
            ["nav.arama"]         = "Advanced Search",
            ["nav.lowstock"]      = "Low Stock",
            ["nav.admin"]         = "Admin Panel",
            ["nav.cikis"]         = "Sign Out",
            ["nav.baslik"]        = "FACTORY STOCK TRACKING APPLICATION",

            ["btn.detay"]         = "Product Detail",
            ["btn.duzenle"]       = "Edit",
            ["btn.sil"]           = "Delete",
            ["btn.excel"]         = "Export Excel",

            ["urunekle.baslik"]   = "ADD PRODUCT",
            ["urunekle.ekle"]     = "Add Product",
            ["urunekle.temizle"]  = "Clear",
            ["urunekle.pdfyukle"] = "Upload PDF",
            ["urunekle.pdfsec"]   = "Select Certificate PDF...",
            ["urunekle.pdfsil"]   = "Remove PDF",
            ["urunekle.pdfonay"]  = "PDF Selected",
            ["urunekle.pdfyok"]   = "No PDF selected",

            ["detay.baslik"]      = "PRODUCT DETAIL",
            ["detay.kapat"]       = "Close",
            ["detay.sertifika"]   = "View Certificate (PDF)",
            ["detay.sertifikayok"]= "No certificate PDF for this product.",
            ["detay.pdfhata"]     = "Error opening PDF",
            ["detay.pdfindir"]    = "Download / Open PDF",

            ["duzenle.baslik"]    = "EDIT PRODUCT",
            ["duzenle.guncelle"]  = "Update",
            ["duzenle.iptal"]     = "Cancel",
            ["duzenle.pdfmevcut"] = "PDF exists",
            ["duzenle.pdfdegis"]  = "Replace PDF",
            ["duzenle.pdfsil"]    = "Remove PDF",

            ["silonay.mesaj"]     = "Are you sure you want to\ndelete this product?",
            ["silonay.evet"]      = "Yes",
            ["silonay.hayir"]     = "No",

            ["lowstock.yeni"]     = "New",
            ["lowstock.duzenle"]  = "Edit",
            ["lowstock.kritik"]   = "Critical Products: ",

            ["genel.tamam"]       = "OK",
            ["genel.iptal"]       = "Cancel",
            ["genel.uyari"]       = "Warning",
            ["genel.hata"]        = "Error",
            ["genel.basarili"]    = "Success",
            ["ue.customer"]       = "Customer",
            ["ue.certificate"]    = "Certificate No",
            ["ue.material"]       = "Material",
            ["ue.batch"]          = "Batch No",
            ["ue.parent"]         = "Parent Coil No",
            ["ue.heat"]           = "Heat No",
            ["ue.grade"]          = "Grade",
            ["ue.thk"]            = "Thickness (mm)",
            ["ue.width"]          = "Width (mm)",
            ["ue.length"]         = "Length (mm)",
            ["ue.adet"]           = "Quantity",
            ["dash.tablo.urun"]   = "Product",
            ["dash.tablo.firma"]  = "Company",
            ["dash.tablo.tarih"]  = "Date",
            ["urunler.col.urun"]  = "Product",
            ["urunler.col.mat"]   = "Material",
            ["urunler.col.grade"] = "Grade",
            ["urunler.col.thk"]   = "Thickness",
            ["urunler.col.width"] = "Width",
            ["urunler.col.length"]= "Length",
            ["urunler.col.stok"]  = "Stock",
            ["urunler.col.firma"] = "Company",
            ["urunler.col.tarih"] = "Date",
            ["ls.col.urun"]       = "Product",
            ["ls.col.mat"]        = "Material",
            ["ls.col.grade"]      = "Grade",
            ["ls.col.width"]      = "Width",
            ["ls.col.length"]     = "Length",
            ["ls.col.stok"]       = "Current Stock",
            ["ls.col.limit"]      = "Limit",
            ["ls.col.durum"]      = "Status",
            ["ls.durum.kritik"]   = "CRITICAL",
            ["ls.durum.azaliyor"] = "DECREASING",
            ["ls.durum.normal"]   = "NORMAL",
            ["admin.kul.ad"]      = "Username",
            ["admin.kul.rol"]     = "Role",
            ["admin.kul.giris"]   = "Last Login",
            ["admin.kul.durum"]   = "Status",
            ["admin.kul.sifre"]   = "Password",
            ["admin.kul.sayi"]    = " users",
            ["admin.aktif"]       = "Online",
            ["admin.offline"]     = "Offline",
            ["admin.hic"]         = "Never logged in",
            ["admin.sil.onay"]    = "will be deleted. This cannot be undone. Are you sure?",
            ["admin.sil.baslik"]  = "Delete Confirmation",
            ["admin.guncellendi"] = "User updated.",
            ["genel.bilgi"]       = "Info",

            ["login.baslik"]          = "FACTORY STOCK TRACKING APPLICATION",
            ["login.desc"]            = "Professional stock system\r\nfor production and warehouse management",
            ["login.kullanici"]       = "Username",
            ["login.sifre"]           = "Password",
            ["login.btn"]             = "LOG IN",
            ["login.bos"]             = "Please enter username and password.",
            ["login.hatali"]          = "Incorrect username or password.",
            ["login.basarisiz"]       = "Login Failed",
            ["dash.toplamUrun"]       = "Total Products",
            ["dash.kritikStok"]       = "Critical Stock",
            ["dash.firmaSayisi"]      = "Company Count",
            ["arama.bulunan"]         = "Results: ",
            ["ls.kritik"]             = "Critical Products: ",
            ["ls.limit.baslik.yeni"]  = "SET LOW STOCK LIMIT",
            ["ls.limit.baslik.duz"]   = "EDIT LOW STOCK LIMIT",
            ["ls.limit.form.yeni"]    = "Set Low Stock Limit",
            ["ls.limit.form.duz"]     = "Edit Low Stock Limit",
            ["ls.limit.aciklama"]     = "Stock quantity to trigger low stock alert:",
            ["ls.limit.adet"]         = "units",
            ["ls.limit.bilgi"]        = "CRITICAL alert when this value is reached.\n   DECREASING shown when within 10 units above.",
            ["ls.limit.onayla"]       = "Confirm",
            ["ls.limit.guncelle"]     = "Update",
            ["ls.gstok"]              = "Current Stock: ",
            ["ls.sec.baslik.yeni"]    = "SET NEW LOW STOCK LIMIT",
            ["ls.sec.baslik.duz"]     = "EDIT LOW STOCK LIMIT",
            ["ls.sec.form.yeni"]      = "New Low Stock Limit — Select Product",
            ["ls.sec.form.duz"]       = "Edit Low Stock — Select Product",
            ["ls.sec.alt.yeni"]       = "Double-click a product to set its low stock limit.",
            ["ls.sec.alt.duz"]        = "Double-click a product to edit its low stock limit.",
            ["admin.baslik"]          = "ADMIN — STOCK MOVEMENT LOG",
            ["admin.altbaslik"]       = "All users\' stock in/out movements",
            ["admin.filtre"]          = "Filter by User:",
            ["admin.yenile"]          = "Refresh",
            ["admin.yenikul"]         = "New User",
            ["admin.col.tarih"]       = "Date / Time",
            ["admin.col.kul"]         = "User",
            ["admin.col.urun"]        = "Product",
            ["admin.col.eski"]        = "Previous Stock",
            ["admin.col.yeni"]        = "New Stock",
            ["admin.col.fark"]        = "Change",
            ["admin.col.islem"]       = "Action",
            ["admin.kayit"]           = " records",
            ["admin.kulyeni.baslik"]  = "Create New User",
            ["admin.kulyeni.ad"]      = "Username:",
            ["admin.kulyeni.sifre"]   = "Password:",
            ["admin.kulyeni.rol"]     = "User Role:",
            ["admin.kulyeni.rol1"]    = "Warehouse Staff",
            ["admin.kulyeni.rol2"]    = "Engineer",
            ["admin.kulyeni.rol3"]    = "Admin",
            ["admin.kulyeni.kaydet"]  = "Create User",
            ["admin.kulyeni.bos"]     = "Username and password cannot be empty.",
            ["admin.kulyeni.var"]     = "This username already exists.",
            ["admin.kulyeni.ok"]      = "User created successfully.",
            ["admin.paneli"]          = "Admin Panel",
            ["urunler.toplam"]        = "Total Products: ",
            ["urunler.sec"]           = "Please select a product from the list first.",
            ["urunler.silindi"]       = "Product deleted successfully.",
            ["oturum.kapat.mesaj"]    = "Are you sure you want to sign out?",
            ["oturum.kapat.baslik"]   = "Sign Out",
        };

        /// <summary>Verilen anahtara göre aktif dilde metni döndürür.</summary>
        public static string T(string anahtar)
        {
            var dict = AktifDil == Dil.TR ? _tr : _en;
            return dict.TryGetValue(anahtar, out string deger) ? deger : anahtar;
        }
    }
}