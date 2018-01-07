using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for SuaDiem.xaml
    /// </summary>
    public partial class formBangDiem : Window
    {
        private string MaHS;
        SqlConnection conn = Connection.KetNoi();
        public string getMaHS()
        {
            return this.MaHS;
        }
        public void setMaHS(string maHS)
        {
            this.MaHS = maHS;
        }
        public formBangDiem()
        {
            InitializeComponent();
            //fill_combobox_Check_lop();           
        }
        public formBangDiem(string MaHS)
        {
            InitializeComponent();
            setMaHS(MaHS);
            cbHocKi.Items.Add(1);
            cbHocKi.Items.Add(2);
            fillComponent();                                 
        }

        //public static string strConnectionString = "Data Source=DESKTOP-DLT0AO8;Initial Catalog=QLHS;Integrated Security=True";
        //SqlConnection conn = new SqlConnection(strConnectionString);
        void fillComponent()
        {
            txtMaHS.Text = getMaHS();
            try
            {
                conn.Open();
                SqlCommand cm = new SqlCommand();
                cm.Connection = conn;
                cm.CommandType = CommandType.Text;
                cm.Parameters.AddWithValue("@_mahocsinh", getMaHS());
                cm.CommandText = @"SELECT HOCSINH.MAHS, LOP.TENLOP, HOCKY, HOTEN,  TENNAMHOC FROM HOCSINH LEFT JOIN KETQUAMON ON HOCSINH.MAHS = KETQUAMON.MAHS, LOP, NAMHOC WHERE HOCSINH.MAHS = @_mahocsinh AND KETQUAMON.NAMHOC = NAMHOC.MANAMHOC AND HOCSINH.MALOP = LOP.MALOP";

                SqlDataAdapter adapter = new SqlDataAdapter(cm);

                DataTable dtInfo = new DataTable();
                dtInfo.Clear();
                adapter.Fill(dtInfo);
                // MessageBox.Show(dtInfo.Rows[0][0].ToString())
                txtMaHS.Text = dtInfo.Rows[0][0].ToString();
                txtLopHoc.Text = dtInfo.Rows[0][1].ToString();
                //txtHocKi.Text = dtInfo.Rows[0][2].ToString();
                txtTenHS.Text = dtInfo.Rows[0][3].ToString();
                txtNamHoc.Text = dtInfo.Rows[0][4].ToString();
                conn.Close();
            }
            catch (SqlException)
            {
                conn.Close();
                MessageBox.Show("Có lỗi trong việc kết nối SQL server", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            // dgBangDiem.ItemsSource = dtDataTable.DefaultView;



        }       

        void LoadData()
        {
            //cbHocKi.Text
            try
            {
                conn.Open();
                SqlCommand cm = new SqlCommand();
                cm.Connection = conn;
                cm.CommandType = CommandType.Text;
                cm.Parameters.AddWithValue("@_mahocsinh", getMaHS());
                cm.Parameters.AddWithValue("@_hocki", Int32.Parse(cbHocKi.Text));
                cm.CommandText = @"SELECT TENMH as 'Tên môn học', H1D1, H1D2, H1D3, H1D4, H1D5, H2D1, H2D2, H2D3, H2D4, H2D5, THI as 'Điểm thi', DTB as 'Điểm TB', DANHGIA as 'Đánh giá' FROM HOCSINH LEFT JOIN KETQUAMON ON HOCSINH.MAHS = KETQUAMON.MAHS, MONHOC MH WHERE HOCSINH.MAHS = @_mahocsinh AND MH.MAMH=KETQUAMON.MAMH AND HOCKY=@_hocki";

                SqlDataAdapter sdaDataAdapter = new SqlDataAdapter(cm);

                DataTable dtDataTable = new DataTable();
                dtDataTable.Clear();
                sdaDataAdapter.Fill(dtDataTable);
                dgBangDiem.ItemsSource = dtDataTable.DefaultView;
                conn.Close();
            }
            catch (SqlException)
            {
                conn.Close();
                MessageBox.Show("Có lỗi trong việc kết nối SQL server", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void cbHocKi_DropDownClosed(object sender, EventArgs e)
        {
            LoadData();
        }

    }
}
