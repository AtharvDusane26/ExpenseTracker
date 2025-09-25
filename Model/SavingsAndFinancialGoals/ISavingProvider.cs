using ExpenseTracker.Model.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    public interface ISavingsProvider
    {
        double SavingsBalance { get; }
        List<ISaving> Savings { get; }
        ISaving AddToSavings(double amount, string category = "General");
        void WithdrawFromSavings(double amount, string category = null);
        List<ISaving> GetAllSavings();
        List<INotification> GetSavingsReminders(int daysBefore = 1);
    }
}
