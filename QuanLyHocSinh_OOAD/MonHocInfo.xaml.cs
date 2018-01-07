using System.Data.SqlClient;
using System.Windows;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for MonHocInfo.xaml
    /// </summary>
    public partial class MonHocInfo : Window
    {
        //public static string strConnectionString = "Data Source=DESKTOP-DLT0AO8;Initial Catalog=QLHS;Integrated Security=True";
        //SqlConnection conn = new SqlConnection(strConnectionString);
        SqlConnection conn = Connection.KetNoi();
        int iLuaChon;
        string strMaMH;
        //Xem = 0 nhap = 1 sua = -1
        public MonHocInfo()
        {
            InitializeComponent();
        }

        public MonHocInfo(int LuaChon)
        {
            InitializeComponent();
            iLuaChon = LuaChon;
        }

        public MonHocInfo(int LuaChon, string MaMH)
        {
            InitializeComponent();
            iLuaChon = LuaChon;
            strMaMH = MaMH;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtMaMH.MaxLength = 4;

            //use to add items in combo box
            cmbHS1.Items.Add("1 cột điểm");
            cmbHS1.Items.Add("2 cột điểm");
            cmbHS1.Items.Add("3 cột điểm");
            cmbHS1.Items.Add("4 cột điểm");
            cmbHS1.Items.Add("5 cột điểm");

            //use to add items in combo box
            cmbHS2.Items.Add("1 cột điểm");
            cmbHS2.Items.Add("2 cột điểm");
            cmbHS2.Items.Add("3 cột điểm");
            cmbHS2.Items.Add("4 cột điểm");
            cmbHS2.Items.Add("5 cột điểm");

            // use to add to combo box Khoi
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT MAKHOA, TENKHOA FROM KHOA", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                
                while (dr.Read())
                {
                    cmbKhoa.Items.Add(string.Concat(dr.GetString(0) + "-" + dr.GetString(1)));
                }
            }
            catch (SqlException)
            {
                MessageBox.Show("Có lỗi trong quá trình lấy dữ liệu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            conn.Close();

            if (iLuaChon == 0)
            {
                txtMaMH.IsReadOnly = true;
                txtTenMH.IsReadOnly = true;
                cmbHS1.IsEnabled = false;
                cmbHS2.IsEnabled = false;
                cmbKhoa.IsEnabled = false;
                btnLuu.IsEnabled = false;
                ShowData(strMaMH);
            }
            else if (iLuaChon == -1)
            {
                txtMaMH.IsReadOnly = true;
                ShowData(strMaMH);
            }

        }

        void ShowData(string MaMH)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT MAMH, TENMH, MAKHOA, TENKHOA, HESO1, HESO2 FROM MONHOC, KHOA WHERE MAMH = @MAMH AND KHOA = MAKHOA", conn);
                cmd.Parameters.AddWithValue("@MAMH", MaMH);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    txtMaMH.Text = dr.GetString(0);
                    txtTenMH.Text = dr.GetString(1);
                    cmbKhoa.SelectedValue = string.Concat(dr.GetString(2) + "-" + dr.GetString(3));
                    cmbHS1.SelectedIndex = dr.GetInt32(4) - 1;
                    cmbHS2.SelectedIndex = dr.GetInt32(5) - 1;

                }
            }
            catch (SqlException)
            {

            }
            conn.Close();
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (txtMaMH.Text.Length == 0)
            {
                MessageBox.Show("Mời bạn nhập mã môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (cmbKhoa.SelectedIndex < 0)
            {
                MessageBox.Show("Mời bạn chọn khoa của môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            if (cmbHS1.SelectedIndex < 0)
            {
                MessageBox.Show("Mời bạn chọn số cột điểm hệ số 1 của môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbHS2.SelectedIndex < 0)
            {
                MessageBox.Show("Mời bạn chọn số cột điểm hệ số 2 của môn học", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (iLuaChon == 1)
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO MONHOC VALUES(@MAMH,@TENMH,@KHOA,@HS1,@HS2)";
                    cmd.Parameters.AddWithValue("@MAMH", txtMaMH.Text);
                    cmd.Parameters.AddWithValue("@TENMH", txtTenMH.Text);
                    cmd.Parameters.AddWithValue("@KHOA", cmbKhoa.Text.Substring(0, 4));
                    cmd.Parameters.AddWithValue("HS1", cmbHS1.Text.Substring(0, 1));
                    cmd.Parameters.AddWithValue("@HS2", cmbHS2.Text.Substring(0, 1));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bạn đã nhập môn học thành công");
                    txtMaMH.Clear();
                    txtTenMH.Clear();
                    cmbKhoa.SelectedIndex = -1;
                    conn.Close();
                }
                catch(SqlException)
                {
                    MessageBox.Show("Có vấn đề trong việc nhập dữ liệu môn học");
                    conn.Close();
                }
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = " UPDATE MONHOC SET TENMH = @TENMH, KHOA = @KHOA, KHOI = @KHOI, HESO1 = @HS1, HESO2 = @HS2 WHERE MAMH = @MAMH";
                    cmd.Parameters.AddWithValue("@MAMH", txtMaMH.Text);
                    cmd.Parameters.AddWithValue("@TENMH", txtTenMH.Text);
                    cmd.Parameters.AddWithValue("@KHOA", cmbKhoa.Text.Substring(0,4));
                    cmd.Parameters.AddWithValue("HS1", cmbHS1.Text.Substring(0,1));
                    cmd.Parameters.AddWithValue("@HS2", cmbHS2.Text.Substring(0,1));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Bạn đã sửa môn học thành công");
                    this.Close();
                }
                catch (SqlException)
                {
                    MessageBox.Show("Có vấn đề trong việc sửa dữ liệu môn học");
                    conn.Close();
                }
            }

        }



    }
}
