using ExpenseTracker.Model;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.ObjectModel;

namespace ExpenseTracker.ViewModel
{
    public class OutcomeViewModel : ExpenseTrackerViewModelBase
    {
        private ObservableCollection<IOutcome> _outcomes;
        private IOutcome _selectedOutcome;

        public OutcomeViewModel() : base()
        {
            Init();
        }

        protected void Init()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Outcomes = new ObservableCollection<IOutcome>(userManager.GetAllOutcomes());
        }

        public ObservableCollection<IOutcome> Outcomes
        {
            get => _outcomes;
            set
            {
                if (_outcomes != value)
                {
                    _outcomes = value;
                    OnPropertyChanged(nameof(Outcomes));
                }
            }
        }

        public IOutcome SelectedOutcome
        {
            get => _selectedOutcome;
            set
            {
                if (_selectedOutcome != value)
                {
                    _selectedOutcome = value;
                    OnPropertyChanged(nameof(SelectedOutcome));
                }
            }
        }

        protected override void Refresh()
        {
            Init();
            base.Refresh();
        }

        public void AddOutcome()
        {
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            if (!userManager.CheckPaidOutcome(SelectedOutcome , out string error))
            {
                messageBox.Show(error, new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Warning), "Warning");
                return;
            }
            var result = messageBox.Show($"Are you sure you want to pay Rs.{SelectedOutcome.Amount}?", new MessageBoxArgs(MessageBoxButtons.YesNo, MessageBoxImage.Question), "Confirm Add to Balance");
            if (result == MessageBoxResult.No)
                return;
            if (userManager.CreateOutcomeAsExpense(SelectedOutcome.Id))
            {
                messageBox.Show("Outcome added to balance successfully.", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Success");
            }
            else
            {
                messageBox.Show("Failed to add outcome to balance.", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
            Refresh();
        }
      
        public void EditOutcome()
        {
            var vm = new OutcomeEntryViewModel();
            vm.Fill(SelectedOutcome);
            var sv = ServiceProvider.Instance.Resolve<IViewService>();
            sv.ShowDialog(vm);
            Refresh();
        }
    }
}
