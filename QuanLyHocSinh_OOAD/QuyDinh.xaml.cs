using System;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for QuyDinh.xaml
    /// </summary>
    public partial class QuyDinh : Window
    {
        //public static string strConnectionString = "Data Source=DESKTOP-DLT0AO8;Initial Catalog=QLHS;Integrated Security=True";
        //SqlConnection conn = new SqlConnection(strConnectionString);
        SqlConnection conn = Connection.KetNoi();
        int iCount = 0;
        int[] str = new int [4];
        public QuyDinh()
        {
            InitializeComponent();
            LoadComponent();
        }

        private void LoadComponent()
        {
            iCount = 0;
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from QUYDINH";

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    str[iCount] = dr.GetByte(2);
                    iCount++;
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();

            txtTuoiMin.Text = str[0].ToString();
            txtTuoiMax.Text = str[1].ToString();
            txtSiSoMax.Text = str[2].ToString();
            txtDiemDat.Text = str[3].ToString();
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
                cmd.CommandText = "update QUYDINH set SL = @SL where MAQD = @MAQD";
                cmd.Parameters.AddWithValue("@SL", Convert.ToInt32(txtTuoiMin.Text));
                cmd.Parameters.AddWithValue("@MAQD", "QD01");
                cmd.ExecuteNonQuery();

                cmd.CommandText = "update QUYDINH set SL = @_SL where MAQD = @_MAQD";
                cmd.Parameters.AddWithValue("@_SL", Convert.ToInt32(txtTuoiMax.Text));
                cmd.Parameters.AddWithValue("@_MAQD", "QD02");
                cmd.ExecuteNonQuery();

                cmd.CommandText = "update QUYDINH set SL = @__SL where MAQD = @__MAQD";
                cmd.Parameters.AddWithValue("@__SL", Convert.ToInt32(txtSiSoMax.Text));
                cmd.Parameters.AddWithValue("@__MAQD", "QD03");
                cmd.ExecuteNonQuery();

                cmd.CommandText = "update QUYDINH set SL = @___SL where MAQD = @___MAQD";
                cmd.Parameters.AddWithValue("@___SL", Convert.ToInt32(txtDiemDat.Text));
                cmd.Parameters.AddWithValue("@___MAQD", "QD04");
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();

            LoadComponent();
        }
    }
}
