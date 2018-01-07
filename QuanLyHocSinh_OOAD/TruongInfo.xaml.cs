using System.Data.SqlClient;
using System.Windows;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for TruongInfo.xaml
    /// </summary>
    public partial class TruongInfo : Window
    {

        //public static string strConnectionString = "Data Source=DESKTOP-DLT0AO8;Initial Catalog=QLHS;Integrated Security=True";
        //SqlConnection conn = new SqlConnection(strConnectionString);
        SqlConnection conn = Connection.KetNoi();

        private void LoadComponent()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from THONGTINTRG", conn);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    txtTenTrg.Text = dr.GetString(1);
                    txtDiaChi.Text = dr.GetString(2);
                    txtHieuTruong.Text = dr.GetString(3);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            conn.Close();
        }

        public TruongInfo()
        {
            InitializeComponent();
            LoadComponent();
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "update THONGTINTRG set TENTRG = @TENTRG, DIACHI = @DIACHI, HIEUTRUONG = @HIEUTRUONG where STT = 1";
                cmd.Parameters.AddWithValue("@TENTRG", txtTenTrg.Text);
                cmd.Parameters.AddWithValue("@DIACHI", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@HIEUTRUONG", txtHieuTruong.Text);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
            MessageBox.Show("Bạn sửa thông tin thành công");

            btnThoat_Click(sender, e);
        }
    }
}
