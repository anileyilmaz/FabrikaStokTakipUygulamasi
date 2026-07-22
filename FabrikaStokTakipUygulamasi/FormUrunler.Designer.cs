namespace FabrikaStokTakipUygulamasi
{
    partial class FormUrunler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            panelTop = new System.Windows.Forms.Panel();
            btnExcel = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnEdit = new System.Windows.Forms.Button();
            btnDetail = new System.Windows.Forms.Button();
            lblTotalProduct = new System.Windows.Forms.Label();
            dgvProducts = new System.Windows.Forms.DataGridView();
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
            colStok = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTarih = new System.Windows.Forms.DataGridViewTextBoxColumn();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelTop.Controls.Add(btnExcel);
            panelTop.Controls.Add(btnDelete);
            panelTop.Controls.Add(btnEdit);
            panelTop.Controls.Add(btnDetail);
            panelTop.Controls.Add(lblTotalProduct);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(1381, 92);
            panelTop.TabIndex = 0;
            // 
            // btnExcel
            // 
            btnExcel.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExcel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
            btnExcel.ForeColor = System.Drawing.Color.Black;
            btnExcel.Location = new System.Drawing.Point(443, 23);
            btnExcel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnExcel.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new System.Drawing.Size(128, 46);
            btnExcel.TabIndex = 4;
            btnExcel.Text = "Excel Aktar";
            btnExcel.UseVisualStyleBackColor = false;
            btnExcel.Click += btnExcel_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
            btnDelete.ForeColor = System.Drawing.Color.Black;
            btnDelete.Location = new System.Drawing.Point(303, 23);
            btnDelete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnDelete.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(128, 46);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Sil";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = System.Drawing.Color.FromArgb(243, 156, 18);
            btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
            btnEdit.ForeColor = System.Drawing.Color.Black;
            btnEdit.Location = new System.Drawing.Point(163, 23);
            btnEdit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnEdit.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(128, 46);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Düzenle";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDetail
            // 
            btnDetail.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            btnDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
            btnDetail.Location = new System.Drawing.Point(23, 23);
            btnDetail.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnDetail.Padding = new System.Windows.Forms.Padding(24, 0, 0, 0);
            btnDetail.Name = "btnDetail";
            btnDetail.Size = new System.Drawing.Size(128, 46);
            btnDetail.TabIndex = 3;
            btnDetail.Text = "Ürün Detay";
            btnDetail.UseVisualStyleBackColor = false;
            btnDetail.Click += btnDetail_Click;
            // 
            // lblTotalProduct
            // 
            lblTotalProduct.AutoSize = true;
            lblTotalProduct.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
            lblTotalProduct.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
            lblTotalProduct.Location = new System.Drawing.Point(1108, 35);
            lblTotalProduct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalProduct.Name = "lblTotalProduct";
            lblTotalProduct.Size = new System.Drawing.Size(117, 20);
            lblTotalProduct.TabIndex = 1;
            lblTotalProduct.Text = "Toplam Ürün: 0";
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AllowUserToResizeColumns = false;
            dgvProducts.AllowUserToResizeRows = false;
            dgvProducts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 162);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvProducts.ColumnHeadersHeight = 52;
            dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colFirma, colCertificate, colMaterial, colBatch, colParent, colHeat, colGrade, colThickness, colWidth, colLength, colStok, colTarih });
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 162);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvProducts.DefaultCellStyle = dataGridViewCellStyle2;
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.Location = new System.Drawing.Point(29, 104);
            dgvProducts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowTemplate.Height = 35;
            dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new System.Drawing.Size(1312, 577);
            dgvProducts.TabIndex = 1;
            dgvProducts.CellContentClick += dgvProducts_CellContentClick;
            // 
            // colFirma
            // 
            colFirma.HeaderText = "Customer";
            colFirma.Name = "colFirma";
            colFirma.ReadOnly = true;
            // 
            // colCertificate
            // 
            colCertificate.HeaderText = "Certificate No";
            colCertificate.Name = "colCertificate";
            colCertificate.ReadOnly = true;
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
            // colStok
            // 
            colStok.HeaderText = "Ürün Adedi";
            colStok.Name = "colStok";
            colStok.ReadOnly = true;
            // 
            // colTarih
            // 
            colTarih.HeaderText = "Tarih";
            colTarih.Name = "colTarih";
            colTarih.ReadOnly = true;
            colTarih.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FormUrunler
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            ClientSize = new System.Drawing.Size(1381, 763);
            Controls.Add(dgvProducts);
            Controls.Add(panelTop);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormUrunler";
            Text = "FormUrunler";
            Load += FormUrunler_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTotalProduct;
        private System.Windows.Forms.DataGridView dgvProducts;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colStok;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTarih;
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
    }
}