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
using System.Windows.Shapes;

namespace ExpenseTracker.View
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : UserControl, IDynamicView
    {
        private LoginViewModel _component;
        public LoginForm()
        {
            InitializeComponent();
        }
        public ViewCreatingArgs ViewCreatingArgs { get; } = new ViewCreatingArgs
        {
            Title = "Expense Tracker | Login Form",
            ResizeMode = ViewResizeMode.NoResize,
            StartupLocation = ViewStartupLocation.CenterOwner,
            ShowInTaskbar = false,
            WindowState = ViewState.Normal,
            IsModal = true
        };
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (_component.CreateUser())
            {
                var window = Window.GetWindow(this);
                window.DialogResult = true;
                window.Close();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(_component.LogIn())
            {
                var window = Window.GetWindow(this);
                window.DialogResult = true;
                window.Close();
            }
        }
            public void SetComponent(object component)
            {
               _component = component as LoginViewModel;
            }
    }
}
