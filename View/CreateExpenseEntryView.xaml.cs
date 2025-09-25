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
    /// Interaction logic for CreateExpenseEntryView.xaml
    /// </summary>
    public partial class CreateExpenseEntryView : UserControl,IDynamicView
    {
        private ExpenseEntryViewModel _component;
        public CreateExpenseEntryView()
        {
            InitializeComponent();
        }
        public ViewCreatingArgs ViewCreatingArgs { get; } = new ViewCreatingArgs
        {
            Title = "Expense Tracker | Add Outcome",
            ResizeMode = ViewResizeMode.NoResize,
            StartupLocation = ViewStartupLocation.CenterOwner,
            ShowInTaskbar = false,
            WindowState = ViewState.Normal,
            IsModal = true
        };
        public void SetComponent(object component)
        {
            _component = component as ExpenseEntryViewModel;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ExpenseEntryViewModel vm)
            {
                vm.Save();

                // close dialog via IViewService in your app
                var viewService = ExpenseTracker.Model.Services.ServiceProvider.Instance.Resolve<IViewService>();
                viewService.Close(this);
            }
        }
    }
}
