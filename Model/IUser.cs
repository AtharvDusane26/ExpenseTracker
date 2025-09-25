using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.Transactions;
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
        void Create(string name, string phoneNumber, int age, double initialBalance);
        void UpdateBalance(double amount);
    }
}
