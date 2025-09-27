using ExpenseTracker.Model;
using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExpenseTracker.ViewModel
{
    [AssociateViewType("ExpenseTracker.View.WithdrawSavingsView")]
    public class WithdrawSavingsViewModel : ExpenseTrackerViewModelBase
    {
        private UserManager _userManager;
        private IMessageBoxService _messageBox;
        private double _savingAmount;
        private double _amountToWithdraw;
        private double _amount;
        private ISaving _saving;
        public WithdrawSavingsViewModel(ISaving saving) : base()
        {
            _userManager = ServiceProvider.Instance.Resolve<UserManager>();
            _messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
            _saving = saving ?? throw new ArgumentNullException(nameof(saving));
            Init();
        }
        private void Init()
        {
            _amount = _userManager.GetBalance();
            _savingAmount = _saving.Amount;
            _amountToWithdraw = 0;

        }
        public double SavingAmount
        {
            get => _savingAmount;
            set => SetProperty(ref _savingAmount, value);
        }
        public double AmountToWithdraw
        {
            get => _amountToWithdraw;
            set => SetProperty(ref _amountToWithdraw, value);
           
        }
        public double Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }
        public void Withdraw()
        {
            if (AmountToWithdraw <= 0)
            {
                _messageBox.Show("Amount to withdraw must be greater than zero.", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Warning), "Invalid Amount");
                return;
            }
            if (AmountToWithdraw > SavingAmount)
            {
                _messageBox.Show("Amount to withdraw cannot be greater than saving amount.", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Warning), "Invalid Amount");
                return;
            }
            var result = _messageBox.Show($"Are you sure you want to withdraw {AmountToWithdraw:C} from your savings?", new MessageBoxArgs(MessageBoxButtons.YesNo, MessageBoxImage.Question), "Confirm Withdrawal");
            if (result == MessageBoxResult.Yes)
            {
                _userManager.WithdrawFromSavings(AmountToWithdraw,_saving.SavingId);
                SavingAmount -= AmountToWithdraw;
                AmountToWithdraw = 0;
                _amount = _userManager.GetBalance();
                _messageBox.Show("Withdrawal successful.", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Success");
                OnPropertyChanged(nameof(SavingAmount));
                OnPropertyChanged(nameof(Amount));
            }
        }
    }
}
