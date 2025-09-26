using ExpenseTracker.Model;
using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.OutcomeSources;
using System.Runtime.Serialization;
using static ExpenseTracker.DataManagement.Entities.EntityDailyIncome;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public class EntityUser : EntityBase
    {
        public EntityUser(string primaryKey, string foreignKey = null) : base(primaryKey, foreignKey) { }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public double Balance { get; set; }

        [DataMember]
        public List<EntityTransaction> Transactions { get; set; }
        [DataMember]
        public List<EntityExpense> UserExpenses { get; set; }
        [DataMember]
        public List<EntityFinancialGoal> Goals { get; set; }
        [DataMember]
        public List<EntitySaving> Savings { get; set; }
        [DataMember]
        public List<EntityNotification> Notifications { get; set; }
        [DataMember]
        public List<EntityTransactionHistory> TransactionHistory { get; set; }

        public User Get()
        {
            var user = new User(Id);
            user.Name = this.Name;
            user.PhoneNumber = this.PhoneNumber;
            user.Age = this.Age;
            user.Balance = this.Balance;
            if (Transactions != null)
            {
                foreach (var entityTransaction in Transactions)
                {
                    user.Transactions.Add((entityTransaction.Get()));
                }
            }
            if (UserExpenses != null)
            {
                foreach (var entityExpense in UserExpenses)
                {
                    user.UserExpenses.Add(entityExpense.Get());
                }
            }
            if (Goals != null)
            {
                foreach (var entityGoal in Goals)
                {
                    user.Goals.Add(entityGoal.Get());
                }
            }
            if (Savings != null)
            {
                foreach (var entitySaving in Savings)
                {
                    user.Savings.Add(entitySaving.Get());
                }
            }
            if (Notifications != null)
            {
                foreach (var entityNotification in Notifications)
                {
                    user.Notifications.Add(entityNotification.Get());
                }
            }
            if (TransactionHistory != null)
            {
                foreach (var history in TransactionHistory)
                {
                    user.TransactionHistory.Add(history.Get());
                }
            }
            return user;
        }

        public void Set(User value)
        {
            this.Name = value.Name;
            this.PhoneNumber = value.PhoneNumber;
            this.Age = value.Age;
            this.Balance = value.Balance;
            if (value.Transactions != null)
            {
                this.Transactions = new List<EntityTransaction>();
                foreach (var transaction in value.Transactions)
                {
                    switch (transaction)
                    {
                        case DailyIncome dailyIncome:
                            var entityDailyIncome = new EntityDailyIncome(dailyIncome.Id, this.Id);
                            entityDailyIncome.Set(dailyIncome);
                            this.Transactions.Add(entityDailyIncome);
                            break;
                        case MonthlyIncome monthlyIncome:
                            var entityMonthlyIncome = new EntityMonthlyIncome(monthlyIncome.Id, this.Id);
                            entityMonthlyIncome.Set(monthlyIncome);
                            this.Transactions.Add(entityMonthlyIncome);
                            break;
                        case YearlyIncome yearlyIncome:
                            var entityYearlyIncome = new EntityYearlyIncome(yearlyIncome.Id, this.Id);
                            entityYearlyIncome.Set(yearlyIncome);
                            this.Transactions.Add(entityYearlyIncome);
                            break;
                        case Outcome outcome:
                            var entityOutcome = new EntityOutcome(outcome.Id, this.Id);
                            entityOutcome.Set(outcome);
                            this.Transactions.Add(entityOutcome);
                            break;
                    }
                }
            }
            if (value.UserExpenses != null)
            {
                this.UserExpenses = new List<EntityExpense>();
                foreach (var expense in value.UserExpenses)
                {
                    var entityExpense = new EntityExpense(expense.ExpenseId, this.Id);
                    entityExpense.Set(expense);
                    this.UserExpenses.Add(entityExpense);
                }
            }
            if (value.Goals != null)
            {
                this.Goals = new List<EntityFinancialGoal>();
                foreach (var goal in value.Goals)
                {
                    var entityGoal = new EntityFinancialGoal(goal.GoalId, this.Id);
                    entityGoal.Set(goal);
                    this.Goals.Add(entityGoal);
                }
            }
            if (value.Savings != null)
            {
                this.Savings = new List<EntitySaving>();
                foreach (var saving in value.Savings)
                {
                    var entitySaving = new EntitySaving(saving.SavingId, this.Id);
                    entitySaving.Set(saving);
                    this.Savings.Add(entitySaving);
                }
            }
            if (value.Notifications != null)
            {
                this.Notifications = new List<EntityNotification>();
                foreach (var notification in value.Notifications)
                {
                    var entityNotification = new EntityNotification(notification.Id, this.Id);
                    entityNotification.Set(notification);
                    this.Notifications.Add(entityNotification);
                }
            }
            if (value.TransactionHistory != null)
            {
                this.TransactionHistory = new List<EntityTransactionHistory>();
                foreach (var history in value.TransactionHistory)
                {
                    var entityHistory = new EntityTransactionHistory(history.Id, this.Id);
                    entityHistory.Set(history);
                    this.TransactionHistory.Add(entityHistory);
                }
            }
        }
    }
}
