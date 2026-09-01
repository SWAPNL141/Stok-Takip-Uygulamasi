using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Reflection;

namespace StokTakipUygulamasi
{
    public partial class frmAnaSayfa : Form
    {
        public frmAnaSayfa()
        {
            InitializeComponent();

            this.Load -= frmAnaSayfa_Load;
            this.Load += frmAnaSayfa_Load;

            this.btnMusteriSec.Click -= btnMusteriSec_Click;
            this.btnMusteriSec.Click += btnMusteriSec_Click;

            this.btnEkle.Click -= btnEkle_Click;
            this.btnEkle.Click += btnEkle_Click;

            this.btnSil.Click -= btnSil_Click;
            this.btnSil.Click += btnSil_Click;

            this.btnSatisYap.Click -= btnSatisYap_Click;
            this.btnSatisYap.Click += btnSatisYap_Click;

            this.menuTedarikci.Click -= menuTedarikci_Click;
            this.menuTedarikci.Click += menuTedarikci_Click;

            this.menuUrun.Click -= menuUrun_Click;
            this.menuUrun.Click += menuUrun_Click;

            this.menuSatis.Click -= menuSatis_Click;
            this.menuSatis.Click += menuSatis_Click;

            this.menuMusteri.Click -= menuMusteri_Click;
            this.menuMusteri.Click += menuMusteri_Click;

            this.menuPersonel.Click -= menuPersonel_Click;
            this.menuPersonel.Click += menuPersonel_Click;

            grdUrunListesi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdUrunListesi.MultiSelect = false;
            grdSatilacakUrunler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdSatilacakUrunler.MultiSelect = false;

            this.grdSatilacakUrunler.CellEndEdit -= grdSatilacakUrunler_CellEndEdit;
            this.grdSatilacakUrunler.CellEndEdit += grdSatilacakUrunler_CellEndEdit;
        }

        int seciliMusteriId = 0;
        public int PersonelId = 0;
        public string personelAdSoyad = "";
        public string personelYetki = "";

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow rowToRemove = null;

                if (grdSatilacakUrunler.SelectedRows.Count > 0)
                {
                    rowToRemove = grdSatilacakUrunler.SelectedRows
                        .Cast<DataGridViewRow>()
                        .FirstOrDefault(r => !r.IsNewRow);
                }

                if (rowToRemove == null && grdSatilacakUrunler.CurrentCell != null)
                {
                    var owning = grdSatilacakUrunler.CurrentCell.OwningRow;
                    if (owning != null && !owning.IsNewRow) rowToRemove = owning;
                }

                if (rowToRemove == null && grdSatilacakUrunler.SelectedCells.Count > 0)
                {
                    var cell = grdSatilacakUrunler.SelectedCells[0];
                    var owning = cell.OwningRow;
                    if (owning != null && !owning.IsNewRow) rowToRemove = owning;
                }

                if (rowToRemove != null)
                {
                    DataGridViewCell miktarCell = null;
                    if (rowToRemove.DataGridView.Columns.Contains("Miktar")) miktarCell = rowToRemove.Cells["Miktar"];
                    else if (rowToRemove.DataGridView.Columns.Contains("miktar")) miktarCell = rowToRemove.Cells["miktar"];
                    else if (rowToRemove.Cells.Count > 2) miktarCell = rowToRemove.Cells[2];

                    int miktar = 0;
                    if (miktarCell != null && int.TryParse(Convert.ToString(miktarCell.Value), out miktar) && miktar > 1)
                    {
                        int yeniMiktar = miktar - 1;
                        if (rowToRemove.DataBoundItem is System.Data.DataRowView drv)
                        {
                            drv.Row[miktarCell.ColumnIndex] = yeniMiktar;
                        }
                        else
                        {
                            miktarCell.Value = yeniMiktar;
                        }

                        if (rowToRemove.Cells.Count > 3)
                        {
                            decimal birim = 0m;
                            decimal.TryParse(Convert.ToString(rowToRemove.Cells[3].Value), out birim);
                            if (rowToRemove.Cells.Count > 4) rowToRemove.Cells[4].Value = yeniMiktar * birim;
                        }

                        Hesapla();
                        MessageBox.Show("Ürün miktarı 1 azaltıldı.");
                        return;
                    }

                    var dt = grdSatilacakUrunler.DataSource as System.Data.DataTable;
                    if (dt != null && rowToRemove.DataBoundItem is System.Data.DataRowView drvRow)
                    {
                        drvRow.Row.Delete();
                    }
                    else
                    {
                        grdSatilacakUrunler.Rows.Remove(rowToRemove);
                    }

                    Hesapla();
                    MessageBox.Show("Ürün listeden çıkarıldı");
                    return;
                }

                MessageBox.Show("Silinecek bir satır seçiniz.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TryGetIntFromCell(DataGridViewCell cell, out int value)
        {
            value = 0;
            if (cell?.Value == null) return false;
            return int.TryParse(Convert.ToString(cell.Value), out value);
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdUrunListesi.CurrentRow == null)
                {
                    MessageBox.Show("Lütfen listeden bir ürün seçiniz.");
                    return;
                }

                int urunKodu = 0;
                var dataItem = grdUrunListesi.CurrentRow.DataBoundItem;
                if (dataItem != null)
                {
                    var prop = dataItem.GetType().GetProperty("urun_kodu");
                    if (prop != null)
                        int.TryParse(Convert.ToString(prop.GetValue(dataItem)), out urunKodu);
                }
                if (urunKodu == 0)
                {
                    TryGetIntFromCell(grdUrunListesi.CurrentRow.Cells["urun_kodu"] ?? grdUrunListesi.CurrentRow.Cells[0], out urunKodu);
                }

                if (urunKodu <= 0)
                {
                    MessageBox.Show("Geçersiz ürün kodu.");
                    return;
                }

                int kodIndex = GetKodColumnIndex();

                foreach (DataGridViewRow row in grdSatilacakUrunler.Rows)
                {
                    if (row.IsNewRow) continue;
                    var cellVal = row.Cells.Count > kodIndex ? row.Cells[kodIndex].Value : null;
                    if (cellVal == null) continue;
                    if (int.TryParse(Convert.ToString(cellVal), out int kod) && kod == urunKodu)
                    {
                        int mevcutMiktar = 1;
                        if (row.Cells.Count > 2)
                            int.TryParse(Convert.ToString(row.Cells[2].Value), out mevcutMiktar);
                        int yeniMiktar = mevcutMiktar <= 0 ? 1 : mevcutMiktar + 1;
                        if (row.Cells.Count > 2) row.Cells[2].Value = yeniMiktar;

                        decimal mevcutFiyat = 0m;
                        if (row.Cells.Count > 3)
                            decimal.TryParse(Convert.ToString(row.Cells[3].Value), out mevcutFiyat);

                        if (row.Cells.Count > 4) row.Cells[4].Value = (yeniMiktar * mevcutFiyat);

                        Hesapla();
                        MessageBox.Show("Ürün listeye eklendi");
                        return;
                    }
                }

                string urunAdi = "";
                decimal birimFiyat = 0m;
                if (dataItem != null)
                {
                    var pAdi = dataItem.GetType().GetProperty("urun_adi");
                    var pFiyat = dataItem.GetType().GetProperty("fiyat");
                    urunAdi = pAdi != null ? Convert.ToString(pAdi.GetValue(dataItem)) : Convert.ToString(grdUrunListesi.CurrentRow.Cells["urun_adi"]?.Value);
                    decimal.TryParse(Convert.ToString(pFiyat != null ? pFiyat.GetValue(dataItem) : grdUrunListesi.CurrentRow.Cells["fiyat"]?.Value), out birimFiyat);
                }
                else
                {
                    urunAdi = Convert.ToString(grdUrunListesi.CurrentRow.Cells["urun_adi"]?.Value ?? grdUrunListesi.CurrentRow.Cells[1].Value);
                    decimal.TryParse(Convert.ToString(grdUrunListesi.CurrentRow.Cells["fiyat"]?.Value), out birimFiyat);
                }

                int miktar = 1;
                decimal toplam = miktar * birimFiyat;

                var dt = grdSatilacakUrunler.DataSource as System.Data.DataTable;
                if (dt != null)
                {
                    var newRow = dt.NewRow();
                    if (dt.Columns.Count > 0) newRow[0] = urunKodu;
                    if (dt.Columns.Count > 1) newRow[1] = urunAdi;
                    if (dt.Columns.Count > 2) newRow[2] = miktar;
                    if (dt.Columns.Count > 3) newRow[3] = birimFiyat;
                    if (dt.Columns.Count > 4) newRow[4] = toplam;
                    dt.Rows.Add(newRow);
                }
                else
                {
                    grdSatilacakUrunler.Rows.Add(urunKodu, urunAdi, miktar, birimFiyat, toplam);
                }

                Hesapla();
                MessageBox.Show("Ürün listeye eklendi");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuTedarikci_Click(object sender, EventArgs e)
        {
            frmTedarikciIslemler frmTedarikci = new frmTedarikciIslemler();
            DialogResult result = frmTedarikci.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                Bilgilerilistele();
            }
        }

        private void menuUrun_Click(object sender, EventArgs e)
        {
            frmUrunIslemleri frmUrun = new frmUrunIslemleri();
            DialogResult result = frmUrun.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                Bilgilerilistele();
                if (this.Controls.ContainsKey("grdSatilacakUrunlar"))
                {
                    var grid = this.Controls["grdSatilacakUrunlar"] as DataGridView;
                    grid?.Rows.Clear();
                }
            }
        }

        private void menuSatis_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new frmSatisDetaylari())
                {
                    frm.ShowDialog();
                }

                Bilgilerilistele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Satış Detayları açılırken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuMusteri_Click(object sender, EventArgs e)
        {
            frmMusteriIslemleri frmMusteri = new frmMusteriIslemleri();
            DialogResult result = frmMusteri.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                Bilgilerilistele();
            }
        }

        private void menuPersonel_Click(object sender, EventArgs e)
        {
            frmPersonelIslemleri frmPersonel = new frmPersonelIslemleri();
            frmPersonel.oturumAcanPersonelId = PersonelId;
            frmPersonel.ShowDialog();
        }

        private void frmAnaSayfa_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmGiris frm = new frmGiris();
            frm.Show();
        }

        private void frmAnaSayfa_Load(object sender, EventArgs e)
        {
            lblPersonelAdSoyad.Text = personelAdSoyad;
            lblYetki.Text = personelYetki;

            bool isAdmin = false;

            try
            {
                if (PersonelId > 0)
                {
                    using (var db = new stok_takip_dbEntities())
                    {
                        var yetkiAdi = (from p in db.personeller
                                        where p.id == PersonelId
                                        join y in db.yetkiler on p.yetki_id equals y.yetki_id into yy
                                        from y in yy.DefaultIfEmpty()
                                        select y.yetki_adi).FirstOrDefault();

                        if (!string.IsNullOrWhiteSpace(yetkiAdi))
                        {
                            lblYetki.Text = yetkiAdi;
                            personelYetki = yetkiAdi;
                        }

                        isAdmin = string.Equals(yetkiAdi?.Trim(), "Admin", StringComparison.OrdinalIgnoreCase);
                    }
                }

                if (!isAdmin && !string.IsNullOrWhiteSpace(personelYetki))
                {
                    isAdmin = personelYetki.IndexOf("yönet", StringComparison.OrdinalIgnoreCase) >= 0;
                }
            }
            catch
            {
                isAdmin = false;
            }

            menuPersonel.Enabled = isAdmin;

            Bilgilerilistele();
        }

        private void Bilgilerilistele()
        {
            using (var stok_Takip = new stok_takip_dbEntities())
            {
                var urunSonuc = (from u in stok_Takip.urunler
                                 join k in stok_Takip.kategoriler on u.kategori_id equals k.kategori_id
                                 join t in stok_Takip.tedarikciler on u.tedarikci_id equals t.id
                                 where u.stok_adedi >= 1
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

                grdUrunListesi.DataSource = urunSonuc;

                if (grdUrunListesi.Columns.Contains("urun_kodu"))
                {
                    grdUrunListesi.Columns["urun_kodu"].Width = 60;
                    grdUrunListesi.Columns["urun_kodu"].HeaderText = "Kod";
                }
                if (grdUrunListesi.Columns.Contains("urun_adi"))
                {
                    grdUrunListesi.Columns["urun_adi"].Width = 230;
                    grdUrunListesi.Columns["urun_adi"].HeaderText = "Ürün Adı";
                }
                if (grdUrunListesi.Columns.Contains("stok_adedi"))
                {
                    grdUrunListesi.Columns["stok_adedi"].Width = 60;
                    grdUrunListesi.Columns["stok_adedi"].HeaderText = "Stok";
                }
                if (grdUrunListesi.Columns.Contains("fiyat"))
                {
                    grdUrunListesi.Columns["fiyat"].Width = 50;
                    grdUrunListesi.Columns["fiyat"].HeaderText = "Birim Fiyat";
                    grdUrunListesi.Columns["fiyat"].DefaultCellStyle.Format = "0.00";
                }
                if (grdUrunListesi.Columns.Contains("kategori_adi"))
                {
                    grdUrunListesi.Columns["kategori_adi"].Width = 130;
                    grdUrunListesi.Columns["kategori_adi"].HeaderText = "Kategori";
                }
                if (grdUrunListesi.Columns.Contains("firma_adi"))
                {
                    grdUrunListesi.Columns["firma_adi"].Width = 110;
                    grdUrunListesi.Columns["firma_adi"].HeaderText = "Tedarikçi";
                }
                if (grdUrunListesi.Columns.Contains("aciklama"))
                {
                    grdUrunListesi.Columns["aciklama"].Width = 200;
                    grdUrunListesi.Columns["aciklama"].HeaderText = "Açıklama";
                }
            }
        }

        private void btnMusteriSec_Click(object sender, EventArgs e)
        {
            try
            {

                mskTxtMusteriTelefon.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                string girilenTelefon = (mskTxtMusteriTelefon.Text ?? "").Trim();

                if (string.IsNullOrEmpty(girilenTelefon) || Regex.Replace(girilenTelefon, @"\D", "").Length < 10)
                {
                    MessageBox.Show("Geçerli bir telefon numarası giriniz");
                    return;
                }

                string girilenDigits = Regex.Replace(girilenTelefon, @"\D", "");
                musteriler musteri = null;

                using (var stok_Takip = new stok_takip_dbEntities())
                {
                    musteri = stok_Takip.musteriler
                        .FirstOrDefault(m => (m.telefon ?? "") == girilenTelefon);

                    if (musteri == null)
                    {
                        musteri = stok_Takip.musteriler
                            .AsEnumerable()
                            .FirstOrDefault(m => Regex.Replace(Convert.ToString(m.telefon ?? ""), @"\D", "") == girilenDigits);
                    }
                }

                if (musteri != null)
                {
                    txtMusteriAdSoyad.Text = musteri.ad_soyad;
                    seciliMusteriId = musteri.id;
                }
                else
                {
                    MessageBox.Show("Bu telefon numarasına kayıtlı müşteri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata oluştu");
            }
            finally
            {
                mskTxtMusteriTelefon.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
            }
        }

        private void Hesapla()
        {
            try
            {
                if (grdSatilacakUrunler.CurrentRow != null && !grdSatilacakUrunler.CurrentRow.IsNewRow)
                {
                    int miktar = Convert.ToInt32(grdSatilacakUrunler.CurrentRow.Cells[2].Value ?? 0);
                    decimal birimFiyat = Convert.ToDecimal(grdSatilacakUrunler.CurrentRow.Cells[3].Value ?? 0m);
                    decimal toplam = miktar * birimFiyat;
                    grdSatilacakUrunler.CurrentRow.Cells[4].Value = toplam;
                }

                decimal genelToplam = 0m;
                foreach (DataGridViewRow row in grdSatilacakUrunler.Rows)
                {
                    if (row.IsNewRow) continue;
                    object val = row.Cells[4].Value;
                    decimal d;
                    if (decimal.TryParse(Convert.ToString(val), out d))
                        genelToplam += d;
                }

                txtGenelToplam.Text = genelToplam.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdSatilacakUrunler_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var currentRow = grdSatilacakUrunler.CurrentRow;
                if (currentRow == null || currentRow.IsNewRow) return;

                string hücreDegeri = currentRow.Cells[e.ColumnIndex].Value?.ToString() ?? "1";
                int yeniMiktar;
                if (!int.TryParse(hücreDegeri, out yeniMiktar) || yeniMiktar <= 0)
                    yeniMiktar = 1;

                using (var stok_Takip = new stok_takip_dbEntities())
                {
                    int urunKodu = Convert.ToInt32(currentRow.Cells[0].Value);
                    var stokkontrolurun = stok_Takip.urunler
                        .FirstOrDefault(u => u.urun_kodu == urunKodu);

                    if (stokkontrolurun != null && stokkontrolurun.stok_adedi.HasValue && yeniMiktar > stokkontrolurun.stok_adedi.Value)
                        yeniMiktar = stokkontrolurun.stok_adedi.Value;
                }

                currentRow.Cells[e.ColumnIndex].Value = yeniMiktar;
                Hesapla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            try
            {
                if (seciliMusteriId == 0)
                {
                    MessageBox.Show("Satış yapılacak müşteriyi seçiniz");
                    return;
                }

                if (PersonelId <= 0)
                {
                    MessageBox.Show("Geçerli bir personel oturumu yok. Lütfen tekrar giriş yapın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var satirlar = grdSatilacakUrunler.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList();
                if (satirlar.Count == 0)
                {
                    MessageBox.Show("Satışı yapılacak ürünleri ekleyiniz");
                    return;
                }

                foreach (var row in satirlar)
                {
                    if (!TryGetIntFromCell(row.Cells["Kod"] ?? row.Cells[0], out int kod) ||
                        !int.TryParse(Convert.ToString(row.Cells["Miktar"]?.Value ?? row.Cells[2].Value), out int miktar) ||
                        !decimal.TryParse(Convert.ToString(row.Cells["BirimFiyat"]?.Value ?? row.Cells[3].Value), out decimal fiyat))
                    {
                        MessageBox.Show("Satış listesinde geçersiz bir değer var. Lütfen kontrol edin.");
                        return;
                    }
                }

                using (var stok_Takip = new stok_takip_dbEntities())
                {
                    if (!stok_Takip.personeller.Any(p => p.id == PersonelId))
                    {
                        MessageBox.Show("Satışı kaydedecek personel veritabanında bulunamadı. Lütfen oturumu kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    satislar satis = new satislar
                    {
                        musteri_id = seciliMusteriId,
                        personel_id = PersonelId,
                        satis_tarih = DateTime.Now,
                        notlar = txtNot.Text
                    };
                    stok_Takip.satislar.Add(satis);
                    stok_Takip.SaveChanges();

                    foreach (var row in satirlar)
                    {
                        TryGetIntFromCell(row.Cells["Kod"] ?? row.Cells[0], out int kod);
                        int miktar = Convert.ToInt32(row.Cells["Miktar"]?.Value ?? row.Cells[2].Value);
                        decimal fiyat = Convert.ToDecimal(row.Cells["BirimFiyat"]?.Value ?? row.Cells[3].Value);

                        var urun = stok_Takip.urunler.FirstOrDefault(u => u.urun_kodu == kod);
                        if (urun == null)
                            throw new InvalidOperationException($"Ürün kodu bulunamadı: {kod}");

                        satis_detaylari sd = new satis_detaylari
                        {
                            satis_id = satis.satis_id,
                            urun_kodu = kod,
                            miktar = miktar,
                            fiyat = fiyat
                        };
                        stok_Takip.satis_detaylari.Add(sd);

                        urun.stok_adedi = (urun.stok_adedi ?? 0) - miktar;
                    }

                    stok_Takip.SaveChanges();
                }

                MessageBox.Show("Satış işlemi başarılı");
                Bilgilerilistele();
                grdSatilacakUrunler.Rows.Clear();
                txtMusteriAdSoyad.Clear();
                mskTxtMusteriTelefon.Clear();
                txtNot.Clear();
                seciliMusteriId = 0;
                txtGenelToplam.Text = "0.00";
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
            {
                var baseEx = dbEx.GetBaseException();
                string entries = string.Join(", ", dbEx.Entries.Select(entry => entry.Entity.GetType().Name));
                MessageBox.Show("DbUpdateException: " + dbEx.Message + "\nBase: " + (baseEx?.Message ?? "") + "\nEntries: " + entries, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Satış sırasında hata: " + ex.GetBaseException().Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetKodColumnIndex()
        {
            if (grdSatilacakUrunler.Columns.Contains("Kod"))
                return grdSatilacakUrunler.Columns["Kod"].Index;
            if (grdSatilacakUrunler.Columns.Contains("urun_kodu"))
                return grdSatilacakUrunler.Columns["urun_kodu"].Index;
            return 0;
        }

        private void grdUrunListesi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}