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
    public partial class frmStokuAzalanUrunler : Form
    {
        public frmStokuAzalanUrunler()
        {
            InitializeComponent();

            this.Load -= frmStokuAzalanUrunler_Load;
            this.Load += frmStokuAzalanUrunler_Load;

            this.txtAraUrunAdi.TextChanged -= txtAraUrunAdi_TextChanged;
            this.txtAraUrunAdi.TextChanged += txtAraUrunAdi_TextChanged;

            this.grdStokuAzalanUrunler.CellClick -= grdStokuAzalanUrunler_CellClick;
            this.grdStokuAzalanUrunler.CellClick += grdStokuAzalanUrunler_CellClick;

            this.btnStokGuncelle.Click -= btnStokGuncelle_Click;
            this.btnStokGuncelle.Click += btnStokGuncelle_Click;

            this.grdStokuAzalanUrunler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.grdStokuAzalanUrunler.MultiSelect = false;
        }

        private void grdStokuAzalanUrunler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void frmStokuAzalanUrunler_Load(object sender, EventArgs e)
        {
            Bilgilerilistele();
        }
        private void Bilgilerilistele()
        {
            using (stok_takip_dbEntities stok_Takip = new stok_takip_dbEntities())
            {
                var sonuc = (from u in stok_Takip.urunler
                             where (u.stok_adedi < 20 && u.urun_adi.Contains(txtAraUrunAdi.Text))
                             select new
                             {
                                 u.urun_kodu,
                                 u.urun_adi,
                                 u.fiyat,
                                 u.kategoriler.kategori_adi,
                                 u.tedarikciler.firma_adi,
                                 u.aciklama,
                                 u.stok_adedi
                             }).ToList();
                grdStokuAzalanUrunler.DataSource = sonuc;
            }
            ConfigureStokAzalanUrunlerColumns(grdStokuAzalanUrunler);
            grdStokuAzalanUrunler.ClearSelection();
        }
        private void txtAraUrunAdi_TextChanged(object sender, EventArgs e)
        {
            Bilgilerilistele();
        }
        private void grdStokuAzalanUrunler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grdStokuAzalanUrunler.CurrentRow != null)
                {
                    nmrStokAdedi.Value = Convert.ToInt32(grdStokuAzalanUrunler.CurrentRow.Cells["stok_adedi"].Value);
                }
            }
            catch
            {
                MessageBox.Show("Hata olustu");
            }
        }
        private void btnStokGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                using (stok_takip_dbEntities stok_Takip = new stok_takip_dbEntities())
                {
                    if (grdStokuAzalanUrunler.SelectedRows.Count > 0)
                    {
                        int urunKodu = Convert.ToInt32(grdStokuAzalanUrunler.CurrentRow.Cells["urun_kodu"].Value);
                        urunler urun = stok_Takip.urunler.Where(u => u.urun_kodu == urunKodu).FirstOrDefault();
                        urun.stok_adedi = Convert.ToInt32(nmrStokAdedi.Value);
                        stok_Takip.SaveChanges();
                        Bilgilerilistele();
                        nmrStokAdedi.ResetText();
                    }
                    else
                        MessageBox.Show("Stok miktarını güncelleyeceğiniz ürünü seçiniz");
                }
            }
            catch
            {
                MessageBox.Show("Stok güncelleme işleminde hata oluştu");
            }
        }

        private void ConfigureStokAzalanUrunlerColumns(DataGridView grid)
        {
            if (grid == null) return;

            if (grid.Columns.Contains("urun_kodu"))
            {
                grid.Columns["urun_kodu"].HeaderText = "Kod";
                grid.Columns["urun_kodu"].Width = 60;
            }
            if (grid.Columns.Contains("Kod"))
            {
                grid.Columns["Kod"].HeaderText = "Kod";
                grid.Columns["Kod"].Width = 60;
            }

            if (grid.Columns.Contains("urun_adi"))
                grid.Columns["urun_adi"].HeaderText = "Ürün Adı";
            if (grid.Columns.Contains("UrunAdi") || grid.Columns.Contains("Urun"))
                grid.Columns["UrunAdi"].HeaderText = "Ürün Adı";

            if (grid.Columns.Contains("stok_adedi"))
            {
                grid.Columns["stok_adedi"].HeaderText = "Stok";
                grid.Columns["stok_adedi"].Width = 70;
            }
            if (grid.Columns.Contains("stoksayisi") || grid.Columns.Contains("Miktar"))
            {
                if (grid.Columns.Contains("Miktar")) grid.Columns["Miktar"].HeaderText = "Stok";
                if (grid.Columns.Contains("stoksayisi")) grid.Columns["stoksayisi"].HeaderText = "Stok";
            }

            if (grid.Columns.Contains("kritik_seviye"))
            {
                grid.Columns["kritik_seviye"].HeaderText = "Kritik Seviye";
                grid.Columns["kritik_seviye"].Width = 90;
            }

            if (grid.Columns.Contains("fiyat"))
            {
                grid.Columns["fiyat"].HeaderText = "Birim Fiyat";
                grid.Columns["fiyat"].DefaultCellStyle.Format = "0.00";
            }

            grid.ClearSelection();
        }
    }
}
