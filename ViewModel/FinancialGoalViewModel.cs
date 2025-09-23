using ExpenseTracker.Model;
using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class FinancialGoalsViewModel
    {
        public ObservableCollection<IFinancialGoal> Goals { get; set; }

        public FinancialGoalsViewModel()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Goals = new ObservableCollection<IFinancialGoal>(userManager.GetAllGoals());
        }
    }
}
