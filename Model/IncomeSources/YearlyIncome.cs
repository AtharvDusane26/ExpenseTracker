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
        public DateTime DateOfIncome { get; set; }
        public YearlyIncome(string name, double amount, SourceOfIncome source = SourceOfIncome.Other)
            : base(name, amount, source) { }
    }
}
