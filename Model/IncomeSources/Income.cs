using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public abstract class Income : Transaction, IIncome
    {
        private SourceOfIncome _source;
        public DateTime? DateOfCredited { get; set; } = null;

        public Income(string name, double amount, SourceOfIncome source = SourceOfIncome.Salary)
            : base(name, amount)
        {
            _source = source;
        }

        public SourceOfIncome SourceOfIncome => _source;

        public void UpdateSourceOfIncome(SourceOfIncome source) => _source = source;
       public abstract bool CheckForLastCredit(out string message);

    }
}
