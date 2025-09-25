using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.Services;
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
    /// Interaction logic for IncomeView.xaml
    /// </summary>
    public partial class IncomeView : UserControl
    {
        private IncomeViewModel _component;
        public IncomeView()
        {
            InitializeComponent();
            DataContext = _component = new IncomeViewModel();
        }

        private void btnAddIncome_Click(object sender, RoutedEventArgs e)
        {
            var vm = new IncomeEntryViewModel();
            var sv = ServiceProvider.Instance.Resolve<IViewService>();
            sv.ShowDialog(vm);
            _component.OnTabChanged?.Invoke();
        }

        private void btnAddToBalance_Click(object sender, RoutedEventArgs e)
        {

            _component.AddIncome();
        }

        private void EditIncome_Click(object sender, RoutedEventArgs e)
        {
            _component.EditIncome();
        }
    }
}
