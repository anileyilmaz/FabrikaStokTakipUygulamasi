using System;
using System.Drawing;
using System.Windows.Forms;

namespace StokTakipUI
{
    /// <summary>
    /// "Yeni" veya "Düzenle" modunda açılan popup form.
    /// mod = "yeni"    → LowStockLimit tanımlanmamış ürünleri listeler
    /// mod = "duzenle" → LowStockLimit tanımlanmış ürünleri listeler
    /// </summary>
    public class FormLowStockSecim : Form
    {
        private static readonly Color CNavy    = Color.FromArgb(44, 62, 80);
        private static readonly Color CWhite   = Color.White;
        private static readonly Color CBg      = Color.FromArgb(236, 240, 241);

        private readonly string _mod;   // "yeni" veya "duzenle"
        private DataGridView dgv;
        public bool Degisiklik { get; private set; } = false;

        public FormLowStockSecim(string mod)
        {
            _mod = mod;
            BuildUI();
            Yukle();
        }

        private void BuildUI()
        {
            bool duzenle = _mod == "duzenle";

            this.Text            = LangManager.T(duzenle ? "ls.sec.form.duz" : "ls.sec.form.yeni");
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimizeBox     = false;
            this.BackColor       = CBg;
            this.Size            = new Size(900, 560);
            this.MinimumSize     = new Size(700, 420);
            this.Font            = new Font("Segoe UI", 9.5f);

            // ── Başlık paneli ─────────────────────────────────────────────
            var panelHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 68,
                BackColor = CNavy
            };

            var lblBaslik = new Label
            {
                Text      = LangManager.T(duzenle ? "ls.sec.baslik.duz" : "ls.sec.baslik.yeni"),
                Font      = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = CWhite,
                AutoSize  = true,
                Location  = new Point(20, 14)
            };
            panelHeader.Controls.Add(lblBaslik);

            var lblAlt = new Label
            {
                Text      = LangManager.T(duzenle ? "ls.sec.alt.duz" : "ls.sec.alt.yeni"),
                Font      = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(189, 195, 199),
                AutoSize  = true,
                Location  = new Point(22, 44)
            };
            panelHeader.Controls.Add(lblAlt);

            // ── DataGridView ──────────────────────────────────────────────
            var hdrStyle = new DataGridViewCellStyle
            {
                BackColor          = CNavy,
                ForeColor          = CWhite,
                Font               = new Font("Segoe UI", 9.75f, FontStyle.Bold),
                SelectionBackColor = CNavy,
                SelectionForeColor = CWhite,
                Alignment          = DataGridViewContentAlignment.MiddleLeft
            };

            dgv = new DataGridView
            {
                Dock                           = DockStyle.Fill,
                AllowUserToAddRows             = false,
                AllowUserToDeleteRows          = false,
                AllowUserToResizeColumns       = false,
                AllowUserToResizeRows          = false,
                AutoSizeColumnsMode            = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor                = CWhite,
                CellBorderStyle                = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle       = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersDefaultCellStyle  = hdrStyle,
                ColumnHeadersHeight            = 38,
                ColumnHeadersHeightSizeMode    = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                EnableHeadersVisualStyles      = false,
                GridColor                      = Color.FromArgb(220, 220, 220),
                MultiSelect                    = false,
                ReadOnly                       = true,
                RowHeadersVisible              = false,
                SelectionMode                  = DataGridViewSelectionMode.FullRowSelect,
                Cursor                         = Cursors.Hand
            };
            dgv.RowTemplate.Height = 34;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);

            // Kolonlar
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId",       Visible = false });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colUrun",      HeaderText = LangManager.Ingilizce ? "Product" : "Ürün Cinsi"   });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaterial",  HeaderText = "Material"     });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGrade",     HeaderText = "Grade"        });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colWidth",     HeaderText = "Width"        });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLength",    HeaderText = "Length"       });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStok",      HeaderText = LangManager.Ingilizce ? "Current Stock" : "Güncel Stok"  });

            if (_mod == "duzenle")
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLimit", HeaderText = LangManager.Ingilizce ? "Current Limit" : "Mevcut Limit" });

            dgv.CellDoubleClick += Dgv_CellDoubleClick;

            // ── İptal butonu ──────────────────────────────────────────────
            var panelAlt = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 54,
                BackColor = CBg
            };
            var btnKapat = new Button
            {
                Text      = LangManager.T("detay.kapat"),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = CWhite,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Size      = new Size(130, 36),
                Location  = new Point(12, 9),
                Cursor    = Cursors.Hand
            };
            btnKapat.FlatAppearance.BorderSize = 0;
            btnKapat.Click += (s, e) => this.Close();
            panelAlt.Controls.Add(btnKapat);

            this.Controls.Add(dgv);           // Fill → ortayı kaplar
            this.Controls.Add(panelAlt);      // Bottom
            this.Controls.Add(panelHeader);   // Top
        }

        private void Yukle()
        {
            dgv.Rows.Clear();
            foreach (var u in StokVeritabani.TumUrunler())
            {
                bool limitTanimli = u.LowStockLimit >= 0;

                if (_mod == "yeni"    &&  limitTanimli) continue;
                if (_mod == "duzenle" && !limitTanimli) continue;

                if (_mod == "duzenle")
                    dgv.Rows.Add(u.Id, u.UrunCinsi, u.Material, u.Grade, u.Width, u.Length, u.Stok, u.LowStockLimit);
                else
                    dgv.Rows.Add(u.Id, u.UrunCinsi, u.Material, u.Grade, u.Width, u.Length, u.Stok);

                // Stok renklendir
                var row = dgv.Rows[dgv.Rows.Count - 1];
                RenklenRow(row, u);
            }

            dgv.ClearSelection();
            dgv.CurrentCell = null;
        }

        private void RenklenRow(DataGridViewRow row, Urun u)
        {
            if (_mod == "yeni")
            {
                // Limit yok, stoka göre renk ver (varsayılan 5)
                int limit = 5;
                if      (u.Stok <= limit)       SetRowColor(row, Color.FromArgb(255, 205, 210), Color.FromArgb(183, 28, 28));
                else if (u.Stok <= limit + 10)  SetRowColor(row, Color.FromArgb(255, 236, 179), Color.FromArgb(230, 81, 0));
                else                            SetRowColor(row, Color.FromArgb(200, 230, 201), Color.FromArgb(27, 94, 32));
            }
            else
            {
                int limit = u.LowStockLimit;
                if      (u.Stok <= limit)       SetRowColor(row, Color.FromArgb(255, 205, 210), Color.FromArgb(183, 28, 28));
                else if (u.Stok <= limit + 10)  SetRowColor(row, Color.FromArgb(255, 236, 179), Color.FromArgb(230, 81, 0));
                else                            SetRowColor(row, Color.FromArgb(200, 230, 201), Color.FromArgb(27, 94, 32));
            }
        }

        private void SetRowColor(DataGridViewRow row, Color bg, Color fg)
        {
            row.DefaultCellStyle.BackColor = bg;
            row.DefaultCellStyle.ForeColor = fg;
        }

        private void Dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgv.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells["colId"].Value);

            Urun urun = null;
            foreach (var u in StokVeritabani.TumUrunler())
                if (u.Id == id) { urun = u; break; }
            if (urun == null) return;

            bool duzenle = _mod == "duzenle";
            using (var frm = new FormLowStockLimit(urun, duzenle))
            {
                frm.ShowDialog(this);
                if (frm.Kaydedildi)
                {
                    Degisiklik = true;
                    Yukle();   // listeyi yenile, işlenmiş ürün kaybolur
                }
            }
        }
    }
}
