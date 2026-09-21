using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class EditForm : Form
    {
        private ProductRepository _productRepo = new ProductRepository();
        private ProductTypeRepository _typeRepo = new ProductTypeRepository();
        private int _productId;

        // เรียกใช้: new EditForm(id).ShowDialog();
        public EditForm(int productId)
        {
            InitializeComponent();
            _productId = productId;
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            cboType.DisplayMember = "Name";
            cboType.ValueMember = "Id";
            cboType.DataSource = _typeRepo.GetAll();

            Product product = _productRepo.SearchById(_productId);
            if (product == null)
            {
                MessageBox.Show("ไม่พบข้อมูลสินค้า");
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            txtName.Text = product.Name;
            txtPrice.Text = product.Price.ToString();
            txtDetail.Text = product.Detail;
            cboType.SelectedValue = product.TypeId;
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
            product.Id = _productId;
            product.Name = txtName.Text.Trim();
            product.Price = price;
            product.Detail = txtDetail.Text.Trim();
            product.TypeId = (int)cboType.SelectedValue;

            try
            {
                _productRepo.Update(product);
                MessageBox.Show("แก้ไขข้อมูลเรียบร้อย");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("แก้ไขไม่สำเร็จ: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ปุ่มควบคุม เช่น Backspace
            if (char.IsControl(e.KeyChar)) return;

            // ตัวเลข 0-9
            if (char.IsDigit(e.KeyChar)) return;

            // จุดทศนิยม อนุญาตแค่ 1 จุด
            if (e.KeyChar == '.' && !txtPrice.Text.Contains(".")) return;

            // นอกนั้นไม่รับ
            e.Handled = true;
        }
    }
    }
