using ExpenseTracker.Model;
using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class IncomeViewModel
    {
        public ObservableCollection<IIncome> Incomes { get; set; }

        public IncomeViewModel()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Incomes = new ObservableCollection<IIncome>(userManager.GetAllIncomes());
        }
    }
}
