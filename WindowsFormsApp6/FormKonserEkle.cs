using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class FormKonserEkle : Form
    {
        public FormKonserEkle()
        {
            InitializeComponent();
        }
        konserTableAdapters.Saat_BilgileriTableAdapter knsrsaat = new konserTableAdapters.Saat_BilgileriTableAdapter();
        SqlConnection baglanti = new SqlConnection("Data Source=MURAT;Initial Catalog=Konser_Bileti;Integrated Security=True");
        private void FormKonserEkle_Load(object sender, EventArgs e)
        {
            SanatciveAlanGoster(comboBox1, "select *from sanatci_bilgileri", "sanatciadi");
            SanatciveAlanGoster(comboBox2, "select *from alan_bilgileri", "alanadi");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //saat değişkenini ve sanatçı bilgileri saatekleme sorgusuyla kaydeder.
            RadioButtonSeciliyse();
            if (saat != "")
            {
                knsrsaat.saatekleme(comboBox1.Text, comboBox2.Text, dateTimePicker1.Text, saat);
                MessageBox.Show("Saat Ekleme İşlemi Yapıldı", "Kayıt");
            }
            else if (saat == "")
            {
                MessageBox.Show("Saat Seçimi Yapmadınız!", "Uyarı");
            }
            comboBox2.Text = "";
            comboBox1.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToShortDateString();

        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //radiobutton'ları aktif hale getirir.
            foreach (Control item3 in groupBox1.Controls)
            {
                item3.Enabled = true;
            }
            //radiobutton'ları bugünkü tarih ve seçilen tarih ile kıyaslayarak aktif veya inaktif hale getirir.
            DateTime bugün = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime yeni = DateTime.Parse(dateTimePicker1.Text);
            if (yeni == bugün)
            {
                foreach (Control item in groupBox1.Controls)
                {
                    if (DateTime.Parse(DateTime.Now.ToShortTimeString()) > DateTime.Parse(item.Text))
                    {
                        item.Enabled = false;
                    }
                }
                Tarih_Karşılaştır();
            }
            else if (yeni > bugün)
            {
                Tarih_Karşılaştır();
            }
            else if (yeni < bugün)
            {
                MessageBox.Show("Geriye Dönük İşlem Yapılamaz");
                dateTimePicker1.Text = DateTime.Now.ToShortDateString();
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Text = DateTime.Now.ToShortDateString();
        }
        string saat = "";
        //radiobutton ile seçilen saati "saat" değişkenine aktarır.
        private void RadioButtonSeciliyse()
        {
            if(radioButton1.Checked == true) saat = radioButton1.Text;
            else if(radioButton2.Checked == true) saat = radioButton2.Text;
            else if (radioButton3.Checked == true) saat = radioButton3.Text;
            else if (radioButton4.Checked == true) saat = radioButton4.Text;
            else if (radioButton5.Checked == true) saat = radioButton5.Text;
            else if (radioButton6.Checked == true) saat = radioButton6.Text;
            else if (radioButton7.Checked == true) saat = radioButton7.Text;
            else if (radioButton8.Checked == true) saat = radioButton8.Text;
            else if (radioButton9.Checked == true) saat = radioButton9.Text;
            else if (radioButton10.Checked == true) saat = radioButton10.Text;
            else if (radioButton11.Checked == true) saat = radioButton11.Text;
            else if (radioButton12.Checked == true) saat = radioButton12.Text;
        }
        //sanatçı ve alan bilgisini getirir.
        private void SanatciveAlanGoster(ComboBox combo, string sql, string sql2)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand(sql, baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read()==true) 
            {
                combo.Items.Add(read[sql2].ToString());
            }
            baglanti.Close();
        }
        //seçilen tarihe göre o tarihte dolu olan saatleri radiobutton da inaktif hale getirir.
        private void Tarih_Karşılaştır()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from saat_bilgileri where alanadi='" + comboBox2.Text + "' and tarih='" + dateTimePicker1.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read() == true)
            {
                foreach (Control item2 in groupBox1.Controls)
                {
                    if (read["seans"].ToString() == item2.Text)
                    {
                        item2.Enabled = false;
                    }
                }
            }
            baglanti.Close();
        }       
    }
}
