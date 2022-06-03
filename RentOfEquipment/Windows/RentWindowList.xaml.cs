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

    public partial class RentWindowList : Window
    {
        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По цене",
            "По количеству сдаваемого оборудования",
            "Сначала новые",
            "Дата аренды подходит к концу",
        };

        public RentWindowList()
        {
            InitializeComponent();
            Filter();
            cbSort.ItemsSource = listSort;
            cbSort.SelectedIndex = 0;
        }

        private void Filter()
        {
            List<EF.EquipmentSale> listEquipmentSale = new List<EF.EquipmentSale>();

            listEquipmentSale = ClassHelper.AppData.Context.EquipmentSale.ToList();

            listEquipmentSale = listEquipmentSale.
                Where(i => i.Equipment.Name.ToLower().Contains(txtSearch.Text.ToLower())
                || (i.IsDeleted != false)).
                ToList();

            switch (cbSort.SelectedIndex)
            {
                case 0:
                    listEquipmentSale = listEquipmentSale.OrderBy(i => i.Id).ToList();
                    break;

                case 1:
                    listEquipmentSale = listEquipmentSale.OrderBy(i => i.TotalCost).ToList();
                    break;

                case 2:
                    listEquipmentSale = listEquipmentSale.OrderBy(i => i.Qty).ToList();
                    break;

                case 3:
                    listEquipmentSale = listEquipmentSale.OrderBy(i => i.StartRentDate).ToList();
                    break;

                case 4:
                    listEquipmentSale = listEquipmentSale.OrderBy(i => i.EndRentDate).ToList();
                    break;

                default:
                    listEquipmentSale = listEquipmentSale.OrderBy(i => i.Id).ToList();
                    break;
            }

            lvRent.ItemsSource = listEquipmentSale;
        }

        private void btnAddRent_Click(object sender, RoutedEventArgs e)
        {
            RentWindow rentWindow = new RentWindow();
            rentWindow.ShowDialog();
            Filter();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void lvRent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                var resClick = MessageBox.Show("Удалить аренда?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    if (lvRent.SelectedItem is EF.EquipmentSale)
                    {
                        var rent = lvRent.SelectedItem as EF.EquipmentSale;
                        rent.IsDeleted = true;
                        ClassHelper.AppData.Context.SaveChanges();
                        MessageBox.Show("Аренда успешна удалена", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                        Filter();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void lvRent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvRent.SelectedItem is EF.EquipmentSale)
            {
                var rent = lvRent.SelectedItem as EF.EquipmentSale;

                ReturnRentWindow returnRentWindow = new ReturnRentWindow(rent);
                returnRentWindow.ShowDialog();
                Filter();

            }
        }
    }
}
