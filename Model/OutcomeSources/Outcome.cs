using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.OutcomeSources
{
    public class Outcome : IOutcome
    {
        private string _name;
        private double _amount;
        private OutcomeType _outComeType;
        private bool _freeze = false;
        public Outcome(string name,float amount)
        {
            _name = name;
            _amount = amount;
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public double Amount
        {
            get
            {
                return _amount;
            }
        }
        public OutcomeType OutComeType
        {
            get
            {
                return _outComeType;
            }
        }
        public bool Freeze
        {
            get
            {
                return _freeze;
            }
        }
        public void UpdateOutcomeType(OutcomeType outcomeType)
        {
            _outComeType = outcomeType;
        }
        public void UpdateFreezingStatus(bool status)
        {
            _freeze = status;
        }
    }
}
