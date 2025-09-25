using ExpenseTracker.Model;
using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    public class IncomeViewModel : ExpenseTrackerViewModelBase
    {
        private ObservableCollection<IIncome> _incomes;
        private IIncome _selectedIncome;
        public IncomeViewModel() : base()
        {
            Init();
        }
        protected void Init()
        {
            Refresh();
        }
        public void Refresh()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Incomes = new ObservableCollection<IIncome>(userManager.GetAllIncomes());
        }
        public ObservableCollection<IIncome> Incomes
        {
            get => _incomes;
            set
            {
                if (_incomes != value)
                {
                    _incomes = value;
                    OnPropertyChanged(nameof(Incomes));
                }
            }
        }
        public IIncome SelectedIncome
        {
            get => _selectedIncome;
            set
            {
                if (_selectedIncome != value)
                {
                    _selectedIncome = value;
                    OnPropertyChanged(nameof(SelectedIncome));
                }
            }
        }
        protected override void Refresh()
        {
            Init();
            base.Refresh();

        }
        public void AddIncome()
        {
            var mb = ServiceProvider.Instance.Resolve<IMessageBoxService>();
           
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            userManager.UpdateBalance(SelectedIncome);
            Refresh();
        }
        public void EditIncome()
        {
            var vm = new IncomeEntryViewModel();
            vm.Fill(SelectedIncome);
            var sv = ServiceProvider.Instance.Resolve<IViewService>();
            sv.ShowDialog(vm);
            Refresh();
        }
    }
}
