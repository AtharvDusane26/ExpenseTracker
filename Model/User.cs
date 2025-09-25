using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Transactions;
using System.Xml.Linq;
using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Notifications;

namespace ExpenseTracker.Model
{
    public partial class User : IUser, ITransactionProvider, ISavingsProvider, IFinancialGoalsProvider, IExpenseProvider, IIncomeProvider, IOutcomeProvider, INotificationProvider
    {
        private string _userId;
        private string _name;
        private string _phoneNumber;
        private int _age;
        private double _balance;
        private List<ITransaction> _transactions;
        private List<IExpense> _userExpenses;
        private List<ISaving> _savings;
        private List<IFinancialGoal> _goals;
        private readonly List<INotification> _notifications;
        public User(string userId)
        {
            _userId = userId;
            _transactions = new List<ITransaction>();
            _userExpenses = new List<IExpense>();
            _savings = new List<ISaving>();
            _goals = new List<IFinancialGoal>();
            _notifications = new List<INotification>();
        }

        public string UserId
        {
            get
            {
                return _userId;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            internal set
            {
                _name = value;
            }
        }
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            internal set
            {
                _phoneNumber = value;
            }
        }
        public int Age
        {
            get
            {
                return _age;
            }
            internal set
            {
                if (value < 0)
                    throw new ArgumentException("Age cannot be negative.");
                _age = value;
            }
        }
        public double Balance
        {
            get
            {
                return _balance;
            }
            internal set
            {
                if (value < 0)
                    throw new ArgumentException("Balance cannot be negative.");
                _balance = value;
            }
        }

        public List<ITransaction> Transactions
        {
            get
            {
                return _transactions;
            }
        }
        public List<IExpense> UserExpenses
        {
            get
            {
                return _userExpenses;
            }
        }
        public double SavingsBalance
        {
            get
            {
                return _savings.Sum(s => s.Amount);
            }
        }
        public List<IIncome> Incomes
        {
            get
            {
                return _transactions.OfType<IIncome>().ToList();
            }
        }
        public List<IOutcome> Outcomes
        {
            get
            {
                return _transactions.OfType<IOutcome>().ToList();
            }
        }
        public List<IFinancialGoal> Goals
        {
            get
            {
                return _goals;
            }
        }
        public List<ISaving> Savings
        {
            get
            {
                return _savings;
            }
        }
        public List<INotification> Notifications
        {
            get
            {
                return _notifications;
            }
        }

        public void Create(string name, string phoneNumber, int age, double initialBalance)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Age = age;
            Balance = initialBalance;
        }
        // ------------------- ITransactionProvider -------------------
        // ------------------- IIncomeProvider -------------------

        public IIncome GetIncome(string incomeName)
        {
            return Incomes.FirstOrDefault(i => i.Name.Equals(incomeName, StringComparison.OrdinalIgnoreCase));
        }

        public void AddIncome(IIncome income)
        {
            if (income == null) return;
            _transactions.Add(income);
            //  Balance += income.Amount; // immediately increase balance
        }

        public void DeleteIncome(string incomeName)
        {
            var income = GetIncome(incomeName);
            if (income != null)
            {
                _transactions.Remove(income);
            }
        }
        public void UpdateBalance(double amount)
        {
            Balance += amount;
        }
        public void UpdateIncome(IIncome income)
        {
            var existing = Incomes.FirstOrDefault(i => (i as ITransaction).Id == (income as Income).Id);
            if (existing != null)
            {
                _transactions.Remove(existing);
                income.DateOfCredited = existing.DateOfCredited;
                _transactions.Add(income);
            }
        }
        // ------------------- ITransactionProvider -------------------
        // ------------------- IOutcomeProvider -------------------

        public IOutcome GetOutcome(string outcomeName)
        {
            return Outcomes.FirstOrDefault(o => o.Name.Equals(outcomeName, StringComparison.OrdinalIgnoreCase));
        }

        public void AddOutcome(IOutcome outcome)
        {
            if (outcome == null) return;
            _transactions.Add(outcome);
        }

        public void DeleteOutcome(string outcomeName)
        {
            var outcome = GetOutcome(outcomeName);
            if (outcome != null)
            {
                _transactions.Remove(outcome);
            }
        }

        public void UpdateOutcome(IOutcome updatedOutcome)
        {
            if (updatedOutcome == null) return;

            var existing = Outcomes.FirstOrDefault(o => (o as ITransaction).Id == (updatedOutcome as ITransaction).Id);
            if (existing != null)
            {
                _transactions.Remove(existing);
                _transactions.Add(updatedOutcome);
            }
        }
        // ------------------- INotificationProvider -------------------

        // Add a new notification
        public void AddNotification(INotification notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            _notifications.Add(notification);
        }
        // Delete a notification by reference object ID or Name
        public void DeleteNotification(string id)
        {
            if (string.IsNullOrEmpty(id)) return;

            var toRemove = _notifications.FirstOrDefault(n =>
            {
                return n.Name == id || (n.ReferenceObjectId?.ToString() == id);
            });

            if (toRemove != null)
                _notifications.Remove(toRemove);
        }
        // ------------------- ISavingProvider -------------------
        public ISaving AddToSavings(double amount, string category = "General")
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                var id = Guid.NewGuid().ToString();
                var saving = new Saving(id, amount, category);
                saving.UpdateDate(DateTime.Now);
                _savings.Add(saving);
                return saving;
            }
            return null;
        }

        public void WithdrawFromSavings(double amount, string category = null)
        {
            double remaining = amount;

            var applicableSavings = string.IsNullOrEmpty(category)
                ? _savings
                : _savings.Where(s => s.Category == category).ToList();

            foreach (var s in applicableSavings)
            {
                if (remaining <= 0) break;

                if (s.Amount <= remaining)
                {
                    remaining -= s.Amount;
                    _savings.Remove(s);
                }
                else
                {
                    s.UpdateAmount(s.Amount - remaining);
                    remaining = 0;
                }
            }

            if (remaining < amount) Balance += (amount - remaining);
        }

        public List<ISaving> GetAllSavings()
        {
            return _savings;
        }

        public List<INotification> GetSavingsReminders(int daysBefore = 1)
        {
            var notifications = new List<INotification>();
            var notificationProvider = this as INotificationProvider;
            if (notificationProvider == null)
                return notifications;

            // Example threshold check
            if (SavingsBalance < 1000)
            {
                string referenceId = "SavingsBalance";
                string message = $"Your total savings balance is low: Rs.{SavingsBalance}";

                // Check if a similar notification already exists
                var existingNotification = notificationProvider.Notifications
                    .FirstOrDefault(n => n.ReferenceObjectId.ToString() == referenceId && n.Type == NotificationType.Warning);

                if (existingNotification == null)
                {
                    var notification = new Notification(
                        Guid.NewGuid().ToString(),
                        "Low Savings Balance",
                        referenceId,
                        NotificationType.Warning,
                        message,
                        DateTime.Now
                    );
                    notificationProvider.AddNotification(notification);
                    existingNotification = notification;
                }

                notifications.Add(existingNotification);
            }

            return notifications.OrderBy(n => n.Date).ToList();
        }

        public IFinancialGoal CreateGoal(string name, double targetAmount, int durationInYears, double monthlyInterestRate = 0)
        {
            var id = Guid.NewGuid().ToString();
            var goal = new FinancialGoal(id, name, targetAmount, durationInYears);
            goal.UpdateDate(DateTime.Now);
            _goals.Add(goal);
            return goal;
        }

        public void DeleteGoal(string goalId)
        {
            var goal = _goals.FirstOrDefault(g => g.GoalId == goalId);
            if (goal != null)
                _goals.Remove(goal);
        }

        public IFinancialGoal GetGoal(string goalId)
        {
            return _goals.FirstOrDefault(g => g.GoalId == goalId);
        }
    }
}
