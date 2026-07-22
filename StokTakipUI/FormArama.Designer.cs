namespace StokTakipUI
{
    partial class FormArama
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            panelTop = new System.Windows.Forms.Panel();
            lblKayitSayisi = new System.Windows.Forms.Label();
            btnTemizle = new System.Windows.Forms.Button();
            btnAra = new System.Windows.Forms.Button();
            txtSearch = new System.Windows.Forms.TextBox();
            cmbSearchType = new System.Windows.Forms.ComboBox();
            panelFilter = new System.Windows.Forms.Panel();
            lblFilterCustomer = new System.Windows.Forms.Label();
            cmbUrun = new System.Windows.Forms.ComboBox();
            lblFilterParent = new System.Windows.Forms.Label();
            cmbDiameter = new System.Windows.Forms.ComboBox();
            lblFilterBatch = new System.Windows.Forms.Label();
            cmbMaterial = new System.Windows.Forms.ComboBox();
            lblFilterGrade = new System.Windows.Forms.Label();
            cmbGrade = new System.Windows.Forms.ComboBox();
            lblFilterHeat = new System.Windows.Forms.Label();
            cmbThickness = new System.Windows.Forms.ComboBox();
            lblThicknessRange = new System.Windows.Forms.Label();
            txtThicknessMin = new System.Windows.Forms.TextBox();
            lblDash1 = new System.Windows.Forms.Label();
            txtThicknessMax = new System.Windows.Forms.TextBox();
            lblWidth = new System.Windows.Forms.Label();
            txtWidthMin = new System.Windows.Forms.TextBox();
            lblDash2 = new System.Windows.Forms.Label();
            txtWidthMax = new System.Windows.Forms.TextBox();
            lblLength = new System.Windows.Forms.Label();
            txtLengthMin = new System.Windows.Forms.TextBox();
            lblDash3 = new System.Windows.Forms.Label();
            txtLengthMax = new System.Windows.Forms.TextBox();
            lblStock = new System.Windows.Forms.Label();
            txtStockMin = new System.Windows.Forms.TextBox();
            lblDash4 = new System.Windows.Forms.Label();
            txtStockMax = new System.Windows.Forms.TextBox();
            dgvUrunler = new System.Windows.Forms.DataGridView();
            colFirma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCertificate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colHeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colThickness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            panelTop.SuspendLayout();
            panelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUrunler).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = System.Drawing.Color.FromArgb(30, 42, 56);
            panelTop.Controls.Add(lblKayitSayisi);
            panelTop.Controls.Add(btnTemizle);
            panelTop.Controls.Add(btnAra);
            panelTop.Controls.Add(txtSearch);
            panelTop.Controls.Add(cmbSearchType);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(1414, 60);
            panelTop.TabIndex = 0;
            panelTop.Paint += panelTop_Paint;
            // 
            // lblKayitSayisi
            // 
            lblKayitSayisi.AutoSize = true;
            lblKayitSayisi.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            lblKayitSayisi.ForeColor = System.Drawing.Color.White;
            lblKayitSayisi.Location = new System.Drawing.Point(900, 20);
            lblKayitSayisi.Name = "lblKayitSayisi";
            lblKayitSayisi.Size = new System.Drawing.Size(106, 17);
            lblKayitSayisi.TabIndex = 4;
            lblKayitSayisi.Text = "Bulunan Kayıt: 0";
            // 
            // btnTemizle
            // 
            btnTemizle.BackColor = System.Drawing.Color.FromArgb(127, 140, 141);
            btnTemizle.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTemizle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            btnTemizle.ForeColor = System.Drawing.Color.White;
            btnTemizle.Location = new System.Drawing.Point(608, 14);
            btnTemizle.Name = "btnTemizle";
            btnTemizle.Size = new System.Drawing.Size(110, 32);
            btnTemizle.TabIndex = 3;
            btnTemizle.Text = "TEMİZLE";
            btnTemizle.UseVisualStyleBackColor = false;
            btnTemizle.Click += btnTemizle_Click;
            // 
            // btnAra
            // 
            btnAra.BackColor = System.Drawing.Color.FromArgb(46, 134, 193);
            btnAra.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnAra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAra.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            btnAra.ForeColor = System.Drawing.Color.White;
            btnAra.Location = new System.Drawing.Point(503, 14);
            btnAra.Name = "btnAra";
            btnAra.Size = new System.Drawing.Size(92, 32);
            btnAra.TabIndex = 2;
            btnAra.Text = "ARA";
            btnAra.UseVisualStyleBackColor = false;
            btnAra.Click += btnAra_Click;
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtSearch.Location = new System.Drawing.Point(198, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new System.Drawing.Size(290, 23);
            txtSearch.TabIndex = 1;
            // 
            // cmbSearchType
            // 
            cmbSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbSearchType.FormattingEnabled = true;
            cmbSearchType.Items.AddRange(new object[] { "Tümü", "Material", "Grade", "Customer", "Certificate" });
            cmbSearchType.Location = new System.Drawing.Point(18, 16);
            cmbSearchType.Name = "cmbSearchType";
            cmbSearchType.Size = new System.Drawing.Size(163, 23);
            cmbSearchType.TabIndex = 0;
            // 
            // panelFilter
            // 
            panelFilter.AutoScroll = true;
            panelFilter.BackColor = System.Drawing.Color.Gainsboro;
            panelFilter.Controls.Add(lblFilterCustomer);
            panelFilter.Controls.Add(cmbUrun);
            panelFilter.Controls.Add(lblFilterParent);
            panelFilter.Controls.Add(cmbDiameter);
            panelFilter.Controls.Add(lblFilterBatch);
            panelFilter.Controls.Add(cmbMaterial);
            panelFilter.Controls.Add(lblFilterGrade);
            panelFilter.Controls.Add(cmbGrade);
            panelFilter.Controls.Add(lblFilterHeat);
            panelFilter.Controls.Add(cmbThickness);
            panelFilter.Controls.Add(lblThicknessRange);
            panelFilter.Controls.Add(txtThicknessMin);
            panelFilter.Controls.Add(lblDash1);
            panelFilter.Controls.Add(txtThicknessMax);
            panelFilter.Controls.Add(lblWidth);
            panelFilter.Controls.Add(txtWidthMin);
            panelFilter.Controls.Add(lblDash2);
            panelFilter.Controls.Add(txtWidthMax);
            panelFilter.Controls.Add(lblLength);
            panelFilter.Controls.Add(txtLengthMin);
            panelFilter.Controls.Add(lblDash3);
            panelFilter.Controls.Add(txtLengthMax);
            panelFilter.Controls.Add(lblStock);
            panelFilter.Controls.Add(txtStockMin);
            panelFilter.Controls.Add(lblDash4);
            panelFilter.Controls.Add(txtStockMax);
            panelFilter.Dock = System.Windows.Forms.DockStyle.Left;
            panelFilter.Location = new System.Drawing.Point(0, 60);
            panelFilter.Name = "panelFilter";
            panelFilter.Size = new System.Drawing.Size(285, 727);
            panelFilter.TabIndex = 1;
            // 
            // lblFilterCustomer
            // 
            lblFilterCustomer.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblFilterCustomer.ForeColor = System.Drawing.Color.FromArgb(80, 90, 105);
            lblFilterCustomer.Location = new System.Drawing.Point(20, 14);
            lblFilterCustomer.Name = "lblFilterCustomer";
            lblFilterCustomer.Size = new System.Drawing.Size(242, 18);
            lblFilterCustomer.TabIndex = 0;
            lblFilterCustomer.Text = "Customer";
            // 
            // cmbUrun
            // 
            cmbUrun.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            cmbUrun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbUrun.Font = new System.Drawing.Font("Segoe UI", 9F);
            cmbUrun.FormattingEnabled = true;
            cmbUrun.Location = new System.Drawing.Point(20, 34);
            cmbUrun.Name = "cmbUrun";
            cmbUrun.Size = new System.Drawing.Size(242, 23);
            cmbUrun.TabIndex = 1;
            // 
            // lblFilterParent
            // 
            lblFilterParent.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblFilterParent.ForeColor = System.Drawing.Color.FromArgb(80, 90, 105);
            lblFilterParent.Location = new System.Drawing.Point(20, 76);
            lblFilterParent.Name = "lblFilterParent";
            lblFilterParent.Size = new System.Drawing.Size(242, 18);
            lblFilterParent.TabIndex = 2;
            lblFilterParent.Text = "Parent Coil No";
            // 
            // cmbDiameter
            // 
            cmbDiameter.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            cmbDiameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbDiameter.Font = new System.Drawing.Font("Segoe UI", 9F);
            cmbDiameter.FormattingEnabled = true;
            cmbDiameter.Location = new System.Drawing.Point(20, 96);
            cmbDiameter.Name = "cmbDiameter";
            cmbDiameter.Size = new System.Drawing.Size(242, 23);
            cmbDiameter.TabIndex = 3;
            // 
            // lblFilterBatch
            // 
            lblFilterBatch.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblFilterBatch.ForeColor = System.Drawing.Color.FromArgb(80, 90, 105);
            lblFilterBatch.Location = new System.Drawing.Point(20, 138);
            lblFilterBatch.Name = "lblFilterBatch";
            lblFilterBatch.Size = new System.Drawing.Size(242, 18);
            lblFilterBatch.TabIndex = 4;
            lblFilterBatch.Text = "Batch No";
            // 
            // cmbMaterial
            // 
            cmbMaterial.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            cmbMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbMaterial.Font = new System.Drawing.Font("Segoe UI", 9F);
            cmbMaterial.FormattingEnabled = true;
            cmbMaterial.Location = new System.Drawing.Point(20, 158);
            cmbMaterial.Name = "cmbMaterial";
            cmbMaterial.Size = new System.Drawing.Size(242, 23);
            cmbMaterial.TabIndex = 5;
            // 
            // lblFilterGrade
            // 
            lblFilterGrade.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblFilterGrade.ForeColor = System.Drawing.Color.FromArgb(80, 90, 105);
            lblFilterGrade.Location = new System.Drawing.Point(20, 200);
            lblFilterGrade.Name = "lblFilterGrade";
            lblFilterGrade.Size = new System.Drawing.Size(242, 18);
            lblFilterGrade.TabIndex = 6;
            lblFilterGrade.Text = "Grade";
            // 
            // cmbGrade
            // 
            cmbGrade.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            cmbGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbGrade.Font = new System.Drawing.Font("Segoe UI", 9F);
            cmbGrade.FormattingEnabled = true;
            cmbGrade.Location = new System.Drawing.Point(20, 220);
            cmbGrade.Name = "cmbGrade";
            cmbGrade.Size = new System.Drawing.Size(242, 23);
            cmbGrade.TabIndex = 7;
            // 
            // lblFilterHeat
            // 
            lblFilterHeat.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblFilterHeat.ForeColor = System.Drawing.Color.FromArgb(80, 90, 105);
            lblFilterHeat.Location = new System.Drawing.Point(20, 262);
            lblFilterHeat.Name = "lblFilterHeat";
            lblFilterHeat.Size = new System.Drawing.Size(242, 18);
            lblFilterHeat.TabIndex = 8;
            lblFilterHeat.Text = "Heat No";
            // 
            // cmbThickness
            // 
            cmbThickness.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            cmbThickness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbThickness.Font = new System.Drawing.Font("Segoe UI", 9F);
            cmbThickness.FormattingEnabled = true;
            cmbThickness.Location = new System.Drawing.Point(20, 282);
            cmbThickness.Name = "cmbThickness";
            cmbThickness.Size = new System.Drawing.Size(242, 23);
            cmbThickness.TabIndex = 9;
            // 
            // lblThicknessRange
            // 
            lblThicknessRange.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblThicknessRange.ForeColor = System.Drawing.Color.FromArgb(80, 90, 105);
            lblThicknessRange.Location = new System.Drawing.Point(20, 328);
            lblThicknessRange.Name = "lblThicknessRange";
            lblThicknessRange.Size = new System.Drawing.Size(242, 18);
            lblThicknessRange.TabIndex = 10;
            lblThicknessRange.Text = "Thickness (mm)";
            // 
            // txtThicknessMin
            // 
            txtThicknessMin.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtThicknessMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtThicknessMin.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtThicknessMin.Location = new System.Drawing.Point(20, 350);
            txtThicknessMin.Name = "txtThicknessMin";
            txtThicknessMin.Size = new System.Drawing.Size(100, 23);
            txtThicknessMin.TabIndex = 11;
            txtThicknessMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDash1
            // 
            lblDash1.AutoSize = true;
            lblDash1.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblDash1.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            lblDash1.Location = new System.Drawing.Point(126, 354);
            lblDash1.Name = "lblDash1";
            lblDash1.Size = new System.Drawing.Size(19, 15);
            lblDash1.TabIndex = 12;
            lblDash1.Text = "—";
            // 
            // txtThicknessMax
            // 
            txtThicknessMax.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtThicknessMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtThicknessMax.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtThicknessMax.Location = new System.Drawing.Point(148, 350);
            txtThicknessMax.Name = "txtThicknessMax";
            txtThicknessMax.Size = new System.Drawing.Size(100, 23);
            txtThicknessMax.TabIndex = 13;
            txtThicknessMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblWidth
            // 
            lblWidth.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblWidth.ForeColor = System.Drawing.Color.FromArgb(80, 90, 105);
            lblWidth.Location = new System.Drawing.Point(20, 396);
            lblWidth.Name = "lblWidth";
            lblWidth.Size = new System.Drawing.Size(242, 18);
            lblWidth.TabIndex = 14;
            lblWidth.Text = "Width (mm)";
            // 
            // txtWidthMin
            // 
            txtWidthMin.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtWidthMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtWidthMin.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtWidthMin.Location = new System.Drawing.Point(20, 418);
            txtWidthMin.Name = "txtWidthMin";
            txtWidthMin.Size = new System.Drawing.Size(100, 23);
            txtWidthMin.TabIndex = 15;
            txtWidthMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDash2
            // 
            lblDash2.AutoSize = true;
            lblDash2.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblDash2.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            lblDash2.Location = new System.Drawing.Point(126, 422);
            lblDash2.Name = "lblDash2";
            lblDash2.Size = new System.Drawing.Size(19, 15);
            lblDash2.TabIndex = 16;
            lblDash2.Text = "—";
            // 
            // txtWidthMax
            // 
            txtWidthMax.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtWidthMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtWidthMax.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtWidthMax.Location = new System.Drawing.Point(148, 418);
            txtWidthMax.Name = "txtWidthMax";
            txtWidthMax.Size = new System.Drawing.Size(100, 23);
            txtWidthMax.TabIndex = 17;
            txtWidthMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLength
            // 
            lblLength.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblLength.ForeColor = System.Drawing.Color.FromArgb(80, 90, 105);
            lblLength.Location = new System.Drawing.Point(20, 464);
            lblLength.Name = "lblLength";
            lblLength.Size = new System.Drawing.Size(242, 18);
            lblLength.TabIndex = 18;
            lblLength.Text = "Length (mm)";
            // 
            // txtLengthMin
            // 
            txtLengthMin.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtLengthMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtLengthMin.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtLengthMin.Location = new System.Drawing.Point(20, 486);
            txtLengthMin.Name = "txtLengthMin";
            txtLengthMin.Size = new System.Drawing.Size(100, 23);
            txtLengthMin.TabIndex = 19;
            txtLengthMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDash3
            // 
            lblDash3.AutoSize = true;
            lblDash3.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblDash3.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            lblDash3.Location = new System.Drawing.Point(126, 490);
            lblDash3.Name = "lblDash3";
            lblDash3.Size = new System.Drawing.Size(19, 15);
            lblDash3.TabIndex = 20;
            lblDash3.Text = "—";
            // 
            // txtLengthMax
            // 
            txtLengthMax.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtLengthMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtLengthMax.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtLengthMax.Location = new System.Drawing.Point(148, 486);
            txtLengthMax.Name = "txtLengthMax";
            txtLengthMax.Size = new System.Drawing.Size(100, 23);
            txtLengthMax.TabIndex = 21;
            txtLengthMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStock
            // 
            lblStock.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            lblStock.ForeColor = System.Drawing.Color.FromArgb(80, 90, 105);
            lblStock.Location = new System.Drawing.Point(20, 532);
            lblStock.Name = "lblStock";
            lblStock.Size = new System.Drawing.Size(242, 18);
            lblStock.TabIndex = 22;
            lblStock.Text = "Güncel Stok";
            // 
            // txtStockMin
            // 
            txtStockMin.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtStockMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtStockMin.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtStockMin.Location = new System.Drawing.Point(20, 554);
            txtStockMin.Name = "txtStockMin";
            txtStockMin.Size = new System.Drawing.Size(100, 23);
            txtStockMin.TabIndex = 23;
            txtStockMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDash4
            // 
            lblDash4.AutoSize = true;
            lblDash4.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblDash4.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            lblDash4.Location = new System.Drawing.Point(126, 558);
            lblDash4.Name = "lblDash4";
            lblDash4.Size = new System.Drawing.Size(19, 15);
            lblDash4.TabIndex = 24;
            lblDash4.Text = "—";
            // 
            // txtStockMax
            // 
            txtStockMax.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtStockMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtStockMax.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtStockMax.Location = new System.Drawing.Point(148, 554);
            txtStockMax.Name = "txtStockMax";
            txtStockMax.Size = new System.Drawing.Size(100, 23);
            txtStockMax.TabIndex = 25;
            txtStockMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dgvUrunler
            // 
            dgvUrunler.AllowUserToAddRows = false;
            dgvUrunler.AllowUserToDeleteRows = false;
            dgvUrunler.AllowUserToResizeColumns = false;
            dgvUrunler.AllowUserToResizeRows = false;
            dgvUrunler.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvUrunler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvUrunler.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUrunler.ColumnHeadersHeight = 52;
            dgvUrunler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUrunler.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colFirma, colCertificate, colMaterial, colBatch, colParent, colHeat, colGrade, colThickness, colWidth, colLength, colStock });
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(30, 42, 56);
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(214, 234, 248);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(30, 42, 56);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvUrunler.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUrunler.EnableHeadersVisualStyles = false;
            dgvUrunler.Location = new System.Drawing.Point(295, 68);
            dgvUrunler.MultiSelect = false;
            dgvUrunler.Name = "dgvUrunler";
            dgvUrunler.ReadOnly = true;
            dgvUrunler.RowHeadersVisible = false;
            dgvUrunler.RowTemplate.Height = 34;
            dgvUrunler.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvUrunler.Size = new System.Drawing.Size(1111, 711);
            dgvUrunler.TabIndex = 2;
            // 
            // colFirma
            // 
            colFirma.HeaderText = "Customer";
            colFirma.MinimumWidth = 110;
            colFirma.Name = "colFirma";
            colFirma.ReadOnly = true;
            // 
            // colCertificate
            // 
            colCertificate.HeaderText = "Certificate No";
            colCertificate.MinimumWidth = 120;
            colCertificate.Name = "colCertificate";
            colCertificate.ReadOnly = true;
            // 
            // colMaterial
            // 
            colMaterial.HeaderText = "Material Sp.";
            colMaterial.MinimumWidth = 110;
            colMaterial.Name = "colMaterial";
            colMaterial.ReadOnly = true;
            // 
            // colBatch
            // 
            colBatch.HeaderText = "Batch No";
            colBatch.MinimumWidth = 100;
            colBatch.Name = "colBatch";
            colBatch.ReadOnly = true;
            // 
            // colParent
            // 
            colParent.HeaderText = "Parent Coil No";
            colParent.MinimumWidth = 130;
            colParent.Name = "colParent";
            colParent.ReadOnly = true;
            // 
            // colHeat
            // 
            colHeat.HeaderText = "Heat No";
            colHeat.MinimumWidth = 100;
            colHeat.Name = "colHeat";
            colHeat.ReadOnly = true;
            // 
            // colGrade
            // 
            colGrade.HeaderText = "Grade";
            colGrade.MinimumWidth = 80;
            colGrade.Name = "colGrade";
            colGrade.ReadOnly = true;
            // 
            // colThickness
            // 
            colThickness.HeaderText = "THK(mm)";
            colThickness.MinimumWidth = 85;
            colThickness.Name = "colThickness";
            colThickness.ReadOnly = true;
            // 
            // colWidth
            // 
            colWidth.HeaderText = "Width(mm)";
            colWidth.MinimumWidth = 90;
            colWidth.Name = "colWidth";
            colWidth.ReadOnly = true;
            // 
            // colLength
            // 
            colLength.HeaderText = "Length(mm)";
            colLength.MinimumWidth = 95;
            colLength.Name = "colLength";
            colLength.ReadOnly = true;
            // 
            // colStock
            // 
            colStock.HeaderText = "Ürün Adedi";
            colStock.MinimumWidth = 100;
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            // 
            // FormArama
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Gainsboro;
            ClientSize = new System.Drawing.Size(1414, 787);
            Controls.Add(dgvUrunler);
            Controls.Add(panelFilter);
            Controls.Add(panelTop);
            Name = "FormArama";
            Text = "FormArama";
            Load += FormArama_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelFilter.ResumeLayout(false);
            panelFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUrunler).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel         panelTop;
        private System.Windows.Forms.Panel         panelFilter;
        private System.Windows.Forms.DataGridView  dgvUrunler;
        private System.Windows.Forms.TextBox       txtSearch;
        private System.Windows.Forms.ComboBox      cmbSearchType;
        private System.Windows.Forms.Button        btnAra;
        private System.Windows.Forms.Button        btnTemizle;
        private System.Windows.Forms.Label         lblKayitSayisi;
        private System.Windows.Forms.Label         lblFilterCustomer;
        private System.Windows.Forms.Label         lblFilterParent;
        private System.Windows.Forms.Label         lblFilterBatch;
        private System.Windows.Forms.Label         lblFilterGrade;
        private System.Windows.Forms.Label         lblFilterHeat;
        private System.Windows.Forms.Label         lblThicknessRange;
        private System.Windows.Forms.Label         lblWidth;
        private System.Windows.Forms.Label         lblLength;
        private System.Windows.Forms.Label         lblStock;
        private System.Windows.Forms.Label         lblDash1;
        private System.Windows.Forms.Label         lblDash2;
        private System.Windows.Forms.Label         lblDash3;
        private System.Windows.Forms.Label         lblDash4;
        private System.Windows.Forms.ComboBox      cmbUrun;
        private System.Windows.Forms.ComboBox      cmbMaterial;
        private System.Windows.Forms.ComboBox      cmbGrade;
        private System.Windows.Forms.ComboBox      cmbThickness;
        private System.Windows.Forms.ComboBox      cmbDiameter;
        private System.Windows.Forms.TextBox       txtThicknessMin;
        private System.Windows.Forms.TextBox       txtThicknessMax;
        private System.Windows.Forms.TextBox       txtWidthMin;
        private System.Windows.Forms.TextBox       txtWidthMax;
        private System.Windows.Forms.TextBox       txtLengthMin;
        private System.Windows.Forms.TextBox       txtLengthMax;
        private System.Windows.Forms.TextBox       txtStockMin;
        private System.Windows.Forms.TextBox       txtStockMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirma;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCertificate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThickness;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock;
    }
}
