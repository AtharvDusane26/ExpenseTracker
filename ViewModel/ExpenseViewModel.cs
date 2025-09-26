using ExpenseTracker.Model;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class ExpenseViewModel : ExpenseTrackerViewModelBase
    {
        private ObservableCollection<IExpense> _expenses;
        private IExpense _selectedExpense;

        public ExpenseViewModel()
        {
            Init();
        }

        protected void Init()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Expenses = new ObservableCollection<IExpense>(userManager.GetAllExpenses().Where(o => o.DateOfExpense.Month == DateTime.Now.Month));
        }
        public bool IsEditable
        {
            get
            {
                return SelectedExpense != null && SelectedExpense.Category != "Outcome";
            }
        }
        public ObservableCollection<IExpense> Expenses
        {
            get => _expenses;
            set
            {
                if (_expenses != value)
                {
                    _expenses = value;
                    OnPropertyChanged(nameof(Expenses));
                }
            }
        }

        public IExpense SelectedExpense
        {
            get => _selectedExpense;
            set
            {
                if (_selectedExpense != value)
                {
                    _selectedExpense = value;
                    OnPropertyChanged(nameof(SelectedExpense));
                    OnPropertyChanged(nameof(IsEditable));
                }
            }
        }

        protected override void Refresh()
        {
            Init();
            base.Refresh();
        }

        public void AddExpense()
        {
            var vm = new ExpenseEntryViewModel();
            var sv = ServiceProvider.Instance.Resolve<IViewService>();
            sv.ShowDialog(vm);
            Refresh();
        }

        public void EditExpense()
        {
            if (SelectedExpense == null) return;
            var vm = new ExpenseEntryViewModel();
            vm.Fill(SelectedExpense);
            var sv = ServiceProvider.Instance.Resolve<IViewService>();
            sv.ShowDialog(vm);
            Refresh();
        }

        public void ToggleFreezeSelected()
        {
            if (SelectedExpense != null)
            {
                SelectedExpense.FreezeTransaction(!SelectedExpense.Freeze);
                Refresh();
            }
        }
    }
}
