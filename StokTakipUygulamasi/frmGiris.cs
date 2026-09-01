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
    public partial class frmGiris : Form
    {
        public frmGiris()
        {
            InitializeComponent();

            this.KeyPreview = true;

            this.KeyDown -= frmGiris_KeyDown;
            this.KeyDown += frmGiris_KeyDown;
        }

        public static int personelId;
        public static string personelAdSoyad;
        public static string personelYetki;

        private static string Sifrele(string metin)
        {
            if (metin == null) metin = "";
            var byteDegeri = Encoding.UTF8.GetBytes(metin);
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var sifreliByteDegeri = md5.ComputeHash(byteDegeri);
                return Convert.ToBase64String(sifreliByteDegeri);
            }
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            try
            {
                string kAdi = txtKullaniciAdi.Text?.Trim();
                string girilenSifre = txtSifre.Text?.Trim() ?? "";
                string hesaplananHash = Sifrele(girilenSifre);

                using (var db = new stok_takip_dbEntities())
                {
                    var dbPersonel = db.personeller.FirstOrDefault(p => p.kullanici_adi == kAdi);
                    if (dbPersonel == null)
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre hatalı");
                        return;
                    }

                    bool hashesMatch = string.Equals(dbPersonel.sifre, hesaplananHash, StringComparison.Ordinal);
                    if (!hashesMatch)
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre hatalı");
                        return;
                    }

                    string yetkiAdi = null;
                    if (dbPersonel.yetki_id.HasValue)
                    {
                        yetkiAdi = db.yetkiler
                            .Where(y => y.yetki_id == dbPersonel.yetki_id.Value)
                            .Select(y => y.yetki_adi)
                            .FirstOrDefault();
                    }

                    this.Hide();
                    personelId = dbPersonel.id;
                    personelAdSoyad = dbPersonel.ad + " " + dbPersonel.soyad;
                    personelYetki = yetkiAdi;

                    using (var anaSayfa = new frmAnaSayfa())
                    {
                        anaSayfa.PersonelId = personelId;
                        anaSayfa.personelAdSoyad = personelAdSoyad;
                        anaSayfa.personelYetki = personelYetki;

                        anaSayfa.ShowDialog();
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Giriş hatası");
            }
        }
        private void frmGiris_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void frmGiris_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;

            var active = this.ActiveControl;

            if (active == txtSifre)
            {
                btnGiris.Focus();
                return;
            }

            if (active == btnGiris)
            {
                btnGiris.PerformClick();
                return;
            }

            this.SelectNextControl(active, true, true, true, true);
        }
    }
}