using ExpenseTracker.Model.Services;
using ExpenseTracker.Model.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    internal class SavingsManager
    {
        private readonly ISavingsProvider _user;

        internal SavingsManager(ISavingsProvider user)
        {
            _user = user;
        }

        internal void AddToSavings(double amount,DateTime date, string category = "General")
        {
            _user.AddToSavings(amount, date,category);
            Save();
            AddNotification("Savings Added", $"Savings_{(_user as User).UserId}", NotificationType.Credited, $"Rs.{amount} added to savings in category '{category}'.");
        }

        internal void WithdrawFromSavings(double amount)
        {
            _user.WithdrawFromSavings(amount);
            Save();
            AddNotification("Savings Withdrawn", $"Savings_{(_user as User).UserId}", NotificationType.Debited, $"Rs.{amount} withdrawn from savings.");
        }

        internal double GetSavingsBalance() => _user.SavingsBalance;

        internal List<INotification> GetReminders(int daysBefore = 1)
        {
            return _user.GetSavingsReminders(daysBefore);
        }

        internal List<ISaving> Get() => _user.Savings.ToList();

        private void Save()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            userManager.Save(_user as User);
        }

        private void AddNotification(string name, string referenceId, NotificationType type, string message)
        {
            var notificationManager = ServiceProvider.Instance.Resolve<NotificationManager>();
            if (notificationManager != null)
            {
                notificationManager.AddNotification(name, referenceId, type, message, DateTime.Now);
            }
        }
    }
}
