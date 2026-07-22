namespace StokTakipUI
{
    partial class FormUrunEkle
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            btnUrunEkle = new System.Windows.Forms.Button();
            lblTitle = new System.Windows.Forms.Label();
            grpGenel = new System.Windows.Forms.GroupBox();
            txtParent = new System.Windows.Forms.TextBox();
            txtBatch = new System.Windows.Forms.TextBox();
            txtMaterial = new System.Windows.Forms.TextBox();
            txtCertificate = new System.Windows.Forms.TextBox();
            txtCustomer = new System.Windows.Forms.TextBox();
            lblParent = new System.Windows.Forms.Label();
            Batch = new System.Windows.Forms.Label();
            Material = new System.Windows.Forms.Label();
            Certificate = new System.Windows.Forms.Label();
            Customer = new System.Windows.Forms.Label();
            grpOlcu = new System.Windows.Forms.GroupBox();
            txtLength = new System.Windows.Forms.TextBox();
            txtWidth = new System.Windows.Forms.TextBox();
            txtTHK = new System.Windows.Forms.TextBox();
            txtGrade = new System.Windows.Forms.TextBox();
            txtHeat = new System.Windows.Forms.TextBox();
            Lenght = new System.Windows.Forms.Label();
            lblWidth = new System.Windows.Forms.Label();
            THK = new System.Windows.Forms.Label();
            Grade = new System.Windows.Forms.Label();
            Heat = new System.Windows.Forms.Label();
            grpStok = new System.Windows.Forms.GroupBox();
            txtAdet = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            btnTemizle = new System.Windows.Forms.Button();
            grpSertifika = new System.Windows.Forms.GroupBox();
            btnPdfSec = new System.Windows.Forms.Button();
            btnPdfKaldir = new System.Windows.Forms.Button();
            lblPdfDurum = new System.Windows.Forms.Label();
            grpGenel.SuspendLayout();
            grpOlcu.SuspendLayout();
            grpStok.SuspendLayout();
            grpSertifika.SuspendLayout();
            SuspendLayout();
            //
            // contextMenuStrip1
            //
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            //
            // btnUrunEkle
            //
            btnUrunEkle.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            btnUrunEkle.Cursor = System.Windows.Forms.Cursors.Hand;
            btnUrunEkle.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnUrunEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnUrunEkle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            btnUrunEkle.ForeColor = System.Drawing.Color.White;
            btnUrunEkle.Location = new System.Drawing.Point(780, 540);
            btnUrunEkle.Name = "btnUrunEkle";
            btnUrunEkle.Size = new System.Drawing.Size(160, 45);
            btnUrunEkle.TabIndex = 23;
            btnUrunEkle.Text = "Ürün Ekle";
            btnUrunEkle.UseVisualStyleBackColor = false;
            btnUrunEkle.Click += btnUrunEkle_Click;
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(35, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(158, 37);
            lblTitle.TabIndex = 24;
            lblTitle.Text = "ÜRÜN EKLE";
            //
            // grpGenel
            //
            grpGenel.Controls.Add(txtParent);
            grpGenel.Controls.Add(txtBatch);
            grpGenel.Controls.Add(txtMaterial);
            grpGenel.Controls.Add(txtCertificate);
            grpGenel.Controls.Add(txtCustomer);
            grpGenel.Controls.Add(lblParent);
            grpGenel.Controls.Add(Batch);
            grpGenel.Controls.Add(Material);
            grpGenel.Controls.Add(Certificate);
            grpGenel.Controls.Add(Customer);
            grpGenel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            grpGenel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            grpGenel.Location = new System.Drawing.Point(35, 90);
            grpGenel.Name = "grpGenel";
            grpGenel.Size = new System.Drawing.Size(420, 250);
            grpGenel.TabIndex = 25;
            grpGenel.TabStop = false;
            grpGenel.Text = "Genel Bilgiler";
            //
            // txtParent
            //
            txtParent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtParent.Location = new System.Drawing.Point(170, 198);
            txtParent.Name = "txtParent";
            txtParent.Size = new System.Drawing.Size(230, 27);
            txtParent.TabIndex = 25;
            //
            // txtBatch
            //
            txtBatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtBatch.Location = new System.Drawing.Point(170, 158);
            txtBatch.Name = "txtBatch";
            txtBatch.Size = new System.Drawing.Size(230, 27);
            txtBatch.TabIndex = 24;
            //
            // txtMaterial
            //
            txtMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtMaterial.Location = new System.Drawing.Point(170, 118);
            txtMaterial.Name = "txtMaterial";
            txtMaterial.Size = new System.Drawing.Size(230, 27);
            txtMaterial.TabIndex = 23;
            //
            // txtCertificate
            //
            txtCertificate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCertificate.Location = new System.Drawing.Point(170, 78);
            txtCertificate.Name = "txtCertificate";
            txtCertificate.Size = new System.Drawing.Size(230, 27);
            txtCertificate.TabIndex = 22;
            //
            // txtCustomer
            //
            txtCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtCustomer.Location = new System.Drawing.Point(170, 38);
            txtCustomer.Name = "txtCustomer";
            txtCustomer.Size = new System.Drawing.Size(230, 27);
            txtCustomer.TabIndex = 21;
            //
            // lblParent
            //
            lblParent.AutoSize = true;
            lblParent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            lblParent.Location = new System.Drawing.Point(20, 200);
            lblParent.Name = "lblParent";
            lblParent.Size = new System.Drawing.Size(75, 13);
            lblParent.TabIndex = 20;
            lblParent.Text = "Parent Coil No";
            //
            // Batch
            //
            Batch.AutoSize = true;
            Batch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            Batch.Location = new System.Drawing.Point(20, 160);
            Batch.Name = "Batch";
            Batch.Size = new System.Drawing.Size(52, 13);
            Batch.TabIndex = 19;
            Batch.Text = "Batch No";
            //
            // Material
            //
            Material.AutoSize = true;
            Material.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            Material.Location = new System.Drawing.Point(20, 120);
            Material.Name = "Material";
            Material.Size = new System.Drawing.Size(63, 13);
            Material.TabIndex = 18;
            Material.Text = "Material Sp.";
            //
            // Certificate
            //
            Certificate.AutoSize = true;
            Certificate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            Certificate.Location = new System.Drawing.Point(20, 80);
            Certificate.Name = "Certificate";
            Certificate.Size = new System.Drawing.Size(71, 13);
            Certificate.TabIndex = 17;
            Certificate.Text = "Certificate No";
            //
            // Customer
            //
            Customer.AutoSize = true;
            Customer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            Customer.Location = new System.Drawing.Point(20, 40);
            Customer.Name = "Customer";
            Customer.Size = new System.Drawing.Size(51, 13);
            Customer.TabIndex = 16;
            Customer.Text = "Customer";
            //
            // grpOlcu
            //
            grpOlcu.Controls.Add(txtLength);
            grpOlcu.Controls.Add(txtWidth);
            grpOlcu.Controls.Add(txtTHK);
            grpOlcu.Controls.Add(txtGrade);
            grpOlcu.Controls.Add(txtHeat);
            grpOlcu.Controls.Add(Lenght);
            grpOlcu.Controls.Add(lblWidth);
            grpOlcu.Controls.Add(THK);
            grpOlcu.Controls.Add(Grade);
            grpOlcu.Controls.Add(Heat);
            grpOlcu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            grpOlcu.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            grpOlcu.Location = new System.Drawing.Point(520, 90);
            grpOlcu.Name = "grpOlcu";
            grpOlcu.Size = new System.Drawing.Size(420, 250);
            grpOlcu.TabIndex = 26;
            grpOlcu.TabStop = false;
            grpOlcu.Text = "Ürün Ölçüleri";
            //
            // txtLength
            //
            txtLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtLength.Location = new System.Drawing.Point(170, 198);
            txtLength.Name = "txtLength";
            txtLength.Size = new System.Drawing.Size(230, 27);
            txtLength.TabIndex = 30;
            //
            // txtWidth
            //
            txtWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtWidth.Location = new System.Drawing.Point(170, 158);
            txtWidth.Name = "txtWidth";
            txtWidth.Size = new System.Drawing.Size(230, 27);
            txtWidth.TabIndex = 29;
            //
            // txtTHK
            //
            txtTHK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtTHK.Location = new System.Drawing.Point(170, 118);
            txtTHK.Name = "txtTHK";
            txtTHK.Size = new System.Drawing.Size(230, 27);
            txtTHK.TabIndex = 28;
            //
            // txtGrade
            //
            txtGrade.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtGrade.Location = new System.Drawing.Point(170, 78);
            txtGrade.Name = "txtGrade";
            txtGrade.Size = new System.Drawing.Size(230, 27);
            txtGrade.TabIndex = 27;
            //
            // txtHeat
            //
            txtHeat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtHeat.Location = new System.Drawing.Point(170, 38);
            txtHeat.Name = "txtHeat";
            txtHeat.Size = new System.Drawing.Size(230, 27);
            txtHeat.TabIndex = 26;
            //
            // Lenght
            //
            Lenght.AutoSize = true;
            Lenght.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            Lenght.Location = new System.Drawing.Point(20, 200);
            Lenght.Name = "Lenght";
            Lenght.Size = new System.Drawing.Size(62, 13);
            Lenght.TabIndex = 25;
            Lenght.Text = "Lenght(mm)";
            //
            // lblWidth
            //
            lblWidth.AutoSize = true;
            lblWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            lblWidth.Location = new System.Drawing.Point(20, 160);
            lblWidth.Name = "lblWidth";
            lblWidth.Size = new System.Drawing.Size(57, 13);
            lblWidth.TabIndex = 24;
            lblWidth.Text = "Width(mm)";
            //
            // THK
            //
            THK.AutoSize = true;
            THK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            THK.Location = new System.Drawing.Point(20, 120);
            THK.Name = "THK";
            THK.Size = new System.Drawing.Size(51, 13);
            THK.TabIndex = 23;
            THK.Text = "THK(mm)";
            //
            // Grade
            //
            Grade.AutoSize = true;
            Grade.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            Grade.Location = new System.Drawing.Point(20, 80);
            Grade.Name = "Grade";
            Grade.Size = new System.Drawing.Size(36, 13);
            Grade.TabIndex = 22;
            Grade.Text = "Grade";
            //
            // Heat
            //
            Heat.AutoSize = true;
            Heat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            Heat.Location = new System.Drawing.Point(20, 40);
            Heat.Name = "Heat";
            Heat.Size = new System.Drawing.Size(47, 13);
            Heat.TabIndex = 21;
            Heat.Text = "Heat No";
            //
            // grpStok
            //
            grpStok.Controls.Add(txtAdet);
            grpStok.Controls.Add(label1);
            grpStok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            grpStok.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            grpStok.Location = new System.Drawing.Point(35, 370);
            grpStok.Name = "grpStok";
            grpStok.Size = new System.Drawing.Size(420, 80);
            grpStok.TabIndex = 27;
            grpStok.TabStop = false;
            grpStok.Text = "Stok Bilgisi";
            //
            // txtAdet
            //
            txtAdet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtAdet.Location = new System.Drawing.Point(170, 35);
            txtAdet.Name = "txtAdet";
            txtAdet.Size = new System.Drawing.Size(120, 27);
            txtAdet.TabIndex = 24;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            label1.Location = new System.Drawing.Point(20, 38);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 15);
            label1.TabIndex = 23;
            label1.Text = "Ürün Adedi";
            //
            // btnTemizle
            //
            btnTemizle.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            btnTemizle.Cursor = System.Windows.Forms.Cursors.Hand;
            btnTemizle.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnTemizle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTemizle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            btnTemizle.ForeColor = System.Drawing.Color.White;
            btnTemizle.Location = new System.Drawing.Point(780, 595);
            btnTemizle.Name = "btnTemizle";
            btnTemizle.Size = new System.Drawing.Size(160, 45);
            btnTemizle.TabIndex = 28;
            btnTemizle.Text = "Temizle";
            btnTemizle.UseVisualStyleBackColor = false;
            btnTemizle.Click += btnTemizle_Click;
            //
            // grpSertifika
            //
            grpSertifika.Controls.Add(btnPdfSec);
            grpSertifika.Controls.Add(btnPdfKaldir);
            grpSertifika.Controls.Add(lblPdfDurum);
            grpSertifika.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            grpSertifika.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            grpSertifika.Location = new System.Drawing.Point(520, 370);
            grpSertifika.Name = "grpSertifika";
            grpSertifika.Size = new System.Drawing.Size(420, 140);
            grpSertifika.TabIndex = 29;
            grpSertifika.TabStop = false;
            grpSertifika.Text = "Sertifika (PDF)";
            //
            // btnPdfSec
            //
            btnPdfSec.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnPdfSec.Cursor = System.Windows.Forms.Cursors.Hand;
            btnPdfSec.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnPdfSec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPdfSec.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            btnPdfSec.ForeColor = System.Drawing.Color.White;
            btnPdfSec.Location = new System.Drawing.Point(15, 38);
            btnPdfSec.Name = "btnPdfSec";
            btnPdfSec.Size = new System.Drawing.Size(185, 38);
            btnPdfSec.TabIndex = 0;
            btnPdfSec.Text = "📄 Sertifika PDF Seç...";
            btnPdfSec.UseVisualStyleBackColor = false;
            btnPdfSec.Click += btnPdfSec_Click;
            //
            // btnPdfKaldir
            //
            btnPdfKaldir.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            btnPdfKaldir.Cursor = System.Windows.Forms.Cursors.Hand;
            btnPdfKaldir.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnPdfKaldir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPdfKaldir.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            btnPdfKaldir.ForeColor = System.Drawing.Color.White;
            btnPdfKaldir.Location = new System.Drawing.Point(210, 38);
            btnPdfKaldir.Name = "btnPdfKaldir";
            btnPdfKaldir.Size = new System.Drawing.Size(185, 38);
            btnPdfKaldir.TabIndex = 1;
            btnPdfKaldir.Text = "✖ PDF Kaldır";
            btnPdfKaldir.UseVisualStyleBackColor = false;
            btnPdfKaldir.Visible = false;
            btnPdfKaldir.Click += btnPdfKaldir_Click;
            //
            // lblPdfDurum
            //
            lblPdfDurum.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblPdfDurum.ForeColor = System.Drawing.Color.FromArgb(149, 165, 166);
            lblPdfDurum.Location = new System.Drawing.Point(15, 90);
            lblPdfDurum.Name = "lblPdfDurum";
            lblPdfDurum.Size = new System.Drawing.Size(390, 22);
            lblPdfDurum.TabIndex = 2;
            lblPdfDurum.Text = "PDF seçilmedi";
            //
            // FormUrunEkle
            //
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.Color.Gainsboro;
            ClientSize = new System.Drawing.Size(1184, 661);
            Controls.Add(btnTemizle);
            Controls.Add(grpSertifika);
            Controls.Add(grpStok);
            Controls.Add(grpOlcu);
            Controls.Add(grpGenel);
            Controls.Add(lblTitle);
            Controls.Add(btnUrunEkle);
            Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            Name = "FormUrunEkle";
            Text = "Ürün Ekle";
            Load += FormUrunEkle_Load;
            grpGenel.ResumeLayout(false);
            grpGenel.PerformLayout();
            grpOlcu.ResumeLayout(false);
            grpOlcu.PerformLayout();
            grpStok.ResumeLayout(false);
            grpStok.PerformLayout();
            grpSertifika.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button btnUrunEkle;
        private System.Windows.Forms.Label  lblTitle;
        private System.Windows.Forms.GroupBox grpGenel;
        private System.Windows.Forms.TextBox txtParent, txtBatch, txtMaterial, txtCertificate, txtCustomer;
        private System.Windows.Forms.Label   lblParent, Batch, Material, Certificate, Customer;
        private System.Windows.Forms.GroupBox grpOlcu;
        private System.Windows.Forms.TextBox txtLength, txtWidth, txtTHK, txtGrade, txtHeat;
        private System.Windows.Forms.Label   Lenght, lblWidth, THK, Grade, Heat;
        private System.Windows.Forms.GroupBox grpStok;
        private System.Windows.Forms.TextBox txtAdet;
        private System.Windows.Forms.Label   label1;
        private System.Windows.Forms.Button  btnTemizle;
        // PDF kontrolleri
        private System.Windows.Forms.GroupBox grpSertifika;
        private System.Windows.Forms.Button   btnPdfSec;
        private System.Windows.Forms.Button   btnPdfKaldir;
        private System.Windows.Forms.Label    lblPdfDurum;
    }
}
