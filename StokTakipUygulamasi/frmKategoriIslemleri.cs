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
    public partial class frmKategoriIslemleri : Form
    {
        stok_takip_dbEntities stok_Takip;
        int secilikategoriId = 0;
        public frmKategoriIslemleri()
        {
            InitializeComponent();

            this.Load -= frmkategoriIslemleri_Load;
            this.Load += frmkategoriIslemleri_Load;

            this.grdKategoriler.CellClick -= grdKategoriler_CellClick;
            this.grdKategoriler.CellClick += grdKategoriler_CellClick;

            this.grdKategoriler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.grdKategoriler.MultiSelect = false;
        }

        private void Bilgilerilistele() {
            using (var stok_Takip = new stok_takip_dbEntities())
            {
                var list = stok_Takip.kategoriler.ToList();
                grdKategoriler.DataSource = list;

                // Navigasyon / ilişki sütununu kaldır
                if (grdKategoriler.Columns.Contains("urunler"))
                    grdKategoriler.Columns.Remove("urunler");

                // Kolon başlıklarını ve görünümü DataSource atandıktan sonra kesinleştir
                if (grdKategoriler.Columns.Contains("kategori_id"))
                {
                    var c = grdKategoriler.Columns["kategori_id"];
                    c.HeaderText = "ID";
                    c.Width = 65;
                    c.DisplayIndex = 0;
                    c.ReadOnly = true;
                }

                if (grdKategoriler.Columns.Contains("kategori_adi"))
                {
                    var c = grdKategoriler.Columns["kategori_adi"];
                    c.HeaderText = "Kategori Adı";
                    c.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    c.DisplayIndex = 1;
                    c.ReadOnly = true;
                }

                // Genel görünüm ayarları (grdSatilacakUrunler ile uyumlu)
                grdKategoriler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grdKategoriler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grdKategoriler.MultiSelect = false;
                grdKategoriler.RowHeadersVisible = false;
                grdKategoriler.EnableHeadersVisualStyles = false;
                grdKategoriler.GridColor = System.Drawing.Color.LightGray;

                grdKategoriler.ClearSelection();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (secilikategoriId == 0)
                {
                    if (!string.IsNullOrEmpty(txtKategoriAdi.Text))
                    {
                        stok_Takip = new stok_takip_dbEntities();
                        kategoriler kategori = new kategoriler();
                        kategori.kategori_adi = txtKategoriAdi.Text;
                        stok_Takip.kategoriler.Add(kategori);
                        stok_Takip.SaveChanges();
                        txtKategoriAdi.Clear();
                        Bilgilerilistele();
                    }
                    else
                    {
                        MessageBox.Show("Kategori adını giriniz.");
                    }
                }
                else
                {
                    MessageBox.Show("Yeni kategori eklemek için 'Yeni Kayat' butonuna tıklayınız");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kayıt Hatası");
            }
        }

        private void grdKategoriler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void frmkategoriIslemleri_Load(object sender, EventArgs e)
        {
            Bilgilerilistele();

        }

        private void btnYeniKayit_Click(object sender, EventArgs e)
        {
            secilikategoriId = 0;
            txtKategoriAdi.Clear();
        }
        private void grdKategoriler_CellClick(object sender, DataGridViewCellEventArgs e) {
            try
            {
                if (grdKategoriler.CurrentRow != null)
                {
                    secilikategoriId = Convert.ToInt32(grdKategoriler.CurrentRow.Cells["kategori_id"].Value.ToString());
                    txtKategoriAdi.Text = grdKategoriler.CurrentRow.Cells["kategori_adi"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Olustu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (secilikategoriId != 0)
                {
                    var kategorikontrol = stok_Takip.urunler.
                    Where(u => u.kategori_id.ToString().Contains(secilikategoriId.ToString())).ToList();
                    if (kategorikontrol.Count() > 0)
                        MessageBox.Show("Bu kayıt silinemez. Silmek istediğiniz kategoriye ait ürünler mevcut.");
                    else
                    {
                        stok_Takip = new stok_takip_dbEntities();
                        kategoriler silinecekkategori = stok_Takip.kategoriler.
                        Where(k => k.kategori_id == secilikategoriId).First();
                        stok_Takip.kategoriler.Remove(silinecekkategori);
                        stok_Takip.SaveChanges();
                        Bilgilerilistele();
                        txtKategoriAdi.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Silinecek kaydı seçiniz.");
                }
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
                if (secilikategoriId != 0)
                {
                    if (!string.IsNullOrEmpty(txtKategoriAdi.Text))
                    {
                        stok_Takip = new stok_takip_dbEntities();
                        kategoriler guncellenecekKategori = stok_Takip.kategoriler.
                        Where(k => k.kategori_id == secilikategoriId).First();
                        guncellenecekKategori.kategori_adi = txtKategoriAdi.Text;
                        stok_Takip.SaveChanges();
                        Bilgilerilistele();
                        txtKategoriAdi.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Kategori adını giriniz.");
                    }
                }
                else
                {
                    MessageBox.Show("Güncellenecek kaydı seçiniz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Güncelleme Hatası");
            }
        }
    }
}
