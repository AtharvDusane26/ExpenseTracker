using ExpenseTracker.Model.Services;
using ExpenseTracker.ViewModel;
using Microsoft.Win32;
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
using MessageBoxImage = System.Windows.MessageBoxImage;

namespace ExpenseTracker.View
{
    /// <summary>
    /// Interaction logic for ReportsView.xaml
    /// </summary>
    public partial class ReportsView : UserControl
    {
        private ReportViewModel _component;
        public ReportsView()
        {
            InitializeComponent();
            DataContext = _component = new ReportViewModel();
        }

        private void ExportReport_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Not Implemented Yet", "Expense Tracker", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
            //_component.ExportReport("");
        }

        private void CreateTextFile(string content)
        {
            // Create a save file dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Save Text File",
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FileName = "Transaction History" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt"
            };

            if (saveFileDialog.ShowDialog() == true) // returns bool? in WPF
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, content);

                System.Windows.MessageBox.Show($"File saved successfully at:\n{saveFileDialog.FileName}",
                                "Success",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("No file selected. File not saved.",
                                "Cancelled",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           var report = _component.GetTransactionHistory();
            if(report.Length > 0)
            {
                CreateTextFile(report);

            }
        }
    }
    
}
