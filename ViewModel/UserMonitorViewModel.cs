using ExpenseTracker.Model;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class UserMonitorViewModel
    {
        public ObservableCollection<string> Insights { get; set; }

        public UserMonitorViewModel()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Insights = new ObservableCollection<string>(userManager.UserMonitor.MonitorExpenses());
        }
    }
}
