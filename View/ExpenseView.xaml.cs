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
    /// Interaction logic for ExpenseView.xaml
    /// </summary>
    public partial class ExpenseView : UserControl
    {
        private ExpenseViewModel _component;
        public ExpenseView()
        {
            InitializeComponent();
            DataContext = _component = new ExpenseViewModel();
            Loaded += (o, e) => _component.OnTabChanged?.Invoke();
        }

        private void btnAddExpense_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ExpenseViewModel vm)
            {
                vm.AddExpense();
            }
        }

        private void EditExpense_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ExpenseViewModel vm)
            {
                vm.EditExpense();
            }
        }

        private void btnMarkFrozen_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ExpenseViewModel vm)
            {
                vm.ToggleFreezeSelected();
            }
        }
    }
}
