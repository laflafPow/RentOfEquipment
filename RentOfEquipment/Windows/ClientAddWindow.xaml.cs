using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
using Microsoft.Win32;
using RentOfEquipment.ClassHelper;

namespace RentOfEquipment.Windows
{
    /// <summary>
    /// Логика взаимодействия для ClientAddWindow.xaml
    /// </summary>
    public partial class ClientAddWindow : Window
    {
        bool isEdit = false;
        EF.Client editClient = new EF.Client();
        string pathPhoto = null;

        public ClientAddWindow()
        {
            InitializeComponent();
            cbRole.ItemsSource = ClassHelper.AppData.Context.Role.ToList();
            cbRole.DisplayMemberPath = "Name";
            cbRole.SelectedIndex = 0;

            isEdit = false;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public ClientAddWindow(EF.Client client)
        {
            InitializeComponent();
            cbRole.ItemsSource = ClassHelper.AppData.Context.Role.ToList();
            cbRole.DisplayMemberPath = "Name";

            txtLastName.Text = client.LastName;
            txtFirstName.Text = client.FirstName;
            txtMiddleName.Text = client.MiddleName;
            txtPhone.Text = client.Phone;
            txtEmail.Text = client.Email;

            // ?*@?*.?*

            cbRole.SelectedIndex = client.IdGender - 1;

            if (client.Photo != null)
            {
                using (MemoryStream stream = new MemoryStream(client.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    photoUser.Source = bitmapImage;
                }
            }

            tbTitle.Text = "Изменение данных работника";
            btnEmployeeAdd.Content = "Сохранить";

            isEdit = true;

            editClient = client;
        }


        private void btnEmployeeAdd_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Пустые значения в поле Фамилия", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Пустые значения в поле Имя", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Пустые значения в поле Телефон", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Пустые значения в поле логин", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Int32.TryParse(txtPhone.Text, out int res))
            {
                MessageBox.Show("В поле телефон возможны только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtEmail.Text != "")
            {
                if (IsValidEmail(txtEmail.Text) == false)
                {
                    MessageBox.Show("E-mail не соответсвует маске (?*@?*.?*)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (isEdit)
            {
                var resClick = MessageBox.Show("Изменить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    editClient.LastName = txtLastName.Text;
                    editClient.FirstName = txtFirstName.Text;
                    editClient.MiddleName = txtMiddleName.Text;
                    editClient.Phone = txtPhone.Text;
                    editClient.Email = txtEmail.Text;

                    // ?*@?*.?*

                    editClient.IdGender = (cbRole.SelectedItem as EF.Gender).Id;

                    if (pathPhoto != null)
                    {
                        editClient.Photo = File.ReadAllBytes(pathPhoto);
                    }

                    ClassHelper.AppData.Context.SaveChanges();

                    MessageBox.Show("Пользователь изменен");
                    this.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                var resClick = MessageBox.Show("Добавить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    EF.Employee newEmployee = new EF.Employee();

                    newEmployee.LastName = txtLastName.Text;
                    newEmployee.FirstName = txtFirstName.Text;
                    newEmployee.MiddleName = txtMiddleName.Text;
                    newEmployee.Phone = txtPhone.Text;
                    newEmployee.Email = txtEmail.Text;
                    newEmployee.IdRole = (cbRole.SelectedItem as EF.Role).Id;
                    newEmployee.Login = txtLogin.Text;
                    newEmployee.Password = txtPassword.Password;

                    if (pathPhoto != null)
                    {
                        newEmployee.Photo = File.ReadAllBytes(pathPhoto);
                    }

                    ClassHelper.AppData.Context.Employee.Add(newEmployee);
                    ClassHelper.AppData.Context.SaveChanges();

                    MessageBox.Show("Пользователь добавлен");

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Files|*.jpg;*.jpeg;*.png;";
            if (openFile.ShowDialog() == true)
            {
                photoUser.Source = new BitmapImage(new Uri(openFile.FileName));
                pathPhoto = openFile.FileName;
            }
        }
    }
}
