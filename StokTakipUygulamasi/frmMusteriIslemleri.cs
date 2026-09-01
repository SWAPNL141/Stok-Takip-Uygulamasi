using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StokTakipUygulamasi
{
    public partial class frmMusteriIslemleri : Form
    {
        stok_takip_dbEntities stok_Takip;
        int seciliMusteriID = 0;

        public frmMusteriIslemleri()
        {
            InitializeComponent();

            this.Load -= frmMusteriIslemleri_Load;
            this.Load += frmMusteriIslemleri_Load;

            this.grdMusteriler.CellClick -= grdMusteriler_CellClick;
            this.grdMusteriler.CellClick += grdMusteriler_CellClick;

            this.btnEkle.Click -= btnEkle_Click;
            this.btnEkle.Click += btnEkle_Click;

            this.btnSil.Click -= btnSil_Click;
            this.btnSil.Click += btnSil_Click;

            this.btnGuncelle.Click -= btnGuncelle_Click;
            this.btnGuncelle.Click += btnGuncelle_Click;

            this.grdMusteriler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.grdMusteriler.MultiSelect = false;
        }

        private void frmMusteriIslemleri_Load(object sender, EventArgs e)
        {
            MusterileriListele();
        }

        private void MusterileriListele()
        {
            stok_Takip = new stok_takip_dbEntities();
            grdMusteriler.DataSource = stok_Takip.musteriler.OrderBy(m => m.ad_soyad).ToList();

            if (grdMusteriler.Columns.Contains("satislar"))
                grdMusteriler.Columns.Remove("satislar");

            if (grdMusteriler.Columns.Contains("id"))
            {
                grdMusteriler.Columns["id"].HeaderText = "ID";
                grdMusteriler.Columns["id"].Width = 50;
                grdMusteriler.Columns["id"].ReadOnly = true;
            }

            if (grdMusteriler.Columns.Contains("firma_adi"))
            {
                grdMusteriler.Columns["firma_adi"].HeaderText = "Firma Adı";
                grdMusteriler.Columns["firma_adi"].Width = 150;
            }

            if (grdMusteriler.Columns.Contains("ad_soyad"))
            {
                grdMusteriler.Columns["ad_soyad"].HeaderText = "Ad Soyad";
                grdMusteriler.Columns["ad_soyad"].Width = 200;
            }

            if (grdMusteriler.Columns.Contains("mail"))
            {
                grdMusteriler.Columns["mail"].HeaderText = "E-Posta";
                grdMusteriler.Columns["mail"].Width = 200;
            }

            if (grdMusteriler.Columns.Contains("adres"))
            {
                grdMusteriler.Columns["adres"].HeaderText = "Adres";
                grdMusteriler.Columns["adres"].Width = 250;
            }

            if (grdMusteriler.Columns.Contains("telefon"))
            {
                grdMusteriler.Columns["telefon"].HeaderText = "Telefon";
                grdMusteriler.Columns["telefon"].Width = 110;
            }

            grdMusteriler.ClearSelection();
        }

        private void FormuTemizle()
        {
            txtAdSoyad.Clear();
            txtFirmaAdi.Clear();
            txtMail.Clear();
            txtAdres.Clear();
            mskTelefon.Clear();
            grdMusteriler.ClearSelection();
            seciliMusteriID = 0;
        }

        private bool BilgiGirisKontrol()
        {
            bool kontrol = false;
            if (txtAdSoyad.TextLength <= 2)
            {
                MessageBox.Show("Müşteri adını soyadını giriniz.");
            }
            else if (!mskTelefon.MaskCompleted)
            {
                MessageBox.Show("Telefon numarasını 10 haneli olarak giriniz.");
            }
            else
            {
                kontrol = true;
            }
            return kontrol;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {

        }

        private void btnSil_Click(object sender, EventArgs e)
        {

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void grdTedarikciler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdMusteriler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grdMusteriler.CurrentRow != null)
                {
                    seciliMusteriID = Convert.ToInt32(grdMusteriler.CurrentRow.Cells["id"].Value.ToString());
                    txtFirmaAdi.Text = grdMusteriler.CurrentRow.Cells["firma_adi"].Value.ToString();
                    txtAdSoyad.Text = grdMusteriler.CurrentRow.Cells["ad_soyad"].Value.ToString();
                    txtMail.Text = grdMusteriler.CurrentRow.Cells["mail"].Value.ToString();
                    txtAdres.Text = grdMusteriler.CurrentRow.Cells["adres"].Value.ToString();
                    mskTelefon.Text = grdMusteriler.CurrentRow.Cells["telefon"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}