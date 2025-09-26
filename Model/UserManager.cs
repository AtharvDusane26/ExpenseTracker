using ExpenseTracker.DataManagement.Serialization;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.MonitoringAndReporting;
using ExpenseTracker.Model.Notifications;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Services;
using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Transactions;
using ExpenseTracker.ViewModel;
using LiveChartsCore;
using System;
using System.Collections.Generic;

namespace ExpenseTracker.Model
{
    public class UserManager : IDisposable
    {
        private User _user;
        private TransactionManager _transactionManager;
        private ExpenseManager _expenseManager;
        private SavingsManager _savingsManager;
        private FinancialGoalsManager _goalsManager;
        private ReportManager _reportManager;
        private UserMonitor _userMonitor;
        private NotificationManager _notificationManager;
        public void SetUser(string name)
        {
            _user = GetUser(name) ?? throw new ArgumentNullException("user not found");
            InitializeManagers();
        }
        public ReportManager ReportManager => _reportManager;
        public UserMonitor UserMonitor => _userMonitor;
        private void InitializeManagers()
        {
            _transactionManager = new TransactionManager(_user);
            _expenseManager = new ExpenseManager(_user);
            _savingsManager = new SavingsManager(_user);
            _goalsManager = new FinancialGoalsManager(_user);
            _reportManager = new ReportManager(_user);
            _userMonitor = new UserMonitor(_user);
            var services = ServiceProvider.Instance;
            _notificationManager = services.Resolve<NotificationManager>();
            _notificationManager.Initialize(_user);
        }
        private User GetUser(string name)
        {
            var services = ServiceProvider.Instance;
            var serializableBase = services.Resolve<SerializableBase>();
            var users = serializableBase.Get();
            if (users != null && users.Count > 0)
            {
                var user = users.Find(u => u.Name == name);
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }
        public void UpdateBalance(IIncome income, bool revertAndUpdate = false, double oldBalance = 0)
        {
            var userIncome = _user.GetIncome(income.Name);
            if (userIncome != null)
            {
                var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
                if (!userIncome.CheckForLastCredit(out string message))
                {
                    messageBox.Show(message, new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Warning), "Warning");
                   // CreateNotification("Balance Update Failed", income.Id, NotificationType.Other, message);
                    return;
                }
                var result = messageBox.Show($"Are you sure you want to add Rs.{userIncome.Amount} to your balance?", new MessageBoxArgs(MessageBoxButtons.YesNo, MessageBoxImage.Question), "Confirm Add to Balance");
                if (result == MessageBoxResult.No)
                    return;
                if (revertAndUpdate)
                {
                    _user.Balance -= oldBalance;
                    _user.Balance += userIncome.Amount;
                }
                else
                {
                    _user.Balance += userIncome.Amount;
                }
                userIncome.DateOfCredited = DateTime.Now;
                Save(_user);
                messageBox.Show("Balance Updated", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Balance Updated");
               // CreateNotification("Balance Updated", userIncome.Id, NotificationType.Credited, $"Rs.{userIncome.Amount} added to your balance.");
            }
        }
        // ---------------- CREATE NEW USER ----------------
        public User CreateUser(string name, string phoneNumber = "", int age = 0, double initialBalance = 0)
        {
            _user = new User(Guid.NewGuid().ToString());
            _user.Create(name, phoneNumber, age, initialBalance);
            InitializeManagers();
            Save(_user);
            return _user;
        }
        public bool CheckIfUserNameAlreadyExist(string name)
        {
            var services = ServiceProvider.Instance;
            var serializableBase = services.Resolve<SerializableBase>();
            var users = serializableBase.Get();
            if (users.Any(o => o.Name == name))
                return true;
            return false;
        }
        // ---------------- INCOME ----------------
        public IIncome AddIncome(IncomeType type, string name, double amount)
        {
            var income = _transactionManager.CreateIncome(type, name, amount);
            //CreateNotification(
            //    $"Income Added: {name}",
            //    income.Id,
            //    NotificationType.Credited,
            //    $"New income '{name}' of Rs.{amount} added."
            //);
            return income;
        }

        public void DeleteIncome(string name)
        {
            _transactionManager.DeleteIncome(name);
        }
        public void UpdateIncome(string id, IncomeType type, string name, double amount, bool freeze, bool giveReminder, int tDay)
        {
            _transactionManager.UpdateIncome(id, type, name, amount, freeze, giveReminder, tDay);
            //CreateNotification("Income Updated", id, NotificationType.Other, $"Income '{name}' updated.");

        }
        public List<IIncome> GetAllIncomes() => _transactionManager.GetAllIncomes();

        // ---------------- OUTCOME ----------------
        public IOutcome AddOutcome(string name, double amount, OutcomeType outcomeType, int dayOfTransaction)
        {
            var outcome = _transactionManager.CreateOutcome(name, amount, outcomeType, dayOfTransaction);
         //   CreateNotification("Outcome Added", outcome.Id, NotificationType.Debited, $"New outcome '{name}' of Rs.{amount} created.");
            return outcome;
        }

        public void DeleteOutcome(string name)
        {
            _transactionManager.DeleteOutcome(name);
        }
        public void UpdateOutcome(string id, string name, double amount, OutcomeType outcomeType, int dayOfTransaction, bool freeze, bool giveReminder,DateTime? lastPaidDate)
        {
            _transactionManager.UpdateOutcome(id, name, amount, outcomeType, dayOfTransaction, freeze, giveReminder, lastPaidDate);
          //  CreateNotification("Outcome Updated", id, NotificationType.Other, $"Outcome '{name}' updated.");

        }
        public List<IOutcome> GetAllOutcomes()
        {
            return _transactionManager.GetAllOutcomes();
        }
        public bool CheckPaidOutcome(IOutcome outcome, out string errorMessage)
        {
            errorMessage = "";
            if (outcome.Amount > _user.Balance)
            {
                errorMessage = "Insufficient balance to pay this outcome.";
                return false;
            }
            switch (outcome.OutComeType)
            {
                case Model.StaticData.OutcomeType.Daily:
                    if (outcome.LastPaidDate?.Day == DateTime.Today.Day)
                    {
                        errorMessage = "Amount is already paid for the day";
                        return false;
                    }
                    return true;
                case Model.StaticData.OutcomeType.Monthly:
                    if (outcome.LastPaidDate?.Month == DateTime.Today.Month)
                    {
                        errorMessage = "Amount is already paid for this month";
                        return false;
                    }
                    return true;
                case Model.StaticData.OutcomeType.Yearly:
                    if (outcome.LastPaidDate?.Year == DateTime.Today.Year)
                    {
                        errorMessage = "Amount is already paid for this year";
                        return false;
                    }
                    return true;
            }
            return true;
        }
        public bool CreateOutcomeAsExpense(string outcomeId)
        {           
            return _transactionManager.CreateOutcomeAsExpense(outcomeId);
        }

        // ---------------- EXPENSE ----------------
        public IExpense AddExpense(string name, double amount, DateTime date, string description, bool freeze, string category = "General")
        {
            var expense = _expenseManager.CreateExpense(name, amount, date, description, freeze, category);
           // CreateNotification("Expense Added", expense.ExpenseId, NotificationType.Debited, $"Expense '{name}' of Rs.{amount} added in category '{category}'.");
            return expense;
        }

        public void UpdateExpense(string id, string name, double amount, DateTime date, string description, bool freeze, string category = "General")
        {
            _expenseManager.UpdateExpense(id, name, amount, date, description, freeze, category);
           // CreateNotification("Expense Updated", id, NotificationType.Other, $"Expense '{name}' updated.");
        }

        public void DeleteExpense(string id)
        {
            _expenseManager.DeleteExpense(id);

        }
        public List<IExpense> GetAllExpenses()
        {
            return _user.UserExpenses;
        }
        // ---------------- Savings ----------------
        public void AddToSavings(double amount,DateTime date, string category = "General")
        {
            _savingsManager.AddToSavings(amount, date, category);
        }
        public List<ISaving> GetAllSavings()
        {
            return _savingsManager.Get();
        }
        public void WithdrawFromSavings(double amount)
        {
            _savingsManager.WithdrawFromSavings(amount);
        }
        public double GetSavingsBalance()
        {
            return _savingsManager.GetSavingsBalance();
        }

        public List<INotification> GetSavingsReminders(int daysBefore = 1)
        {
            return _savingsManager.GetReminders(daysBefore);
        }

        // ---------------- Financial Goals ----------------
        public IFinancialGoal CreateGoal(string name, double targetAmount, int durationInYears, double monthlyInterestRate = 0)
        {
            return _goalsManager.CreateGoal(name, targetAmount, durationInYears, monthlyInterestRate);
        }
        public void UpdateGoal(string id,string name, double targetAmount, int durationInYears, double monthlyInterestRate = 0)
        {
            _goalsManager.UpdateGoal(id,name, targetAmount, durationInYears, monthlyInterestRate);
        }
        public void DeleteGoal(string goalId)
        {
            _goalsManager.DeleteGoal(goalId);
        }
        public IFinancialGoal GetGoal(string goalId)
        {
            return _goalsManager.GetGoal(goalId);
        }
        public List<IFinancialGoal> GetAllGoals()
        {
            return _goalsManager.GetAllGoals();
        }

        public void AllocateMonthlyContributionToSavings()
        {
            _goalsManager.AllocateMonthlyContributionToSavings();

        }
        public void StartGoal(string id)
        {
            _goalsManager.StartGoal(id);
        }
        public void Stop(string id)
        {
           _goalsManager.Stop(id);
        }
        public void AddAmountToGoal(string id)
        {
            var messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
            if(_goalsManager.AddAmount(id))
            {
                messageBox.Show("Amount added to goal successfully.", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Information), "Amount Added");
                return;
            }
            else
            {
                messageBox.Show("Failed to add amount to goal. Please check notifications for details.", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Warning), "Add Amount Failed");
                return;
            }
        }
        public double CalculateMonthlyContribution(double targetAmount, int durationInYears, double monthlyInterestRate = 0)
        {
            double monthlyContribution = 0;
            var durationInMonths = durationInYears * 12;
            if (monthlyInterestRate <= 0)
            {
                monthlyContribution = targetAmount / durationInMonths;
            }
            else
            {
                double r = monthlyInterestRate; // monthly rate in decimal
                int n = durationInMonths;
                monthlyContribution = (targetAmount * r) / (Math.Pow(1 + r, n) - 1);
            }
            return Math.Ceiling(monthlyContribution);
        }
        // ---------------- REMINDERS ----------------
        public List<INotification> GetReminders(int daysBefore = 3)
        {
            return _transactionManager.GetUpcomingReminders(daysBefore);
        }

        // ---------------- PROCESS DUE OUTCOMES ----------------
        //public void ProcessDueOutcomes()
        //{
        //    _transactionManager.CreateDueOutcomesAsExpense();
        //}

        // ---------------- BALANCE ----------------
        public double GetBalance()
        {
            return _user.Balance;
        }
        internal void Save(User user)
        {
            var services = ServiceProvider.Instance;
            var serializableBase = services.Resolve<SerializableBase>();
            var users = serializableBase.Get();
            if (users.Any(o => o.UserId == user.UserId))
            {
                users.RemoveAll(u => u.UserId == user.UserId);
            }
            users.Add(user);
            serializableBase.Set(users);
        }       
        private void ClearReadNotifications()
        {
            var nm = ServiceProvider.Instance.Resolve<NotificationManager>();
            var readNf = nm.GetAllNotifications()?.Where(o => o.IsRead)?.ToList();
            if (readNf == null || readNf.Count == 0) return;
            foreach (var item in readNf)
            {
                _user.AddToHistory(item.Message);
                nm.DeleteNotification(item.Id);
            }
        }
        public void Dispose()
        {
            ClearReadNotifications();
        }
    }

}
