using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for NamHocInfo.xaml
    /// </summary>
    public partial class NamHocInfo : Window
    {
        //public static string strConnectionString = "Data Source=DESKTOP-DLT0AO8;Initial Catalog=QLHS;Integrated Security=True";
        //SqlConnection conn = new SqlConnection(strConnectionString);
        SqlConnection conn = Connection.KetNoi();

        public NamHocInfo()
        {
            InitializeComponent();
            LoadData("select MANAMHOC as 'Mã năm học', TENNAMHOC as 'Thời gian' from NAMHOC");
        }

        void LoadData(string str)
        {
            try
            {
                SqlDataAdapter sdaDataAdapter = new SqlDataAdapter(str, Connection.strConnectionString);
                DataTable dtDataTable = new DataTable();
                dtDataTable.Clear();
                sdaDataAdapter.Fill(dtDataTable);
                dataGrid.ItemsSource = dtDataTable.DefaultView;

            }
            catch (SqlException)
            {
                MessageBox.Show("Có lỗi trong việc kết nối SQL server", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool IsNumber(string strValue)
        {
            foreach (char c in strValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex < 0)
                return;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "delete from NAMHOC where MANAMHOC = @MANAMHOC";
                cmd.Parameters.AddWithValue("@MANAMHOC", ((DataRowView)dataGrid.SelectedItem).Row[0].ToString());

                cmd.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();
            LoadData("select MANAMHOC as 'Mã năm học', TENNAMHOC as 'Thời gian' from NAMHOC");

        }

        private void btnNhap_Click(object sender, RoutedEventArgs e)
        {
            if (IsNumber(txtBatDau.Text) == false || IsNumber(txtKetThuc.Text) == false)
                return;

            if (txtBatDau.Text.Length != 4 || txtKetThuc.Text.Length != 4)
            {
                MessageBox.Show("Mời bạn nhập đầy đủ 4 số của 1 năm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string MANH = string.Concat(txtBatDau.Text.ToString().Substring(2, 2) + "-" + txtKetThuc.Text.ToString().Substring(2, 2));
            string TENNH = string.Concat(txtBatDau.Text + "-" + txtKetThuc.Text);

            
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "insert into NAMHOC values (@MANH,@TENNH)";
                cmd.Parameters.AddWithValue("@MANH", MANH);
                cmd.Parameters.AddWithValue("@TENNH", TENNH);
                cmd.ExecuteNonQuery();                
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();
            LoadData("select MANAMHOC as 'Mã năm học', TENNAMHOC as 'Thời gian' from NAMHOC");

        }
    }
}
