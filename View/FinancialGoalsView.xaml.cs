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
    /// Interaction logic for FinancialGoalsView.xaml
    /// </summary>
    public partial class FinancialGoalsView : UserControl
    {
        private FinancialGoalsViewModel _component;
        public FinancialGoalsView()
        {
            InitializeComponent();
            DataContext = _component = new FinancialGoalsViewModel();
        }

        private void btnCreateGoal_Click(object sender, RoutedEventArgs e)
        {
            _component.AddGoal();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _component.AddAmountToGoal();
        }

        private void DeleteGoal_Click(object sender, RoutedEventArgs e)
        {
            _component.DeleteGoal();
        }

        private void EditGoal_Click(object sender, RoutedEventArgs e)
        {
            _component.EditGoal();
        }

        private void StopGoal_Click(object sender, RoutedEventArgs e)
        {
            _component.StopGoal();
        }
    }
}
