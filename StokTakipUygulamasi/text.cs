using System;
using System.Windows.Forms;

namespace StokTakipUygulamasi
{
    public partial class text : Form
    {
        public text()
        {
            InitializeComponent();

            // Eğer Designer'da bazı Click event'leri bağlı değilse burada garantiye alıyoruz
            this.btnfrmMusteriIslemleri.Click += btnfrmMusteriIslemleri_Click;
            this.btnfrmPersonelIslemleri.Click += btnfrmPersonelIslemleri_Click;
            this.btnfrmStokuAzalanUrunler.Click += btnfrmStokuAzalanUrunler_Click;
            this.btnfrmTedarikciIslemler.Click += btnfrmTedarikciIslemler_Click;
            this.btnfrmUrunIslemleri.Click += btnfrmUrunIslemleri_Click;
        }

        private void btnfrmAnaSayfa_Click(object sender, EventArgs e)
        {
            frmAnaSayfa frm = new frmAnaSayfa();
            frm.Show();
        }

        private void btnfrmGiris_Click(object sender, EventArgs e)
        {
            frmGiris frm2 = new frmGiris();
            frm2.Show();
        }

        private void btnfrmKategoriIslemleri_Click(object sender, EventArgs e)
        {
            frmKategoriIslemleri frm = new frmKategoriIslemleri();
            frm.Show();
        }

        private void btnfrmMusteriIslemleri_Click(object sender, EventArgs e)
        {
            frmMusteriIslemleri frm = new frmMusteriIslemleri();
            frm.Show();
        }

        private void btnfrmPersonelIslemleri_Click(object sender, EventArgs e)
        {
            frmPersonelIslemleri frm = new frmPersonelIslemleri();
            // Mevcut oturum sahibini mümkünse aktar
            try { frm.oturumAcanPersonelId = frmGiris.personelId; } catch { }
            frm.Show();
        }

        private void btnfrmStokuAzalanUrunler_Click(object sender, EventArgs e)
        {
            frmStokuAzalanUrunler frm = new frmStokuAzalanUrunler();
            frm.Show();
        }

        private void btnfrmTedarikciIslemler_Click(object sender, EventArgs e)
        {
            frmTedarikciIslemler frm = new frmTedarikciIslemler();
            frm.Show();
        }

        private void btnfrmUrunIslemleri_Click(object sender, EventArgs e)
        {
            frmUrunIslemleri frm = new frmUrunIslemleri();
            frm.Show();
        }

        private void btnfrmSatisDetaylari_Click(object sender, EventArgs e)
        {
            frmSatisDetaylari frm = new frmSatisDetaylari();
            frm.Show();
        }
    }
}