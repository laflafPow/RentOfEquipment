using Microsoft.Win32;
using RentOfEquipment.ClassHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
    /// Логика взаимодействия для EquipmentAddWindow.xaml
    /// </summary>
    public partial class EquipmentAddWindow : Window
    {
        bool isEdit = false;
        EF.Equipment editEquipment = new EF.Equipment();
        string pathPhoto = null;

        public EquipmentAddWindow()
        {
            InitializeComponent();
            cmbEquipmentType.ItemsSource = AppData.Context.TypeEquipment.ToList();
            cmbEquipmentType.DisplayMemberPath = "Name";
            cmbEquipmentType.SelectedIndex = 0;

            cmbRentStatus.ItemsSource = new List<String>
            {
                "Сдается",
                "Не активен"
            };
            cmbRentStatus.SelectedIndex = 0;

            isEdit = false;
        }

        public EquipmentAddWindow(EF.Equipment equipment)
        {
            InitializeComponent();
            cmbEquipmentType.ItemsSource = AppData.Context.TypeEquipment.ToList();
            cmbEquipmentType.DisplayMemberPath = "Name";

            cmbRentStatus.ItemsSource = new List<String>
            {
                "Сдается",
                "Не активен"
            };

            txtEquipmentName.Text = equipment.Name;
            txtPrice.Text = ((int)(equipment.Price)).ToString();
            txtProductLive.Text = equipment.ProductLive.ToString();
            txtQtyInWarehouse.Text = equipment.QtyInWarehouse.ToString();

            if (equipment.Status)
            {
                cmbRentStatus.SelectedIndex = 0;
            }
            else cmbRentStatus.SelectedIndex = 1;

            cmbEquipmentType.SelectedIndex = equipment.IdType - 1;

            if (equipment.Photo != null)
            {
                using (MemoryStream stream = new MemoryStream(equipment.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    photoUser.Source = bitmapImage;
                }
            }

            tbTitle.Text = "Изменение данных оборудования";
            btnEquipmentAdd.Content = "Сохранить";

            isEdit = true;

            editEquipment = equipment;
        }

        private void btnEquipmentAdd_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(txtEquipmentName.Text))
            {
                MessageBox.Show("Пустые значения в поле Имя оборудования", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Пустые значения в поле Цена", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Double.TryParse(txtPrice.Text, out double res))
            {
                MessageBox.Show("В поле Цена возможны только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtProductLive.Text))
            {
                MessageBox.Show("Пустые значения в поле Изношенность (в годах)", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Byte.TryParse(txtProductLive.Text, out byte result))
            {
                MessageBox.Show("В поле Изношенность (в годах) возможны только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrWhiteSpace(txtQtyInWarehouse.Text))
            {
                MessageBox.Show("Пустые значения в поле Количество на складе", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Int32.TryParse(txtQtyInWarehouse.Text, out int resulto))
            {
                MessageBox.Show("В поле Количество на складе возможны только цифры", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (isEdit)
            {
                var resClick = MessageBox.Show("Изменить оборудование?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    editEquipment.Name = txtEquipmentName.Text;
                    editEquipment.Price = Convert.ToInt32(txtPrice.Text);
                    editEquipment.ProductLive = Convert.ToByte(txtProductLive.Text);
                    editEquipment.QtyInWarehouse = Convert.ToInt32(txtQtyInWarehouse.Text);

                    editEquipment.IdType = (cmbEquipmentType.SelectedItem as EF.TypeEquipment).Id;
                    if (cmbRentStatus.SelectedIndex == 0)
                    {
                        editEquipment.Status = true;
                    }
                    else editEquipment.Status = false;

                    if (pathPhoto != null)
                    {
                        editEquipment.Photo = File.ReadAllBytes(pathPhoto);
                    }

                    ClassHelper.AppData.Context.SaveChanges();

                    MessageBox.Show("Оборудование изменено");
                    this.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                var resClick = MessageBox.Show("Добавить оборудование?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    EF.Equipment newEquipment = new EF.Equipment();

                    newEquipment.Name = txtEquipmentName.Text;
                    newEquipment.Price = Convert.ToInt32(txtPrice.Text);
                    newEquipment.ProductLive = Convert.ToByte(txtProductLive.Text);
                    newEquipment.QtyInWarehouse = Convert.ToInt32(txtQtyInWarehouse.Text);

                    newEquipment.IdType = (cmbEquipmentType.SelectedItem as EF.TypeEquipment).Id;
                    if (cmbRentStatus.SelectedIndex == 0)
                    {
                        newEquipment.Status = true;
                    }
                    else newEquipment.Status = false;

                    if (pathPhoto != null)
                    {
                        newEquipment.Photo = File.ReadAllBytes(pathPhoto);
                    }

                    ClassHelper.AppData.Context.Equipment.Add(newEquipment);
                    ClassHelper.AppData.Context.SaveChanges();

                    MessageBox.Show("Оборудование добавлено");

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Files|*.jpg;*.jpeg;*.png;";
            if (openFile.ShowDialog() == true)
            {
                photoUser.Source = new BitmapImage(new Uri(openFile.FileName));
                pathPhoto = openFile.FileName;
            }
        }
    }
}
