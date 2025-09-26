using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Services;
using ExpenseTracker.Model.Notifications;

namespace ExpenseTracker.Model.Transactions
{
    internal class TransactionManager
    {
        private readonly ITransactionProvider _user;

        internal TransactionManager(ITransactionProvider user)
        {
            _user = user;
        }

        public IIncomeProvider IncomeProvider => _user as IIncomeProvider;
        public IOutcomeProvider OutcomeProvider => _user as IOutcomeProvider;

        // ---------------- INCOME ----------------
        internal IIncome CreateIncome(IncomeType type, string name, double amount, DateTime? date = null)
        {
            IIncome income = type switch
            {
                IncomeType.Daily => new DailyIncome(name, amount),
                IncomeType.Monthly => new MonthlyIncome(name, amount),
                IncomeType.Yearly => new YearlyIncome(name, amount),
                _ => throw new ArgumentException("Invalid income type")
            };
            var id = Guid.NewGuid().ToString();
            income.Create(id);
            IncomeProvider.AddIncome(income);
            Save();

            // Notification
            AddNotification(name, id, NotificationType.Credited, $"Income '{name}' of Rs.{amount} added.");

            return income;
        }

        internal void DeleteIncome(string name)
        {
            var income = IncomeProvider.GetIncome(name);
            if (income != null)
            {
                IncomeProvider.DeleteIncome(name);
                AddNotification(name, income.Id, NotificationType.Other, $"Income '{name}' deleted.");
            }
            Save();
        }

        internal void UpdateIncome(string id, IncomeType type, string name, double amount, bool freeze, bool giveReminder, int dayOfTransaction)
        {
            IIncome income = type switch
            {
                IncomeType.Daily => new DailyIncome(name, amount),
                IncomeType.Monthly => new MonthlyIncome(name, amount),
                IncomeType.Yearly => new YearlyIncome(name, amount),
                _ => throw new ArgumentException("Invalid income type")
            };
            income.Create(id);
            income.FreezeTransaction(freeze);
            income.SetReminder(giveReminder);
            (income as ITransaction).UpdateTransactionDay(dayOfTransaction);
            IncomeProvider.UpdateIncome(income);
            Save();

            // Notification
            AddNotification(name, id, NotificationType.Other, $"Income '{name}' updated.");
        }

        internal List<IIncome> GetAllIncomes() => IncomeProvider.Incomes;

        // ---------------- OUTCOME ----------------
        internal IOutcome CreateOutcome(string name, double amount, OutcomeType type, int dayOfTransaction)
        {
            var outcome = new Outcome(name, amount, type);
            outcome.Create(Guid.NewGuid().ToString());
            outcome.UpdateTransactionDay(dayOfTransaction);
            OutcomeProvider.AddOutcome(outcome);
            Save();

            // Notification
            AddNotification(name, outcome.Id, NotificationType.Debited, $"Outcome '{name}' of Rs.{amount} added.");

            return outcome;
        }

        internal void DeleteOutcome(string name)
        {
            var outcome = OutcomeProvider.GetOutcome(name);
            if (outcome != null)
            {
                OutcomeProvider.DeleteOutcome(name);
                AddNotification(name, outcome.Id, NotificationType.Other, $"Outcome '{name}' deleted.");
            }
            Save();
        }

        internal void UpdateOutcome(string id, string name, double amount, OutcomeType outcomeType, int dayOfTransaction, bool freeze, bool giveReminder,DateTime? lastPaidDate)
        {
            var outcome = new Outcome(name, amount, outcomeType);
            outcome.LastPaidDate = lastPaidDate;
            outcome.Create(id);
            outcome.UpdateTransactionDay(dayOfTransaction);
            outcome.FreezeTransaction(freeze);
            outcome.SetReminder(giveReminder);
            OutcomeProvider.UpdateOutcome(outcome);
            Save();

            // Notification
            AddNotification(name, id, NotificationType.Other, $"Outcome '{name}' updated.");
        }

        internal List<IOutcome> GetAllOutcomes() => OutcomeProvider.Outcomes;

        // ---------------- REMINDERS ----------------
        internal List<INotification> GetUpcomingReminders(int daysBefore = 3)
        {
            var notifications = new List<INotification>();
            DateTime today = DateTime.Today;

            var notificationProvider = _user as INotificationProvider;
            if (notificationProvider == null)
                return notifications;

            foreach (var transaction in _user.Transactions)
            {
                if (transaction.Freeze)
                    continue;

                int daysUntilTransaction = transaction.DayOfTransaction - today.Day;

                NotificationType type;
                string message;

                if (daysUntilTransaction > 0 && daysUntilTransaction <= daysBefore)
                {
                    type = NotificationType.Reminder;
                    message = transaction.Remind();
                }
                else if (daysUntilTransaction < 0)
                {
                    type = NotificationType.Warning;
                    message = transaction.WarnForPendingTransaction();
                }
                else
                {
                    continue;
                }

                var existingNotification = notificationProvider.Notifications
                    .FirstOrDefault(n => n.ReferenceObjectId.ToString() == transaction.Id && n.Type == type);

                if (existingNotification == null)
                {
                    var notification = new Notification(
                        Guid.NewGuid().ToString(),
                        transaction.Name,
                        transaction.Id,
                        type,
                        message,
                        DateTime.Now
                    );

                    notificationProvider.AddNotification(notification);
                    existingNotification = notification;
                }

                notifications.Add(existingNotification);
            }

            return notifications.OrderBy(n => n.Date).ToList();
        }

        // ---------------- DUE OUTCOMES ----------------
        internal bool CreateOutcomeAsExpense(string outcomeId)
        {
            DateTime today = DateTime.Today;
            var outcome = OutcomeProvider.Outcomes.FirstOrDefault(o => o.Id == outcomeId);
            if (outcome == null)
                return false;

            if (!outcome.Freeze)
            {
                var em = new ExpenseManager(_user as User);
                (outcome as Outcome).LastPaidDate = DateTime.Now;
                var expense = em.CreateExpense(
                    outcome.Name,
                    outcome.Amount,
                    today,
                    "Paid recurring outcome",
                    false,
                    "Outcome"
                );

                // Notification
                //AddNotification(expense.ExpenseId, expense.ExpenseId, NotificationType.Debited,
                //    $"Outcome '{outcome.Name}' of Rs.{outcome.Amount} converted to Expense.");
            }
            return true;
        }

        // ---------------- PRIVATE METHODS ----------------
        private void Save()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            userManager.Save(_user as User);
        }

        private void AddNotification(string name, string referenceId, NotificationType type, string message)
        {
           var notificationManager = ServiceProvider.Instance.Resolve<NotificationManager>();
            notificationManager.AddNotification(name, referenceId, type, message, DateTime.Now);

        }
    }
}
