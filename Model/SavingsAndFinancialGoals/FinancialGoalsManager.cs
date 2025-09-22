using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    internal class FinancialGoalsManager
    {
        private readonly IFinancialGoalsProvider _user;

        internal FinancialGoalsManager(IFinancialGoalsProvider user)
        {
            _user = user;
        }

        internal IFinancialGoal CreateGoal(string name, double targetAmount, int durationInYears, double monthlyInterestRate = 0)
        {
            var goal = _user.CreateGoal(name, targetAmount, durationInYears, monthlyInterestRate);
            Save();
            return goal;
        }

        internal void DeleteGoal(string goalId)
        {
            _user.DeleteGoal(goalId);
            Save();
        }

        internal IFinancialGoal GetGoal(string goalId)
        {
            return _user.GetGoal(goalId);
        }

        internal List<IFinancialGoal> GetAllGoals() => _user.Goals;

        internal void AllocateMonthlyContributionToSavings()
        {
            foreach (var goal in _user.Goals)
            {
                (_user as User).AddToSavings(goal.MonthlyContribution);
            }
            Save();
        }
        private void Save()
        {
            UserManager.Save(_user as User);
        }
    }
}
