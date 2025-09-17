using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.OutcomeSources
{
    public interface IOutcome : ITransaction
    {
        OutcomeType OutComeType { get; }
        void UpdateOutcomeType(OutcomeType outcomeType);
       
    }
}
