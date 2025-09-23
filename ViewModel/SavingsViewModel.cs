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
    public class SavingsViewModel
    {
        public double SavingsBalance { get; set; }
        public ObservableCollection<ISaving> SavingsRecords { get; set; }

        public SavingsViewModel()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            SavingsBalance = userManager.GetSavingsBalance();
            SavingsRecords = new ObservableCollection<ISaving>(userManager.GetAllSavings());
        }
    }
}
