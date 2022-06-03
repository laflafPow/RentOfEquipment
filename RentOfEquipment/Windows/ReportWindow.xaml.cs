using System;
using System.Collections.Generic;
using System.IO;
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
using Aspose.Pdf;
using Microsoft.Win32;
using RentOfEquipment.ClassHelper;

namespace RentOfEquipment.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        List<string> listsort = new List<string>()
        {
            "По умолучанию",
            "По наименованию оборудования",
            "По ФИО клиента",
            "По ФИО сотрудника",
            "По дате сдачи",
            "По дате выдачи"
        };

        private EF.Employee AuthUser { get; set; }
        private List<EF.EquipmentSale> equipmentSaleForReport = new List<EF.EquipmentSale>();
        private decimal countSum = 0;

        public ReportWindow(EF.Employee employee)
        {
            this.AuthUser = employee;
            InitializeComponent();

            cmbSort.ItemsSource = listsort;
            cmbSort.SelectedIndex = 0;

            txtAuthUser.Text = $"Вы вошли как: {AuthUser.FIO}";

            Filter();
        }

        private void Filter()
        {
            countSum = 0;
            equipmentSaleForReport = AppData.Context.EquipmentSale.Where(i => i.IsDeleted != true).ToList();

            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    equipmentSaleForReport = equipmentSaleForReport.OrderBy(i => i.Id).ToList();
                    break;

                case 1:
                    equipmentSaleForReport = equipmentSaleForReport.OrderBy(i => i.Equipment.Name).ToList();
                    break;

                case 2:
                    equipmentSaleForReport = equipmentSaleForReport.OrderBy(i => i.Client.FIO).ToList();
                    break;

                case 3:
                    equipmentSaleForReport = equipmentSaleForReport.OrderBy(i => i.Employee.FIO).ToList();
                    break;

                case 4:
                    equipmentSaleForReport = equipmentSaleForReport.OrderBy(i => i.EndRentDate).ToList();
                    break;

                case 5:
                    equipmentSaleForReport = equipmentSaleForReport.OrderBy(i => i.StartRentDate).ToList();
                    break;

                default:
                    equipmentSaleForReport = equipmentSaleForReport.OrderBy(i => i.Id).ToList();
                    break;
            }


            if (dpcStartDate.SelectedDate.HasValue)
            {
                DateTime startDateSort = dpcStartDate.SelectedDate.Value.Date + TimeSpan.Zero;
                equipmentSaleForReport = equipmentSaleForReport.Where(i => i.StartRentDate >= startDateSort).ToList();
            }
            if (dpcEndDate.SelectedDate.HasValue)
            {
                equipmentSaleForReport = equipmentSaleForReport.Where(i => i.EndRentDate <= dpcEndDate.SelectedDate.Value.AddHours(24)).ToList();
            }

            lvEquipment.ItemsSource = equipmentSaleForReport;

            foreach (EF.EquipmentSale clientProduct in equipmentSaleForReport)
            {
                countSum += clientProduct.TotalCost;
            }
            tbResultCost.Text = $"Общий итог: {countSum}";
            tbCountLines.Text = equipmentSaleForReport.Count().ToString();
        }

        private void btnGetPDF_Click(object sender, RoutedEventArgs e)
        {
            // The path to the documents directory.
            SaveFileDialog svg = new SaveFileDialog();
            svg.Filter = "PDF Files(*.pdf)|*.pdf";
            svg.FileName = $"Отчёт от {DateTime.Now.Date.Day}.{DateTime.Now.Date.Month}.{DateTime.Now.Date.Year}";

            if (svg.ShowDialog() == true)
            {
                this.IsEnabled = false;
                this.Opacity = 0.5;
                using (FileStream stream = new FileStream(svg.FileName, FileMode.Create))
                {
                    this.IsEnabled = false;
                    this.Opacity = 0.5;
                    Document document = new Document();
                    // Add page
                    Aspose.Pdf.Page page = document.Pages.Add();
                    // Add text to new page
                    string strDate = "";

                    if (dpcEndDate.SelectedDate.HasValue && dpcStartDate.SelectedDate.HasValue)
                    {
                        strDate = $"Отчёт от {dpcStartDate.SelectedDate.Value.Day}.{dpcStartDate.SelectedDate.Value.Month}.{dpcStartDate.SelectedDate.Value.Year}" +
                            $" до {dpcEndDate.SelectedDate.Value.Day}.{dpcEndDate.SelectedDate.Value.Month}.{dpcEndDate.SelectedDate.Value.Year}:";
                    }
                    else if (dpcStartDate.SelectedDate.HasValue)
                    {
                        strDate = $"Отчёт от {dpcStartDate.SelectedDate.Value.Day}.{dpcStartDate.SelectedDate.Value.Month}.{dpcStartDate.SelectedDate.Value.Year} до конца:";
                    }
                    else if (dpcEndDate.SelectedDate.HasValue)
                    {
                        strDate = $"Отчёт от начала до {dpcEndDate.SelectedDate.Value.Day}.{dpcEndDate.SelectedDate.Value.Month}.{dpcEndDate.SelectedDate.Value.Year}:";
                    }
                    else
                    {
                        strDate = $"Отчёт за всё время:";
                    }
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(strDate));
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(""));


                    Aspose.Pdf.Table reportTable = new Aspose.Pdf.Table();

                    page.Paragraphs.Add(reportTable);

                    reportTable.ColumnAdjustment = ColumnAdjustment.AutoFitToWindow;
                    reportTable.DefaultCellBorder = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, 0.1F);
                    reportTable.DefaultCellTextState = new Aspose.Pdf.Text.TextState(6);
                    reportTable.Border = new Aspose.Pdf.BorderInfo(Aspose.Pdf.BorderSide.All, 0.1F);


                    Aspose.Pdf.MarginInfo margin = new Aspose.Pdf.MarginInfo();
                    margin.Top = 1f;
                    margin.Left = 1f;
                    margin.Right = 1f;
                    margin.Bottom = 1f;
                    reportTable.DefaultCellPadding = margin;

                    Aspose.Pdf.Row row1 = reportTable.Rows.Add();
                    row1.Cells.Add("Номер");
                    row1.Cells.Add("Код");
                    row1.Cells.Add("Фио Клиента");
                    row1.Cells.Add("Код оборудования");
                    row1.Cells.Add("Наименование");
                    row1.Cells.Add("Код сотрудника");
                    row1.Cells.Add("Фио сотрудника");
                    row1.Cells.Add("Дата выдачи");
                    row1.Cells.Add("Дата сдачи");
                    row1.Cells.Add("Итоговая стоимость");

                    int numberRow = 1;
                    foreach (EF.EquipmentSale cp in equipmentSaleForReport)
                    {
                        Aspose.Pdf.Row newRow = reportTable.Rows.Add();
                        newRow.Cells.Add(Convert.ToString(numberRow));
                        newRow.Cells.Add(Convert.ToString(cp.Id));
                        newRow.Cells.Add(Convert.ToString(cp.Client.FIO));
                        newRow.Cells.Add(Convert.ToString(cp.IdEquipment));
                        newRow.Cells.Add(Convert.ToString(cp.Equipment.Name));
                        newRow.Cells.Add(Convert.ToString(cp.IdEmployee));
                        newRow.Cells.Add(Convert.ToString(cp.Employee.FIO));
                        newRow.Cells.Add(Convert.ToString(cp.StartRentDate));
                        newRow.Cells.Add(Convert.ToString(cp.EndRentDate));
                        newRow.Cells.Add(Convert.ToString(cp.TotalCost));
                        numberRow++;
                    }

                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(""));
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment($"Общий итог: {countSum}"));

                    // Save updated PDF
                    document.Save(stream);
                    MessageBox.Show($"Отчёт успешно сохранён в {svg.FileName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    stream.Close();
                }
                this.Opacity = 1;
                this.IsEnabled = true;
            }
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void dpcStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void dpcEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
    }
}
