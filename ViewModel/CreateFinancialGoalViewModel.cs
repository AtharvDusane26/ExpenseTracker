using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using ExpenseTracker.Model.SavingsAndFinancialGoals;
    using ExpenseTracker.Model;
    using ExpenseTracker.Model.Services;
    using System.Windows;
    using MessageBoxImage = Model.Services.MessageBoxImage;

    [AssociateViewType("ExpenseTracker.View.CreateFinancialGoalView")]
    public class CreateFinancialGoalViewModel : ExpenseTrackerViewModelBase
    {
        private string _name;
        private double _targetAmount;
        private int _durationInYears;
        private double _monthlyInterestRate;

        private bool _isEditMode;
        private IFinancialGoal _goal;
        private UserManager _userManager;

        public CreateFinancialGoalViewModel()
        {
            _userManager = ServiceProvider.Instance.Resolve<UserManager>();
        }

        // Bindable properties
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public double TargetAmount
        {
            get => _targetAmount;
            set => SetProperty(ref _targetAmount, value);
        }

        public int DurationInYears
        {
            get => _durationInYears;
            set => SetProperty(ref _durationInYears, value);
        }

        public double MonthlyInterestRate
        {
            get => _monthlyInterestRate;
            set => SetProperty(ref _monthlyInterestRate, value);
        }

        // Fill method for editing existing goal
        public void Fill(IFinancialGoal goal)
        {
            if (goal != null)
            {
                _goal = goal;
                _isEditMode = true;

                Name = goal.Name;
                TargetAmount = goal.TargetAmount;
                DurationInYears = Math.Abs(goal.DurationInMonths / 12);
                MonthlyInterestRate = goal.MonthlyInterestRate;
            }
        }

        // Add or update goal
        public void SaveGoal()
        {
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();

            if (!ValidateInput(out string errorMessage))
            {
                messageBox.Show(errorMessage,
                    new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Input Error");
                return;
            }

            if (_isEditMode && _goal != null)
            {
                _userManager.UpdateGoal(_goal.GoalId, Name, TargetAmount, DurationInYears, MonthlyInterestRate);
                messageBox.Show("Goal updated successfully.",
                    new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Information");
            }
            else
            {
                var goal = _userManager.CreateGoal(Name, TargetAmount, DurationInYears, MonthlyInterestRate);
                messageBox.Show("Goal created successfully.",
                    new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Information");
            }
        }

        // Optional: Check monthly installment
        public double CalculateMonthlyInstallment()
        {
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
            if (!ValidateInput(out string errorMessage))
            {
                messageBox.Show(errorMessage,
                    new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Input Error");
                return 0;
            }
            return _userManager.CalculateMonthlyContribution(TargetAmount, DurationInYears, MonthlyInterestRate);
        }
        // Validation
        private bool ValidateInput(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                errorMessage = "Goal Name cannot be empty.";
                return false;
            }

            if (TargetAmount <= 0)
            {
                errorMessage = "Target Amount must be greater than 0.";
                return false;
            }

            if (DurationInYears <= 0)
            {
                errorMessage = "Duration must be at least 1 year.";
                return false;
            }

            if (MonthlyInterestRate < 0)
            {
                errorMessage = "Interest Rate cannot be negative.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }

}
