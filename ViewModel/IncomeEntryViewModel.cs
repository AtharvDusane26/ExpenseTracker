using ExpenseTracker.Model;
using ExpenseTracker.Model.IncomeSources;
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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using ExpenseTracker.Model.Transactions;

namespace ExpenseTracker.ViewModel
{
    [AssociateViewType("ExpenseTracker.View.CreateIncomeView")]
    public class IncomeEntryViewModel : ExpenseTrackerViewModelBase
    {
        private string _name;
        private double _amount;
        private SourceOfIncome _sourceOfIncome;
        private DateTime _dateOfIncome = DateTime.Today;
        private IncomeType _incomeType;
        private bool _freeze;
        private int _dayOfTransaction;
        private bool _giveReminder;
        private UserManager _userManager;
        private IIncome _income;
        public IncomeEntryViewModel() : base()
        {
            _userManager = ServiceProvider.Instance.Resolve<UserManager>();
        }
        // Options for combo boxes
        public ObservableCollection<SourceOfIncome> SourceOfIncomeOptions { get; } =
            new ObservableCollection<SourceOfIncome>((SourceOfIncome[])Enum.GetValues(typeof(SourceOfIncome)));

        public ObservableCollection<IncomeType> IncomeTypeOptions { get; } =
            new ObservableCollection<IncomeType>((IncomeType[])Enum.GetValues(typeof(IncomeType)));

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public double Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public SourceOfIncome SourceOfIncome
        {
            get => _sourceOfIncome;
            set => SetProperty(ref _sourceOfIncome, value);
        }

        public DateTime DateOfIncome
        {
            get => _dateOfIncome;
            set => SetProperty(ref _dateOfIncome, value);
        }

        public IncomeType IncomeType
        {
            get => _incomeType;
            set => SetProperty(ref _incomeType, value);
        }

        public bool Freeze
        {
            get => _freeze;
            set => SetProperty(ref _freeze, value);
        }

        public int DayOfTransaction
        {
            get => _dayOfTransaction;
            set => SetProperty(ref _dayOfTransaction, value);
        }

        public bool GiveReminder
        {
            get => _giveReminder;
            set => SetProperty(ref _giveReminder, value);
        }

        public void AddIncome()
        {
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
            if (!ValidateInput(out string errorMessage))
            {
                messageBox.Show(errorMessage, new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Input Error");
                return;
            }
            if(_income == null)
            {
                var inc = _userManager.AddIncome(IncomeType, Name, Amount);
                inc.FreezeTransaction(Freeze);
                inc.SetReminder(GiveReminder);
                (inc as ITransaction).UpdateTransactionDay(DayOfTransaction);
                messageBox.Show("Income created", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Information");
            }

            else
            {
               _userManager.UpdateIncome(_income.Id, IncomeType, Name, Amount, Freeze, GiveReminder,DayOfTransaction);
                messageBox.Show("Income updated", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Information");
            }

        }
        public void Fill(IIncome income)
        {
            if (income != null)
            {
                _income = income;
                Name = income.Name;
                Amount = income.Amount;
                SourceOfIncome = income.SourceOfIncome;
                Freeze = income.Freeze;
                DayOfTransaction = income.DayOfTransaction;
                GiveReminder = income.GiveReminder;
                if (income is MonthlyIncome)
                    IncomeType = IncomeType.Monthly;
                else if (income is YearlyIncome)
                    IncomeType = IncomeType.Yearly;
                else
                    IncomeType = IncomeType.Daily;
            }
        }
        private bool ValidateInput(out string errorMessage)
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
    }
}
