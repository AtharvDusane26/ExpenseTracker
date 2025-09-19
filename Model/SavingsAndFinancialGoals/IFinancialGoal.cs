using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    public interface IFinancialGoal
    {
        string GoalId { get; }
        string Name { get; }
        double TargetAmount { get; }
        int DurationInMonths { get; }
        double MonthlyContribution { get; }
        DateTime StartDate { get; }
    }
}
