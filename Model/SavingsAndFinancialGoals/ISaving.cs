using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    public interface ISaving
    {
        string SavingId { get; }
        double Amount { get; }
        DateTime Date { get; }
        string Category { get; }
        void UpdateAmount(double newAmount);
    }
}
