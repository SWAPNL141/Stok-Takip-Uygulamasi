using System;
using System.Linq;
using System.Windows.Forms;

namespace StokTakipUygulamasi
{
    public partial class frmSatisDetaylari : Form
    {
        int seciliSatisId = 0;

        public frmSatisDetaylari()
        {
            InitializeComponent();

            this.Load -= frmSatisDetaylari_Load;
            this.Load += frmSatisDetaylari_Load;

            this.grdSatislar.CellClick -= grdSatislar_CellClick;
            this.grdSatislar.CellClick += grdSatislar_CellClick;

            this.grdSatislar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.grdSatislar.MultiSelect = false;

            this.grdSatilanUrunler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.grdSatilanUrunler.MultiSelect = false;
        }

        private void frmSatisDetaylari_Load(object sender, EventArgs e)
        {
            Bilgilerilistele();
        }

        private void Bilgilerilistele()
        {
            try
            {
                using (var stok_Takip = new stok_takip_dbEntities())
                {
                    var satisSonuc = (from s in stok_Takip.satislar
                                      join m in stok_Takip.musteriler on s.musteri_id equals m.id
                                      join p in stok_Takip.personeller on s.personel_id equals p.id
                                      orderby s.satis_tarih descending
                                      select new
                                      {
                                          s.satis_id,
                                          Musteri = m.ad_soyad,
                                          PersonelAd = p.ad,
                                          PersonelSoyad = p.soyad,
                                          SatisTarihi = s.satis_tarih,
                                          SatisNotu = s.notlar
                                      }).ToList();

                    grdSatislar.DataSource = satisSonuc;
                }

                // Yeni: ConfigureSatislarColumns metodunu çağır
                ConfigureSatislarColumns(grdSatislar);

                grdSatislar.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSatisIptal_Click(object sender, EventArgs e)
        {
            var cevap = MessageBox.Show("Seçilen satışı iptal etmek istediğinizden emin misiniz?", "SATIŞ İPTAL İŞLEMİ", MessageBoxButtons.YesNo);
            if (cevap == DialogResult.No)
                return
                ;
            try
            {
                if (grdSatislar.CurrentRow == null) return;

                if (seciliSatisId == 0)
                {
                    object val = grdSatislar.CurrentRow.Cells[0].Value;
                    int.TryParse(Convert.ToString(val), out seciliSatisId);
                }

                if (seciliSatisId == 0)
                {
                    MessageBox.Show("İptal edilecek satış seçili değil.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var stok_Takip = new stok_takip_dbEntities())
                {
                    var iptalSatislistesi = stok_Takip.satis_detaylari
                        .Where(d => d.satis_id == seciliSatisId)
                        .ToList();

                    foreach (var det in iptalSatislistesi)
                    {
                        var urun = stok_Takip.urunler.FirstOrDefault(u => u.urun_kodu == det.urun_kodu);
                        if (urun != null)
                        {
                            urun.stok_adedi = (urun.stok_adedi ?? 0) + det.miktar;
                        }
                    }

                    stok_Takip.satis_detaylari.RemoveRange(iptalSatislistesi);

                    var iptalEdilecekSatis = stok_Takip.satislar.FirstOrDefault(s => s.satis_id == seciliSatisId);
                    if (iptalEdilecekSatis != null)
                    {
                        stok_Takip.satislar.Remove(iptalEdilecekSatis);
                    }

                    stok_Takip.SaveChanges();
                }

                MessageBox.Show("Satış iptal edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Bilgilerilistele();
                grdSatilanUrunler.DataSource = null;
                seciliSatisId = 0;
                txtSatisToplam.Text = "0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Hesapla()
        {
            try
            {
                if (grdSatilanUrunler != null && grdSatilanUrunler.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow))
                {
                    decimal genelToplam = grdSatilanUrunler.Rows.Cast<DataGridViewRow>()
                        .Where(r => !r.IsNewRow)
                        .Sum(t =>
                        {
                            object val = t.Cells["Toplam"]?.Value;
                            decimal d;
                            return decimal.TryParse(Convert.ToString(val), out d) ? d : 0m;
                        });

                    txtSatisToplam.Text = genelToplam.ToString("0.00");
                }
                else
                {
                    txtSatisToplam.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdSatislar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grdSatislar.CurrentRow == null) return;

                object cellVal = grdSatislar.CurrentRow.Cells[0].Value;
                if (!int.TryParse(Convert.ToString(cellVal), out seciliSatisId))
                {
                    seciliSatisId = 0;
                    grdSatilanUrunler.DataSource = null;
                    return;
                }

                using (var stok_Takip = new stok_takip_dbEntities())
                {
                    var satisSonuc = (from sd in stok_Takip.satis_detaylari
                                      join u in stok_Takip.urunler on sd.urun_kodu equals u.urun_kodu
                                      where sd.satis_id == seciliSatisId
                                      select new
                                      {
                                          SatisId = sd.satis_id,
                                          UrunKodu = sd.urun_kodu,
                                          UrunAdi = u.urun_adi,
                                          Miktar = sd.miktar,
                                          Fiyat = sd.fiyat,
                                          Toplam = (sd.miktar * sd.fiyat)
                                      }).ToList();

                    grdSatilanUrunler.DataSource = satisSonuc;
                }

                ConfigureSatilanUrunlerColumns(grdSatilanUrunler);

                Hesapla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureSatilanUrunlerColumns(DataGridView grid)
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
            if (grid.Columns.Contains("urunAdi"))
                grid.Columns["urunAdi"].HeaderText = "Ürün Adı";

            if (grid.Columns.Contains("miktar"))
                grid.Columns["miktar"].HeaderText = "Miktar";
            if (grid.Columns.Contains("Miktar"))
                grid.Columns["Miktar"].HeaderText = "Miktar";

            if (grid.Columns.Contains("fiyat"))
            {
                grid.Columns["fiyat"].HeaderText = "Birim Fiyat";
                grid.Columns["fiyat"].DefaultCellStyle.Format = "0.00";
            }
            if (grid.Columns.Contains("BirimFiyat"))
            {
                grid.Columns["BirimFiyat"].HeaderText = "Birim Fiyat";
                grid.Columns["BirimFiyat"].DefaultCellStyle.Format = "0.00";
            }

            if (grid.Columns.Contains("toplam"))
            {
                grid.Columns["toplam"].HeaderText = "Toplam";
                grid.Columns["toplam"].DefaultCellStyle.Format = "0.00";
            }
            if (grid.Columns.Contains("Toplam"))
            {
                grid.Columns["Toplam"].HeaderText = "Toplam";
                grid.Columns["Toplam"].DefaultCellStyle.Format = "0.00";
            }

            grid.ClearSelection();
        }

        private void ConfigureSatislarColumns(DataGridView grid)
        {
            if (grid == null) return;

            // Projection'da kullanılan isimlere göre başlık ayarları
            if (grid.Columns.Contains("satis_id"))
            {
                grid.Columns["satis_id"].HeaderText = "Satış ID";
                grid.Columns["satis_id"].Width = 60;
            }

            if (grid.Columns.Contains("Musteri"))
                grid.Columns["Musteri"].HeaderText = "Müşteri";

            if (grid.Columns.Contains("PersonelAd"))
                grid.Columns["PersonelAd"].HeaderText = "Personel Ad";

            if (grid.Columns.Contains("PersonelSoyad"))
                grid.Columns["PersonelSoyad"].HeaderText = "Personel Soyad";

            if (grid.Columns.Contains("SatisTarihi"))
            {
                grid.Columns["SatisTarihi"].HeaderText = "Tarih";
                grid.Columns["SatisTarihi"].DefaultCellStyle.Format = "g";
                grid.Columns["SatisTarihi"].Width = 140;
            }

            if (grid.Columns.Contains("SatisNotu"))
                grid.Columns["SatisNotu"].HeaderText = "Notlar";

            // Geriye dönük / alternatif kolon isimleri için güvenlik kontrolleri
            if (grid.Columns.Contains("musteri_id"))
                grid.Columns["musteri_id"].HeaderText = "Müşteri ID";
            if (grid.Columns.Contains("personel_id"))
                grid.Columns["personel_id"].HeaderText = "Personel ID";
            if (grid.Columns.Contains("satis_tarih"))
            {
                grid.Columns["satis_tarih"].HeaderText = "Tarih";
                grid.Columns["satis_tarih"].DefaultCellStyle.Format = "g";
                grid.Columns["satis_tarih"].Width = 140;
            }
            if (grid.Columns.Contains("notlar"))
                grid.Columns["notlar"].HeaderText = "Notlar";

            grid.ClearSelection();
        }
    }
}