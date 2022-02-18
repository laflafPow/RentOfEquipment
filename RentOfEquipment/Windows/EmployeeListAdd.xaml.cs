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
        public EmployeeListAdd()
        {
            InitializeComponent();
            cbRole.ItemsSource = ClassHelper.AppData.Context.Role.ToList();
            cbRole.DisplayMemberPath = "Name";
            cbRole.SelectedIndex = 0;


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



            try
            {
                EF.Employee newEmployee = new EF.Employee();
                newEmployee.LastName = txtLastName.Text;
                newEmployee.FirstName = txtFirstName.Text;
                newEmployee.MiddleName = txtMiddleName.Text;
                newEmployee.Phone = txtPhone.Text;
                newEmployee.Login = txtLogin.Text;
                newEmployee.Password = txtPassword.Password;
                newEmployee.IdRole = (cbRole.SelectedItem as EF.Role).Id;
                ClassHelper.AppData.Context.Employee.Add(newEmployee);
                
            }

            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
