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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RentOfEquipment.Windows;

namespace RentOfEquipment
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnEmployeeList_Click(object sender, RoutedEventArgs e)
        {
            EmployeeListWindow employeeListWindow = new EmployeeListWindow();
            this.Hide();
            employeeListWindow.ShowDialog();
            this.Show();
        }

        private void btnClientList_Click(object sender, RoutedEventArgs e)
        {
            ClientListWindow clientListWindow = new ClientListWindow();
            this.Hide();
            clientListWindow.ShowDialog();
            this.Show();
        }

        private void btnEquipmentList_Click(object sender, RoutedEventArgs e)
        {
            EquipmentListWindow equipmentListWindow = new EquipmentListWindow();
            this.Hide();
            equipmentListWindow.ShowDialog();
            this.Show();
        }

        private void btnRent_Click(object sender, RoutedEventArgs e)
        {
            RentWindowList rentWindowList = new RentWindowList();
            this.Hide();
            rentWindowList.ShowDialog();
            this.Show();
        }
    }
}
