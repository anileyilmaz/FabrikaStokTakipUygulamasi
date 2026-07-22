namespace FabrikaStokTakipUygulamasi
{
    partial class FormDashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblDashboard = new System.Windows.Forms.Label();
            this.panelTotal = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalIkon = new System.Windows.Forms.Label();
            this.panelCritical = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCriticalIkon = new System.Windows.Forms.Label();
            this.panelCompany = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblCompanyIkon = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvRecent = new System.Windows.Forms.DataGridView();
            this.colFirma    = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colGrade    = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colThk      = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colWidth    = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLength   = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colStok     = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDate     = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop.SuspendLayout();
            this.panelTotal.SuspendLayout();
            this.panelCritical.SuspendLayout();
            this.panelCompany.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecent)).BeginInit();
            this.SuspendLayout();
            //
            // panelTop
            //
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.lblDashboard);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1184, 80);
            this.panelTop.TabIndex = 0;
            //
            // lblDashboard
            //
            this.lblDashboard.AutoSize = true;
            this.lblDashboard.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblDashboard.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblDashboard.Location = new System.Drawing.Point(25, 22);
            this.lblDashboard.Name = "lblDashboard";
            this.lblDashboard.Size = new System.Drawing.Size(138, 32);
            this.lblDashboard.TabIndex = 0;
            this.lblDashboard.Text = "Dashboard";
            //
            // panelTotal
            //
            this.panelTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.panelTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTotal.Controls.Add(this.label2);
            this.panelTotal.Controls.Add(this.label1);
            this.panelTotal.Controls.Add(this.lblTotalIkon);
            this.panelTotal.Location = new System.Drawing.Point(40, 120);
            this.panelTotal.Name = "panelTotal";
            this.panelTotal.Size = new System.Drawing.Size(250, 120);
            this.panelTotal.TabIndex = 1;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(28, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Toplam Ürün";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(25, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 47);
            this.label1.TabIndex = 0;
            this.label1.Text = "125";
            //
            // panelCritical
            //
            this.panelCritical.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.panelCritical.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCritical.Controls.Add(this.label3);
            this.panelCritical.Controls.Add(this.label4);
            this.panelCritical.Controls.Add(this.lblCriticalIkon);
            this.panelCritical.Location = new System.Drawing.Point(320, 120);
            this.panelCritical.Name = "panelCritical";
            this.panelCritical.Size = new System.Drawing.Size(250, 120);
            this.panelCritical.TabIndex = 2;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(28, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Kritik Stok";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(25, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 47);
            this.label4.TabIndex = 0;
            this.label4.Text = "8";
            //
            // panelCompany
            //
            this.panelCompany.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.panelCompany.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCompany.Controls.Add(this.label5);
            this.panelCompany.Controls.Add(this.label6);
            this.panelCompany.Controls.Add(this.lblCompanyIkon);
            this.panelCompany.Location = new System.Drawing.Point(600, 120);
            this.panelCompany.Name = "panelCompany";
            this.panelCompany.Size = new System.Drawing.Size(250, 120);
            this.panelCompany.TabIndex = 2;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(28, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "Firma Sayısı";
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(25, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 47);
            this.label6.TabIndex = 0;
            this.label6.Text = "24";
            //
            // lblTotalIkon
            //
            this.lblTotalIkon.AutoSize = true;
            this.lblTotalIkon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 22F);
            this.lblTotalIkon.ForeColor = System.Drawing.Color.FromArgb(60, 255, 255, 255);
            this.lblTotalIkon.Location = new System.Drawing.Point(195, 12);
            this.lblTotalIkon.Name = "lblTotalIkon";
            this.lblTotalIkon.Text = FabrikaStokTakipUygulamasi.UIStil.Glyph.Kutu;
            //
            // lblCriticalIkon
            //
            this.lblCriticalIkon.AutoSize = true;
            this.lblCriticalIkon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 22F);
            this.lblCriticalIkon.ForeColor = System.Drawing.Color.FromArgb(60, 255, 255, 255);
            this.lblCriticalIkon.Location = new System.Drawing.Point(195, 12);
            this.lblCriticalIkon.Name = "lblCriticalIkon";
            this.lblCriticalIkon.Text = FabrikaStokTakipUygulamasi.UIStil.Glyph.Uyarim;
            //
            // lblCompanyIkon
            //
            this.lblCompanyIkon.AutoSize = true;
            this.lblCompanyIkon.Font = new System.Drawing.Font("Segoe MDL2 Assets", 22F);
            this.lblCompanyIkon.ForeColor = System.Drawing.Color.FromArgb(60, 255, 255, 255);
            this.lblCompanyIkon.Location = new System.Drawing.Point(195, 12);
            this.lblCompanyIkon.Name = "lblCompanyIkon";
            this.lblCompanyIkon.Text = FabrikaStokTakipUygulamasi.UIStil.Glyph.Kisiler;
            //
            // chartStokDagilimi
            //
            this.chartStokDagilimi = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartStokDagilimi)).BeginInit();
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chartStokDagilimi.ChartAreas.Add(chartArea1);
            this.chartStokDagilimi.BackColor = System.Drawing.Color.White;
            this.chartStokDagilimi.BorderlineColor = System.Drawing.Color.FromArgb(189, 195, 199);
            this.chartStokDagilimi.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chartStokDagilimi.BorderlineWidth = 1;
            this.chartStokDagilimi.Location = new System.Drawing.Point(890, 120);
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            series1.Name = "Seri1";
            series1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.chartStokDagilimi.Series.Add(series1);
            this.chartStokDagilimi.Name = "chartStokDagilimi";
            this.chartStokDagilimi.Size = new System.Drawing.Size(254, 120);
            this.chartStokDagilimi.TabIndex = 5;
            this.chartStokDagilimi.Text = "chartStokDagilimi";
            ((System.ComponentModel.ISupportInitialize)(this.chartStokDagilimi)).EndInit();
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(40, 290);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(196, 25);
            this.label7.TabIndex = 3;
            this.label7.Text = "Son Eklenen Ürünler";
            //
            // dgvRecent
            //
            this.dgvRecent.AllowUserToAddRows = false;
            this.dgvRecent.AllowUserToDeleteRows = false;
            this.dgvRecent.AllowUserToResizeColumns = false;
            this.dgvRecent.AllowUserToResizeRows = false;
            this.dgvRecent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRecent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecent.ColumnHeadersHeight         = 52;
            this.dgvRecent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRecent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colFirma, colMaterial, colGrade, colThk, colWidth, colLength, colStok, colDate });
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecent.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecent.EnableHeadersVisualStyles = false;
            this.dgvRecent.Location = new System.Drawing.Point(40, 330);
            this.dgvRecent.MultiSelect = false;
            this.dgvRecent.Name = "dgvRecent";
            this.dgvRecent.ReadOnly = true;
            this.dgvRecent.RowHeadersVisible = false;
            this.dgvRecent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecent.Size = new System.Drawing.Size(1110, 300);
            this.dgvRecent.TabIndex = 4;
            this.dgvRecent.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecent_CellContentClick);
            //
            // colFirma
            this.colFirma.HeaderText = "Customer";      this.colFirma.Name = "colFirma";     this.colFirma.ReadOnly = true;
            // colMaterial
            this.colMaterial.HeaderText = "Material Sp."; this.colMaterial.Name = "colMaterial"; this.colMaterial.ReadOnly = true;
            // colGrade
            this.colGrade.HeaderText = "Grade";         this.colGrade.Name = "colGrade";     this.colGrade.ReadOnly = true;
            // colThk
            this.colThk.HeaderText   = "THK(mm)";       this.colThk.Name = "colThk";         this.colThk.ReadOnly = true;
            // colWidth
            this.colWidth.HeaderText = "Width(mm)";     this.colWidth.Name = "colWidth";     this.colWidth.ReadOnly = true;
            // colLength
            this.colLength.HeaderText= "Length(mm)";    this.colLength.Name = "colLength";   this.colLength.ReadOnly = true;
            // colStok
            this.colStok.HeaderText  = "Ürün Adedi";    this.colStok.Name = "colStok";       this.colStok.ReadOnly = true;
            // colDate
            this.colDate.HeaderText  = "Tarih";         this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            this.colDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //
            // FormDashboard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.chartStokDagilimi);
            this.Controls.Add(this.dgvRecent);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panelCompany);
            this.Controls.Add(this.panelCritical);
            this.Controls.Add(this.panelTotal);
            this.Controls.Add(this.panelTop);
            this.Name = "FormDashboard";
            this.Text = "FormDashboard";
            this.Load += new System.EventHandler(this.FormDashboard_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelTotal.ResumeLayout(false);
            this.panelTotal.PerformLayout();
            this.panelCritical.ResumeLayout(false);
            this.panelCritical.PerformLayout();
            this.panelCompany.ResumeLayout(false);
            this.panelCompany.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblDashboard;
        private System.Windows.Forms.Panel panelTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalIkon;
        private System.Windows.Forms.Panel panelCritical;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCriticalIkon;
        private System.Windows.Forms.Panel panelCompany;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblCompanyIkon;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStokDagilimi;
        private System.Windows.Forms.DataGridView dgvRecent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirma;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThk;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStok;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
    }
}