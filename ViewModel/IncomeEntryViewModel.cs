using ExpenseTracker.Model;
using ExpenseTracker.Model.Services;
using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExpenseTracker.ViewModel
{
    [AssociateViewType("ExpenseTracker.View.CreateIncomeView")]
    public class IncomeEntryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private UserManager _userManager;
        public string Name { get; set; }
        public float Amount { get; set; }
        public SourceOfIncome SourceOfIncome { get; set; }
        public DateTime DateOfIncome { get; set; } = DateTime.Today;
        public IncomeType IncomeType { get; set; }
        public bool Freeze { get; set; }
        public int DayOfTransaction { get; set; }
        public bool GiveReminder { get; set; }

        // Options for combo boxes
        public ObservableCollection<SourceOfIncome> SourceOfIncomeOptions { get; } =
            new ObservableCollection<SourceOfIncome>((SourceOfIncome[])Enum.GetValues(typeof(SourceOfIncome)));

        public ObservableCollection<IncomeType> IncomeTypeOptions { get; } =
            new ObservableCollection<IncomeType>((IncomeType[])Enum.GetValues(typeof(IncomeType)));


        public IncomeEntryViewModel()
        {
            _userManager = ServiceProvider.Instance.Resolve<UserManager>();

        }

        public void AddIncome()
        {
            if (!ValidateInput(out string errorMessage))
            {
                var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
                messageBox.Show(errorMessage, new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Input Error");
                return;
            }
            var inc = _userManager.AddIncome(IncomeType, Name, Amount, DateOfIncome);
            inc.FreezeTransaction(Freeze);
            inc.SetReminder(GiveReminder);
        }
        public bool ValidateInput(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                errorMessage = "Name cannot be empty.";
                return false;
            }

            if (Amount <= 0)
            {
                errorMessage = "Amount must be greater than 0.";
                return false;
            }

            if (!Enum.IsDefined(typeof(SourceOfIncome), SourceOfIncome))
            {
                errorMessage = "Please select a valid Source of Income.";
                return false;
            }

            if (!Enum.IsDefined(typeof(IncomeType), IncomeType))
            {
                errorMessage = "Please select a valid Income Type.";
                return false;
            }

            if (DayOfTransaction < 1 || DayOfTransaction > 31)
            {
                errorMessage = "Day of Transaction must be between 1 and 31.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
