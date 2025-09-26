using ExpenseTracker.ViewModel;
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

namespace ExpenseTracker.View
{
    /// <summary>
    /// Interaction logic for SavingView.xaml
    /// </summary>
    public partial class SavingView : UserControl
    {
        private SavingsViewModel _component;
        public SavingView()
        {
            InitializeComponent();
            DataContext = _component = new SavingsViewModel();
            Loaded += (o,e) => _component.OnTabChanged?.Invoke();
        }

        private void EditSaving_Click(object sender, RoutedEventArgs e)
        {
            _component.EditSaving();
        }

        private void btnAddSaving_Click(object sender, RoutedEventArgs e)
        {
            _component.AddSaving();
        }
    }
}
