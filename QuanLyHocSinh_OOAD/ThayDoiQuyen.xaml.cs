using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for ThayDoiQuyDinh.xaml
     
    /// </summary>
    public partial class ThayDoiQuyDinh : Window
    {
        SqlConnection conn = Connection.KetNoi();
        public ThayDoiQuyDinh()
        {
            InitializeComponent();
            LoadComponent();
        }

        private void LoadComponent()
        {
            txtGiaoVien.Clear();
            cmbQuyenTruyCap.Items.Add("1 - Toàn quyền truy cập cho ban giám hiệu");
            cmbQuyenTruyCap.Items.Add("2 - Quyền truy cập của các nhân viên văn phòng");
            cmbQuyenTruyCap.Items.Add("3 - Quyền truy cập của giáo viên");

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select TENDN from TAIKHOAN";

                SqlDataReader drDataReader = cmd.ExecuteReader();

                while (drDataReader.Read())
                {
                    cmbTenDN.Items.Add(drDataReader.GetString(0));
                }
                conn.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Có lỗi trong quá trình kết nối", "Thông báo", MessageBoxButton.OK);
            }
        }

        private void cmbTenDN_DropDownClosed(object sender, EventArgs e)
        {
            conn.Open();
            string tentk = cmbTenDN.Text;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select gv.MAGV, gv.HOTEN from GIAOVIEN gv, TAIKHOAN tk where gv.MAGV = tk.MAGV and tk.TENDN = '" + tentk + "'";
            SqlDataReader drDataReader = cmd.ExecuteReader();
            while (drDataReader.Read())
            {
                string strValue = drDataReader.GetString(0) + "-" + drDataReader.GetString(1);
                this.txtGiaoVien.Text = strValue;
            }
            conn.Close();
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTenDN.SelectedIndex < 0)
            {
                MessageBox.Show("Mời bạn chọn tài khoản cần thay đổi quyền truy cập!");
                return;
            }

            if (cmbQuyenTruyCap.SelectedIndex < 0)
            {
                MessageBox.Show("Mời bạn chọn quyền truy cập!");
                return;
            }
            conn.Open();
            string tentk = cmbTenDN.Text;
            int iQuyen = int.Parse(cmbQuyenTruyCap.SelectedItem.ToString().Substring(0, 1));
            SqlCommand cm = new SqlCommand();
            cm.Connection = conn;
            cm.CommandText = "update TAIKHOAN SET QUYENSUDUNG = @iQuyen WHERE TENDN = @tenTK";
            cm.Parameters.AddWithValue("@iQuyen", iQuyen);
            cm.Parameters.AddWithValue("@tenTK", tentk);
            //SqlDataReader drDataReader = cmd.ExecuteReader();
            cm.ExecuteNonQuery();
            MessageBox.Show("Thay đổi quyền thành công", "Thông báo", MessageBoxButton.OK);
            conn.Close();
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
