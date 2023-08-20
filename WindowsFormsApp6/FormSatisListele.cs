using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using OfficeOpenXml;
namespace WindowsFormsApp6
{
    public partial class FormSatisListele : Form
    {
        public FormSatisListele()
        {
            InitializeComponent();
        }
        konserTableAdapters.Satis_BilgileriTableAdapter satislistesi = new konserTableAdapters.Satis_BilgileriTableAdapter();
        private void FormSatisListele_Load(object sender, EventArgs e)
        {            
            ToplamUcretHesabı();
        }
        private void ToplamUcretHesabı()
        {
            int ucrettoplam = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                ucrettoplam += Convert.ToInt32(dataGridView1.Rows[i].Cells["ucret"].Value);
            }
            label1.Text = "Toplam Satış=" + ucrettoplam + "TL";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // XML verisini DataTable'e aktarma
            DataSet dataSet = new DataSet();
            dataSet.ReadXml("veriler.xml"); // XML dosyasının adını burada belirtin
            DataTable dataTable = dataSet.Tables[0]; // İlk tabloyu seçiyoruz

            // DataTable'i DataGridView kontrolüne bağlama
            dataGridView1.DataSource = dataTable;
            //dataGridView1.DataSource = satislistesi.satislistesi2();
            ToplamUcretHesabı();
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = satislistesi.tarihegörelistele2(dateTimePicker1.Text);
            ToplamUcretHesabı();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("veriler.xml");
            // Excel dosyasını oluşturma
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Yeni bir Excel çalışma kitabı oluşturma
                ExcelWorkbook workbook = excelPackage.Workbook;
                ExcelWorksheet worksheet = workbook.Worksheets.Add("NewDataSet");

                // XML verilerini Excel sayfasına yazma
                XmlNodeList TableNodes = xmlDocument.SelectNodes("//Table");
                int row = 1;
                foreach (XmlNode TableNode in TableNodes)
                {
                    string SatisId = TableNode.SelectSingleNode("SatisId").InnerText;
                    string izlemeNo = TableNode.SelectSingleNode("izlemeNo").InnerText;
                    string AlanAdi = TableNode.SelectSingleNode("AlanAdi").InnerText;
                    string SanatciAdi = TableNode.SelectSingleNode("SanatciAdi").InnerText;
                    string Tarih = TableNode.SelectSingleNode("Tarih").InnerText;
                    string Saat = TableNode.SelectSingleNode("Saat").InnerText;
                    string Ad = TableNode.SelectSingleNode("Ad").InnerText;
                    string Soyad = TableNode.SelectSingleNode("Soyad").InnerText;
                    string Ucret = TableNode.SelectSingleNode("Ucret").InnerText;
                    string Tarih2 = TableNode.SelectSingleNode("Tarih2").InnerText;

                    worksheet.Cells[row, 1].Value = SatisId;
                    worksheet.Cells[row, 2].Value = izlemeNo;
                    worksheet.Cells[row, 3].Value = AlanAdi;
                    worksheet.Cells[row, 4].Value = SanatciAdi;
                    worksheet.Cells[row, 5].Value = Tarih;
                    worksheet.Cells[row, 6].Value = Saat;
                    worksheet.Cells[row, 7].Value = Ad;
                    worksheet.Cells[row, 8].Value = Soyad;
                    worksheet.Cells[row, 9].Value = Ucret;
                    worksheet.Cells[row, 10].Value = Tarih2;

                    row++;
                }
                FileInfo excelFile = new FileInfo("veriler.xlsx");
                excelPackage.SaveAs(excelFile);

                string excelFilePath = "veriler.xlsx"; // Excel dosyasının tam yolu

                // Excel dosyasını açma işlemi
                System.Diagnostics.Process.Start(excelFilePath);
            }
        }
    }
}
