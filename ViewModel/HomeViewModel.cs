using ExpenseTracker.Model;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public double UserBalance { get; set; }
        public double SavingsBalance { get; set; }
        public ObservableCollection<string> Notifications { get; set; } = new ObservableCollection<string>();

        public HomeViewModel()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            UserBalance = userManager.GetBalance();
            SavingsBalance = userManager.GetSavingsBalance();
           Notifications = new ObservableCollection<string>(userManager.GetReminders(1).Concat(userManager.GetSavingsReminders(1)));
        }
    }
}
