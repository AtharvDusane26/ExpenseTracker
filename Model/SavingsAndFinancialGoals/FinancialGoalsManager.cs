using ExpenseTracker.Model.Services;
using ExpenseTracker.Model.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

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
            AddNotification("Goal Created", goal.GoalId, NotificationType.Other, $"Financial goal '{name}' created with target Rs.{targetAmount}.");
            return goal;
        }

        internal void DeleteGoal(string goalId)
        {
            var goal = _user.GetGoal(goalId);
            _user.DeleteGoal(goalId);
            Save();
            if (goal != null)
                AddNotification("Goal Deleted", goal.GoalId, NotificationType.Other, $"Financial goal '{goal.Name}' has been deleted.");
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
                AddNotification(
                    "Monthly Contribution",
                    goal.GoalId,
                    NotificationType.Credited,
                    $"Rs.{goal.MonthlyContribution} contributed to savings for goal '{goal.Name}'."
                );
            }
            Save();
        }

        private void Save()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            userManager.Save(_user as User);
        }

        private void AddNotification(string name, string referenceId, NotificationType type, string message)
        {
            var notificationManager = ServiceProvider.Instance.Resolve<NotificationManager>();
            if (notificationManager != null)
            {
                notificationManager.AddNotification(name, referenceId, type, message, DateTime.Now);
            }
        }
    }
}
