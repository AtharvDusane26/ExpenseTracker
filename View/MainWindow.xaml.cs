using ExpenseTracker.Model.Notifications;
using ExpenseTracker.Model.Services;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControl[] _views;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _views = new UserControl[]
           {
                new HomeView(),
                new IncomeView(),
                new OutcomeView(),
                new ExpenseView(),
                new SavingView(),
                new FinancialGoalsView(),
                new ReportsView(),
                new UserMonitorView()
           };
            // Set default view to Home
            ContentArea.Content = _views[0];
        }

        private void TabButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag.ToString(), out int index))
            {
                ContentArea.Content = _views[index];
            }
        }
    }
}