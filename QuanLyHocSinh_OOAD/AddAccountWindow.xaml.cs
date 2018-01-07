using System.Data.SqlClient;
using System.Windows;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for AddAccountWindow.xaml
    /// </summary>
    public partial class AddAccountWindow : Window
    {
        //public static string strConnectionString = "Data Source=DESKTOP-DLT0AO8;Initial Catalog=QLHS;Integrated Security=True";
        //SqlConnection conn = new SqlConnection(strConnectionString);
        SqlConnection conn = Connection.KetNoi();

        public AddAccountWindow()
        {
            InitializeComponent();
            LoadComponent();
        }

        private void LoadComponent()
        {
            txtTenDN.Clear();
            cmbQuyenTruyCap.Items.Add("1 - Toàn quyền truy cập cho ban giám hiệu");
            cmbQuyenTruyCap.Items.Add("2 - Quyền truy cập của các nhân viên văn phòng");
            cmbQuyenTruyCap.Items.Add("3 - Quyền truy cập của giáo viên");

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from GIAOVIEN";

                SqlDataReader drDataReader = cmd.ExecuteReader();

                while (drDataReader.Read())
                {
                    cmbMaGV.Items.Add(drDataReader.GetString(0) + "-" + drDataReader.GetString(1));
                }
                cmbMaGV.Items.Add("Mặc định");
            }
            catch (SqlException)
            {
                MessageBox.Show("Có lỗi trong quá trình kết nối", "Thông báo", MessageBoxButton.OK);
            }
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMaGV.SelectedIndex < 0)
            {
                MessageBox.Show("Mời bạn chọn mã giáo viên cho tài khoản muốn tạo ");
                return;
            }

            if (cmbQuyenTruyCap.SelectedIndex < 0)
            {
                MessageBox.Show("Mời bạn chọn quyền truy cập cho tài khoản muốn tạo");
                return;
            }

            if (passboxPassword.Password.Length == 0)
            {
                MessageBox.Show("Mời bạn nhập password cho tài khoản muốn tạo");
                return;
            }

            if (passboxConfirm.Password.Length == 0)
            {
                MessageBox.Show("Mời bạn nhập password confirm để kiểm tra password đã nhập");
                return;
            }

            if (passboxPassword.Password.ToString() != passboxConfirm.Password.ToString())
            {
                MessageBox.Show("Mời bạn nhập đúng password trong khung Confirm password để kiểm tra password đã nhập");
                passboxConfirm.Clear();
                return;
            }


            string strMaGV;
            int iQuyen;
            if (cmbMaGV.SelectedItem.ToString() == "Mặc định")
                strMaGV = "";
            else
                strMaGV = cmbMaGV.SelectedItem.ToString().Substring(0, 5);
            iQuyen = int.Parse(cmbQuyenTruyCap.SelectedItem.ToString().Substring(0, 1));
            Password pass = new Password(txtTenDN.Text, strMaGV, passboxPassword.Password.ToString(), iQuyen);
            pass.AddUsers();

            conn.Close();

            this.Close();
        }
    }
}
