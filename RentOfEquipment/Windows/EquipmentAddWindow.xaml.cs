using RentOfEquipment.ClassHelper;
using System;
using System.Collections.Generic;
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

namespace RentOfEquipment.Windows
{
    /// <summary>
    /// Логика взаимодействия для EquipmentAddWindow.xaml
    /// </summary>
    public partial class EquipmentAddWindow : Window
    {
        bool isEdit = false;
        EF.Equipment editEquipment = new EF.Equipment();
        string pathPhoto = null;

        public EquipmentAddWindow()
        {
            InitializeComponent();
            cbGender.ItemsSource = AppData.Context.Gender.ToList();
            cbGender.DisplayMemberPath = "Name";
            cbGender.SelectedIndex = 0;

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

        public EquipmentAddWindow(EF.Equipment equipment)
        {
            InitializeComponent();
            cbGender.ItemsSource = ClassHelper.AppData.Context.Gender.ToList();
            cbGender.DisplayMemberPath = "Name";

            txtLastName.Text = equipment.Name;
            txtFirstName.Text = equipment.FirstName;
            txtMiddleName.Text = equipment.MiddleName;
            txtPhone.Text = equipment.Phone;
            txtEmail.Text = equipment.Email;
            dpBirthdate.SelectedDate = equipment.Birthdate;

            // ?*@?*.?*

            cbGender.SelectedIndex = client.IdGender - 1;

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

            tbTitle.Text = "Изменение данных клиента";
            btnClientAdd.Content = "Сохранить";

            isEdit = true;

            editClient = client;
        }


        private void btnClientAdd_Click(object sender, RoutedEventArgs e)
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

            if (!Int64.TryParse(txtPhone.Text, out long res))
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

            if (dpBirthdate.SelectedDate.HasValue == false)
            {
                MessageBox.Show("Ошибка в поле даты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
                    editClient.Birthdate = dpBirthdate.SelectedDate.Value;

                    // ?*@?*.?*

                    editClient.IdGender = (cbGender.SelectedItem as EF.Gender).Id;

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
                    EF.Client newClient = new EF.Client();

                    newClient.LastName = txtLastName.Text;
                    newClient.FirstName = txtFirstName.Text;
                    newClient.MiddleName = txtMiddleName.Text;
                    newClient.Phone = txtPhone.Text;
                    newClient.Email = txtEmail.Text;
                    newClient.IdGender = (cbGender.SelectedItem as EF.Gender).Id;
                    newClient.Birthdate = dpBirthdate.SelectedDate.Value;

                    if (pathPhoto != null)
                    {
                        newClient.Photo = File.ReadAllBytes(pathPhoto);
                    }

                    ClassHelper.AppData.Context.Client.Add(newClient);
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
