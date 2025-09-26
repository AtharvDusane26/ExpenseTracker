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
        double CollectedAmount { get; }
        int DurationInMonths { get; }
        double MonthlyContribution { get; }
        double MonthlyInterestRate { get; }
        DateTime StartDate { get; }
        bool Running { get; }
        DateTime EndDate { get; }
        DateTime DateOfLastContribution { get; }
        void AddAmount();

        void Start(DateTime date);
        void Stop();
    }
}
