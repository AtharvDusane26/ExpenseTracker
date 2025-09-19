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
        internal void CreateExpense(string name, double amount, DateTime date, string description, string category = "General")
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
        }

        internal void UpdateExpense(string id, string name, double amount, DateTime date, string description, string category = "General")
        {
            var expense = _user.UserExpenses.FirstOrDefault(e => e.ExpenseId == id) as Expense;
            if (expense != null)
            {
                (_user as User).Balance += expense.Amount; // rollback old amount
                expense.Name = name;
                expense.Amount = amount;
                expense.DateOfExpense = date;
                expense.Description = description;
                expense.Category = category;
                (_user as User).Balance -= amount; // apply new amount
            }
        }

        internal void DeleteExpense(string id)
        {
            var expense = _user.UserExpenses.FirstOrDefault(e => e.ExpenseId == id);
            if (expense != null)
            {
                (_user as User).Balance += expense.Amount; // refund balance
                _user.UserExpenses.Remove(expense);
            }
        }
    }

}


