using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.MonitoringAndReporting
{
    public class ReportManager
    {
        private readonly User _user;

        internal ReportManager(User user)
        {
            _user = user;
        }

        // 1. Summary Report
        public string GetMonthlySummary(int month, int year)
        {
            var incomes = _user.Incomes.Where(i => i.DateOfCredited?.Month == month && i.DateOfCredited?.Year == year).Sum(i => i.Amount);
            var expenses = _user.UserExpenses.Where(e => e.DateOfExpense.Month == month && e.DateOfExpense.Year == year).Sum(e => e.Amount);
            var savings = _user.Savings.Where(s => s.Date.Month == month && s.Date.Year == year).Sum(s => s.Amount);

            return $"📅 {month}/{year} Summary:\n" +
                   $"   Total Income: {incomes:C}\n" +
                   $"   Total Expenses: {expenses:C}\n" +
                   $"   Net Savings: {savings:C}\n" +
                   $"   Balance: {_user.Balance:C}";
        }

        // 2. Category Breakdown
        public Dictionary<string, double> GetExpenseBreakdown(int month, int year)
        {
            return _user.UserExpenses
                .Where(e => e.DateOfExpense.Month == month && e.DateOfExpense.Year == year)
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        }

        // 3. Yearly Trend
        public Dictionary<int, double> GetYearlyExpenseTrend(int year)
        {
            return _user.UserExpenses
                .Where(e => e.DateOfExpense.Year == year)
                .GroupBy(e => e.DateOfExpense.Month)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        }

        // 4. Goal Progress
        public Dictionary<string, double> GetGoalProgress()
        {
            return _user.Goals.ToDictionary(
                g => g.Name,
                g => (g.MonthlyContribution * g.DurationInMonths) / g.TargetAmount * 100
            );
        }

        // 5. Export Stub (later: PDF/Excel/Charts)
        public void ExportReport(string filePath)
        {
            // TODO: Use iTextSharp, EPPlus, etc. for PDF/Excel export
            throw new NotImplementedException("Export functionality not implemented yet.");
        }
    }

 

}
