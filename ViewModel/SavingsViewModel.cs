using ExpenseTracker.Model;
using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class SavingsViewModel : ExpenseTrackerViewModelBase
    {
        private ObservableCollection<ISaving> _savings;
        private ISaving _selectedSaving;

        public SavingsViewModel() : base()
        {
            Init();
        }

        protected void Init()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Savings = new ObservableCollection<ISaving>(userManager.GetAllSavings());
        }

        public ObservableCollection<ISaving> Savings
        {
            get => _savings;
            set
            {
                if (_savings != value)
                {
                    _savings = value;
                    OnPropertyChanged(nameof(Savings));
                }
            }
        }

        public ISaving SelectedSaving
        {
            get => _selectedSaving;
            set
            {
                if (_selectedSaving != value)
                {
                    _selectedSaving = value;
                    OnPropertyChanged(nameof(SelectedSaving));
                }
            }
        }

        protected override void Refresh()
        {
            Init();
            base.Refresh();
        }

        public void AddSaving()
        {
            var vm = new SavingEntryViewModel();
            var sv = ServiceProvider.Instance.Resolve<IViewService>();
            sv.ShowDialog(vm);
            Refresh();
        }

        public void EditSaving()
        {
            if (SelectedSaving == null) return;

            var vm = new SavingEntryViewModel();
            vm.Fill(SelectedSaving);
            var sv = ServiceProvider.Instance.Resolve<IViewService>();
            sv.ShowDialog(vm);
            Refresh();
        }
    }
}
