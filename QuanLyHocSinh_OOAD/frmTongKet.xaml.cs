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
using System.Data;
using Microsoft.Reporting.WinForms;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for frmTongKet.xaml
    /// </summary>
    public partial class frmTongKet : Window
    {
        SqlConnection conn = Connection.KetNoi();
        string strName, strHieuTruong, lop, nienKhoa;
        int hocki;
        string manienkhoa;
        public frmTongKet(int hk, string l, string nienkhoa)
        {
            InitializeComponent();
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
            this.hocki = hk;
            this.lop = l;
            this.manienkhoa = nienkhoa;

            SqlDataAdapter sdaDataAdapter = new SqlDataAdapter("select TENNAMHOC from NAMHOC WHERE MANAMHOC = '" + nienkhoa + "'", Connection.strConnectionString);
            DataTable dtDataTable = new DataTable();
            dtDataTable.Clear();
            sdaDataAdapter.Fill(dtDataTable);

            this.nienKhoa = dtDataTable.Rows[0][0].ToString();
            Load();
        }

        private void Load()
        {
            ReportViewerDemo.Reset();
            DataTable dt = GetData();

            ReportDataSource dsDataSource = new ReportDataSource("DataSet1", dt); //ReportDataSource("", dt);
            ReportViewerDemo.LocalReport.DataSources.Add(dsDataSource);

            ReportViewerDemo.LocalReport.ReportEmbeddedResource = "QuanLyHocSinh_OOAD.TongKet.rdlc";

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
                new ReportParameter("Lop", lop),
                new ReportParameter("NienKhoa", nienKhoa),
                new ReportParameter("HocKi", hocki.ToString())
            };
            ReportViewerDemo.LocalReport.SetParameters(rptParameter);
            ReportViewerDemo.RefreshReport();
        }

        private DataTable GetData()
        {

            DataTable dtDataTable = new DataTable();

            try
            {
                string strCommand = string.Concat("SELECT  HOCSINH.MAHS, HOCSINH.HOTEN AS 'HOCKY', TONGKET.DTB_HOCKY, TONGKET.XEPLOAI FROM HOCSINH INNER JOIN TONGKET ON HOCSINH.MAHS = TONGKET.MAHS WHERE TONGKET.HOCKY = '" + hocki + "' AND HOCSINH.MALOP = '" + lop + "'AND TONGKET.NAMHOC = '" + manienkhoa +"'");

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
