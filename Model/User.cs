using System;
using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Transactions;
using System.Xml.Linq;

namespace ExpenseTracker.Model
{
    public partial class User : IUser, ITransactionProvider, ISavingsProvider, IFinancialGoalsProvider, IExpenseProvider
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
        public User(string userId)
        {
            _userId = userId;
            _transactions = new List<ITransaction>();
            _userExpenses = new List<IExpense>();
            _savings = new List<ISaving>();
            _goals = new List<IFinancialGoal>();
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
            Balance += income.Amount; // immediately increase balance
        }

        public void DeleteIncome(string incomeName)
        {
            var income = GetIncome(incomeName);
            if (income != null)
            {
                _transactions.Remove(income);
                Balance -= income.Amount; // rollback balance
            }
        }

        public void UpdateIncome(IIncome updatedIncome)
        {
            if (updatedIncome == null) return;

            var existing = Incomes.FirstOrDefault(i => (i as ITransaction).Id == (updatedIncome as ITransaction).Id);
            if (existing != null)
            {
                _transactions.Remove(existing);
                Balance -= existing.Amount; // rollback old amount
                _transactions.Add(updatedIncome);
                Balance += updatedIncome.Amount; // apply new amount
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
        // ------------------- ISavingProvider -------------------
        public ISaving AddToSavings(double amount, string category = "General")
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                var id = Guid.NewGuid().ToString();
                var saving = new Saving(id,amount, category);
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

        public List<string> GetSavingsReminders(int daysBefore = 1)
        {
            var reminders = new List<string>();
            if (SavingsBalance < 1000) // example threshold
                reminders.Add($"Your total savings balance is low: {SavingsBalance}");
            return reminders;
        }
        public IFinancialGoal CreateGoal(string name, double targetAmount, int durationInYears, double monthlyInterestRate = 0)
        {
            var id = Guid.NewGuid().ToString();
            var goal = new FinancialGoal(id,name, targetAmount, durationInYears);
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
