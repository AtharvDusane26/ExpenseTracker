using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.OutcomeSources
{
    public class Outcome : Transaction, IOutcome
    {
        private OutcomeType _outComeType;
        public Outcome(string name, float amount) : base(name, amount)
        {

        }
        public OutcomeType OutComeType
        {
            get
            {
                return _outComeType;
            }
        }
        public void UpdateOutcomeType(OutcomeType outcomeType)
        {
            _outComeType = outcomeType;
        }
    }
}
