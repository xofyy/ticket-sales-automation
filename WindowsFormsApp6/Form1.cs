using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=MURAT;Initial Catalog=Konser_Bileti;Integrated Security=True");
        private void button1_Click(object sender, EventArgs e)
        {
            FormAlanEkle ekle = new FormAlanEkle();
            ekle.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FormSanatcıGrupEkle ekle = new FormSanatcıGrupEkle();
            ekle.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            FormKonserEkle ekle = new FormKonserEkle();
            ekle.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FormKonserListele ekle = new FormKonserListele();
            ekle.Show();
        }
        //Satış listeleme formuna giderken verileri .xml formatına dönüştürür.
        private void button8_Click(object sender, EventArgs e)
        {
            XmlDonustur();
            FormSatisListele ekle = new FormSatisListele();
            ekle.Show();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SanatcıAlanIzNoGetir(comboBoxsa, "select *from sanatci_bilgileri", "sanatciadi");
            SanatcıAlanIzNoGetir(comboBoxaa, "select *from alan_bilgileri", "alanadi");           
            SanatcıAlanIzNoGetir(comboBoxinip, "select *from satis_bilgileri", "izlemeno");
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxucrt.Items.Clear();
            KonserAfisiGoster();
            UcretGetir();
        }
        //belirtilen alanların boş kalmaması şartıyla veritabanında belirlenen SatisYap sorgusuyla verileri ekler,
        //satış işlemini yapar.
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBoxin.Text != "" && comboBoxucrt.Text!="")
            {
                try
                {
                    satis.SatisYap(comboBoxaa.Text, comboBoxsa.Text, comboBoxkt.Text, comboBoxks.Text, textBoxad.Text, textBoxsyd.Text, comboBoxucrt.Text, DateTime.Now.ToShortDateString(), textBoxin.Text);
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata Oluştu, İzleyici No'yu Kontrol Ediniz!" + hata.Message, "Uyarı");
                }
            }
            else
            {
                MessageBox.Show("Numara Seçimi Yapmadınız", "Uyarı");
            }
            textBoxin.Text = string.Empty;
        }
        private void comboBoxaa_SelectedIndexChanged(object sender, EventArgs e)
        {
            KonserTarihiGetir();
        }
        private void comboBoxkt_SelectedIndexChanged(object sender, EventArgs e)
        {
            KonserSaatiGetir();
        }
        //belirtilen alanların boş kalmaması şartıyla veritabanında belirlenen biletiptal sorgusuyla verileri siler,
        //iptal işlemini yapar.
        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBoxinip.Text != "" && comboBoxaa.Text !="" && comboBoxsa.Text != "" && comboBoxkt.Text != "")
            {
                try
                {
                    satis.biletiptal(comboBoxaa.Text, comboBoxsa.Text, comboBoxkt.Text, comboBoxks.Text, comboBoxinip.Text);
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata Oluştu!" + hata.Message, "Uyarı");
                }
            }
        }
        //button ile uygulamadan çıkış yapar.
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //tüm verileri siler.
        private void button9_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from alan_bilgileri", baglanti);
            komut.ExecuteNonQuery();
            SqlCommand komut1 = new SqlCommand("delete from saat_bilgileri", baglanti);
            komut1.ExecuteNonQuery();
            SqlCommand komut2 = new SqlCommand("delete from sanatci_bilgileri", baglanti);
            komut2.ExecuteNonQuery();
            SqlCommand komut3 = new SqlCommand("delete from satis_bilgileri", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
        }
        private void comboBoxinip_MouseClick(object sender, MouseEventArgs e)
        {
            comboBoxinip.Items.Clear();
            SanatcıAlanIzNoGetir(comboBoxinip, "select *from satis_bilgileri", "izlemeno");
        }
        //yenile buttonuna basıldığında yeni girilen verileri methodlar ile getirir.
        private void button10_Click(object sender, EventArgs e)
        {
            comboBoxsa.Items.Clear();
            comboBoxaa.Items.Clear();
            comboBoxinip.Items.Clear();
            SanatcıAlanIzNoGetir(comboBoxsa, "select *from sanatci_bilgileri", "sanatciadi");
            SanatcıAlanIzNoGetir(comboBoxaa, "select *from alan_bilgileri", "alanadi");
            SanatcıAlanIzNoGetir(comboBoxinip, "select *from satis_bilgileri", "izlemeno");
        }
        //sql'de kaydedilen verileri xml'e dönüştürür.
        private void XmlDonustur()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from satis_bilgileri", baglanti);
            SqlDataAdapter adtr = new SqlDataAdapter(komut);
            DataSet ds = new DataSet();
            adtr.Fill(ds);
            string xmlData = ds.GetXml();
            File.WriteAllText("veriler.xml", xmlData);
            baglanti.Close();

        }
        //seçilen sanatçıya göre o sanatçıya ait afişi gösterir.
        private void KonserAfisiGoster()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from sanatci_bilgileri where sanatciadi='"+comboBoxsa.Text+"'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                pictureBox1.ImageLocation = read["resim"].ToString();
            }
            baglanti.Close();
        }
        //methoda gönderilen comboboxlara yine gönderilen veritabanı bilgileriyle
        //sanatçı ismi, alan ismi ve izleyici no atar.
        private void SanatcıAlanIzNoGetir(ComboBox combo, string sql1, string sql2)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand(sql1, baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                combo.Items.Add(read[sql2].ToString());
            }
            baglanti.Close();
        }
        //Seçilen sanatçıya ait konser ücretini getirir.
        //combobox içinde aynı değerler varsa sadece birini gösterir.
        private void UcretGetir()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from sanatci_bilgileri where sanatciadi='"+comboBoxsa.SelectedItem+"'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (!comboBoxucrt.Items.Contains(read["ucret"].ToString()))
                {
                    comboBoxucrt.Items.Add(read["ucret"].ToString());
                }
            }
            baglanti.Close();
        }
        //Seçilen sanatçı adı ve alana göre konser tarihi bilgisini getirir
        //ve işlemin yapıldığı günün tarihinden önceki tarih bilgilerini getirmez.
        konserTableAdapters.Satis_BilgileriTableAdapter satis = new konserTableAdapters.Satis_BilgileriTableAdapter();       
        private void KonserTarihiGetir()
        {
            comboBoxkt.Text = "";
            comboBoxks.Text = "";
            comboBoxkt.Items.Clear();
            comboBoxks.Items.Clear();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from saat_bilgileri where sanatciadi='" + comboBoxsa.SelectedItem + "' and alanadi='" + comboBoxaa.SelectedItem + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (DateTime.Parse(read["tarih"].ToString())>=DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    if (!comboBoxkt.Items.Contains(read["tarih"].ToString()))
                    {
                        comboBoxkt.Items.Add(read["tarih"].ToString());
                    }                   
                }                
            }
            baglanti.Close();
        }
        //Seçilen sanatçı adı ve alana göre konser saati bilgisini getirir
        //tarih getirme methoduyla aynı şekilde işlem yapılan tarihten önceki saat bilgilerini getirmez.
        private void KonserSaatiGetir()
        {
            comboBoxks.Text = "";
            comboBoxks.Items.Clear();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from saat_bilgileri where sanatciadi='" + comboBoxsa.SelectedItem + "' and alanadi='" + comboBoxaa.SelectedItem + "' and tarih='" + comboBoxkt.SelectedItem + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (DateTime.Parse(read["tarih"].ToString()) == DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    if (DateTime.Parse(read["seans"].ToString()) > DateTime.Parse(DateTime.Now.ToShortTimeString()));
                    {
                        comboBoxks.Items.Add(read["seans"].ToString());
                    }                                      
                }
                else if (DateTime.Parse(read["tarih"].ToString()) > DateTime.Parse(DateTime.Now.ToShortDateString()))
                {                    
                        comboBoxks.Items.Add(read["seans"].ToString());
                }
            }
            baglanti.Close();
        }        
    }
}
