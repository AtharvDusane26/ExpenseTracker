using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public interface IIncome : ITransaction
    {
        SourceOfIncome SourceOfIncome { get; }
        void UpdateSourceOfIncome(SourceOfIncome sourceOfIncome);
    }
}
