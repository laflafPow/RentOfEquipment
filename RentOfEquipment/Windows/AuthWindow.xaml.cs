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

namespace RentOfEquipment.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            var authUser = ClassHelper.AppData.Context.Employee.ToList().
                Where(i => i.Login == txtLogin.Text && i.Password == txtPassword.Text).
                FirstOrDefault();

            if (authUser != null)
            {
                MainWindow mainWindow = new MainWindow(authUser);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }

        private void txtLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text == "Логин")
            {
                txtLogin.FontStyle = FontStyles.Normal;
                txtLogin.Foreground = Brushes.Black;
                txtLogin.Text = null;
            }
        }

        private void txtLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text == "")
            {
                txtLogin.FontStyle = FontStyles.Italic;
                txtLogin.Foreground = Brushes.LightGray;
                txtLogin.Text = "Логин";
            }
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text == "Пароль")
            {
                txtPassword.FontStyle = FontStyles.Normal;
                txtPassword.Foreground = Brushes.Black;
                txtPassword.Text = null;
            }
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.FontStyle = FontStyles.Italic;
                txtPassword.Foreground = Brushes.LightGray;
                txtPassword.Text = "Пароль";
            }
        }
    }
}
