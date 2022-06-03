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
using LibraryClass;

namespace RentOfEquipment.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReturnRentWindow.xaml
    /// </summary>
    public partial class ReturnRentWindow : Window
    {
        EF.EquipmentSale returnRent = new EF.EquipmentSale();
        public ReturnRentWindow(EF.EquipmentSale rent)
        {
            InitializeComponent();
            tbClient.Text = rent.Client.FIO;
            tbEmployee.Text = rent.Employee.FIO;
            tbEquipment.Text = rent.Equipment.Name;
            tbDateStart.Text = rent.StartRentDate.ToString();
            tbEndRent.Text = rent.EndRentDate.ToString();
            tbTotalCost.Text = rent.TotalCost.ToString();

            returnRent = rent;
        }

        private void dpFactRent_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CalculationClass.DaysInRent(returnRent.StartRentDate.DayOfYear, returnRent.EndRentDate.DayOfYear) - CalculationClass.DaysInRent(returnRent.StartRentDate.DayOfYear, dpFactRent.SelectedDate.Value.DayOfYear) == 0)
            {
                tbTotalCost.Text = returnRent.TotalCost.ToString();
            }
            else if (CalculationClass.DaysInRent(returnRent.StartRentDate.DayOfYear, returnRent.EndRentDate.DayOfYear) - CalculationClass.DaysInRent(returnRent.StartRentDate.DayOfYear, dpFactRent.SelectedDate.Value.DayOfYear) > 0)
            {
                tbTotalCost.Text = CalculationClass.Discount(returnRent.StartRentDate.DayOfYear, returnRent.EndRentDate.DayOfYear, returnRent.Equipment.Price, dpFactRent.SelectedDate.Value.DayOfYear).ToString();
            }
            else
            {
                tbTotalCost.Text = CalculationClass.Penality(returnRent.StartRentDate.DayOfYear, returnRent.EndRentDate.DayOfYear, returnRent.Equipment.Price, dpFactRent.SelectedDate.Value.DayOfYear).ToString();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dpFactRent.SelectedDate == null)
            {
                MessageBox.Show("Поле сдачи аренды не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var resClick = MessageBox.Show("Сдать товар в аренду?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resClick == MessageBoxResult.No)
            {
                return;
            }

            returnRent.TotalCost = Convert.ToDecimal(tbTotalCost.Text);
            returnRent.IsDeleted = true;

            ClassHelper.AppData.Context.SaveChanges();
            MessageBox.Show("Товар возвращен");

            this.Close();
        }
    }
}
