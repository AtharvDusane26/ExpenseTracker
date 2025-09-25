using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public class MonthlyIncome : Income
    {
        public MonthlyIncome(string name, double amount, SourceOfIncome source = SourceOfIncome.Salary)
            : base(name, amount, source) { }
        public override bool CheckForLastCredit(out string message)
        {
            message = string.Empty;
            if(DateOfCredited == null)
                return true;
            if (DateOfCredited?.Month == DateTime.Today.Month)
            {
                message = $"You have already credited your {Name} income for this month.";
                return false;
            }
            return true;
        }
    }
}
