using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.OutcomeSources
{
    public interface IOutcomeProvider : ITransactionProvider
    {
        List<IOutcome> Outcomes { get; }
        IOutcome GetOutcome(string outcomeName);
        void AddOutcome(IOutcome outcome);
        void DeleteOutcome(string outcomeName);
        void UpdateOutcome(IOutcome outcome);
    }
}
