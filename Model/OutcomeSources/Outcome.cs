using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.OutcomeSources
{
    public class Outcome : Transaction, IOutcome
    {
        private OutcomeType _outcomeType;
        public DateTime? LastPaidDate { get; set; } // Optional, track last payment

        public Outcome(string name, double amount, OutcomeType outcomeType)
            : base(name, amount)
        {
            _outcomeType = outcomeType;
        }

        public OutcomeType OutComeType => _outcomeType;

        public void UpdateOutcomeType(OutcomeType outcomeType) => _outcomeType = outcomeType;

        public override string Remind()
        {
            if (LastPaidDate.HasValue && LastPaidDate.Value.Month == DateTime.Today.Month)
                return null; // Already paid for this month
            return $"Reminder: {Name} of Rs.{Amount} is due soon.";
        }

        public override string WarnForPendingTransaction()
        {
            if (!LastPaidDate.HasValue || LastPaidDate.Value.Month < DateTime.Today.Month)
                return $"Pending: {Name} of Rs.{Amount} is overdue!";
            return null;
        }
    }
}
