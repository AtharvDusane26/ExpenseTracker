using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public class YearlyIncome : Income
    {
        public YearlyIncome(string name, double amount, SourceOfIncome source = SourceOfIncome.Other)
            : base(name, amount, source) { }
        public override bool CheckForLastCredit(out string message)
        {
            message = string.Empty;
            if (DateOfCredited == null)
                return true;
            if (DateOfCredited?.Year == DateTime.Today.Year)
            {
                message = $"You have already credited your {Name} income for this year.";
                return false;
            }
            return true;
        }
    }
}
