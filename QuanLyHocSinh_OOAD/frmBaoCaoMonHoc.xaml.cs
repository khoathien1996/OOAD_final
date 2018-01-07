using System.Data;
using System.Data.SqlClient;
using System.Windows;
using Microsoft.Reporting.WinForms;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for frmBaoCaoMonHoc.xaml
    /// </summary>
    public partial class frmBaoCaoMonHoc : Window
    {
        //public static string strConnectionString = "Data Source=DESKTOP-DLT0AO8;Initial Catalog=QLHS;Integrated Security=True";
        //SqlConnection conn = new SqlConnection(strConnectionString);
        SqlConnection conn = Connection.KetNoi();
        string strName, strHieuTruong;
        public frmBaoCaoMonHoc()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReportViewerDemo.Reset();
            DataTable dt = GetData();

            ReportDataSource dsDataSource = new ReportDataSource("DataSet1", dt); //ReportDataSource("", dt);
            ReportViewerDemo.LocalReport.DataSources.Add(dsDataSource);

            //embbedded to RDLC report file
            ReportViewerDemo.LocalReport.ReportEmbeddedResource = "QuanLyHocSinh_OOAD.MonHoc.rdlc";
            //ReportViewerDemo.LocalReport.ReportPath = "\\RDLC_Report\\GiaoVien.rdlc";

            //Get information about School such as school's name and principal's name
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select * from THONGTINTRG";
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    strName = dr.GetString(1);
                    strHieuTruong = dr.GetString(3);
                }

            }
            catch (SqlException)
            {
                MessageBox.Show("Có lỗi trong quá trình liên kết cơ sở dữ liệu");
            }

            conn.Close();




            //Parameters
            ReportParameter[] rptParameter = new ReportParameter[]
            {
                new ReportParameter("TenTruong", strName.ToUpper()),
                new ReportParameter("TenHT", strHieuTruong),
            };
            ReportViewerDemo.LocalReport.SetParameters(rptParameter);


            ReportViewerDemo.RefreshReport();

        }

        private DataTable GetData()
        {
            DataTable dtDataTable = new DataTable();

            try
            {
                string strCommand = "SELECT MAMH, TENMH, HESO1, HESO2 FROM MONHOC";
                SqlDataAdapter daDataAdapter = new SqlDataAdapter(strCommand, Connection.strConnectionString);
                //daDataAdapter = cmd.ExecuteNonQuery();
                daDataAdapter.Fill(dtDataTable);
            }
            catch (SqlException)
            {
                MessageBox.Show("Có lỗi trong quá trình tạo báo cáo, mời bạn thử lại", "Thông báo", MessageBoxButton.OK);
            }

            //conn.Close();
            return dtDataTable;

        }

    }
}
