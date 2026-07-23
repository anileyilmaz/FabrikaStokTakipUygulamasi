# Fabrika Stok Takip Uygulaması — Endüstriyel Teal Görsel Kimlik — Uygulama Planı

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the existing "kurumsal lacivert" palette with a new "Endüstriyel Teal" identity, and add WinForms-feasible "modern SaaS" polish (rounded corners, simulated soft shadow, hover color animation) plus a DPI bump and a richer Dashboard, across the same set of forms already migrated to `UIStil` in the previous plan.

**Architecture:** `UIStil.cs`'s existing color *field names* are repointed to new teal/amber RGB values (every form already referencing `UIStil.Lacivert` etc. picks up the new color with zero changes there). Three new additive helper methods are added to `UIStil.cs`: `YuvarlakBolgeUygula` (rounded `Region`), `YumusakGolgeCiz` (layered-rectangle shadow simulation, painted on the parent behind a card), `HoverAnimasyonuBagla` (Timer-based color-lerp hover animation). Per-form tasks wire these onto existing controls **in the `.cs` code-behind constructors only** — no `Designer.cs` control-type changes, no new Control subclasses.

**Tech Stack:** .NET 8 (`net8.0-windows`), WinForms, `System.Drawing.Drawing2D` (already available, no new package).

**Spec:** `docs/superpowers/specs/2026-07-23-teal-visual-refresh-design.md`

## Global Constraints

- No new NuGet packages required for this plan.
- Color field **names** in `UIStil.cs` (`Lacivert`, `LacivertKoyu`, `Mavi`, `MaviAcik`, `Aksan`, `GriAcik`, `Beyaz`, `Kritik`, `Basarili`, `Uyari`, `Notr`, `GriOrta`, `GriMetin`, `GriInput`) must NOT be renamed — only the RGB values of `Lacivert`, `LacivertKoyu`, `Mavi`, `MaviAcik` change (per the table in Task 1). Every other form's existing `UIStil.X` references keep compiling and automatically pick up the new color.
- **Rounded corners and shadow simulation apply ONLY to `Panel` "card" containers and dialog windows — never to `DataGridView` controls.** Setting a custom `Region` on a `DataGridView` risks clipping headers/scrollbars/cell rendering in ways that can't be verified without a Windows runtime; grids keep their existing (already-corporate) square styling from the previous plan.
- Hover color animation applies to primary action `Button`s (the ones a user clicks most: login, save/update, add, primary toolbar actions) — not to every single button in the app. Each task below names exactly which buttons.
- All new wiring happens in **`.cs` code-behind constructors** (after `InitializeComponent();`), as additive lines — never edit `Designer.cs` control *type* declarations, and do not remove any existing `Paint`/`Click` handler wired by the previous plan (icons drawn via `UIStil.SolIkonCiz` must keep working — a rounded `Region` and a shadow are painted independently and do not conflict with icon painting).
- This environment has no .NET SDK — every task's verification step is a self-review (re-read the diff) rather than a compiler run. A GitHub Actions workflow (`.github/workflows/build.yml`, already set up) builds on `windows-latest` on every push to this branch — treat a red CI run after your task's commit as equivalent to a failed local build and fix it.
- Every task ends with a `git commit`, on the existing branch `feature/sqlite-migration-ui-refresh` (do not create a new branch — this is a continuation of unreleased work already on that branch).

---

## File Structure (changed files)

| File | Change |
|---|---|
| `FabrikaStokTakipUygulamasi/UIStil.cs` | New teal/amber color values; three new helper methods |
| `FabrikaStokTakipUygulamasi/Program.cs` | `HighDpiMode.SystemAware` → `HighDpiMode.PerMonitorV2` |
| `FabrikaStokTakipUygulamasi/Form1.cs` | Sidebar nav buttons: hover color animation replacing the static `FlatAppearance.MouseOverBackColor` jump-cut |
| `FabrikaStokTakipUygulamasi/FormLogin.cs` | Login button hover animation; rounded+shadow on the credentials area |
| `FabrikaStokTakipUygulamasi/FormDashboard.cs`, `FormDashboard.Designer.cs` | Rounded+shadow on the three stat cards and the chart card; second chart (last 10 products' stock, bar) |
| `FabrikaStokTakipUygulamasi/FormUrunler.cs`, `FormArama.cs` | Toolbar primary-button hover animation |
| `FabrikaStokTakipUygulamasi/FormUrunEkle.cs`, `FormUrunDuzenle.cs` | Rounded+shadow on main content card / PDF panel; primary button hover animation |
| `FabrikaStokTakipUygulamasi/FormLowStock.cs`, `FormLowStockSecim.cs`, `FormLowStockLimit.cs` | Rounded+shadow on dialog cards; primary button hover animation |
| `FabrikaStokTakipUygulamasi/FormUrunDetay.cs`, `FormSilOnay.cs` | Rounded+shadow on dialog cards; primary button hover animation |
| `FabrikaStokTakipUygulamasi/FormAdmin.cs` | Rounded+shadow on header panel; toolbar primary-button hover animation |

---

### Task 1: `UIStil.cs` — yeni renkler + üç yeni yardımcı, `Program.cs` DPI

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/UIStil.cs`
- Modify: `FabrikaStokTakipUygulamasi/Program.cs`

**Interfaces:**
- Produces: updated `UIStil.Lacivert`/`LacivertKoyu`/`Mavi`/`MaviAcik`/`Aksan` values; new `UIStil.YuvarlakBolgeUygula(Control, int)`, `UIStil.YumusakGolgeCiz(Graphics, Rectangle, int derinlik = 4)`, `UIStil.HoverAnimasyonuBagla(Control, Color normal, Color hover, Action<Color> uygula)`. Consumed by every later task in this plan.

- [ ] **Step 1: Update the four color values**

In `FabrikaStokTakipUygulamasi/UIStil.cs`, find the four lines:

```csharp
        public static readonly Color Lacivert     = Color.FromArgb(44, 62, 80);    // ana marka rengi
        public static readonly Color LacivertKoyu = Color.FromArgb(30, 44, 57);    // header/sidebar koyu tonu
        public static readonly Color Mavi         = Color.FromArgb(41, 128, 185);  // aktif/vurgu
        public static readonly Color MaviAcik     = Color.FromArgb(52, 152, 219);  // hover/ikincil buton
```

Replace with:

```csharp
        public static readonly Color Lacivert     = Color.FromArgb(15, 118, 110);  // Teal — ana marka rengi
        public static readonly Color LacivertKoyu = Color.FromArgb(11, 46, 44);    // TealKoyu — header/sidebar koyu tonu
        public static readonly Color Mavi         = Color.FromArgb(20, 184, 166);  // TealAcik — aktif/vurgu
        public static readonly Color MaviAcik     = Color.FromArgb(45, 212, 191);  // hover/ikincil buton (daha açık teal)
```

Find:

```csharp
        public static readonly Color Aksan        = Color.FromArgb(243, 156, 18);  // birincil eylem (kaydet/düzenle) — turuncu-altın
```

Replace with:

```csharp
        public static readonly Color Aksan        = Color.FromArgb(217, 119, 6);   // birincil eylem (kaydet/düzenle) — amber
```

`GriAcik`, `Beyaz`, `Kritik`, `Basarili`, `Uyari`, `Notr`, `GriOrta`, `GriMetin`, `GriInput` are unchanged — do not touch them.

- [ ] **Step 2: Add the `using` needed for `GraphicsPath`**

At the top of the file, find:

```csharp
using System.Drawing;
using System.Windows.Forms;
```

Replace with:

```csharp
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
```

- [ ] **Step 3: Add the three new helper methods**

Find the closing of the class (the final `}` that matches `public static class UIStil`, right after the `IkonLabel` method — i.e. the line that currently reads just `}` followed by the namespace's closing `}`). Insert the following **before** that final class-closing `}`:

```csharp

        // ── Yuvarlak köşe / gölge / hover animasyonu ────────────────────────

        /// <summary>
        /// Bir kontrolün köşelerini DPI'ya göre ölçeklenmiş yarıçapla yuvarlar.
        /// Kontrol yeniden boyutlanınca bölge otomatik yeniden hesaplanır.
        /// SADECE Panel/dialog gibi kart-benzeri kapsayıcılarda kullanılır —
        /// DataGridView'lerde KULLANILMAZ (bkz. plan Global Constraints).
        /// </summary>
        public static void YuvarlakBolgeUygula(Control c, int yaricap)
        {
            void Uygula()
            {
                if (c.Width <= 0 || c.Height <= 0) return;
                float olcek = c.DeviceDpi / 96f;
                int r = Math.Max(2, (int)(yaricap * olcek));
                using (var yol = YuvarlakDikdortgenYolu(new Rectangle(0, 0, c.Width, c.Height), r))
                    c.Region = new Region(yol);
            }
            c.Resize += (s, e) => Uygula();
            Uygula();
        }

        private static GraphicsPath YuvarlakDikdortgenYolu(Rectangle alan, int r)
        {
            var yol = new GraphicsPath();
            int cap = r * 2;
            yol.AddArc(alan.X, alan.Y, cap, cap, 180, 90);
            yol.AddArc(alan.Right - cap, alan.Y, cap, cap, 270, 90);
            yol.AddArc(alan.Right - cap, alan.Bottom - cap, cap, cap, 0, 90);
            yol.AddArc(alan.X, alan.Bottom - cap, cap, cap, 90, 90);
            yol.CloseFigure();
            return yol;
        }

        /// <summary>
        /// Bir kart panelinin sağ ve alt kenarına, azalan opaklıkta ince çizgiler
        /// çizerek yumuşak bir "gölge hissi" simüle eder (gerçek blur DEĞİLDİR —
        /// WinForms'ta native desteklenmiyor, bkz. tasarım dokümanı).
        /// KULLANIM: panelin kendi Paint'inde değil, panelin PARENT kontrolünün
        /// Paint olayında, panelin Bounds'u verilerek çağrılır — böylece gölge
        /// panelin dışına (sağ/alt) taşan kısmı görünür, panelin kendi içeriğinin
        /// üzerini örtmez:
        ///   this.Paint += (s, e) => UIStil.YumusakGolgeCiz(e.Graphics, panelCard.Bounds);
        /// </summary>
        public static void YumusakGolgeCiz(Graphics g, Rectangle alan, int derinlik = 4)
        {
            for (int i = 1; i <= derinlik; i++)
            {
                int alfa = (int)(60 * (1f - (float)i / (derinlik + 1)));
                using (var kalem = new Pen(Color.FromArgb(alfa, 0, 0, 0)))
                {
                    g.DrawLine(kalem, alan.Right + i, alan.Y + i, alan.Right + i, alan.Bottom + i);
                    g.DrawLine(kalem, alan.X + i, alan.Bottom + i, alan.Right + i, alan.Bottom + i);
                }
            }
        }

        /// <summary>
        /// Bir kontrole fare üzerine gelince/ayrılınca iki renk arasında yumuşak
        /// geçiş animasyonu bağlar. Timer SADECE animasyon sürerken çalışır ve
        /// bitince kendini durdurur (sürekli çalışmaz).
        /// KULLANIM: UIStil.HoverAnimasyonuBagla(btnLogin, UIStil.Mavi, UIStil.MaviAcik,
        ///           renk => btnLogin.BackColor = renk);
        /// </summary>
        public static void HoverAnimasyonuBagla(Control c, Color normal, Color hover, Action<Color> uygula)
        {
            float ilerleme = 0f;
            bool hoverAktif = false;
            var zamanlayici = new System.Windows.Forms.Timer { Interval = 15 };

            zamanlayici.Tick += (s, e) =>
            {
                const float adim = 0.12f;
                ilerleme = hoverAktif ? Math.Min(1f, ilerleme + adim) : Math.Max(0f, ilerleme - adim);
                uygula(RenkKaristir(normal, hover, ilerleme));
                if ((hoverAktif && ilerleme >= 1f) || (!hoverAktif && ilerleme <= 0f))
                    zamanlayici.Stop();
            };

            c.MouseEnter += (s, e) => { hoverAktif = true; zamanlayici.Start(); };
            c.MouseLeave += (s, e) => { hoverAktif = false; zamanlayici.Start(); };
            c.Disposed += (s, e) => zamanlayici.Dispose();

            uygula(normal);
        }

        private static Color RenkKaristir(Color a, Color b, float oran)
        {
            oran = Math.Max(0f, Math.Min(1f, oran));
            int r = (int)(a.R + (b.R - a.R) * oran);
            int g = (int)(a.G + (b.G - a.G) * oran);
            int bl = (int)(a.B + (b.B - a.B) * oran);
            return Color.FromArgb(r, g, bl);
        }
```

- [ ] **Step 4: DPI mode in `Program.cs`**

Find:

```csharp
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
```

Replace with:

```csharp
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
```

- [ ] **Step 5: Self-review**

Re-read the whole `UIStil.cs` file top to bottom: confirm the class still has exactly one closing brace matching `public static class UIStil` (the three new methods must be *inside* it, not after it), confirm `System` and `System.Drawing.Drawing2D` are both referenced correctly (`Math.Max`/`Math.Min` need `System`; `GraphicsPath` needs `System.Drawing.Drawing2D`), and confirm no existing member (`Glyph.*`, `SolIkonCiz`, `IkonLabel`, `GlyphFont`, font factory methods) was accidentally modified or duplicated. Confirm `Program.cs` has exactly one `SetHighDpiMode` call and it now reads `PerMonitorV2`.

- [ ] **Step 6: Commit**

```bash
git add FabrikaStokTakipUygulamasi/UIStil.cs FabrikaStokTakipUygulamasi/Program.cs
git commit -m "feat: teal/amber palette values and rounded-corner/shadow/hover-animation helpers"
```

---

### Task 2: Form1 (kenar çubuğu) — hover animasyonu

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/Form1.cs`

**Interfaces:**
- Consumes: `UIStil.HoverAnimasyonuBagla`, `UIStil.Mavi`, `UIStil.MaviAcik` (Task 1).

The sidebar nav buttons currently jump instantly between colors via `FlatAppearance.MouseOverBackColor`/`MouseDownBackColor` (set in `Form1.Designer.cs`, unchanged — do not touch `Designer.cs` for this task). This task adds a **smooth** hover transition on top by animating `BackColor` directly. Note: `Form1.cs`'s `ButonuAktifYap` method already sets `aktifButon.BackColor` to mark the active page — the hover animation must not fight with that for the *currently active* button. Simplest correct approach: skip hover-animating whichever button is currently active (its solid background already signals state); animate the other five.

- [ ] **Step 1: Add hover animation wiring**

In `Form1.cs`'s constructor, right after the existing block of six `Paint +=` icon lines added in the previous plan (the ones calling `UIStil.SolIkonCiz`), add:

```csharp
            foreach (var btn in new[] { btnDashboard, btnUrunler, btnUrunEkle, btnArama, btnLowStock })
            {
                var b = btn;
                UIStil.HoverAnimasyonuBagla(b, Color.FromArgb(52, 73, 94), UIStil.MaviAcik, renk =>
                {
                    if (b != aktifButon) b.BackColor = renk;
                });
            }
```

`Color.FromArgb(52, 73, 94)` is `panelSidebar`'s existing background (the buttons' ambient "resting" color) — this is intentionally a raw literal here, not a `UIStil` field, because it matches the sidebar panel's own `BackColor` set in `Form1.Designer.cs`, which this plan does not touch.

`btnAdmin` and `btnOturumKapat` are intentionally excluded from this loop: `btnAdmin` has its own fixed dark background (`FromArgb(30, 44, 57)`) to visually distinguish it as a sensitive action, and `btnOturumKapat` is a destructive/exit action styled in red — neither should invite the same "playful" hover treatment as ordinary navigation.

- [ ] **Step 2: Self-review**

Re-read `Form1.cs`: confirm the `foreach` loop captures each button into a local `var b = btn;` (required — looping and capturing the loop variable directly in the lambda would make every closure reference the same final value in older C#, though modern C# foreach variables are per-iteration; the explicit local copy is defensive and harmless either way, keep it for clarity). Confirm `ButonuAktifYap`'s existing logic (which sets `aktifButon.BackColor = Color.FromArgb(41, 128, 185)` — note: this literal should still visually read fine against the new teal palette since it's close to `UIStil.Mavi`'s old value; this plan does not change `ButonuAktifYap`, only adds the hover loop) still runs untouched.

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/Form1.cs
git commit -m "style: smooth hover color animation on sidebar nav buttons"
```

---

### Task 3: FormLogin — yuvarlak kart + gölge + buton animasyonu

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormLogin.cs`

**Interfaces:**
- Consumes: `UIStil.YuvarlakBolgeUygula`, `UIStil.YumusakGolgeCiz`, `UIStil.HoverAnimasyonuBagla`, `UIStil.Mavi`, `UIStil.MaviAcik` (Task 1).

`FormLogin` has no single "card" panel around the credentials fields today (`txtUsername`, `panelSifre`, `btnLogin` etc. sit directly on the form, to the right of `panelLeft`). Introducing a new wrapping panel would require `Designer.cs` layout surgery this plan avoids. Instead, this task rounds `panelLeft` itself (the dark teal brand panel) on its right edge only in visual effect via a full-rounded region (acceptable — it's a full-height side panel, rounding all four corners is fine since it's flush with the form's edges at top/bottom/left; the visual rounding will show only actually where the panel doesn't touch the form boundary, which for a `Dock = Left` panel is only the right edge, distinguishing it as a two-tone card look), and adds hover animation to `btnLogin` and `btnDilToggle`.

- [ ] **Step 1: Round `panelLeft`'s outer edge and add its shadow**

In `FormLogin.cs`'s constructor:

```csharp
        public FormLogin()
        {
            InitializeComponent();
            DiliUygula();
            LangManager.DilDegisti += DiliUygula;
        }
```

Replace with:

```csharp
        public FormLogin()
        {
            InitializeComponent();
            DiliUygula();
            LangManager.DilDegisti += DiliUygula;

            UIStil.YuvarlakBolgeUygula(panelLeft, 0); // düz kenar korunur (Dock=Left tam form yüksekliği) — yarıçap 0, gölge asıl vurgu
            this.Paint += (s, e) => UIStil.YumusakGolgeCiz(e.Graphics, panelLeft.Bounds, 6);

            UIStil.HoverAnimasyonuBagla(btnLogin, UIStil.Mavi, UIStil.MaviAcik, renk => btnLogin.BackColor = renk);
            UIStil.HoverAnimasyonuBagla(btnDilToggle, Color.FromArgb(22, 160, 133), UIStil.MaviAcik, renk => btnDilToggle.BackColor = renk);
        }
```

(`YuvarlakBolgeUygula(panelLeft, 0)` is called with a `0` radius deliberately — it establishes the DPI-aware `Resize`-driven `Region` recompute plumbing for `panelLeft` without visually rounding a panel that's flush against three form edges, where rounding would look broken. The shadow (`YumusakGolgeCiz`) is the real visual change here: a soft dark edge along `panelLeft`'s right/bottom, giving it a "raised card" look against the lighter right-hand content area.)

- [ ] **Step 2: Self-review**

Re-read `FormLogin.cs`: confirm `btnGozToggle`'s existing `Click` handler (show/hide password) is untouched, confirm the two new `HoverAnimasyonuBagla` calls don't remove or conflict with `btnDilToggle`'s existing `Paint` handler (the globe icon from the previous plan) — both a `Paint` handler and this task's `BackColor`-only hover animation can coexist on the same button without interference, since the icon `Paint` handler draws *after* the button's own background render on each repaint, including during the color-lerp animation.

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormLogin.cs
git commit -m "style: shadow on the login brand panel, hover animation on login/language buttons"
```

---

### Task 4: FormDashboard — yuvarlak kartlar + gölge + ikinci grafik

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormDashboard.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormDashboard.Designer.cs`

**Interfaces:**
- Consumes: `UIStil.YuvarlakBolgeUygula`, `UIStil.YumusakGolgeCiz` (Task 1); `StokVeritabani.TumUrunler()` (existing, unchanged signature).

- [ ] **Step 1: Add a second chart control (bar chart of the last 10 products' stock)**

In `FormDashboard.Designer.cs`, alongside the existing `chartArea1`/`series1`/`legend1` locals at the top of `InitializeComponent`, add a second set:

```csharp
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
```

Add a field declaration alongside `chartStokDagilimi`:

```csharp
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSonUrunler;
```

Right after the `chartStokDagilimi` setup block finishes (right after its `((System.ComponentModel.ISupportInitialize)(this.chartStokDagilimi)).EndInit();` line), insert:

```csharp
            //
            // chartSonUrunler
            //
            this.chartSonUrunler = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartSonUrunler)).BeginInit();
            chartArea2.BackColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea2";
            chartArea2.AxisX.LabelStyle.Font = new System.Drawing.Font("Segoe UI", 6.5F);
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(230, 230, 230);
            this.chartSonUrunler.ChartAreas.Add(chartArea2);
            this.chartSonUrunler.BackColor = System.Drawing.Color.White;
            this.chartSonUrunler.BorderlineColor = System.Drawing.Color.FromArgb(189, 195, 199);
            this.chartSonUrunler.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chartSonUrunler.BorderlineWidth = 1;
            this.chartSonUrunler.Location = new System.Drawing.Point(890, 280);
            series2.ChartArea = "ChartArea2";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            series2.Name = "Seri2";
            series2.Font = new System.Drawing.Font("Segoe UI", 7F);
            series2.Color = System.Drawing.Color.FromArgb(15, 118, 110);
            this.chartSonUrunler.Series.Add(series2);
            this.chartSonUrunler.Name = "chartSonUrunler";
            this.chartSonUrunler.Size = new System.Drawing.Size(254, 170);
            this.chartSonUrunler.TabIndex = 6;
            this.chartSonUrunler.Text = "chartSonUrunler";
            ((System.ComponentModel.ISupportInitialize)(this.chartSonUrunler)).EndInit();
```

(`Location (890, 280)` places it directly below `chartStokDagilimi` — which per the previous plan sits at `(890, 120)` with `Size (254, 150)`, ending at `y = 270` — leaving a 10px gap, still comfortably above `dgvRecent`'s `y = 330` start.)

Find:

```csharp
            this.Controls.Add(this.chartStokDagilimi);
            this.Controls.Add(this.dgvRecent);
```

Replace with:

```csharp
            this.Controls.Add(this.chartStokDagilimi);
            this.Controls.Add(this.chartSonUrunler);
            this.Controls.Add(this.dgvRecent);
```

- [ ] **Step 2: Populate the new chart in `FormDashboard.cs`**

Find the end of the existing `GrafikGuncelle` method (added in the previous plan) and, right after its closing `}`, add a new method:

```csharp
        private void SonUrunlerGrafiguGuncelle()
        {
            var seri = chartSonUrunler.Series["Seri2"];
            seri.Points.Clear();

            List<Urun> urunler;
            try { urunler = StokVeritabani.TumUrunler(); }
            catch { return; }

            foreach (var u in urunler.Take(10))
            {
                string etiket = string.IsNullOrWhiteSpace(u.UrunCinsi) ? ("#" + u.Id) : u.UrunCinsi;
                if (etiket.Length > 10) etiket = etiket.Substring(0, 10) + "…";
                seri.Points.AddXY(etiket, u.Stok);
            }
        }
```

Add `using System.Linq;` to the top of `FormDashboard.cs` if it isn't already present (needed for `.Take(10)`).

Find the call to `GrafikGuncelle(toplam, kritik);` inside `DiliUygula()` and add the new call right after it:

```csharp
            GrafikGuncelle(toplam, kritik);
            SonUrunlerGrafiguGuncelle();
```

- [ ] **Step 3: Round + shadow the three stat cards and both chart cards**

In `FormDashboard.cs`'s constructor:

```csharp
        public FormDashboard()
        {
            InitializeComponent();
        }
```

Replace with:

```csharp
        public FormDashboard()
        {
            InitializeComponent();

            foreach (var panel in new[] { panelTotal, panelCritical, panelCompany })
                UIStil.YuvarlakBolgeUygula(panel, 14);

            UIStil.YuvarlakBolgeUygula(chartStokDagilimi, 10);
            UIStil.YuvarlakBolgeUygula(chartSonUrunler, 10);

            this.Paint += (s, e) =>
            {
                UIStil.YumusakGolgeCiz(e.Graphics, panelTotal.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, panelCritical.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, panelCompany.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, chartStokDagilimi.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, chartSonUrunler.Bounds);
            };
        }
```

- [ ] **Step 4: Self-review**

Re-read `FormDashboard.Designer.cs`: confirm `chartSonUrunler` is (a) declared as a field, (b) constructed, (c) added to `this.Controls`, (d) has `ChartAreas`/`Series` populated — the same four-point check used for `chartStokDagilimi` in the previous plan. Re-read `FormDashboard.cs`: confirm `SonUrunlerGrafiguGuncelle` is actually called (from `DiliUygula`, which itself runs on `Load` and on every language change), confirm `Urun`/`StokVeritabani` types resolve (same namespace, no new `using` needed beyond `System.Linq`). Confirm the constructor's `Paint` lambda references only controls that exist as fields at the time it runs (all five do, since `InitializeComponent()` already ran).

- [ ] **Step 5: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormDashboard.cs FabrikaStokTakipUygulamasi/FormDashboard.Designer.cs
git commit -m "feat: rounded/shadowed dashboard cards and a second chart (last 10 products' stock)"
```

---

### Task 5: FormUrunler + FormArama — birincil buton hover animasyonu

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormUrunler.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormArama.cs`

**Interfaces:**
- Consumes: `UIStil.HoverAnimasyonuBagla`, `UIStil.Mavi`, `UIStil.MaviAcik`, `UIStil.Aksan` (Task 1).

- [ ] **Step 1: `FormUrunler.cs`**

In the constructor, right after the four existing `Paint +=` icon lines (from the previous plan), add:

```csharp
            UIStil.HoverAnimasyonuBagla(btnDetail, Color.FromArgb(44, 62, 80), UIStil.MaviAcik, renk => btnDetail.BackColor = renk);
            UIStil.HoverAnimasyonuBagla(btnEdit,   UIStil.Aksan,               Color.FromArgb(245, 180, 90), renk => btnEdit.BackColor = renk);
```

(Only `btnDetail`/`btnEdit` — the two most-used actions — get animated; `btnDelete` intentionally keeps its static red to avoid a "softening" hover effect on a destructive action, and `btnExcel` is a secondary/occasional action.)

- [ ] **Step 2: `FormArama.cs`**

In the constructor, right after the two existing `Paint +=` icon lines (from the previous plan), add:

```csharp
            UIStil.HoverAnimasyonuBagla(btnAra, Color.FromArgb(46, 134, 193), UIStil.MaviAcik, renk => btnAra.BackColor = renk);
```

(Only `btnAra` — the primary action; `btnTemizle` is a secondary/reset action and keeps its static muted gray.)

- [ ] **Step 3: Self-review**

Confirm in both files that the new lines reference only button field names that actually exist (`btnDetail`, `btnEdit` in `FormUrunler.Designer.cs`; `btnAra` in `FormArama.Designer.cs` — all already used by the previous plan's `Paint` wiring in the same constructors, so their existence is already established). Confirm no existing `Click`/`Paint` handler was removed.

- [ ] **Step 4: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormUrunler.cs FabrikaStokTakipUygulamasi/FormArama.cs
git commit -m "style: hover animation on the primary action buttons in FormUrunler and FormArama"
```

---

### Task 6: FormUrunEkle + FormUrunDuzenle — kart gölgesi + buton animasyonu

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormUrunEkle.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormUrunDuzenle.cs`

**Interfaces:**
- Consumes: `UIStil.YuvarlakBolgeUygula`, `UIStil.YumusakGolgeCiz`, `UIStil.HoverAnimasyonuBagla`, `UIStil.Aksan`, `UIStil.Notr` (Task 1).

- [ ] **Step 1: `FormUrunEkle.cs`**

In the constructor, right after the four existing `Paint +=` icon lines (from the previous plan, ending with the `btnPdfKaldir.Paint += ...` line and the closing `}` of the constructor), insert **before** that closing `}`:

```csharp

            foreach (var grp in new GroupBox[] { grpGenel, grpOlcu, grpStok, grpSertifika })
                UIStil.YuvarlakBolgeUygula(grp, 10);

            UIStil.HoverAnimasyonuBagla(btnUrunEkle, UIStil.Aksan, Color.FromArgb(245, 180, 90), renk => btnUrunEkle.BackColor = renk);
```

- [ ] **Step 2: `FormUrunDuzenle.cs`**

In `BuildUI()`, find where `panelCard` and `panelPdf` are constructed and added to `this.Controls` (near the end of the method, the block starting `this.Controls.Add(panelHeader);`). Right after that block, add:

```csharp
            UIStil.YuvarlakBolgeUygula(panelCard, 14);
            UIStil.YuvarlakBolgeUygula(panelPdf, 12);
            this.Paint += (s, e) =>
            {
                UIStil.YumusakGolgeCiz(e.Graphics, panelCard.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, panelPdf.Bounds);
            };
            UIStil.HoverAnimasyonuBagla(btnGuncelle, CAccent, Color.FromArgb(245, 180, 90), renk => btnGuncelle.BackColor = renk);
```

(`CAccent` is the field already defined in this file, pointed at `UIStil.Aksan` by the previous plan — use the local field name, not `UIStil.Aksan` directly, to match this file's existing convention.)

- [ ] **Step 3: Self-review**

Confirm `GroupBox` rounding doesn't clip its own caption text (a `GroupBox`'s `Text` renders inside its own bounds near the top-left; a 10px-radius rounded region set via `Control.Region` only clips the four corners, the caption sits well clear of them for these panels' sizes — this is a low-risk visual check the human should still glance at in Task 7's manual QA). Confirm `panelCard`/`panelPdf` in `FormUrunDuzenle.cs` are the exact local variable names used in `BuildUI()` (they are — this file builds its whole UI in code, not `Designer.cs`, so these are regular C# local variables captured by the lambdas, which is safe since `BuildUI()` runs once during construction and the panels live for the dialog's lifetime).

- [ ] **Step 4: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormUrunEkle.cs FabrikaStokTakipUygulamasi/FormUrunDuzenle.cs
git commit -m "style: rounded group/card panels and primary-button hover animation on product add/edit forms"
```

---

### Task 7: FormLowStock + FormLowStockSecim + FormLowStockLimit

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormLowStock.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormLowStockSecim.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormLowStockLimit.cs`

**Interfaces:**
- Consumes: `UIStil.YuvarlakBolgeUygula`, `UIStil.YumusakGolgeCiz`, `UIStil.HoverAnimasyonuBagla` (Task 1).

- [ ] **Step 1: `FormLowStock.cs`**

In the constructor, right after the two existing `Paint +=` icon lines (from the previous plan), add:

```csharp
            UIStil.HoverAnimasyonuBagla(btnYeni,     Color.FromArgb(39, 174, 96),  Color.FromArgb(90, 200, 140), renk => btnYeni.BackColor = renk);
            UIStil.HoverAnimasyonuBagla(btnDuzenle,  UIStil.Aksan,                 Color.FromArgb(245, 180, 90), renk => btnDuzenle.BackColor = renk);
```

- [ ] **Step 2: `FormLowStockSecim.cs`**

This form's `FormBorderStyle` is `Sizable` (user-resizable), so the fixed-`Bounds` shadow pattern used elsewhere in this task does not apply here (the window can be resized, making a cached shadow position stale). Scope this form to hover animation only, on the close button.

In `BuildUI()`, find where `panelHeader`, `dgv`, and `panelAlt` are added to `this.Controls` (the three lines at the very end of the method). Right after them, add:

```csharp
            UIStil.HoverAnimasyonuBagla(btnKapat, Color.FromArgb(149, 165, 166), Color.FromArgb(180, 190, 190), renk => btnKapat.BackColor = renk);
```

- [ ] **Step 3: `FormLowStockLimit.cs`**

This form's `FormBorderStyle` is `FixedDialog` (not user-resizable), so the fixed-`Bounds` shadow pattern is safe here.

In `BuildUI()`, find where `panelHeader`, `panelCard`, `btnKaydet`, `btnIptal` are added to `this.Controls` (the four lines at the very end of the method). Right after them, add:

```csharp
            UIStil.YuvarlakBolgeUygula(panelCard, 10);
            this.Paint += (s, e) => UIStil.YumusakGolgeCiz(e.Graphics, panelCard.Bounds);

            Color btnKaydetNormalRenk = _duzenle ? CAccent : CNavy;
            UIStil.HoverAnimasyonuBagla(btnKaydet, btnKaydetNormalRenk, Color.FromArgb(245, 180, 90), renk => btnKaydet.BackColor = renk);
```

- [ ] **Step 4: Self-review**

Confirm `FormLowStockSecim.cs` received **only** the `btnKapat` hover line and nothing else (no `Region`/shadow — this form is `Sizable`, per Step 2's reasoning). Confirm `FormLowStockLimit.cs`'s `btnKaydetNormalRenk` correctly mirrors the color logic already used elsewhere in that same file for `btnKaydet`'s initial `BackColor` (`_duzenle ? CAccent : CNavy`) — re-read the file's existing `btnKaydet` construction block to confirm this matches rather than guessing.

- [ ] **Step 5: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormLowStock.cs FabrikaStokTakipUygulamasi/FormLowStockSecim.cs FabrikaStokTakipUygulamasi/FormLowStockLimit.cs
git commit -m "style: hover animation across Low Stock forms; rounded/shadowed card on the fixed-size limit dialog"
```

---

### Task 8: FormUrunDetay + FormSilOnay

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormUrunDetay.cs`
- Modify: `FabrikaStokTakipUygulamasi/FormSilOnay.cs`

**Interfaces:**
- Consumes: `UIStil.YuvarlakBolgeUygula`, `UIStil.YumusakGolgeCiz`, `UIStil.HoverAnimasyonuBagla` (Task 1).

Both forms are `FormBorderStyle.FixedDialog` (not resizable), so the fixed-`Bounds` shadow pattern applies safely to both.

- [ ] **Step 1: `FormUrunDetay.cs`**

In `BuildUI()`, find where `panelHeader`, `panelCard`, `panelPdf`, `btnKapat` are added to `this.Controls` (the four lines at the very end of the method). Right after them, add:

```csharp
            UIStil.YuvarlakBolgeUygula(panelCard, 14);
            UIStil.YuvarlakBolgeUygula(panelPdf, 10);
            this.Paint += (s, e) =>
            {
                UIStil.YumusakGolgeCiz(e.Graphics, panelCard.Bounds);
                UIStil.YumusakGolgeCiz(e.Graphics, panelPdf.Bounds);
            };
            UIStil.HoverAnimasyonuBagla(btnKapat, CNavy, Color.FromArgb(30, 90, 110), renk => btnKapat.BackColor = renk);
```

- [ ] **Step 2: `FormSilOnay.cs`**

In `BuildUI()`, find where the controls are added via `this.Controls.AddRange(new Control[] { panelStripe, lblIcon, lblMesaj, lblUrun, btnEvet, btnHayir });`. Right after that line, add:

```csharp
            UIStil.HoverAnimasyonuBagla(btnEvet, CRed, Color.FromArgb(215, 90, 75), renk => btnEvet.BackColor = renk);
```

(No rounding/shadow on this one — it's a small, already-compact confirmation dialog where a red stripe + warning icon are the intended focal point; adding a card shadow here would compete with, not support, that warning emphasis. Only the confirm button gets the hover treatment.)

- [ ] **Step 3: Self-review**

Confirm both files' `panelCard`/`panelPdf`/`btnKapat`/`btnEvet` names match the actual local variables declared in each `BuildUI()` method (all are pre-existing from the previous plan, not new). Confirm `FormSilOnay.cs` received only the one hover line and no rounding/shadow calls, per Step 2's explicit scope decision.

- [ ] **Step 4: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormUrunDetay.cs FabrikaStokTakipUygulamasi/FormSilOnay.cs
git commit -m "style: rounded/shadowed cards on product detail dialog, hover animation on both dialogs' primary buttons"
```

---

### Task 9: FormAdmin

**Files:**
- Modify: `FabrikaStokTakipUygulamasi/FormAdmin.cs`

**Interfaces:**
- Consumes: `UIStil.HoverAnimasyonuBagla`, `UIStil.Mavi` (Task 1).

`FormAdmin`'s content area is dominated by two `DataGridView`-filled tabs (Kullanıcılar, Stok Hareketleri) with thin toolbars — there is no large "card" panel here suited to rounding/shadow (the header panel `pHeader` spans the full form width flush to the top edge, where rounding would look broken, same reasoning as `FormLogin`'s `panelLeft`). This task is scoped to hover animation only, on the Hareketler tab's refresh button (the Kullanıcılar tab's three buttons already got icon treatment in the previous plan and are used less frequently than a single clear "primary" action here — adding hover to all three plus `btnYenile` would be inconsistent with this plan's "primary actions only" rule, so only `btnYenile`, the single most-repeated action in this tab, is animated).

- [ ] **Step 1: Add hover animation to the refresh button**

In `BuildHareketlerTab()`, find:

```csharp
            var btnYenile = MkBtn(LangManager.T("admin.yenile"), CBlue, new Point(330, 13), new Size(95, 28));
            btnYenile.Click += (s, e) => HareketleriYukle();
```

Replace with:

```csharp
            var btnYenile = MkBtn(LangManager.T("admin.yenile"), CBlue, new Point(330, 13), new Size(95, 28));
            btnYenile.Click += (s, e) => HareketleriYukle();
            UIStil.HoverAnimasyonuBagla(btnYenile, CBlue, Color.FromArgb(45, 212, 191), renk => btnYenile.BackColor = renk);
```

- [ ] **Step 2: Self-review**

Confirm `CBlue` (this file's local color field, pointed at `UIStil.Mavi` by the previous plan) is the correct pre-existing field name and this addition doesn't touch anything else in the 590-line file.

- [ ] **Step 3: Commit**

```bash
git add FabrikaStokTakipUygulamasi/FormAdmin.cs
git commit -m "style: hover animation on the Admin panel's refresh button"
```

---

### Task 10: Manuel QA (insan operatör — Windows + Visual Studio gerekir)

**This task cannot be executed by an agent in this environment.** Checklist for the human operator, on Windows with Visual Studio, after Tasks 1–9 are committed and CI is green.

- [ ] **Step 1: Build & first look**

`Ctrl+Shift+B`, then `F5`. Expected: builds clean (CI should already confirm this per push, but double-check locally). Login screen shows the new teal left panel with a visible soft shadow along its right/bottom edge (not a hard black line — a soft gradient-like fade).

- [ ] **Step 2: Sidebar hover**

Move the mouse slowly over each non-active sidebar nav button. Expected: background color eases in/out over roughly 150–200ms rather than snapping instantly. The currently-active page's button should NOT animate on hover (it keeps its solid active color).

- [ ] **Step 3: Dashboard**

Open Dashboard. Expected: the three stat cards and both chart panels have visibly rounded corners and a soft shadow peeking out from their bottom-right edge. The new bar chart (below the doughnut) shows up to 10 bars labeled with product names, colored teal.

- [ ] **Step 4: Dialogs**

Open Ürün Ekle, Ürün Düzenle (from Ürünler → select a row → Düzenle), Ürün Detay, Sil Onayı, Low Stock → Yeni/Düzenle → double-click a row (LowStockLimit dialog). Expected: rounded card panels with soft shadows on the non-resizable dialogs (Düzenle, Detay, Limit dialog); the resizable Low Stock seçim dialog has no rounding/shadow (by design) but its Kapat button still animates on hover.

- [ ] **Step 5: DPI check**

If you have access to a second monitor or can change Windows display scaling (Settings → Display → Scale), set it to 125% or 150%, move the app to that display (or restart it under that scaling), and confirm rounded corners still look proportionate (not oversized or barely visible) and nothing is visually clipped or overlapping.

- [ ] **Step 6: Report back**

Report which of the above passed/failed. As with the previous plan, any failure is expected to be small and isolated (a color that reads oddly, a shadow that looks too heavy/light — easily tuned via the `derinlik` parameter or the specific RGB literals in the relevant task's file) rather than a structural problem.
