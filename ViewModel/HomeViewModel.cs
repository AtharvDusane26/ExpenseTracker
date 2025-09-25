using ExpenseTracker.Model;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class HomeViewModel : ExpenseTrackerViewModelBase
    {

        private double _userBalance;
        private double _savingsBalance;
        private ObservableCollection<string> _notifications = new ObservableCollection<string>();
        public HomeViewModel() : base()
        {
            Init();
        }
        private void Init()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            UserBalance = userManager.GetBalance();
            SavingsBalance = userManager.GetSavingsBalance();
            Notifications = new ObservableCollection<string>(
                userManager.GetReminders(1).Select(o => o.Message)
                           .Concat(userManager.GetSavingsReminders(1).Select(o => o.Message))
            );
        }
        public double UserBalance
        {
            get => _userBalance;
            set => SetProperty(ref _userBalance, value);
        }

        public double SavingsBalance
        {
            get => _savingsBalance;
            set => SetProperty(ref _savingsBalance, value);
        }

        public ObservableCollection<string> Notifications
        {
            get => _notifications;
            set => SetProperty(ref _notifications, value);
        }
        protected override void Refresh()
        {
            Init();
            base.Refresh();
        }
    }
}
