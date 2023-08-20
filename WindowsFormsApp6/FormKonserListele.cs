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
    public partial class FormKonserListele : Form
    {
        public FormKonserListele()
        {
            InitializeComponent();
        }       
        SqlConnection baglanti = new SqlConnection("Data Source=MURAT;Initial Catalog=Konser_Bileti;Integrated Security=True");
        DataTable tablo = new DataTable();
        private void konserListesi(string sql) 
        { 
            baglanti.Open(); 
            SqlDataAdapter adtr = new SqlDataAdapter(sql, baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        private void FormKonserListele_Load(object sender, EventArgs e)
        {
            tablo.Clear();
            konserListesi("select * from saat_bilgileri where tarih like'" + dateTimePicker1.Text + "'");
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            tablo.Clear();
            konserListesi("select * from saat_bilgileri where tarih like'" + dateTimePicker1.Text + "'");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            tablo.Clear();
            konserListesi("select *from saat_bilgileri");
        }
    }
}
