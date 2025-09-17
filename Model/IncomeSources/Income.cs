using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public abstract class Income : IIncome
    {
        private string _id;
        private string _name;
        private float _amount;
        private SourceOfIncome _sourceOfIncome;
        private bool _freeze = false;

        public string IncomeId
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        public string IncomeName
        {
            get
            {
                return _name;
            }
            protected set
            {
                _name = value;
            }
        }
        public float Amount
        {
            get
            {
                return _amount;
            }           
        }

        public SourceOfIncome SourceOfIncome
        {
            get
            {
                return _sourceOfIncome;
            }
            protected set
            {
                _sourceOfIncome = value;
            }
        }
        public bool Freeze
        {
            get
            {
                return _freeze;
            }
        }
        public virtual void CreateIncome(string incomeName, SourceOfIncome sourceOfIncome)
        {
            IncomeId = Guid.NewGuid().ToString();
            IncomeName = incomeName;
            SourceOfIncome = sourceOfIncome;
        }
        public void UpdateFreezingStatus(bool status)
        {
            _freeze = status;
        }
        public virtual bool UpdateAmount(float newAmount)
        {
            if (_freeze)
                return false;
            _amount = newAmount;
            return true;
        }

    }
}
