using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for DangNhap.xaml
    /// </summary>
    public partial class DangNhap : Window
    {

        //public static string strConnectionString = "Data Source=DESKTOP-DLT0AO8;Initial Catalog=QLHS;Integrated Security=True";
        //SqlConnection conn = new SqlConnection(strConnectionString);
        SqlConnection conn = Connection.KetNoi();

        public DangNhap()
        {
            InitializeComponent();
        }

        public void CountUser()
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM TAIKHOAN", conn);
                int iCount = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (iCount == 0)
                {
                    System.Windows.MessageBox.Show("Mời bạn tạo 1 tài khoản trước khi đăng nhập", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    AddAccountWindow newWindow = new AddAccountWindow();
                    newWindow.ShowDialog();
                }
            }
            catch (SqlException ex)
            {
                System.Windows.MessageBox.Show(string.Concat("Error:" + ex.ToString()), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            conn.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string a = tenDN.Text;
            string b = matKhau.Password;
            Password TaiKhoanTraVe = new Password();
            string str = TaiKhoanTraVe.DangNhap(a, b);
            if (str == null)
                return;
            else
            {
                MainWindow window = new MainWindow(str);
                window.Show();
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CountUser();
            tenDN.Focus();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string a = tenDN.Text;
                string b = matKhau.Password;
                Password TaiKhoanTraVe = new Password();
                string str = TaiKhoanTraVe.DangNhap(a, b);
                if (str == null)
                    return;
                else
                {
                    MainWindow window = new MainWindow(str);
                    window.Show();
                    this.Close();
                }
                return;
            }
        }

        private void tenDN_GotFocus(object sender, RoutedEventArgs e)
        {
            tenDN.Text = "";
            tenDN.GotFocus -= tenDN_GotFocus;
            tenDN.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void matKhau_GotFocus(object sender, RoutedEventArgs e)
        {
            matKhau.Password = "";
            matKhau.GotFocus -= matKhau_GotFocus;
        }

    
    }
}
