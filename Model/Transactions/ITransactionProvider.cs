using ExpenseTracker.Model.OutcomeSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Transactions
{
    public interface ITransactionProvider 
    {
        List<ITransaction> Transactions { get; }
        List<ITransactionHistory> TransactionHistory { get; }
        void AddToHistory(string message);
    }
}
