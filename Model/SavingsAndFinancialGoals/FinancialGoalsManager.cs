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
        internal void UpdateGoal(string id, string name, double targetAmount, int durationInYears, double monthlyInterestRate = 0)
        {
            _user.UpdateGoal(id, name, targetAmount, durationInYears, monthlyInterestRate);
            Save();
            AddNotification("Goal Updated", id, NotificationType.Other, $"Financial goal '{name}' has been updated.");
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
                (_user as User).AddToSavings(goal.MonthlyContribution, DateTime.Now);
                AddNotification(
                    "Monthly Contribution",
                    goal.GoalId,
                    NotificationType.Credited,
                    $"Rs.{goal.MonthlyContribution} contributed to savings for goal '{goal.Name}'."
                );
            }
            Save();
        }
        internal void StartGoal(string id)
        {
            var goal = _user.GetGoal(id);
            if (goal != null && !goal.Running)
            {
                goal.Start(DateTime.Now);
                Save();
                AddNotification("Goal Started", goal.GoalId, NotificationType.Other, $"Financial goal '{goal.Name}' has been started from '{DateTime.Now}'.");
            }
        }
        internal void Stop(string id)
        {
            var goal = _user.GetGoal(id);
            if (goal != null && goal.Running)
            {
                goal.Stop();
                Save();
                AddNotification("Goal Stopped", goal.GoalId, NotificationType.Other, $"Financial goal '{goal.Name}' has been stopped.");
            }
        }
        internal bool AddAmount(string id)
        {
            var goal = _user.GetGoal(id);
            if (goal != null && goal.Running)
            {
                if (goal.DateOfLastContribution.Month == DateTime.Now.Month)
                {
                    AddNotification(
                        "Contribution Already Made",
                        goal.GoalId,
                        NotificationType.Other,
                        $"Monthly contribution for goal '{goal.Name}' has already been made for this month."
                    );
                    return false;
                }
                if (goal.MonthlyContribution > (_user as User).Balance)
                {
                    AddNotification(
                        "Insufficient Balance",
                        goal.GoalId,
                        NotificationType.LowBalance,
                        $"Insufficient balance to add monthly contribution of Rs.{goal.MonthlyContribution} to goal '{goal.Name}'. Current balance: Rs.{(_user as User).Balance}."
                    );
                    return false;
                }
                var cmt = goal.CollectedAmount;
                goal.AddAmount();
                (_user as User).AddToSavings(goal.MonthlyContribution, DateTime.Now, goal.Name);
                Save();
                AddNotification(
                    "Amount Added to Goal",
                    goal.GoalId,
                    NotificationType.Debited,
                    $"Rs.{goal.MonthlyContribution} added to goal '{goal.Name}'. Collected amount: Rs.{cmt} -> Rs.{goal.CollectedAmount}."
                );
                return true;
            }
            return false;
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
