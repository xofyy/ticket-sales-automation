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
    public partial class FormAlanEkle : Form
    {
        public FormAlanEkle()
        {
            InitializeComponent();
        }
        private void FormAlanEkle_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        konserTableAdapters.Alan_BilgileriTableAdapter alan = new konserTableAdapters.Alan_BilgileriTableAdapter();
        //alanekleme sorgusu ile farklı olması şartı ile alan eklemesi yapar.
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                alan.alanekleme(textBox1.Text);
                MessageBox.Show("Alan Eklendi");
            }
            catch (Exception)
            {

                MessageBox.Show("Aynı alanı daha önce eklediniz!", "Uyarı!");
            }
            textBox1.Text = "";
        }
        //alansilme işlemini yapar
        private void button2_Click(object sender, EventArgs e)
        {
            alan.AlanSilme(textBox1.Text);
            MessageBox.Show("Alan Silindi");
        }
    }
}
