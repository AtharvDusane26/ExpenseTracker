using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public interface IIncome : ITransaction
    {
        public DateTime? DateOfCredited { get; set; }
        SourceOfIncome SourceOfIncome { get; }
        void UpdateSourceOfIncome(SourceOfIncome sourceOfIncome);
        bool CheckForLastCredit(out string message);
    }
}
