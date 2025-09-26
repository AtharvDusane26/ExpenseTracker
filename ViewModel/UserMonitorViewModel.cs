using ExpenseTracker.Model;
using ExpenseTracker.Model.MonitoringAndReporting;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class UserMonitorViewModel : ExpenseTrackerViewModelBase
    {
        private readonly UserMonitor _monitor;

        public ObservableCollection<string> ExpenseInsights { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> SavingsInsights { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> GoalInsights { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> OutcomeInsights { get; } = new ObservableCollection<string>();
        public string PredictedExpenses { get; private set; }

        public UserMonitorViewModel()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>(); // your current user
            _monitor = userManager.UserMonitor;
            LoadInsights();
        }

        public void LoadInsights()
        {
            ExpenseInsights.Clear();
            foreach (var insight in _monitor.MonitorExpenses())
                ExpenseInsights.Add(insight);

            SavingsInsights.Clear();
            foreach (var insight in _monitor.MonitorSavings())
                SavingsInsights.Add(insight);

            GoalInsights.Clear();
            foreach (var insight in _monitor.MonitorGoals())
                GoalInsights.Add(insight);

            OutcomeInsights.Clear();
            foreach (var insight in _monitor.MonitorOutcomes())
                OutcomeInsights.Add(insight);

            PredictedExpenses = _monitor.PredictNextMonthExpenses();
            OnPropertyChanged(nameof(PredictedExpenses));
        }
    }
}
