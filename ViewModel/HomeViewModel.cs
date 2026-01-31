using ExpenseTracker.Model.Notifications;
using ExpenseTracker.Model.Services;
using ExpenseTracker.ViewModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ExpenseTracker.Model.ViewModels
{
    public class HomeViewModel : ExpenseTrackerViewModelBase
    {
        private double _userBalance;
        private double _savingsBalance;
        private double _expenses; // new
        private INotification _selectedNotification;
        private ObservableCollection<INotification> _notifications = new ObservableCollection<INotification>();

        public HomeViewModel() : base()
        {
            Init();
            MarkAsReadCommand = new RelayCommand<INotification>(MarkNotificationAsRead);
        }

        #region Properties

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

        public double Expenses
        {
            get => _expenses;
            set => SetProperty(ref _expenses, value);
        }

        public INotification SelectedNotification
        {
            get => _selectedNotification;
            set => SetProperty(ref _selectedNotification, value);
        }

        public ObservableCollection<INotification> Notifications
        {
            get => _notifications;
            set => SetProperty(ref _notifications, value);
        }

        public ObservableCollection<ISeries> BalanceSeries { get; set; } = new ObservableCollection<ISeries>();

        public ICommand MarkAsReadCommand { get; }

        #endregion

        #region Initialization

        private void Init()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            UserBalance = userManager.GetBalance();
            SavingsBalance = userManager.GetSavingsBalance();
            Expenses = userManager.GetAllExpenses().Sum(o => o.Amount); // assume expenses = total - savings

            LoadBalanceChart();

            var notificationManager = ServiceProvider.Instance.Resolve<NotificationManager>();
            Notifications = new ObservableCollection<INotification>(
                notificationManager.GetUnreadNotifications()
                                   .OrderByDescending(o => o.Date)
            );
        }

        private void LoadBalanceChart()
        {
            BalanceSeries.Clear();
            BalanceSeries.Add(new PieSeries<double>
            {
                Values = new double[] { UserBalance },
                Name = "Available",
                Fill = new SolidColorPaint(SKColors.LightBlue)
            });
            BalanceSeries.Add(new PieSeries<double>
            {
                Values = new double[] { SavingsBalance },
                Name = "Savings",
              Fill = new SolidColorPaint(SKColors.LightGreen)
            });
            BalanceSeries.Add(new PieSeries<double>
            {
                Values = new double[] { Expenses },
                Name = "Expenses",
                Fill = new SolidColorPaint(SKColors.LightCoral)
            });
            OnPropertyChanged(nameof(BalanceSeries));
        }

        #endregion

        #region Notification Handling

        private void MarkNotificationAsRead(INotification notification)
        {
            if (notification == null) return;

            var notificationManager = ServiceProvider.Instance.Resolve<NotificationManager>();
            notificationManager.MarkAsRead(notification.Id);

            // Remove from collection immediately
            Notifications.Remove(notification);
        }

        #endregion

        protected override void Refresh()
        {
            Init();
            base.Refresh();
        }
    }
}
