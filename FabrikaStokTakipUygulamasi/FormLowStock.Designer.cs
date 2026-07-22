namespace FabrikaStokTakipUygulamasi
{
    partial class FormLowStock
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            panelTop = new System.Windows.Forms.Panel();
            btnYeni = new System.Windows.Forms.Button();
            btnDuzenle = new System.Windows.Forms.Button();
            lblcriticalstock = new System.Windows.Forms.Label();
            lblCriticalCount = new System.Windows.Forms.Label();
            dgvLowStock = new System.Windows.Forms.DataGridView();
            colCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCert = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colHeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colThickness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLowLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLowStock).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            panelTop.Controls.Add(btnYeni);
            panelTop.Controls.Add(btnDuzenle);
            panelTop.Controls.Add(lblcriticalstock);
            panelTop.Controls.Add(lblCriticalCount);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(1381, 83);
            panelTop.TabIndex = 0;
            // 
            // btnYeni
            // 
            btnYeni.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            btnYeni.Cursor = System.Windows.Forms.Cursors.Hand;
            btnYeni.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnYeni.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnYeni.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            btnYeni.ForeColor = System.Drawing.Color.White;
            btnYeni.Location = new System.Drawing.Point(19, 18);
            btnYeni.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnYeni.Name = "btnYeni";
            btnYeni.Padding = new System.Windows.Forms.Padding(26, 0, 0, 0);
            btnYeni.Size = new System.Drawing.Size(128, 44);
            btnYeni.TabIndex = 0;
            btnYeni.Text = "Yeni";
            btnYeni.UseVisualStyleBackColor = false;
            btnYeni.Click += btnYeni_Click;
            // 
            // btnDuzenle
            // 
            btnDuzenle.BackColor = System.Drawing.Color.FromArgb(243, 156, 18);
            btnDuzenle.Cursor = System.Windows.Forms.Cursors.Hand;
            btnDuzenle.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnDuzenle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDuzenle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            btnDuzenle.ForeColor = System.Drawing.Color.Black;
            btnDuzenle.Location = new System.Drawing.Point(159, 18);
            btnDuzenle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnDuzenle.Name = "btnDuzenle";
            btnDuzenle.Padding = new System.Windows.Forms.Padding(26, 0, 0, 0);
            btnDuzenle.Size = new System.Drawing.Size(128, 44);
            btnDuzenle.TabIndex = 1;
            btnDuzenle.Text = "Düzenle";
            btnDuzenle.UseVisualStyleBackColor = false;
            btnDuzenle.Click += btnDuzenle_Click;
            // 
            // lblcriticalstock
            // 
            lblcriticalstock.AutoSize = true;
            lblcriticalstock.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold);
            lblcriticalstock.ForeColor = System.Drawing.Color.White;
            lblcriticalstock.Location = new System.Drawing.Point(315, 21);
            lblcriticalstock.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblcriticalstock.Name = "lblcriticalstock";
            lblcriticalstock.Size = new System.Drawing.Size(181, 30);
            lblcriticalstock.TabIndex = 5;
            lblcriticalstock.Text = "Kritik Stok Takibi";
            // 
            // lblCriticalCount
            // 
            lblCriticalCount.AutoSize = true;
            lblCriticalCount.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            lblCriticalCount.ForeColor = System.Drawing.Color.FromArgb(255, 205, 210);
            lblCriticalCount.Location = new System.Drawing.Point(1003, 28);
            lblCriticalCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCriticalCount.Name = "lblCriticalCount";
            lblCriticalCount.Size = new System.Drawing.Size(103, 20);
            lblCriticalCount.TabIndex = 3;
            lblCriticalCount.Text = "Kritik Ürün: 0";
            // 
            // dgvLowStock
            // 
            dgvLowStock.AllowUserToAddRows = false;
            dgvLowStock.AllowUserToDeleteRows = false;
            dgvLowStock.AllowUserToResizeColumns = false;
            dgvLowStock.AllowUserToResizeRows = false;
            dgvLowStock.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvLowStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvLowStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvLowStock.ColumnHeadersHeight = 52;
            dgvLowStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvLowStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colCustomer, colCert, colMaterial, colBatch, colParent, colHeat, colGrade, colThickness, colWidth, colLength, colStock, colLowLimit, colStatus });
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvLowStock.DefaultCellStyle = dataGridViewCellStyle2;
            dgvLowStock.EnableHeadersVisualStyles = false;
            dgvLowStock.Location = new System.Drawing.Point(0, 83);
            dgvLowStock.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dgvLowStock.MultiSelect = false;
            dgvLowStock.Name = "dgvLowStock";
            dgvLowStock.ReadOnly = true;
            dgvLowStock.RowHeadersVisible = false;
            dgvLowStock.RowTemplate.Height = 35;
            dgvLowStock.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvLowStock.Size = new System.Drawing.Size(1381, 680);
            dgvLowStock.TabIndex = 4;
            dgvLowStock.CellClick += dgvLowStock_CellClick;
            dgvLowStock.CellDoubleClick += dgvLowStock_CellDoubleClick;
            // 
            // colCustomer
            // 
            colCustomer.HeaderText = "Customer";
            colCustomer.Name = "colCustomer";
            colCustomer.ReadOnly = true;
            // 
            // colCert
            // 
            colCert.HeaderText = "Certificate No";
            colCert.Name = "colCert";
            colCert.ReadOnly = true;
            // 
            // colMaterial
            // 
            colMaterial.HeaderText = "Material Sp.";
            colMaterial.Name = "colMaterial";
            colMaterial.ReadOnly = true;
            // 
            // colBatch
            // 
            colBatch.HeaderText = "Batch No";
            colBatch.Name = "colBatch";
            colBatch.ReadOnly = true;
            // 
            // colParent
            // 
            colParent.HeaderText = "Parent Coil No";
            colParent.Name = "colParent";
            colParent.ReadOnly = true;
            // 
            // colHeat
            // 
            colHeat.HeaderText = "Heat No";
            colHeat.Name = "colHeat";
            colHeat.ReadOnly = true;
            // 
            // colGrade
            // 
            colGrade.HeaderText = "Grade";
            colGrade.Name = "colGrade";
            colGrade.ReadOnly = true;
            // 
            // colThickness
            // 
            colThickness.HeaderText = "THK(mm)";
            colThickness.Name = "colThickness";
            colThickness.ReadOnly = true;
            // 
            // colWidth
            // 
            colWidth.HeaderText = "Width(mm)";
            colWidth.Name = "colWidth";
            colWidth.ReadOnly = true;
            // 
            // colLength
            // 
            colLength.HeaderText = "Length(mm)";
            colLength.Name = "colLength";
            colLength.ReadOnly = true;
            // 
            // colStock
            // 
            colStock.HeaderText = "Ürün Adedi";
            colStock.Name = "colStock";
            colStock.ReadOnly = true;
            // 
            // colLowLimit
            // 
            colLowLimit.HeaderText = "Limit";
            colLowLimit.Name = "colLowLimit";
            colLowLimit.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.HeaderText = "Durum";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // FormLowStock
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            ClientSize = new System.Drawing.Size(1381, 763);
            Controls.Add(dgvLowStock);
            Controls.Add(panelTop);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormLowStock";
            Text = "Low Stock Takibi";
            Load += FormLowStock_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLowStock).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblcriticalstock;
        private System.Windows.Forms.Button btnYeni;
        private System.Windows.Forms.Button btnDuzenle;
        private System.Windows.Forms.Label lblCriticalCount;
        private System.Windows.Forms.DataGridView dgvLowStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCert;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThickness;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLowLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    }
}
