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
    /// Логика взаимодействия для SelectClientWindow.xaml
    /// </summary>
    public partial class SelectClientWindow : Window
    {
        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По фамилии",
            "По имени",
            "По электронной почте",
            "По полу",
        };

        public SelectClientWindow()
        {
            InitializeComponent();
            Filter();
            cbSort.ItemsSource = listSort;
            cbSort.SelectedIndex = 0;
        }

        private void Filter()
        {
            List<EF.Client> listClient = new List<EF.Client>();

            listClient = ClassHelper.AppData.Context.Client.ToList();

            listClient = listClient.
                Where(i => i.LastName.ToLower().Contains(txtSearch.Text.ToLower())
                || i.FirstName.ToLower().Contains(txtSearch.Text.ToLower())
                || i.MiddleName.ToLower().Contains(txtSearch.Text.ToLower())
                || i.FIO.ToLower().Contains(txtSearch.Text.ToLower())
                || i.Phone.ToLower().Contains(txtSearch.Text.ToLower())
                || i.Email.ToLower().Contains(txtSearch.Text.ToLower())
                || (i.IsDeleted != false)).
                ToList();

            switch (cbSort.SelectedIndex)
            {
                case 0:
                    listClient = listClient.OrderBy(i => i.Id).ToList();
                    break;

                case 1:
                    listClient = listClient.OrderBy(i => i.LastName).ToList();
                    break;

                case 2:
                    listClient = listClient.OrderBy(i => i.FirstName).ToList();
                    break;

                case 3:
                    listClient = listClient.OrderBy(i => i.Email).ToList();
                    break;

                case 4:
                    listClient = listClient.OrderBy(i => i.IdGender).ToList();
                    break;

                default:
                    listClient = listClient.OrderBy(i => i.Id).ToList();
                    break;
            }

            lvClient.ItemsSource = listClient;
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            ClientAddWindow clientAddWindow = new ClientAddWindow();
            clientAddWindow.ShowDialog();
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

        private void lvClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                var resClick = MessageBox.Show("Удалить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    if (lvClient.SelectedItem is EF.Client)
                    {
                        var client = lvClient.SelectedItem as EF.Client;
                        client.IsDeleted = true;
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

        private void lvClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var resClick = MessageBox.Show("Выбрать этого пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (resClick == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                if (lvClient.SelectedItem is EF.Client)
                {
                    ClassHelper.DataTransmissionClass.GetClient = lvClient.SelectedItem as EF.Client;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
