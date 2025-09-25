using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public class DailyIncome : Income
    {
        public DailyIncome(string name, double amount, SourceOfIncome source = SourceOfIncome.Salary)
            : base(name, amount, source) { }
        public override bool CheckForLastCredit(out string message)
        {
            message = string.Empty;
            if (DateOfCredited == null)
                return true;
            if (DateOfCredited?.Day == DateTime.Today.Day)
            {
                message = $"You have already credited your {Name} income for today.";
                return false;
            }
            return true;
        }
    }
}
