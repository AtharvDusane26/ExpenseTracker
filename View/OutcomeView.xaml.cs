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
    /// Interaction logic for OutcomeView.xaml
    /// </summary>
    public partial class OutcomeView : UserControl 
    {
        private OutcomeViewModel _component;

        public OutcomeView()
        {
            InitializeComponent();
            DataContext = _component= new ViewModel.OutcomeViewModel();
        }

        private void btnAddOutcome_Click(object sender, RoutedEventArgs e)
        {
            var vm = new OutcomeEntryViewModel();
            var sv = ServiceProvider.Instance.Resolve<IViewService>();
            sv.ShowDialog(vm);
            _component.OnTabChanged?.Invoke();
        }

        private void btnDeductFromBalance_Click(object sender, RoutedEventArgs e)
        {
            _component.AddOutcome();
        }

        private void EditOutcome_Click(object sender, RoutedEventArgs e)
        {
            _component.EditOutcome();
        }
    }
}
