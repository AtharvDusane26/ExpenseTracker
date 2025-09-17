using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public abstract class Income : Transaction, IIncome
    {
        private SourceOfIncome _sourceOfIncome;
        public Income(string name, float amount) : base (name,amount)
        {
           
        }      
        public SourceOfIncome SourceOfIncome
        {
            get
            {
                return _sourceOfIncome;
            }
        }  
        public void UpdateSourceOfIncome(SourceOfIncome sourceOfIncome)
        {
           _sourceOfIncome = sourceOfIncome;
        }      
    }
}
