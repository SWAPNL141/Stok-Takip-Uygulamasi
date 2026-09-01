using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StokTakipUygulamasi
{
    public partial class frmPersonelIslemleri : Form
    {
        public frmPersonelIslemleri()
        {
            InitializeComponent();

            this.Load -= frmPersonelIslemleri_Load;
            this.Load += frmPersonelIslemleri_Load;

            this.grdPersoneller.CellClick -= grdPersoneller_CellClick;
            this.grdPersoneller.CellClick += grdPersoneller_CellClick;

            this.btnYeniKayit.Click -= btnYeniKayit_Click;
            this.btnYeniKayit.Click += btnYeniKayit_Click;

            this.btnEkle.Click -= btnEkle_Click;
            this.btnEkle.Click += btnEkle_Click;

            this.btnSil.Click -= btnSil_Click;
            this.btnSil.Click += btnSil_Click;

            this.btnGuncelle.Click -= btnGuncelle_Click;
            this.btnGuncelle.Click += btnGuncelle_Click;

            this.grdPersoneller.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.grdPersoneller.MultiSelect = false;
        }

        public int oturumAcanPersonelId;
        stok_takip_dbEntities stok_Takip;
        int seciliPersonelId = 0;
        private void frmPersonelIslemleri_Load(object sender, EventArgs e)
        {
            PersonelleriListele();
        }

        private void Bilgilerilistele()
        {
            stok_Takip = new stok_takip_dbEntities();
            var sonuc = (from p in stok_Takip.personeller
                         join y in stok_Takip.yetkiler on p.yetki_id equals y.yetki_id
                         select new
                         {
                             p.id,
                             p.ad,
                             p.soyad,
                             p.telefon,
                             p.mail,
                             p.adres,
                             p.kullanici_adi,
                             p.sifre,
                             yetki_adi = y.yetki_adi
                         }).ToList();
            grdPersoneller.DataSource = sonuc;

            if (grdPersoneller.Columns.Contains("id")) grdPersoneller.Columns["id"].Width = 35;
            if (grdPersoneller.Columns.Contains("ad")) grdPersoneller.Columns["ad"].Width = 80;
            if (grdPersoneller.Columns.Contains("soyad")) grdPersoneller.Columns["soyad"].Width = 80;
            if (grdPersoneller.Columns.Contains("yetki_adi")) grdPersoneller.Columns["yetki_adi"].Width = 80;
            if (grdPersoneller.Columns.Contains("kullanici_adi")) grdPersoneller.Columns["kullanici_adi"].Width = 80;

            grdPersoneller.ClearSelection();
            cmbYetkiler.DataSource = stok_Takip.yetkiler.ToList();
            cmbYetkiler.DisplayMember = "yetki_adi";
            cmbYetkiler.ValueMember = "yetki_id";
        }

        private bool BilgiGirisKontrol()
        {
            bool kontrol = false;
            if (txtAd.TextLength <= 2)
            {
                MessageBox.Show("Personelin adını giriniz.");
            }
            else if (txtSoyad.TextLength <= 2)
            {
                MessageBox.Show("Personelin soyadını giriniz.");
            }
            else if (!mskTelefon.MaskCompleted)
            {
                MessageBox.Show("Telefon numarasını 10 haneli olarak giriniz.");
            }
            else if (txtKullanici.TextLength < 5)
            {
                MessageBox.Show("Personelin kullanıcı adını giriniz.");
            }
            else
            {
                kontrol = true;
            }
            return kontrol;
        }

        private static string Sifrele(string metin)
        {
            byte[] byteDegeri = Encoding.UTF8.GetBytes(metin);
            MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();
            byte[] sifreliByteDegeri = csp.ComputeHash(byteDegeri);
            return Convert.ToBase64String(sifreliByteDegeri);
        }

        private void FormuTemizle()
        {
            txtAd.Clear();
            txtSoyad.Clear();
            txtAdres.Clear();
            txtMail.Clear();
            txtKullanici.Clear();
            txtSifre.Clear();
            mskTelefon.Clear();
            grdPersoneller.ClearSelection();
            seciliPersonelId = 0;
        }

        private void mskTelefon_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                seciliPersonelId = 0;

                if (BilgiGirisKontrol())
                {
                    if (txtSifre.TextLength >= 8)
                    {
                        stok_Takip = new stok_takip_dbEntities();
                        personeller personel = new personeller();
                        personel.ad = txtAd.Text;
                        personel.soyad = txtSoyad.Text;
                        personel.telefon = mskTelefon.Text;
                        personel.mail = txtMail.Text;
                        personel.adres = txtAdres.Text;
                        personel.kullanici_adi = txtKullanici.Text;
                        personel.sifre = Sifrele(txtSifre.Text);
                        personel.yetki_id = Convert.ToInt32(cmbYetkiler.SelectedValue);
                        stok_Takip.personeller.Add(personel);
                        stok_Takip.SaveChanges();
                        Bilgilerilistele();
                        FormuTemizle();
                    }
                    else
                    {
                        MessageBox.Show("En az 8 haneli bir şifre giriniz.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kayıt Hatası");
            }
        }

        private void grdPersoneller_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grdPersoneller.CurrentRow != null)
                {
                    seciliPersonelId = Convert.ToInt32(grdPersoneller.CurrentRow.Cells["id"].Value.ToString());
                    txtAd.Text = grdPersoneller.CurrentRow.Cells["ad"].Value.ToString();
                    txtSoyad.Text = grdPersoneller.CurrentRow.Cells["soyad"].Value.ToString();
                    txtMail.Text = grdPersoneller.CurrentRow.Cells["mail"].Value.ToString();
                    txtAdres.Text = grdPersoneller.CurrentRow.Cells["adres"].Value.ToString();
                    mskTelefon.Text = grdPersoneller.CurrentRow.Cells["telefon"].Value.ToString();
                    txtKullanici.Text = grdPersoneller.CurrentRow.Cells["kullanici_adi"].Value.ToString();
                    cmbYetkiler.Text = grdPersoneller.CurrentRow.Cells["yetki_adi"].Value.ToString();
                    txtSifre.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (seciliPersonelId != 0 && seciliPersonelId != oturumAcanPersonelId)
                {
                    stok_Takip = new stok_takip_dbEntities();
                    personeller silinecekPersonel = stok_Takip.personeller.
                        Where(p => p.id == seciliPersonelId).First();
                    stok_Takip.personeller.Remove(silinecekPersonel);
                    stok_Takip.SaveChanges();
                    Bilgilerilistele();
                    FormuTemizle();
                }
                else if (seciliPersonelId == oturumAcanPersonelId)
                {
                    MessageBox.Show("Oturum sahibi olan personel silinemez!");
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
                if (seciliPersonelId != 0)
                {
                    if (BilgiGirisKontrol())
                    {
                        stok_Takip = new stok_takip_dbEntities();
                        personeller guncellenecekPersonel = stok_Takip.personeller.
                            Where(t => t.id == seciliPersonelId).First();
                        guncellenecekPersonel.ad = txtAd.Text;
                        guncellenecekPersonel.soyad = txtSoyad.Text;
                        guncellenecekPersonel.telefon = mskTelefon.Text;
                        guncellenecekPersonel.mail = txtMail.Text;
                        guncellenecekPersonel.adres = txtAdres.Text;
                        guncellenecekPersonel.kullanici_adi = txtKullanici.Text;
                        guncellenecekPersonel.yetki_id = Convert.ToInt32(cmbYetkiler.SelectedValue);
                        if (txtSifre.TextLength >= 8)
                        {
                            guncellenecekPersonel.sifre = Sifrele(txtSifre.Text);
                        }
                        stok_Takip.SaveChanges();
                        Bilgilerilistele();
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

        private void btnYeniKayit_Click(object sender, EventArgs e)
        {
            FormuTemizle();
        }

        private void PersonelleriListele()
        {
            using (var stok_Takip = new stok_takip_dbEntities())
            {
                grdPersoneller.DataSource = stok_Takip.personeller
                    .OrderBy(p => p.ad).ThenBy(p => p.soyad)
                    .ToList();
            }

            if (grdPersoneller.Columns.Contains("satislar"))
                grdPersoneller.Columns.Remove("satislar");

            if (grdPersoneller.Columns.Contains("id"))
            {
                grdPersoneller.Columns["id"].HeaderText = "ID";
                grdPersoneller.Columns["id"].Width = 50;
                grdPersoneller.Columns["id"].ReadOnly = true;
            }

            if (grdPersoneller.Columns.Contains("ad"))
            {
                grdPersoneller.Columns["ad"].HeaderText = "Ad";
                grdPersoneller.Columns["ad"].Width = 120;
            }

            if (grdPersoneller.Columns.Contains("soyad"))
            {
                grdPersoneller.Columns["soyad"].HeaderText = "Soyad";
                grdPersoneller.Columns["soyad"].Width = 140;
            }

            if (grdPersoneller.Columns.Contains("telefon"))
            {
                grdPersoneller.Columns["telefon"].HeaderText = "Telefon";
                grdPersoneller.Columns["telefon"].Width = 110;
            }

            if (grdPersoneller.Columns.Contains("mail"))
            {
                grdPersoneller.Columns["mail"].HeaderText = "E-Posta";
                grdPersoneller.Columns["mail"].Width = 200;
            }

            if (grdPersoneller.Columns.Contains("kullanici_adi"))
            {
                grdPersoneller.Columns["kullanici_adi"].HeaderText = "Kullanıcı Adı";
                grdPersoneller.Columns["kullanici_adi"].Width = 140;
            }

            if (grdPersoneller.Columns.Contains("sifre"))
            {
                // Şifre sütununu gizlemek güvenlik için tercih edilir
                grdPersoneller.Columns["sifre"].Visible = false;
            }

            if (grdPersoneller.Columns.Contains("yetki_id"))
            {
                // İsterseniz yetki adını ayrı sorguyla ekleyip gösterebilirsiniz; şimdilik gizleyin veya başlık verin
                grdPersoneller.Columns["yetki_id"].HeaderText = "Yetki ID";
                grdPersoneller.Columns["yetki_id"].Visible = false;
            }

            if (grdPersoneller.Columns.Contains("adres"))
            {
                grdPersoneller.Columns["adres"].HeaderText = "Adres";
                grdPersoneller.Columns["adres"].Width = 240;
            }

            grdPersoneller.ClearSelection();
        }
    }
}