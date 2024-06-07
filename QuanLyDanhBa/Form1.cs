using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDanhBa
{
    public partial class Form1 : Form
    {
        string Status = "";
        int Index = -1;

        private string connectionString = @"Data Source=VUHOANG\SQLEXPRESS;Initial Catalog=quanLyDanhBa;Integrated Security=True;";
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;

        public Form1()
        {
            InitializeComponent();
        }



        #region Method
        private void LoadDataFormBase()
        {
            try
            {
                // Tạo kết nối đến cơ sở dữ liệu
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Tạo truy vấn SQL
                    string query = "SELECT  tblNguoiDung.hoTen, tblNguoiDung.soDienThoai, tblCoQuan.tenCoQuan, tblNguoiDung.ghiChu   FROM  tblNguoiDung   JOIN    tblCoQuan ON tblNguoiDung.idCoQuan = tblCoQuan.idCoQuan;";

                    // Tạo đối tượng SqlDataAdapter và đổ dữ liệu vào DataTable
                    dataAdapter = new SqlDataAdapter(query, connection);
                    dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Gán DataTable làm nguồn dữ liệu cho DataGridView
                    dtgvPhoneBook.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu từ cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        

        

        void EnableControl(bool isEnableTextBox, bool isEnableDataGridView)
        {
            txbName.Enabled = txbNumberPhone.Enabled = txbOrganization.Enabled = txbNote.Enabled = isEnableTextBox;
            dtgvPhoneBook.Enabled = isEnableDataGridView;
        }
        #endregion

        void ClearTextBox()
        {
            txbName.Clear();
            txbNumberPhone.Clear();
            txbOrganization.Clear();
            txbNote.Clear();
        }
        #region Event
        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearTextBox();
            EnableControl(false, true);
            btnAdd.Enabled = btnUpdate.Enabled = btnDelete.Enabled = true;
            btnSave.Enabled = btnHuy.Enabled = false;

        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            EnableControl(false, true);
            LoadDataFormBase();
            btnSave.Enabled = btnHuy.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EnableControl(true, false);
            btnAdd.Enabled = btnUpdate.Enabled = btnDelete.Enabled = false;
            btnSave.Enabled = btnHuy.Enabled = true;
            Status = "Add";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn hay không
            if (dtgvPhoneBook.CurrentRow != null)
            {
                int rowIndex = dtgvPhoneBook.SelectedRows[0].Index;
                if (rowIndex >= 0 && rowIndex < dataTable.Rows.Count)
                {
                    DataRow row = dataTable.Rows[rowIndex];
                    txbName.Text = row["hoTen"].ToString();
                    txbNumberPhone.Text = row["soDienThoai"].ToString();
                    txbOrganization.Text = row["tenCoQuan"].ToString();
                    txbNote.Text = row["ghiChu"].ToString();
                    Status = "Update";

                    EnableControl(true, false);
                    btnAdd.Enabled = btnUpdate.Enabled = btnDelete.Enabled = false;
                    btnSave.Enabled = btnHuy.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Hãy chọn một bản ghi hợp lệ", "Cảnh báo");
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn một bản ghi", "Cảnh báo");
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txbName.Text;
            string numberPhone = txbNumberPhone.Text;
            string organization = txbOrganization.Text;
            string note = txbNote.Text;

            // Kiểm tra xem Status là "Add" (Thêm mới) hay "Update" (Cập nhật)
            if (Status == "Add")
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "INSERT INTO tblNguoiDung (hoTen, soDienThoai, idCoQuan, ghiChu) VALUES (@hoTen, @soDienThoai, @idCoQuan, @ghiChu)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@hoTen", name);
                        command.Parameters.AddWithValue("@soDienThoai", numberPhone);
                        command.Parameters.AddWithValue("@idCoQuan", organization);
                        command.Parameters.AddWithValue("@ghiChu", note);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm dữ liệu vào cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Status == "Update")
            {
                // Làm tương tự cho việc cập nhật dữ liệu
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "UPDATE tblNguoiDung SET soDienThoai = @soDienThoai, idCoQuan = @idCoQuan, ghiChu = @ghiChu WHERE hoTen = @hoTen";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@soDienThoai", numberPhone);
                        command.Parameters.AddWithValue("@idCoQuan", organization);
                        command.Parameters.AddWithValue("@ghiChu", note);
                        command.Parameters.AddWithValue("@hoTen", name);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật dữ liệu trong cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Sau khi thực hiện lưu hoặc cập nhật dữ liệu, làm mới dữ liệu trên DataGridView
            LoadDataFormBase();

            // Đặt lại trạng thái của các control và làm sạch các TextBox
            EnableControl(false, true);
            ClearTextBox();
            btnAdd.Enabled = btnUpdate.Enabled = btnDelete.Enabled = false;
            btnSave.Enabled = btnHuy.Enabled = true;
        }


        private void dtgvPhoneBook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Index = e.RowIndex;
        }
        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Index < 0)
            {
                MessageBox.Show("Hãy chọn một bản ghi", "Cảnh báo");
                return;
            }
        }
        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            EnableControl(true, false);
            btnAdd.Enabled = btnUpdate.Enabled = btnDelete.Enabled = btnSave.Enabled = false;
            btnHuy.Enabled = true;
        }


        private void txbName_TextChanged(object sender, EventArgs e)
        {
            string search = txbName.Text;
            if (search != "")
            {
                List<PhoneBook> listSearch = new List<PhoneBook>();
                foreach (var item in ListPhoneBook.Instance.ListNumberPhone)
                {
                    if (item.Name.ToLower().Contains(search.ToLower()))
                    {
                        listSearch.Add(item);
                    }
                }

                dtgvPhoneBook.DataSource = null;
                
                dtgvPhoneBook.DataSource = listSearch;
            }
            else
            {
            
            }

        }
        #endregion

    }
}