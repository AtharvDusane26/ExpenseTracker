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
    /// Interaction logic for UserMonitorView.xaml
    /// </summary>
    public partial class UserMonitorView : UserControl
    {
        private ViewModel.UserMonitorViewModel _component;
        public UserMonitorView()
        {
            InitializeComponent();
            DataContext = _component = new ViewModel.UserMonitorViewModel();
            Loaded += (o, e) => _component.LoadInsights();
        }
    }
}
