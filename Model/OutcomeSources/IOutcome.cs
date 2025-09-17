using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.OutcomeSources
{
    public interface IOutcome
    {
        string Name { get; }
        double Amount { get; }
        OutcomeType OutComeType { get; }
        bool Freeze { get; }
        void UpdateOutcomeType(OutcomeType outcomeType);
        void UpdateFreezingStatus(bool status);
    }
}
