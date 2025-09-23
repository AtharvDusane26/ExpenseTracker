using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    internal class SavingsManager
    {
        private readonly ISavingsProvider _user;

        internal SavingsManager(ISavingsProvider user)
        {
            _user = user;
        }

        internal void AddToSavings(double amount, string category = "General")
        {
            _user.AddToSavings(amount, category);
            Save();
        }

        internal void WithdrawFromSavings(double amount)
        {
            _user.WithdrawFromSavings(amount);
            Save();
        }

        internal double GetSavingsBalance() => _user.SavingsBalance;

        internal List<string> GetReminders(int daysBefore = 1)
        {
            return _user.GetSavingsReminders(daysBefore);
        }
        internal List<ISaving> Get() => _user.Savings.ToList();
        private void Save()
        {
            UserManager.Save(_user as User);
        }
    }
}
