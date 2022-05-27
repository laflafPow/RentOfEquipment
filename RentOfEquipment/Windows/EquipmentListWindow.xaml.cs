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
using RentOfEquipment.Windows;

namespace RentOfEquipment.Windows
{
    public partial class EquipmentListWindow : Window
    {
        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По цене",
            "По имени",
            "По типу оборудования",
            "По изношенности",
        };

        public EquipmentListWindow()
        {
            InitializeComponent();
            Filter();
            cbSort.ItemsSource = listSort;
            cbSort.SelectedIndex = 0;
        }

        private void Filter()
        {
            List<EF.Equipment> listEquipment = new List<EF.Equipment>();

            listEquipment = ClassHelper.AppData.Context.Equipment.ToList();

            listEquipment = listEquipment.
                Where(i => i.Name.ToLower().Contains(txtSearch.Text.ToLower())                
                || (i.IsDeleted != false)).
                ToList();

            switch (cbSort.SelectedIndex)
            {
                case 0:
                    listEquipment = listEquipment.OrderBy(i => i.Id).ToList();
                    break;

                case 1:
                    listEquipment = listEquipment.OrderBy(i => i.Price).ToList();
                    break;

                case 2:
                    listEquipment = listEquipment.OrderBy(i => i.Name).ToList();
                    break;

                case 3:
                    listEquipment = listEquipment.OrderBy(i => i.IdType).ToList();
                    break;

                case 4:
                    listEquipment = listEquipment.OrderBy(i => i.ProductLive).ToList();
                    break;

                default:
                    listEquipment = listEquipment.OrderBy(i => i.Id).ToList();
                    break;
            }

            lvEquipment.ItemsSource = listEquipment;
        }

        private void btnAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            EquipmentAddWindow eqiupmentAddWindow = new EquipmentAddWindow();
            eqiupmentAddWindow.ShowDialog();
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

        private void lvEquipment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                var resClick = MessageBox.Show("Удалить оборудование?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    if (lvEquipment.SelectedItem is EF.Equipment)
                    {
                        var equipment = lvEquipment.SelectedItem as EF.Equipment;
                        equipment.IsDeleted = true;
                        ClassHelper.AppData.Context.SaveChanges();
                        MessageBox.Show("Пользователь успешно удален", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                        Filter();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void lvEquipment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvEquipment.SelectedItem is EF.Client)
            {
                var equipment = lvEquipment.SelectedItem as EF.Equipment;

                EquipmentAddWindow equipmentAddWindow = new EquipmentAddWindow(equipment);
                equipmentAddWindow.ShowDialog();
                Filter();

            }
        }
    }
}
