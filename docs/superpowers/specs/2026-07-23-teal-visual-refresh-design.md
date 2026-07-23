# Fabrika Stok Takip Uygulaması — Endüstriyel Teal Görsel Kimlik Yenilemesi

**Tarih:** 2026-07-23
**Durum:** Onaylandı (görsel yön kullanıcı tarafından tarayıcı mockup'ında seçildi: "B — Endüstriyel Teal")

## Amaç

Önceki "kurumsal lacivert/mavi" görsel yenilemesinin üzerine, tamamen yeni bir görsel kimlik uygulamak:

1. Yeni renk paleti: koyu petrol/teal + amber vurgu ("Endüstriyel Teal").
2. WinForms'un izin verdiği ölçüde "modern SaaS" his: yuvarlak köşeler, simüle edilmiş yumuşak gölge, hover renk animasyonu.
3. DPI/ekran ölçeklendirme uyumu (`PerMonitorV2`).
4. Daha zengin Dashboard görselliği (tek grafik yerine 2-3 görsel özet).

## Kapsam Dışı (bilinçli olarak yapılmayacak)

- **Gerçek bulanık (blur) gölge**: WinForms'ta native desteklenmiyor. Yerine yarı saydam katmanlı dikdörtgenlerle simüle edilmiş "yumuşak gölge hissi" kullanılacak — CSS `box-shadow` kalitesinde değildir, kullanıcıya bu netleştirildi ve onaylandı.
- **Karanlık mod**: Önceki turda olduğu gibi kapsam dışı, bu turda da talep edilmedi.
- **Yeni Control alt sınıfları (`class YuvarlakButon : Button`) ile Designer.cs'teki control tiplerini değiştirmek**: Bunun yerine, önceki turda kurulan ve kanıtlanmış düşük riskli yöntem korunuyor — mevcut `Button`/`Panel` control'leri aynı tip kalır, sadece `Paint`/`MouseEnter`/`MouseLeave` olaylarına yeni ortak yardımcı metotlar bağlanır (Designer.cs'te control tipi hiç değişmez, sadece birkaç yeni satır eklenir). Bu, Visual Studio Designer ile ileride düzenlemeyi bozmaz ve önceki 17 görevlik planda defalarca doğrulanmış bir desendir.
- **Gerçek performans/animasyon profili testi**: Bu ortamda derleme/çalıştırma yapılamadığı için (macOS, .NET SDK yok), Timer tabanlı animasyonun gerçek akıcılığı yalnızca Windows'ta insan tarafından doğrulanabilir.

---

## 1) Renk Paleti (yeni `UIStil.cs` değerleri)

| Sabit | Yeni Değer (Hex / RGB) | Kullanım |
|---|---|---|
| `Lacivert` → `Teal` | `#0F766E` / `(15,118,110)` | Ana marka rengi, birincil butonlar |
| `LacivertKoyu` → `TealKoyu` | `#0B2E2C` / `(11,46,44)` | Sidebar, header/footer koyu alanlar |
| `Mavi` → `TealAcik` | `#14B8A6` / `(20,184,166)` | Hover/ikincil vurgu |
| `Aksan` | `#D97706` / `(217,119,6)` | Birincil eylem/amber vurgu (kaydet, düzenle) |
| `GriAcik` | `#F6F8F8` / `(246,248,248)` | Sayfa arkaplanı |
| `Beyaz`, `Kritik`, `Basarili`, `Uyari`, `Notr`, `GriOrta`, `GriMetin`, `GriInput` | değişmez | Durum renkleri zaten paletle uyumlu, dokunulmaz |

Mevcut `UIStil.cs`'teki alan **adları** korunur (`Lacivert`, `LacivertKoyu`, `Mavi` vb.) — sadece atanan RGB değerleri değişir. Bu, önceki turda her forma dağılmış onlarca `UIStil.Lacivert` referansının **hiçbirinin değişmeden** yeni renklere otomatik geçmesini sağlar (isim aynı, anlamı/rengi güncellenir).

## 2) Yuvarlak Köşe + Simüle Gölge + Hover Animasyonu (yeni `UIStil.cs` yardımcıları)

Üç yeni statik yardımcı eklenir:

- **`UIStil.YuvarlakBolgeUygula(Control c, int yaricap)`**: `Control.Region`'ı DPI'ya göre ölçeklenmiş bir `GraphicsPath` yuvarlak dikdörtgene ayarlar. `Control.Resize` olayına da otomatik bağlanır (yeniden boyutlanınca bölge güncellenir).
- **`UIStil.YumusakGolgeCiz(Graphics g, Rectangle alan, int derinlik = 4)`**: Bir panelin `Paint` olayında, panelin **hemen dışına** (sağ ve alt kenarlara) azalan opaklıkta birkaç ince dikdörtgen çizerek yumuşak gölge hissi verir. Panelin kendi `Paint`'inde en son çağrılmalı (önce gölge, sonra panelin kendi içeriği).
- **`UIStil.HoverAnimasyonuBagla(Control c, Color normal, Color hover, Action<Color> uygula)`**: Bir `System.Windows.Forms.Timer` (15ms tick) ile `MouseEnter`/`MouseLeave` arasında `normal`↔`hover` rengini adım adım karıştırır (`ColorLerp`), her adımda `uygula(karisikRenk)` callback'ini çağırır (çağıran taraf bunu `BackColor` veya özel bir `Paint` alanına uygular). Timer, animasyon tamamlanınca kendini durdurur (sürekli çalışmaz, CPU'yu boşuna meşgul etmez).

Bu üç yardımcı, önceki turdaki `SolIkonCiz`/`IkonLabel` ile aynı ruhla (statik, bağımsız, tek sorumluluklu) `UIStil.cs`'e eklenir; mevcut üyeler silinmez.

## 3) DPI

`Program.cs`'te `Application.SetHighDpiMode(HighDpiMode.SystemAware)` → `HighDpiMode.PerMonitorV2` yapılır (Windows 10 1703+ üzerinde daha doğru per-monitor ölçekleme sağlar). `YuvarlakBolgeUygula`'nın yarıçap hesaplaması `Control.DeviceDpi / 96f` oranıyla ölçeklenir, böylece %125/%150 gibi ölçeklerde köşeler orantısız büyük/küçük görünmez.

## 4) Dashboard'a ikinci görsel

Mevcut doughnut grafiğe ek olarak, sağ tarafta (veya altında, yer varsa) ikinci küçük bir grafik: **son eklenen 10 ürünün stok miktarlarını gösteren basit bir çubuk grafik** (zaten `TabloyuDoldur()`'da elde edilen veriden, yeni bir DB sorgusu gerekmez). İki grafik de aynı teal/amber paletiyle boyanır.

## 5) Kapsanan formlar

Önceki turda kurumsal palete geçirilen aynı 13 form, bu kez: (a) yeni renklerle otomatik uyumlanır (isim aynı kaldığı için çoğu form hiç dokunulmadan doğru renkte görünür), (b) seçilmiş ana kartlara/panellere yuvarlak köşe + gölge, (c) ana eylem butonlarına hover animasyonu eklenir. Her formun **hangi spesifik panel/buton'una** bu efektlerin uygulanacağı, plan aşamasında görev bazında netleştirilecek (tüm formlara aynı yoğunlukta değil — örn. dialog pencereleri ve ana kartlar öncelikli, küçük yardımcı butonlar ikinci öncelik).

## Test / Doğrulama Planı

- Bu ortamda derleme/çalıştırma yapılamıyor (Global Constraint, önceki plandan devralınır).
- Her görev sonunda kod okunarak self-review yapılır; GitHub Actions (`windows-latest`) her push'ta otomatik derleme kontrolü yapar (önceki turda kuruldu).
- Nihai görsel/DPI/animasyon doğrulaması insan tarafından Windows + Visual Studio'da yapılır.

## Riskler

- Simüle gölge, gerçek blur'a göre daha "keskin katmanlı" görünebilir; Windows'ta ilk bakışta beğenilmezse `derinlik`/opaklık parametreleri kolayca ayarlanabilir (tek yerden, `UIStil.cs`).
- Timer tabanlı hover animasyonu çok sayıda kontrolde aynı anda tetiklenirse (örn. hızlı fare hareketi) teorik olarak hafif CPU artışı olabilir; her Timer sadece kendi animasyonu bitene kadar çalışıp durduğu için pratikte ihmal edilebilir.
- `PerMonitorV2` değişikliği çok monitörlü/farklı DPI'li ortamlarda test edilemedi (bu ortamda imkansız); Windows tarafında insan doğrulaması gerekir.
