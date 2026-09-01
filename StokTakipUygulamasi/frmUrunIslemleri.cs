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
    public partial class frmUrunIslemleri : Form
    {
        public frmUrunIslemleri()
        {
            InitializeComponent();

            this.Load -= frmUrunIslemleri_Load;
            this.Load += frmUrunIslemleri_Load;

            this.grdUrunler.CellClick -= grdUrunler_CellClick;
            this.grdUrunler.CellClick += grdUrunler_CellClick;

            this.btnYeniKayit.Click -= btnYeniKayit_Click;
            this.btnYeniKayit.Click += btnYeniKayit_Click;

            // Eğer formunuzda bu isimde bir buton varsa, stok azalanları açmak için handler atayın
            this.btnStoguAzalanUrunler.Click -= btnStoguAzalanUrunler_Click;
            this.btnStoguAzalanUrunler.Click += btnStoguAzalanUrunler_Click;

            this.grdUrunler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.grdUrunler.MultiSelect = false;
        }

        stok_takip_dbEntities stok_Takip;
        int seciliUrunID = 0;

        private void frmUrunIslemleri_Load(object sender, EventArgs e) 
        {
            Bilgilerilistele();
        }

        private void Bilgilerilistele()
        {
                stok_Takip = new stok_takip_dbEntities();
                var sonuc = (from u in stok_Takip.urunler
                join k in stok_Takip.kategoriler on u.kategori_id equals k.kategori_id
                join t in stok_Takip.tedarikciler on u.tedarikci_id equals t.id
                select new
                {
                    u.urun_kodu,
                    u.urun_adi,
                    u.stok_adedi,
                    u.fiyat,
                    k.kategori_adi,
                    t.firma_adi,
                    u.aciklama
                }).ToList();
                grdUrunler.DataSource = sonuc;

                ConfigureUrunlerColumns(grdUrunler);

                if (grdUrunler.Columns.Contains("urun_kodu"))
                    grdUrunler.Columns["urun_kodu"].Width = 60;

                grdUrunler.ClearSelection();
                cmbKategori.DataSource = stok_Takip.kategoriler.OrderBy(k => k.kategori_adi).ToList();
                cmbKategori.DisplayMember = "kategori_adi";
                cmbKategori.ValueMember = "kategori_id";
                cmbTedarikci.DataSource = stok_Takip.tedarikciler.OrderBy(t => t.firma_adi).ToList();
                cmbTedarikci.DisplayMember = "firma_adi";
                cmbTedarikci.ValueMember = "id";
            }

        private bool BilgiGirisKontrol()
        {
            decimal kont;
            bool kontrol = false;
            if (string.IsNullOrEmpty(txtUrunAdi.Text))
            {
                MessageBox.Show("Ürün adını giriniz");
            }
            else if (!Decimal.TryParse(txtFiyat.Text, out kont))
            {
                MessageBox.Show("Ürün fiyatını giriniz");
            }
            else
            {
                kontrol = true;
            }
            return kontrol;
        }

        private void FormuTemizle()
        {
            txtUrunAdi.Clear();
            txtAciklama.Clear();
            txtFiyat.Clear();
            nmrStok.Value = 0;
            grdUrunler.ClearSelection();
            seciliUrunID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (seciliUrunID == 0)
                {
                    if (BilgiGirisKontrol())
                    {
                        stok_Takip = new stok_takip_dbEntities();
                        urunler urun = new urunler();
                        urun.urun_adi = txtUrunAdi.Text;
                        urun.kategori_id = Convert.ToInt32(cmbKategori.SelectedValue);
                        urun.tedarikci_id = Convert.ToInt32(cmbTedarikci.SelectedValue);
                        urun.stok_adedi = Convert.ToInt32(nmrStok.Text);
                        urun.fiyat = Convert.ToDecimal(txtFiyat.Text);
                        urun.aciklama = txtAciklama.Text;
                        stok_Takip.urunler.Add(urun);
                        stok_Takip.SaveChanges();
                        Bilgilerilistele();
                        FormuTemizle();
                    }
                }
                else
                {
                    MessageBox.Show("Yeni ürün eklemek için 'Yeni Kayıt' butonuna tıklayınız");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kayıt Hatası");
            }
        }

        private void grdUrunler_CellClick(object sender, DataGridViewCellEventArgs e) {
            try
            {
                if (grdUrunler.CurrentRow != null)
                {
                    seciliUrunID = Convert.ToInt32(grdUrunler.CurrentRow.Cells["urun_kodu"].Value.ToString());
                    txtUrunAdi.Text = grdUrunler.CurrentRow.Cells["urun_adi"].Value.ToString();
                    txtAciklama.Text = grdUrunler.CurrentRow.Cells["aciklama"].Value.ToString();
                    txtFiyat.Text = grdUrunler.CurrentRow.Cells["fiyat"].Value.ToString();
                    nmrStok.Text = grdUrunler.CurrentRow.Cells["stok_adedi"].Value.ToString();
                    cmbKategori.Text = grdUrunler.CurrentRow.Cells["kategori_adi"].Value.ToString();
                    cmbTedarikci.Text = grdUrunler.CurrentRow.Cells["firma_adi"].Value.ToString();
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
                if (seciliUrunID != 0)
                {
                    stok_Takip = new stok_takip_dbEntities();
                    urunler silinecekUrun = stok_Takip.urunler.
                        Where(u => u.urun_kodu == seciliUrunID).First();
                    stok_Takip.urunler.Remove(silinecekUrun);
                    stok_Takip.SaveChanges();
                    Bilgilerilistele();
                    FormuTemizle();
                }
                else
                {
                    MessageBox.Show("Silinecek ürünü seçiniz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Silme İşleminde Hata");
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try {
                if (seciliUrunID != 0)
                {
                    if (BilgiGirisKontrol())
                    {
                        stok_Takip = new stok_takip_dbEntities();
                        urunler guncellenecekUrun = stok_Takip.urunler.Where(u => u.urun_kodu == seciliUrunID).First();
                        guncellenecekUrun.urun_adi = txtUrunAdi.Text;
                        guncellenecekUrun.kategori_id = Convert.ToInt32(cmbKategori.SelectedValue);
                        guncellenecekUrun.tedarikci_id = Convert.ToInt32(cmbTedarikci.SelectedValue);
                        guncellenecekUrun.stok_adedi = (int)nmrStok.Value;
                        guncellenecekUrun.fiyat = Convert.ToDecimal(txtFiyat.Text);
                        guncellenecekUrun.aciklama = txtAciklama.Text;
                        stok_Takip.SaveChanges();
                        Bilgilerilistele();
                        FormuTemizle();
                    }
                }
                else
                {
                    MessageBox.Show("Güncellenecek ürünü seçiniz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Güncelleme Hatası");
            }
        }

        private void btnKategori_Click(object sender, EventArgs e)
        {
            frmKategoriIslemleri frmKategori = new frmKategoriIslemleri();
            DialogResult result = frmKategori.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                Bilgilerilistele();
            }
        }

        private void txtAraUrunAdi_TextChanged(object sender, EventArgs e)
        {
            if (stok_Takip == null)
                stok_Takip = new stok_takip_dbEntities();

            var sonuc = (from u in stok_Takip.urunler
                         join k in stok_Takip.kategoriler on u.kategori_id equals k.kategori_id
                         join t in stok_Takip.tedarikciler on u.tedarikci_id equals t.id
                         where u.urun_adi.Contains(txtAraUrunAdi.Text)
                         select new
                         {
                             u.urun_kodu,
                             u.urun_adi,
                             u.stok_adedi,
                             u.fiyat,
                             k.kategori_adi,
                             t.firma_adi,
                             u.aciklama
                         }).ToList();
            grdUrunler.DataSource = sonuc;
        }

        private void btnYeniKayit_Click(object sender, EventArgs e)
        {
            FormuTemizle();
            txtUrunAdi.Focus();
        }

        private void ConfigureUrunlerColumns(DataGridView grid)
        {
            if (grid == null) return;

            if (grid.Columns.Contains("urun_kodu"))
            {
                grid.Columns["urun_kodu"].HeaderText = "Kod";
                grid.Columns["urun_kodu"].Width = 60;
            }
            if (grid.Columns.Contains("urun_adi"))
            {
                grid.Columns["urun_adi"].HeaderText = "Ürün Adı";
                grid.Columns["urun_adi"].Width = 230;
            }
            if (grid.Columns.Contains("stok_adedi"))
            {
                grid.Columns["stok_adedi"].HeaderText = "Stok";
                grid.Columns["stok_adedi"].Width = 70;
            }
            if (grid.Columns.Contains("fiyat"))
            {
                grid.Columns["fiyat"].HeaderText = "Birim Fiyat";
                grid.Columns["fiyat"].DefaultCellStyle.Format = "0.00";
                grid.Columns["fiyat"].Width = 80;
            }
            if (grid.Columns.Contains("kategori_adi"))
            {
                grid.Columns["kategori_adi"].HeaderText = "Kategori";
                grid.Columns["kategori_adi"].Width = 130;
            }
            if (grid.Columns.Contains("firma_adi"))
            {
                grid.Columns["firma_adi"].HeaderText = "Tedarikçi";
                grid.Columns["firma_adi"].Width = 130;
            }
            if (grid.Columns.Contains("aciklama"))
            {
                grid.Columns["aciklama"].HeaderText = "Açıklama";
                grid.Columns["aciklama"].Width = 200;
            }

            grid.ClearSelection();
        }

        private void btnStoguAzalanUrunler_Click(object sender, EventArgs e)
        {
            using (var frm = new frmStokuAzalanUrunler())
            {
                frm.ShowDialog();
            }
        }
    }
}
