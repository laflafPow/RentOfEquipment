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
    /// Логика взаимодействия для EquipmentSaleList.xaml
    /// </summary>
    public partial class EquipmentSaleList : Window
    {
        public EquipmentSaleList()
        {
            InitializeComponent();
            lvEquipmentSale.ItemsSource = ClassHelper.AppData.Context.EquipmentSale.ToList();
        }
    }
}
