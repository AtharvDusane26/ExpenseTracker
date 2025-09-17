using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public interface ITransaction
    {
        string Id { get; }
        string Name { get; }
        double Amount { get; }
        bool Freeze { get; }
        int DayOfTransaction{ get; }
        bool GiveReminder { get; }
        void FreezeTransaction(bool status);
        void Create(string id);
        bool UpdateAmount(float newAmount);
        void UpdateTransactionDay(int day);
        string Remind();
        string WarnForPendingTransaction();

    }

}
