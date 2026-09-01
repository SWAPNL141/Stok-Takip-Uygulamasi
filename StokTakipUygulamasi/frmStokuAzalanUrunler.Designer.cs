namespace StokTakipUygulamasi
{
    partial class frmStokuAzalanUrunler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStokuAzalanUrunler));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtAraUrunAdi = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStokGuncelle = new System.Windows.Forms.Button();
            this.nmrStokAdedi = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.grdStokuAzalanUrunler = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrStokAdedi)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdStokuAzalanUrunler)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAraUrunAdi
            // 
            this.txtAraUrunAdi.Location = new System.Drawing.Point(103, 31);
            this.txtAraUrunAdi.Name = "txtAraUrunAdi";
            this.txtAraUrunAdi.Size = new System.Drawing.Size(187, 28);
            this.txtAraUrunAdi.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtAraUrunAdi);
            this.groupBox1.Location = new System.Drawing.Point(6, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 80);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Arama";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ürün Adı:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnStokGuncelle);
            this.groupBox2.Controls.Add(this.nmrStokAdedi);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(308, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(440, 80);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stok Bilgisi Güncelleme";
            // 
            // btnStokGuncelle
            // 
            this.btnStokGuncelle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnStokGuncelle.Image = ((System.Drawing.Image)(resources.GetObject("btnStokGuncelle.Image")));
            this.btnStokGuncelle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStokGuncelle.Location = new System.Drawing.Point(326, 25);
            this.btnStokGuncelle.Name = "btnStokGuncelle";
            this.btnStokGuncelle.Size = new System.Drawing.Size(108, 40);
            this.btnStokGuncelle.TabIndex = 11;
            this.btnStokGuncelle.Text = "Güncelle";
            this.btnStokGuncelle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStokGuncelle.UseVisualStyleBackColor = true;
            // 
            // nmrStokAdedi
            // 
            this.nmrStokAdedi.Location = new System.Drawing.Point(154, 31);
            this.nmrStokAdedi.Name = "nmrStokAdedi";
            this.nmrStokAdedi.Size = new System.Drawing.Size(120, 28);
            this.nmrStokAdedi.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Stok Adedi:";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.grdStokuAzalanUrunler);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(766, 360);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Stoğu Azalan Ürünler";
            // 
            // grdStokuAzalanUrunler
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.grdStokuAzalanUrunler.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdStokuAzalanUrunler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdStokuAzalanUrunler.Location = new System.Drawing.Point(6, 123);
            this.grdStokuAzalanUrunler.Name = "grdStokuAzalanUrunler";
            this.grdStokuAzalanUrunler.Size = new System.Drawing.Size(754, 231);
            this.grdStokuAzalanUrunler.TabIndex = 3;
            this.grdStokuAzalanUrunler.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdStokuAzalanUrunler_CellContentClick);
            // 
            // frmStokuAzalanUrunler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::StokTakipUygulamasi.Properties.Resources.vecteezy_abstract_geometric_white_stripe_shapes_with_golden_light_in_7522796;
            this.ClientSize = new System.Drawing.Size(787, 381);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStokuAzalanUrunler";
            this.Text = "Stoğu Azalan Ürünler";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmrStokAdedi)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdStokuAzalanUrunler)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtAraUrunAdi;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nmrStokAdedi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStokGuncelle;
        private System.Windows.Forms.DataGridView grdStokuAzalanUrunler;
    }
}