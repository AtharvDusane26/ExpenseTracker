using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    public interface IFinancialGoalsProvider
    {
        List<IFinancialGoal> Goals { get; }
        IFinancialGoal CreateGoal(string name, double targetAmount, int durationInYears, double monthlyInterestRate = 0);
        void DeleteGoal(string goalId);
        IFinancialGoal GetGoal(string goalId);
    }
}
