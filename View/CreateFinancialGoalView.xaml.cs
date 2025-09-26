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
    /// Interaction logic for CreateFinancialGoalView.xaml
    /// </summary>
    public partial class CreateFinancialGoalView : UserControl,IDynamicView
    {
        private CreateFinancialGoalViewModel  _component;
        public CreateFinancialGoalView()
        {
            InitializeComponent();
        }
        public ViewCreatingArgs ViewCreatingArgs { get; } = new ViewCreatingArgs
        {
            Title = "Expense Tracker | Create Need",
            ResizeMode = ViewResizeMode.NoResize,
            StartupLocation = ViewStartupLocation.CenterOwner,
            ShowInTaskbar = false,
            WindowState = ViewState.Normal,
            IsModal = true
        };
        public void SetComponent(object component)
        {
            _component = component as CreateFinancialGoalViewModel;
        }
        private void Check_Contribution(object sender, RoutedEventArgs e)
        {
            var amt = _component.CalculateMonthlyInstallment();
            if(amt > 0)
                System.Windows.MessageBox.Show($"Monthly Contribution: Rs. {amt}", "",System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _component.SaveGoal();
        }
    }
}
