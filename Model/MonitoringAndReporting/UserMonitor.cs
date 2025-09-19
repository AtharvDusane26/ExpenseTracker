using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.MonitoringAndReporting
{
    public class UserMonitor
    {
        private readonly User _user;

        internal UserMonitor(User user)
        {
            _user = user;
        }

        // 1. Expense Monitoring (Category + Trend)
        public List<string> MonitorExpenses()
        {
            var insights = new List<string>();
            var lastMonth = DateTime.Now.AddMonths(-1).Month;
            var lastMonthExpenses = _user.UserExpenses
                .Where(e => e.DateOfExpense.Month == lastMonth)
                .Sum(e => e.Amount);

            var currentMonthExpenses = _user.UserExpenses
                .Where(e => e.DateOfExpense.Month == DateTime.Now.Month)
                .Sum(e => e.Amount);

            if (currentMonthExpenses > lastMonthExpenses)
                insights.Add($"⚠️ This month's expenses ({currentMonthExpenses:C}) are higher than last month ({lastMonthExpenses:C}).");
            else
                insights.Add($"✅ Expenses are under control compared to last month.");

            // Category-level insights
            var grouped = _user.UserExpenses
                .Where(e => e.DateOfExpense.Month == DateTime.Now.Month)
                .GroupBy(e => e.Category);

            foreach (var g in grouped)
            {
                var categoryTotal = g.Sum(x => x.Amount);
                insights.Add($"📊 {g.Key} spending: {categoryTotal:C}");
            }

            return insights;
        }

        // 2. Savings Monitoring
        public List<string> MonitorSavings()
        {
            var insights = new List<string>();

            if (_user.SavingsBalance < 10000) // threshold can be dynamic
                insights.Add($"⚠️ Savings balance is low ({_user.SavingsBalance:C}). Try to add more.");
            else
                insights.Add($"✅ Savings balance is healthy ({_user.SavingsBalance:C}).");

            // Category-wise breakdown
            var grouped = _user.Savings.GroupBy(s => s.Category);
            foreach (var g in grouped)
            {
                insights.Add($"💰 {g.Key} fund: {g.Sum(s => s.Amount):C}");
            }

            return insights;
        }

        // 3. Goal Monitoring
        public List<string> MonitorGoals()
        {
            var insights = new List<string>();

            foreach (var goal in _user.Goals)
            {
                // calculate remaining months
                var monthsPassed = ((DateTime.Now.Year - goal.StartDate.Year) * 12) + (DateTime.Now.Month - goal.StartDate.Month);
                var monthsLeft = goal.DurationInMonths - monthsPassed;

                if (monthsLeft <= 0)
                {
                    insights.Add($"⚠️ Goal '{goal.Name}' has reached or passed its deadline.");
                    continue;
                }

                // calculate remaining amount
                var remaining = goal.TargetAmount - (goal.MonthlyContribution * monthsPassed);

                // required monthly contribution
                var requiredMonthlyContribution = remaining / monthsLeft;

                if (goal.MonthlyContribution < requiredMonthlyContribution)
                    insights.Add($"⚠️ Goal '{goal.Name}' is underfunded. Increase contribution to at least {requiredMonthlyContribution:0.00}.");
                else
                    insights.Add($"✅ Goal '{goal.Name}' is on track.");
            }

            return insights;
        }

        // 4. Outcome Monitoring (Reminders for EMI, Rent, etc.)
        public List<string> MonitorOutcomes()
        {
            var insights = new List<string>();
            var upcoming = _user.Outcomes.Where(o => !o.Freeze);

            foreach (var outcome in upcoming)
            {
                if ((outcome.DayOfTransaction - DateTime.Now.Day) <= 3)
                {
                    insights.Add($"⏰ Reminder: {outcome.Name} of {outcome.Amount:C} is due soon.");
                }
            }

            return insights;
        }

        // 5. Predictive Insights
        public string PredictNextMonthExpenses()
        {
            var last3Months = _user.UserExpenses
                .Where(e => e.DateOfExpense > DateTime.Now.AddMonths(-3))
                .GroupBy(e => e.DateOfExpense.Month)
                .Select(g => g.Sum(e => e.Amount));

            if (!last3Months.Any()) return "No data to predict.";

            var avg = last3Months.Average();
            return $"📈 Predicted next month’s expenses: {avg:C} (based on last 3 months).";
        }
    }

}
