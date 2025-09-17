using ExpenseTracker.Model.IncomeSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public interface IIncomeProvider
    {
        List<IIncome> Incomes { get; }
        IIncome GetIncome(string incomeName);
        void AddIncome(IIncome income);
        void DeleteIncome(string incomeName);
        void UpdateIncome(IIncome income);
    }
}
