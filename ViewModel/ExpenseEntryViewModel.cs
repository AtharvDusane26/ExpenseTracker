using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model;
using ExpenseTracker.Model.Services;
using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    [AssociateViewType("ExpenseTracker.View.CreateExpenseEntryView")]
    public class ExpenseEntryViewModel : ExpenseTrackerViewModelBase
    {
        private string _name;
        private double _amount;
        private DateTime _dateOfExpense = DateTime.Today;
        private string _description;
        private OutcomeType _category;  // Assuming you have OutcomeType enum for expense type
        private bool _freeze;

        private IExpense _expense;

        private UserManager _userManager;

        public ExpenseEntryViewModel()
        {
            _userManager = ServiceProvider.Instance.Resolve<UserManager>();
            CategoryOptions = new ObservableCollection<OutcomeType>((OutcomeType[])Enum.GetValues(typeof(OutcomeType)));
        }

        // Options for ComboBox
        public ObservableCollection<OutcomeType> CategoryOptions { get; }

        // Properties
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

        public DateTime DateOfExpense
        {
            get => _dateOfExpense;
            set => SetProperty(ref _dateOfExpense, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public OutcomeType Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public bool Freeze
        {
            get => _freeze;
            set => SetProperty(ref _freeze, value);
        }

        // Fill data from existing expense
        public void Fill(IExpense expense)
        {
            if (expense == null) return;

            _expense = expense;
            Name = expense.Name;
            Amount = expense.Amount;
            DateOfExpense = expense.DateOfExpense;
            Description = expense.Description;
            Category = (OutcomeType)Enum.Parse(typeof(OutcomeType), expense.Category);
            Freeze = expense.Freeze;
        }

        // Save expense (add or update)
        public void Save()
        {
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();

            if (!ValidateInput(out string errorMessage))
            {
                messageBox.Show(errorMessage, new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Input Error");
                return;
            }

            if (_expense == null)
            {
                // Add new expense
                _userManager.AddExpense(Name, Amount, DateOfExpense, Description, Freeze,Category.ToString());
               
                messageBox.Show("Expense added successfully", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Success");
            }
            else
            {
                // Update existing expense
                _userManager.UpdateExpense(_expense.ExpenseId, Name, Amount, DateOfExpense, Description, Freeze, Category.ToString());
                messageBox.Show("Expense updated successfully", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Success");
            }
        }

        // Validation
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

            errorMessage = string.Empty;
            return true;
        }
    }
}
