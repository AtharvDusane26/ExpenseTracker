using System;
using System.Collections.ObjectModel;
using ExpenseTracker.Model;
using ExpenseTracker.Model.MonitoringAndReporting;
using ExpenseTracker.Model.Services;

namespace ExpenseTracker.ViewModel
{
    public class ReportViewModel : ExpenseTrackerViewModelBase
    {
        private readonly ReportManager _reportManager;
        private int _selectedMonthIndex; // 0 = January
        private int _selectedYear;
        private string _monthlySummary;

        public ReportViewModel()
        {
            var user = ServiceProvider.Instance.Resolve<UserManager>();
            _reportManager = user.ReportManager;

            // Default month/year
            SelectedMonthIndex = DateTime.Now.Month - 1;
            SelectedYear = DateTime.Now.Year;
            LoadReport();
        }

        // Bindable properties
        public ObservableCollection<string> Months { get; } = new ObservableCollection<string>
        {
            "January","February","March","April","May","June",
            "July","August","September","October","November","December"
        };

        public int SelectedMonthIndex
        {
            get => _selectedMonthIndex;
            set
            {
                SetProperty(ref _selectedMonthIndex, value);
                LoadReport();
            }
        }

        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                SetProperty(ref _selectedYear, value);
                LoadReport();
            }
        }

        public string MonthlySummary
        {
            get => _monthlySummary;
            set => SetProperty(ref _monthlySummary, value);
        }

        public ObservableCollection<KeyValuePair<string, double>> ExpenseBreakdown { get; } = new();
        public ObservableCollection<KeyValuePair<int, double>> YearlyExpenseTrend { get; } = new();
        public ObservableCollection<KeyValuePair<string, double>> GoalProgress { get; } = new();

        // Load data from ReportManager
        private void LoadReport()
        {
            if (_reportManager == null)
                return;

            // Month = index + 1
            int month = SelectedMonthIndex + 1;
            MonthlySummary = _reportManager.GetMonthlySummary(month, SelectedYear);

            // Expense breakdown
            ExpenseBreakdown.Clear();
            foreach (var kv in _reportManager.GetExpenseBreakdown(month, SelectedYear))
                ExpenseBreakdown.Add(kv);

            // Yearly trend
            YearlyExpenseTrend.Clear();
            foreach (var kv in _reportManager.GetYearlyExpenseTrend(SelectedYear))
                YearlyExpenseTrend.Add(kv);

            // Goal progress
            GoalProgress.Clear();
            foreach (var kv in _reportManager.GetGoalProgress())
                GoalProgress.Add(kv);
        }

        // Optional: Export report
        public void ExportReport(string filePath)
        {
            _reportManager.ExportReport(filePath);
        }
        public string GetTransactionHistory()
        {
            return _reportManager.GetTransactionHistory(SelectedMonthIndex + 1);
        }
        protected override void Refresh()
        {
            LoadReport();
            base.Refresh();
        }
    }
}
