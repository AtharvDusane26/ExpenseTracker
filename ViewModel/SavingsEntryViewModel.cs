using ExpenseTracker.Model;
using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.ObjectModel;

namespace ExpenseTracker.ViewModel
{
    [AssociateViewType("ExpenseTracker.View.CreateSavigsView")]
    public class SavingEntryViewModel : ExpenseTrackerViewModelBase
    {
        private double _amount;
        private DateTime _date = DateTime.Today;
        private string _category;

        private ISaving _saving;
        private UserManager _userManager;

        public SavingEntryViewModel()
        {
            _userManager = ServiceProvider.Instance.Resolve<UserManager>();

            // Example: If you want predefined categories, bind a list
            CategoryOptions = new ObservableCollection<string>(new[] { "General", "Emergency", "Travel", "Education", "Other" });
        }

        // Options for ComboBox
        public ObservableCollection<string> CategoryOptions { get; }

        // Properties
        public double Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        // Fill data from existing saving
        public void Fill(ISaving saving)
        {
            if (saving == null) return;

            _saving = saving;
            Amount = saving.Amount;
            Date = saving.Date;
            Category = saving.Category;
        }

        // Save saving (add or update)
        public void Save()
        {
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();

            if (!ValidateInput(out string errorMessage))
            {
                messageBox.Show(errorMessage, new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Input Error");
                return;
            }

            if (_saving == null)
            {
                // Add new saving
                _userManager.AddToSavings(Amount, Date, Category);
                messageBox.Show("Saving added successfully", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Success");
            }
            else
            {
                // Update existing saving
                _saving.UpdateAmount(Amount);
                _saving.UpdateDate(Date);

                messageBox.Show("Saving updated successfully", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Success");
            }
        }

        // Validation
        private bool ValidateInput(out string errorMessage)
        {
            if (Amount <= 0)
            {
                errorMessage = "Amount must be greater than 0.";
                return false;
            }
            if (Amount > _userManager.GetBalance())
            {
                errorMessage = "Amount cannot be greater than current balance.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Category))
            {
                errorMessage = "Category cannot be empty.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
