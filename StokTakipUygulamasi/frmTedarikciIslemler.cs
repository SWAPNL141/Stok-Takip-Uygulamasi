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
    public partial class frmTedarikciIslemler : Form
    {
        stok_takip_dbEntities stok_Takip;
        int seciliTedarikciID = 0;

        public frmTedarikciIslemler()
        {
            InitializeComponent();

            this.Load -= frmTedarikciIslemleri_Load;
            this.Load += frmTedarikciIslemleri_Load;

            this.grdTedarikciler.CellClick -= grdTedarikciler_CellClick;
            this.grdTedarikciler.CellClick += grdTedarikciler_CellClick;

            this.btnYeniKayit.Click -= btnYeniKayit_Click;
            this.btnYeniKayit.Click += btnYeniKayit_Click;

            this.btnEkle.Click -= btnEkle_Click;
            this.btnEkle.Click += btnEkle_Click;

            this.btnSil.Click -= btnSil_Click;
            this.btnSil.Click += btnSil_Click;

            this.btnGuncelle.Click -= btnGuncelle_Click;
            this.btnGuncelle.Click += btnGuncelle_Click;

            this.grdTedarikciler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.grdTedarikciler.MultiSelect = false;
        }

        private void frmTedarikciIslemleri_Load(object sender, EventArgs e)
        {
            Tedarikcilerilistele();
        }

        private void Tedarikcilerilistele() 
        {
            stok_Takip = new stok_takip_dbEntities();
            grdTedarikciler.DataSource = stok_Takip.tedarikciler.OrderBy(t => t.firma_adi).ToList();

            ConfigureTedarikcilerColumns(grdTedarikciler);

            grdTedarikciler.ClearSelection();
        }

        private void ConfigureTedarikcilerColumns(DataGridView grid)
        {
            if (grid == null) return;

            if (grid.Columns.Contains("id"))
            {
                grid.Columns["id"].HeaderText = "ID";
                grid.Columns["id"].Width = 50;
                grid.Columns["id"].ReadOnly = true;
            }

            if (grid.Columns.Contains("firma_adi"))
            {
                grid.Columns["firma_adi"].HeaderText = "Firma Adı";
                grid.Columns["firma_adi"].Width = 180;
            }
            if (grid.Columns.Contains("ad") || grid.Columns.Contains("firma"))
            {
                if (grid.Columns.Contains("ad")) grid.Columns["ad"].HeaderText = "Ad";
                if (grid.Columns.Contains("firma")) grid.Columns["firma"].HeaderText = "Firma";
            }

            if (grid.Columns.Contains("telefon"))
            {
                grid.Columns["telefon"].HeaderText = "Telefon";
                grid.Columns["telefon"].Width = 120;
            }

            if (grid.Columns.Contains("mail"))
            {
                grid.Columns["mail"].HeaderText = "E-Posta";
                grid.Columns["mail"].Width = 200;
            }

            if (grid.Columns.Contains("adres"))
            {
                grid.Columns["adres"].HeaderText = "Adres";
                grid.Columns["adres"].Width = 240;
            }

            if (grid.Columns.Contains("satislar"))
            {
                grid.Columns.Remove("satislar");
            }

            grid.ClearSelection();
        }

        private void FormuTemizle() 
        {
            txtFirmaAdi.Clear();
            txtYetkiliAdi.Clear();
            txtMail.Clear();
            txtAdres.Clear();
            mskTelefon.Text = "";
            grdTedarikciler.ClearSelection();
            seciliTedarikciID = 0;
            txtFirmaAdi.Focus();
        }

        private bool BilgiGirisKontrol()
        {

            bool kontrol = false;
            if (txtFirmaAdi.TextLength < 2)
            {
                MessageBox.Show("Firma adını giriniz.");
            }
            else if (txtYetkiliAdi.TextLength < 2)
            {
                MessageBox.Show("Yetkili adını giriniz.");
            }
            else if (!mskTelefon.MaskCompleted)
            {
                MessageBox.Show("Telefon numarasını 10 haneli olarak giriniz.");
            }
            else if (txtMail.TextLength < 5)
            {
                MessageBox.Show("Mail adresini giriniz");
            }
            else if (txtAdres.TextLength <= 2)
            {
                MessageBox.Show("Firma adresini giriniz.");
            }
            else
            {
                kontrol = true;
            }
            return kontrol;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {

                seciliTedarikciID = 0;

                if (BilgiGirisKontrol())
                {
                    stok_Takip = new stok_takip_dbEntities();
                    tedarikciler tedarikci = new tedarikciler();
                    tedarikci.firma_adi = txtFirmaAdi.Text;
                    tedarikci.yetkili_adi_soyadi = txtYetkiliAdi.Text;
                    tedarikci.telefon = mskTelefon.Text;
                    tedarikci.mail = txtMail.Text;
                    tedarikci.adres = txtAdres.Text;
                    stok_Takip.tedarikciler.Add(tedarikci);
                    stok_Takip.SaveChanges();
                    Tedarikcilerilistele();
                    FormuTemizle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kayıt Hatası");
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (seciliTedarikciID != 0)
                {
                    var tedarikcikontrol = stok_Takip.urunler.
                        Where(u => u.tedarikci_id.ToString().Contains(seciliTedarikciID.ToString())).ToList();
                    if (tedarikcikontrol.Count() > 0)
                        MessageBox.Show("Bu kayıt silinemez. Silmek istediğiniz tedarikçiye ait ürünler mevcut.");
                    else
                    {
                        stok_Takip = new stok_takip_dbEntities();
                        tedarikciler silinecekTedarikci = stok_Takip.tedarikciler.
                            Where(t => t.id == seciliTedarikciID).First();
                        stok_Takip.tedarikciler.Remove(silinecekTedarikci);
                        stok_Takip.SaveChanges();
                        Tedarikcilerilistele();
                        FormuTemizle();
                    }
                }
                else
                    MessageBox.Show("Silinecek kaydı seçiniz.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Silme İşleminde Hata");
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (seciliTedarikciID != 0)
                {
                    if (BilgiGirisKontrol())
                    {
                        stok_Takip = new stok_takip_dbEntities();
                        tedarikciler guncellenecekTedarikci = stok_Takip.tedarikciler.
                        Where(t => t.id == seciliTedarikciID).First();
                        guncellenecekTedarikci.firma_adi = txtFirmaAdi.Text;
                        guncellenecekTedarikci.yetkili_adi_soyadi = txtYetkiliAdi.Text;
                        guncellenecekTedarikci.telefon = mskTelefon.Text;
                        guncellenecekTedarikci.mail = txtMail.Text;
                        guncellenecekTedarikci.adres = txtAdres.Text;
                        stok_Takip.SaveChanges();
                        Tedarikcilerilistele();
                        FormuTemizle();
                    }
                }
                else
                    MessageBox.Show("Güncellenecek kaydı seçiniz."); 
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Güncelleme Hatası");
            }
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
        private void grdTedarikciler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grdTedarikciler.CurrentRow == null) return;

                var row = grdTedarikciler.CurrentRow;
                object idVal = row.Cells["id"].Value;
                int.TryParse(Convert.ToString(idVal), out seciliTedarikciID);

                txtFirmaAdi.Text = Convert.ToString(row.Cells["firma_adi"].Value);
                txtYetkiliAdi.Text = Convert.ToString(row.Cells["yetkili_adi_soyadi"].Value);
                txtMail.Text = Convert.ToString(row.Cells["mail"].Value);
                txtAdres.Text = Convert.ToString(row.Cells["adres"].Value);
                mskTelefon.Text = Convert.ToString(row.Cells["telefon"].Value);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnYeniKayit_Click(object sender, EventArgs e)
        {
            FormuTemizle();
            txtFirmaAdi.Focus();
        }
    }
}
