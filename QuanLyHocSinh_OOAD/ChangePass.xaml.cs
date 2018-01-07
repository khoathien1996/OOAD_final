using System;
using System.Windows;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for ChangePass.xaml
    /// </summary>
    public partial class ChangePass : Window
    {
        public ChangePass()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.Compare(matKhaumoi.Password, matKhaumoi1.Password) == 0)
            {
                Password TaiKhoan = new Password();
                TaiKhoan.ThayDoiPass(tenDN.Text, matKhau.Password, matKhaumoi.Password);
                MessageBox.Show("Mật khẩu đã được thay đổi!");
            }
            else
            {
                MessageBox.Show("Cần xác nhận lại mật khẩu mới", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }
        private void ButtonThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
