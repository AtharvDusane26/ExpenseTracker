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
    using System.Collections.ObjectModel;
    using ExpenseTracker.Model.Services;
    using System.Windows;
    using MessageBoxImage = MessageBoxImage;
    using MessageBoxResult = MessageBoxResult;

    public class FinancialGoalsViewModel : ExpenseTrackerViewModelBase
    {
        private ObservableCollection<IFinancialGoal> _goals;
        private IFinancialGoal _selectedGoal;

        public FinancialGoalsViewModel() : base()
        {
            Init();
        }

        protected void Init()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            Goals = new ObservableCollection<IFinancialGoal>(userManager.GetAllGoals());
        }

        public ObservableCollection<IFinancialGoal> Goals
        {
            get => _goals;
            set
            {
                if (_goals != value)
                {
                    _goals = value;
                    OnPropertyChanged(nameof(Goals));
                }
            }
        }

        public IFinancialGoal SelectedGoal
        {
            get => _selectedGoal;
            set
            {
                if (_selectedGoal != value)
                {
                    _selectedGoal = value;
                    OnPropertyChanged(nameof(SelectedGoal));
                }
            }
        }

        protected override void Refresh()
        {
            Init();
            base.Refresh();
        }

        // Add a new goal
        public void AddGoal()
        {
            var vm = new CreateFinancialGoalViewModel();
            var viewService = ServiceProvider.Instance.Resolve<IViewService>();
            viewService.ShowDialog(vm); // Opens the CreateGoal view
            Refresh();
        }
        public void AddAmountToGoal()
        {
            if (SelectedGoal == null)
                return;
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            if (!SelectedGoal.Running)
            {
                var res = messageBox.Show("The goal is not active.,do you want to start it?", new MessageBoxArgs(MessageBoxButtons.YesNo, MessageBoxImage.Information), "Information");
                if (res == MessageBoxResult.No)
                    return;
                userManager.StartGoal(SelectedGoal.GoalId);
            }
            var result = messageBox.Show(
                $"Are you sure you want to add '{SelectedGoal.MonthlyContribution}'?",
                new MessageBoxArgs(MessageBoxButtons.YesNo, MessageBoxImage.Question),
                "Confirm Add"
            );
            if (result == MessageBoxResult.Yes)
            {
                userManager.AddAmountToGoal(SelectedGoal.GoalId);
                Refresh();
            }
        }
        public void EditGoal()
        {
            if (SelectedGoal == null)
                return;


            var vm = new CreateFinancialGoalViewModel();
            vm.Fill(SelectedGoal); // Fill ViewModel with selected goal data
            var viewService = ServiceProvider.Instance.Resolve<IViewService>();
            viewService.ShowDialog(vm); // Opens the same CreateGoal view in edit mode
            Refresh();
        }
        public void StopGoal()
        {
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            if (SelectedGoal == null)
                return;
            var result = messageBox.Show(
               $"Are you sure you want to stop goal - '{SelectedGoal.Name}'?",
               new MessageBoxArgs(MessageBoxButtons.YesNo, MessageBoxImage.Question),
               "Confirm Stop"
           );
            if (result == MessageBoxResult.Yes)
            {
                userManager.Stop(SelectedGoal.GoalId);
                messageBox.Show("Goal Stopped Successfully", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Success");
            }
        }

        // Optional: Delete selected goal
        public void DeleteGoal()
        {
            if (SelectedGoal == null)
                return;

            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
            var result = messageBox.Show(
                $"Are you sure you want to delete the goal '{SelectedGoal.Name}'?",
                new MessageBoxArgs(MessageBoxButtons.YesNo, MessageBoxImage.Question),
                "Confirm Delete"
            );

            if (result == MessageBoxResult.Yes)
            {
                var userManager = ServiceProvider.Instance.Resolve<UserManager>();
                userManager.DeleteGoal(SelectedGoal.GoalId);
                Refresh();
            }
        }
    }

}
