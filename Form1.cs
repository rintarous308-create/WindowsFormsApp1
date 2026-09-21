using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private ProductRepository _productRepo = new ProductRepository();

        private void LoadProducts()
        {
            try
            {
                string keyword = textBox1.Text;
                List<Product> products = _productRepo.Search(keyword);

                dataGridView1.Rows.Clear();
                string[] tempstr = new string[dataGridView1.ColumnCount];

                for (int i = 0; i < products.Count; i++)
                {
                    tempstr[0] = products[i].Id.ToString();
                    tempstr[1] = products[i].Name;
                    tempstr[2] = products[i].Price.ToString();
                    tempstr[3] = products[i].TypeName;
                    dataGridView1.Rows.Add(tempstr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("โหลดข้อมูลสินค้าไม่สำเร็จ" + ex, "ผลการทำงาน",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            InsertForm insertfrm = new InsertForm();
            insertfrm.ShowDialog(this);
            LoadProducts();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
