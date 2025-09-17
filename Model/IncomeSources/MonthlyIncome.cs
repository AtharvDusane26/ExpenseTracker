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
        private DateTime _dateOfIncome;
        public DateTime DateOfIncome
        {
            get
            {
                return _dateOfIncome;
            }
            set
            {
                _dateOfIncome = value;
            }
        }
    }
}
