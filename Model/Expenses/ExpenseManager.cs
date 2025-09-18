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
        private User _user;
        public ExpenseManager(User user)
        {
            _user = user;
        }
        // to create Expense
        public void CreateExpense(string name, double amount, DateTime date, string description)
        {
            var id = $"{_user.Name}_{_user.UserExpenses.Count() +1}";
            Expense expense = new Expense(id);
            expense.Name = name;
            expense.Amount = amount;
            expense.DateOfExpense = date;
            expense.Descreption = description;

            _user.UserExpenses.Add(expense);

        }
        //To update Expense
        public void UpdateExpense(string id, string name, double amount, DateTime date, string description)
        {
            var expense = _user.UserExpenses.FirstOrDefault(e => e.ExpenseId == id) as Expense;
            if (expense != null)
            {
                expense.Name = name;
                expense.Amount = amount;
                expense.DateOfExpense = date;
                expense.Descreption = description;
            }
        }
        //to delete Expense
        public void DeleteExpense(string id)
        {
            var expense = _user.UserExpenses.FirstOrDefault(e => e.ExpenseId == id);
            if (expense != null)
            {
                _user.UserExpenses.Remove(expense); // works fine even if property is read-only
            }
        }






    }

}


