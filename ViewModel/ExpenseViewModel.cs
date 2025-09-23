using ExpenseTracker.Model;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class ExpensesViewModel
    {
        public ObservableCollection<IExpense> Expenses { get; set; }

        public ExpensesViewModel()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Expenses = new ObservableCollection<IExpense>(userManager.GetAllExpenses());
        }
    }
}
