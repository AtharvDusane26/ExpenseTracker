using ExpenseTracker.DataManagement.Database;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.Notifications;
using ExpenseTracker.Model.Services;
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
         var data =  _user.AddToSavings(amount, date,category);
            Save(data,CRUDOperation.Insert);
            AddNotification("Savings Added", $"Savings_{(_user as User).UserId}", NotificationType.Credited, $"Rs.{amount} added to savings in category '{category}'.");
        }

        internal void WithdrawFromSavings(double amount,string savingId)
        {
            _user.WithdrawFromSavings(amount,savingId);
            AddNotification("Savings Withdrawn", $"Savings_{(_user as User).UserId}", NotificationType.Debited, $"Rs.{amount} withdrawn from savings.");
        }

        internal double GetSavingsBalance() => _user.SavingsBalance;

        internal List<INotification> GetReminders(int daysBefore = 1)
        {
            return _user.GetSavingsReminders(daysBefore);
        }

        internal List<ISaving> Get() => _user.Savings.ToList();

       
        private void Save(ISaving value, CRUDOperation operation)
        {
            var dbService = ServiceProvider.Instance.Resolve<DatabaseServices>();
            dbService.UpdateSavings(value, (_user as User), operation);
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
