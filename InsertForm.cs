using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class InsertForm : Form
    {
        private ProductRepository _productRepo = new ProductRepository();
        private ProductTypeRepository _typeRepo = new ProductTypeRepository();

        public InsertForm()
        {
            InitializeComponent();
        }

        private void InsertForm_Load(object sender, EventArgs e)
        {
            cboType.DisplayMember = "Name";
            cboType.ValueMember = "Id";
            cboType.DataSource = _typeRepo.GetAll();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("กรุณากรอกชื่อสินค้า");
                return;
            }

            double price;
            if (!double.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("ราคาต้องเป็นตัวเลข");
                return;
            }

            if (cboType.SelectedValue == null)
            {
                MessageBox.Show("กรุณาเลือกประเภทสินค้า");
                return;
            }

            Product product = new Product();
            product.Name = txtName.Text.Trim();
            product.Price = price;
            product.Detail = txtDetail.Text.Trim();
            product.TypeId = (int)cboType.SelectedValue;

            try
            {
                _productRepo.Insert(product);
                MessageBox.Show("บันทึกข้อมูลเรียบร้อย");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("บันทึกไม่สำเร็จ: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}