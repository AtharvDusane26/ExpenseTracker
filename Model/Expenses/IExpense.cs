using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Expenses
{
    public interface IExpense
    {
        string ExpenseId { get; }
        string Name { get; }
        double Amount { get; }
        DateTime DateOfExpense { get; }
        string Description { get; }
        string Category { get; }
        bool Freeze { get; }
        void FreezeTransaction(bool status);
    }
}
