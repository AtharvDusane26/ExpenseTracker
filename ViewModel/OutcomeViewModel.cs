using ExpenseTracker.Model;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class OutcomeViewModel
    {
        public ObservableCollection<IOutcome> Outcomes { get; set; }

        public OutcomeViewModel()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Outcomes = new ObservableCollection<IOutcome>(userManager.GetAllOutcomes());
        }
    }
}
