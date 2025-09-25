using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public interface IIncomeProvider : ITransactionProvider
    {
        List<IIncome> Incomes { get; }
        IIncome GetIncome(string incomeName);
        void AddIncome(IIncome income);
        void DeleteIncome(string incomeName);
        void UpdateIncome(IIncome income);
    }
}
