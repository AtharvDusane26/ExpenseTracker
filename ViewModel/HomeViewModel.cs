using ExpenseTracker.Model.Notifications;
using ExpenseTracker.Model.Services;
using ExpenseTracker.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ExpenseTracker.Model.ViewModels
{
    public class HomeViewModel : ExpenseTrackerViewModelBase
    {
        private double _userBalance;
        private double _savingsBalance;
        private INotification _selectedNotification;
        private ObservableCollection<INotification> _notifications = new ObservableCollection<INotification>();

        public HomeViewModel() : base()
        {
            Init();
        }
        public INotification SelectedNotification
        {
            get => _selectedNotification;
            set => SetProperty(ref _selectedNotification, value);

        }
        private void Init()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            UserBalance = userManager.GetBalance();
            SavingsBalance = userManager.GetSavingsBalance();
            var notificationManager = ServiceProvider.Instance.Resolve<NotificationManager>();
            // Combine all notifications (transaction + savings)
            Notifications = new ObservableCollection<INotification>(
                notificationManager.GetUnreadNotifications()
                           .OrderByDescending(o => o.Date) // newest first
            );
        }
        public void HandleNotificationRead(bool value)
        {
            var notificationManager = ServiceProvider.Instance.Resolve<NotificationManager>();
            if (SelectedNotification == null || !value) return;
            if (value)
            {
                notificationManager.MarkAsRead(SelectedNotification.Id);
            }
            else
            {
                notificationManager.MarkAsUnRead(SelectedNotification.Id);
            }
            Init();
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

        public ObservableCollection<INotification> Notifications
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
