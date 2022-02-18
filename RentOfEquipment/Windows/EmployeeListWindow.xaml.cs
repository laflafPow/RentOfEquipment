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
    /// Логика взаимодействия для EmployeeListWindow.xaml
    /// </summary>
    public partial class EmployeeListWindow : Window
    {
        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По фамилии",
            "По имени",
            "По электронной почте",
            "По должности",
        };

        public EmployeeListWindow()
        {
            InitializeComponent();
            Filter();
            cbSort.ItemsSource = listSort;
            cbSort.SelectedIndex = 0;
        }

        private void Filter()
        {
            List<EF.Employee> listEmployee = new List<EF.Employee>();

            listEmployee = ClassHelper.AppData.Context.Employee.ToList();

            listEmployee = listEmployee.Where(i => i.LastName.ToLower().Contains(txtSearch.Text.ToLower()) 
            || i.LastName.ToLower().Contains(txtSearch.Text.ToLower())).
                ToList();

            lvEmployee.ItemsSource = listEmployee;
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            EmployeeListAdd employeeListAdd = new EmployeeListAdd();
            employeeListAdd.ShowDialog();
        }

        private void lvEmployee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
