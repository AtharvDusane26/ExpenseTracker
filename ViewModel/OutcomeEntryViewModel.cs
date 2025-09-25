using System;
using System.Collections.ObjectModel;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model;
using ExpenseTracker.Model.Services;
using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.OutcomeSources;

namespace ExpenseTracker.ViewModel
{
    [AssociateViewType("ExpenseTracker.View.CreateOutcomeView")]
    public class OutcomeEntryViewModel : ExpenseTrackerViewModelBase
    {
        private string _name;
        private double _amount;
        private OutcomeType outcomeType;
        private DateTime _dateOfExpense = DateTime.Today;
        private bool _freeze;
        private int _dayOfTransaction;
        private bool _giveReminder;

        private UserManager _userManager;
        private IOutcome _outcome;

        public OutcomeEntryViewModel() : base()
        {
            _userManager = ServiceProvider.Instance.Resolve<UserManager>();
        }

        // Combo box options
        public ObservableCollection<OutcomeType> OutcomeTypeOptions { get; } =
            new ObservableCollection<OutcomeType>((OutcomeType[])Enum.GetValues(typeof(OutcomeType)));


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

        public OutcomeType OutcomeType
        {
            get => outcomeType;
            set => SetProperty(ref outcomeType, value);
        }

        public DateTime DateOfExpense
        {
            get => _dateOfExpense;
            set => SetProperty(ref _dateOfExpense, value);
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

        public void AddOutcome()
        {
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();

            if (!ValidateInput(out string errorMessage))
            {
                messageBox.Show(errorMessage,
                    new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Input Error");
                return;
            }

            if (_outcome == null)
            {
                var exp = _userManager.AddOutcome( Name, Amount,OutcomeType,DayOfTransaction);
                exp.Create(Guid.NewGuid().ToString());
                exp.FreezeTransaction(Freeze);
                exp.SetReminder(GiveReminder);

                messageBox.Show("Expense created",
                    new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Information");
            }
            else
            {
                _userManager.UpdateOutcome(_outcome.Id, Name, Amount, OutcomeType, DayOfTransaction,Freeze,GiveReminder);

                messageBox.Show("Expense updated",
                    new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Information");
            }
        }

        public void Fill(IOutcome expense)
        {
            if (expense != null)
            {
                _outcome = expense;
                Name = expense.Name;
                Amount = expense.Amount;
                OutcomeType = expense.OutComeType;
                Freeze = expense.Freeze;
                DayOfTransaction = expense.DayOfTransaction;
                GiveReminder = expense.GiveReminder;               
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

            if (!Enum.IsDefined(typeof(OutcomeType), OutcomeType))
            {
                errorMessage = "Please select a valid Expense Category.";
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
