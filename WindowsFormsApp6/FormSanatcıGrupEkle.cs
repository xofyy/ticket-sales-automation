using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class FormSanatcıGrupEkle : Form
    {
        public FormSanatcıGrupEkle()
        {
            InitializeComponent();
        }
        WindowsFormsApp6.konserTableAdapters.Sanatci_BilgileriTableAdapter sanatci = new WindowsFormsApp6.konserTableAdapters.Sanatci_BilgileriTableAdapter();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sanatci.sanatciekleme(textsanatci.Text, textBoxyil.Text, pictureBox1.ImageLocation, Convert.ToString(numericUpDown1.Value));
                MessageBox.Show("Sanatçı Bilgileri Eklendi", "Kayıt");
            }
            catch (Exception)
            {
                MessageBox.Show("Bu sanatçı daha önce eklendi", "Uyarı");
            }           
            foreach (Control item in Controls) if (item is TextBox) item.Text = "";                        
            textBoxyil.Text = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
        }
    }
}
