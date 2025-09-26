using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Transactions
{
    public interface ITransactionHistory
    {
        public string Id { get; }
        DateTime Date { get; }
        double Balance { get; }
        double SavingBalance { get; }
        double TotalExpenseAmount { get; }
        StringBuilder History { get; }
        void Update(double balance, double savingBalance, double totalExpenseAmount,DateTime date);
        void AddHistoryEntry(string entry);
    }
}
