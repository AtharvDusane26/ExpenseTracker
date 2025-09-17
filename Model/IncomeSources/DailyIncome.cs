using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.IncomeSources
{
    public class DailyIncome : Income
    {
        public DailyIncome(string name, float amount) : base(name, amount) 
        {
            
        }
    }
}
