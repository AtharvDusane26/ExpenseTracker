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
    /// Interaction logic for CreateSavigsView.xaml
    /// </summary>
    public partial class CreateSavigsView : UserControl,IDynamicView
    {
        private SavingEntryViewModel _component;
        public CreateSavigsView()
        {
            InitializeComponent();
        }
        public ViewCreatingArgs ViewCreatingArgs { get; } = new ViewCreatingArgs
        {
            Title = "Expense Tracker | Add Savings",
            ResizeMode = ViewResizeMode.NoResize,
            StartupLocation = ViewStartupLocation.CenterOwner,
            ShowInTaskbar = false,
            WindowState = ViewState.Normal,
            IsModal = true
        };
        public void SetComponent(object component)
        {
            _component = component as SavingEntryViewModel;
        }

        private void btnAddSaving_Click(object sender, RoutedEventArgs e)
        {
            _component.Save();
        }
    }
}
