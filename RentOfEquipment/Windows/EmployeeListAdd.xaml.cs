using RentOfEquipment.ClassHelper;
using RentOfEquipment.EF;
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
    /// Логика взаимодействия для EmployeeListAdd.xaml
    /// </summary>
    public partial class EmployeeListAdd : Window
    {
        bool isEdit = false;
        EF.Employee editEmployee = new EF.Employee();

        public EmployeeListAdd()
        {
            InitializeComponent();
            cbRole.ItemsSource = ClassHelper.AppData.Context.Role.ToList();
            cbRole.DisplayMemberPath = "Name";
            cbRole.SelectedIndex = 0;

            isEdit = false;
        }

        public EmployeeListAdd(EF.Employee employee)
        {
            InitializeComponent();
            cbRole.ItemsSource = ClassHelper.AppData.Context.Role.ToList();
            cbRole.DisplayMemberPath = "Name";

            txtLastName.Text = employee.LastName;
            txtFirstName.Text = employee.FirstName;
            txtMiddleName.Text = employee.MiddleName;
            txtPhone.Text = employee.Phone;
            txtEmail.Text = employee.Email;

            // ?*@?*.?*
            txtLogin.Text = employee.Login;
            txtPassword.Password = employee.Password;

            cbRole.SelectedIndex = employee.IdRole - 1;


            tbTitle.Text = "Изменение данных работника";
            btnEmployeeAdd.Content = "Сохранить";

            isEdit = true;

            editEmployee = employee;
        }


        private void btnEmployeeAdd_Click(object sender, RoutedEventArgs e)
        {
            var user = AppData.Context.Employee.ToList().
                Where(i => i.Login == txtLogin.Text).FirstOrDefault();

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

            if (user != null)
            {
                MessageBox.Show("Логин занят!", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Пустые значения в поле пароль", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Int32.TryParse(txtPhone.Text, out int res))
            {
                MessageBox.Show("Недопустимые символы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    editEmployee.LastName = txtLastName.Text;
                    editEmployee.FirstName = txtFirstName.Text;
                    editEmployee.MiddleName = txtMiddleName.Text;
                    editEmployee.Phone = txtPhone.Text;
                    editEmployee.Email = txtEmail.Text;

                    // ?*@?*.?*

                    editEmployee.IdRole = (cbRole.SelectedItem as EF.Role).Id;
                    editEmployee.Login = txtLogin.Text;
                    editEmployee.Password = txtPassword.Password;

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

    }
}
