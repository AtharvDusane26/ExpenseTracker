using ExpenseTracker.DataManagement.Serialization;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.MonitoringAndReporting;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;

namespace ExpenseTracker.Model
{
    public class UserManager
    {
        private User _user;
        private TransactionManager _transactionManager;
        private ExpenseManager _expenseManager;
        private SavingsManager _savingsManager;
        private FinancialGoalsManager _goalsManager;
        private ReportManager _reportManager;
        private UserMonitor _userMonitor;
        public UserManager(string userId)
        {
            _user = GetUser(userId) ?? throw new ArgumentNullException("user not found");
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
        }
        private User GetUser(string id)
        {
            SerializableBase serializableBase = new SerializableBase();
            var users = serializableBase.Get();
            if(users != null && users.Count > 0)
            {
               var user = users.Find(u => u.UserId == id);
                if (user != null)
                {
                    return user;
                }
            }
            return null;
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
        // ---------------- INCOME ----------------
        public IIncome AddIncome(IncomeType type, string name, float amount, DateTime? date = null)
        {
            return _transactionManager.CreateIncome(type, name, amount, date);
        }

        public void DeleteIncome(string name)
        {
            _transactionManager.DeleteIncome(name);
        }
        public void UpdateIncome(IIncome income)
        {
            _transactionManager.UpdateIncome(income);
        }
        public List<IIncome> GetAllIncomes() => _transactionManager.GetAllIncomes();

        // ---------------- OUTCOME ----------------
        public IOutcome AddOutcome(string name, float amount, OutcomeType outcomeType, int dayOfTransaction)
        {
            return _transactionManager.CreateOutcome(name, amount, outcomeType, dayOfTransaction);
        }

        public void DeleteOutcome(string name)
        {
            _transactionManager.DeleteOutcome(name);
        }
        public void UpdateOutcome(IOutcome outcome)
        {
            _transactionManager.UpdateOutcome(outcome);
        }
        public List<IOutcome> GetAllOutcomes()
        {
            return _transactionManager.GetAllOutcomes();
        }

        // ---------------- EXPENSE ----------------
        public void AddExpense(string name, double amount, DateTime date, string description, string category = "General")
            => _expenseManager.CreateExpense(name, amount, date, description, category);

        public void UpdateExpense(string id, string name, double amount, DateTime date, string description, string category = "General")
        {
            _expenseManager.UpdateExpense(id, name, amount, date, description, category);
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
        public void AddToSavings(double amount, string category = "General")
        {
            _savingsManager.AddToSavings(amount, category);
        }

        public void WithdrawFromSavings(double amount)
        {
            _savingsManager.WithdrawFromSavings(amount);
        }
        public double GetSavingsBalance()
        {
            return _savingsManager.GetSavingsBalance();
        }

        public List<string> GetSavingsReminders(int daysBefore = 1)
        {
            return _savingsManager.GetReminders(daysBefore);
        }

        // ---------------- Financial Goals ----------------
        public IFinancialGoal CreateGoal(string name, double targetAmount, int durationInYears, double monthlyInterestRate = 0)
        {
            return _goalsManager.CreateGoal(name, targetAmount, durationInYears, monthlyInterestRate);
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

        // ---------------- REMINDERS ----------------
        public List<string> GetReminders(int daysBefore = 3)
        {
            return _transactionManager.GetUpcomingReminders(daysBefore);
        }

        // ---------------- PROCESS DUE OUTCOMES ----------------
        public void ProcessDueOutcomes()
        {
            _transactionManager.CreateDueOutcomesAsExpense();
        }

        // ---------------- BALANCE ----------------
        public double GetBalance()
        {
            return _user.Balance;
        }
        internal static void Save(User user)
        {
            SerializableBase serializableBase = new SerializableBase();
            var users = serializableBase.Get();
            if(users.Any(o => o.UserId == user.UserId))
            {
                users.RemoveAll(u => u.UserId == user.UserId);
            }
            users.Add(user);
            serializableBase.Set(users);
        }
    }
}
