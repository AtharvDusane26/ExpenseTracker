using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.IncomeSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public interface IUser
    {
        string UserId { get; }
        string Name { get; }
        string PhoneNumber { get; }
        int Age { get; }
        double Balance { get; }
        List<ITransaction> Transactions { get; }
        List<IExpense> UserExpenses { get; }
    }
}
