using ExpenseTracker.Model.Notifications;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Expenses
{
    internal class ExpenseManager
    {
        private IExpenseProvider _user;

        internal ExpenseManager(IExpenseProvider user)
        {
            _user = user;
        }

        internal IExpense CreateExpense(string name, double amount, DateTime date, string description, bool freeze, string category = "General")
        {
            var id = $"{(_user as User)}_Expense_{_user.UserExpenses.Count + 1}";
            Expense expense = new Expense(id)
            {
                Name = name,
                Amount = amount,
                DateOfExpense = date,
                Description = description,
                Category = category
            };

            _user.UserExpenses.Add(expense);
            (_user as User).Balance -= amount; // Deduct balance
            expense.FreezeTransaction(freeze);

            Save();

            AddNotification("Expense Added", expense.ExpenseId, NotificationType.Debited, $"Expense '{name}' of Rs.{amount} added in category '{category}'.");

            return expense;
        }

        internal void UpdateExpense(string id, string name, double amount, DateTime date, string description, bool freeze, string category = "General")
        {
            var expense = _user.UserExpenses.FirstOrDefault(e => e.ExpenseId == id) as Expense;
            if (expense != null)
            {
                // Rollback old amount
                (_user as User).Balance += expense.Amount;

                expense.Name = name;
                expense.Amount = amount;
                expense.DateOfExpense = date;
                expense.Description = description;
                expense.Category = category;

                // Apply new amount
                (_user as User).Balance -= amount;

                Save();

                AddNotification("Expense Updated", expense.ExpenseId, NotificationType.Debited, $"Expense '{name}' updated with new amount Rs.{amount} in category '{category}'.");
            }
        }

        internal void DeleteExpense(string id)
        {
            var expense = _user.UserExpenses.FirstOrDefault(e => e.ExpenseId == id);
            if (expense != null)
            {
                (_user as User).Balance += expense.Amount; // Refund balance
                _user.UserExpenses.Remove(expense);

                Save();

                AddNotification("Expense Deleted", expense.ExpenseId, NotificationType.Credited, $"Expense '{expense.Name}' of Rs.{expense.Amount} deleted and amount refunded to balance.");
            }
        }

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


