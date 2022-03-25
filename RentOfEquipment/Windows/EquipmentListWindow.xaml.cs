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
    /// Логика взаимодействия для EquipmentListWindow.xaml
    /// </summary>
    public partial class EquipmentListWindow : Window
    {
        public EquipmentListWindow()
        {
            InitializeComponent();
            lvEquipment.ItemsSource = ClassHelper.AppData.Context.Equipment.ToList();
        }

        private void lvEquipment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                var resClick = MessageBox.Show("Удалить выбранное оборудование?", "Подтверждение", MessageBoxButton.YesNo);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                if (lvEquipment.SelectedItem is EF.Equipment)
                {
                    var equip = lvEquipment.SelectedItem as EF.Equipment;
                    ClassHelper.AppData.Context.Equipment.Remove(equip);
                    ClassHelper.AppData.Context.SaveChanges();
                    MessageBox.Show("Успешно удалено", "Готово", MessageBoxButton.OK);
                    
                }
            }
        }
    }
}
