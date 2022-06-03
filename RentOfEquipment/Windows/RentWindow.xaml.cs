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
using RentOfEquipment.ClassHelper;
using LibraryClass;

namespace RentOfEquipment.Windows
{
    /// <summary>
    /// Логика взаимодействия для RentWindow.xaml
    /// </summary>
    public partial class RentWindow : Window
    {
        public RentWindow()
        {
            InitializeComponent();
        }

        private void btnSelectUser_Click(object sender, RoutedEventArgs e)
        {
            SelectClientWindow selectClientWindow = new SelectClientWindow();
            selectClientWindow.ShowDialog();

            if (DataTransmissionClass.GetClient != null)
            {
                tbClient.Text = DataTransmissionClass.GetClient.FIO;
            }

        }

        private void btnSelectEmployee_Click(object sender, RoutedEventArgs e)
        {
            SelectEmployeeWindow selectEmployeeWindow = new SelectEmployeeWindow();
            selectEmployeeWindow.ShowDialog();

            if (DataTransmissionClass.GetEmployee != null)
            {
                tbEmployee.Text = DataTransmissionClass.GetEmployee.FIO;
            }
        }

        private void btnSelectEquipment_Click(object sender, RoutedEventArgs e)
        {
            SelectEquipmentWindow selectEquipmentWindow = new SelectEquipmentWindow();
            selectEquipmentWindow.ShowDialog();

            if (DataTransmissionClass.GetEquipment != null)
            {
                tbEquipment.Text = DataTransmissionClass.GetEquipment.Name;
                tbQty.Text = DataTransmissionClass.GetEquipment.QtyInWarehouse.ToString();
                CostUpdate();
            }


        }

        private void dpStartRent_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tbDateStart.Text = dpStartRent.SelectedDate.Value.ToShortDateString();
            CostUpdate();
        }

        private void dpEndRent_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tbEndRent.Text = dpEndRent.SelectedDate.Value.ToShortDateString();
            CostUpdate();
        }

        public void CostUpdate()
        {
            if (dpStartRent.SelectedDate != null && dpEndRent.SelectedDate != null && DataTransmissionClass.GetEquipment != null && txtQty.Text != "" && Convert.ToInt32(txtQty.Text) > 0)
            {
                int DateStart = dpStartRent.SelectedDate.Value.DayOfYear;
                int DateEnd = dpEndRent.SelectedDate.Value.DayOfYear;
                decimal Cost = DataTransmissionClass.GetEquipment.Price;

                if (dpEndRent.SelectedDate.Value < dpStartRent.SelectedDate.Value)
                {
                    tbTotalCost.Text = "0";
                }
                else
                {
                    tbTotalCost.Text = CalculationClass.CostOfRent(CalculationClass.DaysInRent(DateStart, DateEnd), Cost, Convert.ToInt32(txtQty.Text)).ToString();
                }
            }
        }

        private void txtQty_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "1234567890".IndexOf(e.Text) < 0;
        }

        private void txtQty_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Int32.TryParse(txtQty.Text, out int res) == true)
            {
                if (Convert.ToInt32(txtQty.Text) <= 0)
                {
                    MessageBox.Show("Вы не можете арендовать меньше 1-го товара!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                if (tbQty.Text != "")
                {
                    if (Convert.ToInt32(txtQty.Text) > Convert.ToInt32(tbQty.Text))
                    {
                        MessageBox.Show("Такого количества нет на складе!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
            CostUpdate();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (DataTransmissionClass.GetClient == null)
            {
                MessageBox.Show("Необходимо выбрать клиента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DataTransmissionClass.GetEmployee == null)
            {
                MessageBox.Show("Необходимо выбрать сотрудника!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DataTransmissionClass.GetEquipment == null)
            {
                MessageBox.Show("Необходимо выбрать оборудование!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtQty.Text == "")
            {
                MessageBox.Show("Поле \"Количество\" не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Convert.ToInt32(txtQty.Text) > Convert.ToInt32(tbQty.Text) ||
                Convert.ToInt32(txtQty.Text) <= 0)
            {
                MessageBox.Show("Такого количества нет на складе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpStartRent.SelectedDate == null)
            {
                MessageBox.Show("Поле \"Дата начала\" не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpEndRent.SelectedDate == null)
            {
                MessageBox.Show("Поле \"Дата окончания\" не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpStartRent.SelectedDate.Value < DateTime.Now &&
                dpStartRent.SelectedDate.Value.DayOfYear != DateTime.Now.DayOfYear)
            {
                MessageBox.Show("Дата в поле \"Дата начала\" должно быть в настоящем или будущем времени", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpEndRent.SelectedDate.Value < dpStartRent.SelectedDate.Value)
            {
                MessageBox.Show("Дата в поле \"Дата окончания\" должна быть позже даты начала", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var resClick = MessageBox.Show("Добавить запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resClick == MessageBoxResult.No)
            {
                return;
            }

            EF.EquipmentSale rent = new EF.EquipmentSale();
            rent.IdClient = DataTransmissionClass.GetClient.Id;
            rent.IdEmployee = DataTransmissionClass.GetEmployee.Id;
            rent.IdEquipment = DataTransmissionClass.GetEquipment.Id;
            rent.StartRentDate = Convert.ToDateTime(dpStartRent.SelectedDate);
            rent.EndRentDate = Convert.ToDateTime(dpEndRent.SelectedDate);
            rent.TotalCost = Convert.ToDecimal(tbTotalCost.Text);
            ClassHelper.AppData.Context.EquipmentSale.Add(rent);
            ClassHelper.AppData.Context.SaveChanges();
            MessageBox.Show("Запись об аренде успешно добавлена!");

            DataTransmissionClass.GetClient = null;
            DataTransmissionClass.GetEmployee = null;
            DataTransmissionClass.GetEquipment = null;

            this.Close();
        }

        private void txtQty_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
