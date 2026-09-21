using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class SearchForm : Form
    {
        private ProductRepository _productRepo = new ProductRepository();

        public SearchForm()
        {
            InitializeComponent();
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dgvProducts.DataSource = _productRepo.Search(txtKeyword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("โหลดข้อมูลไม่สำเร็จ: " + ex.Message);
            }
        }

        private int GetSelectedId()
        {
            if (dgvProducts.CurrentRow == null) return -1;
            Product p = dgvProducts.CurrentRow.DataBoundItem as Product;
            return p == null ? -1 : p.Id;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadData();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (InsertForm form = new InsertForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int id = GetSelectedId();
            if (id < 0)
            {
                MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการแก้ไข");
                return;
            }

            using (EditForm form = new EditForm(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = GetSelectedId();
            if (id < 0)
            {
                MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการลบ");
                return;
            }

            DialogResult confirm = MessageBox.Show("ต้องการลบสินค้านี้ใช่หรือไม่?", "ยืนยันการลบ",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _productRepo.Delete(id);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ลบไม่สำเร็จ: " + ex.Message);
            }
        }
    }
}
