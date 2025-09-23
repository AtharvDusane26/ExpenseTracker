using ExpenseTracker.Model;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    [AssociateViewType("ExpenseTracker.View.LoginForm")]
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _phoneNumber;
        private string _age;
        private string _initialBalance;
        private UserManager _userManager;
        public event PropertyChangedEventHandler? PropertyChanged;
        public LoginViewModel()
        {
            var services = ServiceProvider.Instance;
            _userManager = services.Resolve<UserManager>();
        }
        public string UserName
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }
        public string Age
        {
            get => _age;
            set
            {
                if (_age != value)
                {
                    _age = value;
                    OnPropertyChanged(nameof(Age));
                }
            }
        }
        public string InitialBalance
        {
            get => _initialBalance;
            set
            {
                if (_initialBalance != value)
                {
                    _initialBalance = value;
                    OnPropertyChanged(nameof(InitialBalance));
                }
            }
        }
        public bool CreateUser()
        {
            int age = 0;
            double initialBalance = 0;
            if (!ValidateInput(out string errorMessage))
            {
                var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
                messageBox.Show(errorMessage, new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Input Error");
               return false;
            }
            if (!string.IsNullOrWhiteSpace(Age))
            {
                int.TryParse(Age, out age);
            }
            if (!string.IsNullOrWhiteSpace(InitialBalance))
            {
                double.TryParse(InitialBalance, out initialBalance);
            }
            _userManager.CreateUser(UserName, PhoneNumber, age, initialBalance);
            return true;
        }
        public bool LogIn()
        {
            try
            {
                _userManager.SetUser(UserName);
                return true;
            }
            catch (ArgumentNullException)
            {
                var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
                messageBox.Show("User not found. Please check the username or create a new account.", new MessageBoxArgs(MessageBoxButtons.OK,MessageBoxImage.Error) ,"Login Error");
                return false;
            }
        }
        private bool ValidateInput(out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(UserName))
            {
                errorMessage = "Username is required.";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(Age) && !int.TryParse(Age, out _))
            {
                errorMessage = "Age must be a valid number.";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(InitialBalance) && !double.TryParse(InitialBalance, out _))
            {
                errorMessage = "Initial Balance must be a valid number.";
                return false;
            }
            return true;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
