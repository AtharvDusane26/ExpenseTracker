using DBConfig;
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
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string PhoneNumber { get; set; }
        [DataMember]
        public virtual int Age { get; set; }
        [DataMember]
        public virtual double Balance { get; set; }

        [DataMember]
        public virtual IList<EntityTransaction> Transactions { get; set; }
        [DataMember]
        public virtual IList<EntityExpense> UserExpenses { get; set; }
        [DataMember]
        public virtual IList<EntityFinancialGoal> Goals { get; set; }
        [DataMember]
        public virtual IList<EntitySaving> Savings { get; set; }
        [DataMember]
        public virtual IList<EntityNotification> Notifications { get; set; }
        [DataMember]
        public virtual IList<EntityTransactionHistory> TransactionHistory { get; set; }

        public virtual User Get()
        {
            var user = new User(Id);
            user.Name = this.Name;
            user.PhoneNumber = this.PhoneNumber;
            user.Age = this.Age;
            user.Balance = this.Balance;
            var reader = DbEntity.Instance;
            var Transactions = reader.GetFiltered<EntityTransaction>(o => o.ParentId == Id);
            if (Transactions != null)
            {
                foreach (var entityTransaction in Transactions)
                {
                    switch(entityTransaction)
                    {
                        case EntityDailyIncome:
                            user.Transactions.Add((entityTransaction as EntityDailyIncome).Get());
                            break;
                        case EntityYearlyIncome:
                            user.Transactions.Add((entityTransaction as EntityYearlyIncome).Get());
                            break;
                        case EntityMonthlyIncome:
                            user.Transactions.Add((entityTransaction as EntityMonthlyIncome).Get());
                            break;
                        case EntityOutcome:
                            user.Transactions.Add((entityTransaction as EntityOutcome).Get());
                            break;
                    }
                }
            }
            var UserExpenses = reader.GetFiltered<EntityExpense>(o => o.ParentId == Id);    
            if (UserExpenses != null)
            {
                foreach (var entityExpense in UserExpenses)
                {
                    user.UserExpenses.Add(entityExpense.Get());
                }
            }
            var Goals = reader.GetFiltered<EntityFinancialGoal>(o => o.ParentId == Id);
            if (Goals != null)
            {
                foreach (var entityGoal in Goals)
                {
                    user.Goals.Add(entityGoal.Get());
                }
            }
            var Savings = reader.GetFiltered<EntitySaving>(o => o.ParentId == Id);
            if (Savings != null)
            {
                foreach (var entitySaving in Savings)
                {
                    user.Savings.Add(entitySaving.Get());
                }
            }
            var Notifications = reader.GetFiltered<EntityNotification>(o => o.ParentId == Id);
            if (Notifications != null)
            {
                foreach (var entityNotification in Notifications)
                {
                    user.Notifications.Add(entityNotification.Get());
                }
            }
            var TransactionHistory = reader.GetFiltered<EntityTransactionHistory>(o => o.ParentId == Id);
            if (TransactionHistory != null)
            {
                foreach (var history in TransactionHistory)
                {
                    user.TransactionHistory.Add(history.Get());
                }
            }
            return user;
        }

        public virtual void Set(User value, string parentId = "")
        {
            if (!String.IsNullOrWhiteSpace(parentId))
                this.ParentId = parentId;
            this.Id = value.UserId;
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
                            var entityDailyIncome = new EntityDailyIncome();
                            entityDailyIncome.Set(dailyIncome,this.Id);
                            this.Transactions.Add(entityDailyIncome);
                            break;
                        case MonthlyIncome monthlyIncome:
                            var entityMonthlyIncome = new EntityMonthlyIncome();
                            entityMonthlyIncome.Set(monthlyIncome,this.Id);
                            this.Transactions.Add(entityMonthlyIncome);
                            break;
                        case YearlyIncome yearlyIncome:
                            var entityYearlyIncome = new EntityYearlyIncome();
                            entityYearlyIncome.Set(yearlyIncome,this.Id);
                            this.Transactions.Add(entityYearlyIncome);
                            break;
                        case Outcome outcome:
                            var entityOutcome = new EntityOutcome();
                            entityOutcome.Set(outcome, this.Id);
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
                    var entityExpense = new EntityExpense();
                    entityExpense.Set(expense,this.Id);
                    this.UserExpenses.Add(entityExpense);
                }
            }
            if (value.Goals != null)
            {
                this.Goals = new List<EntityFinancialGoal>();
                foreach (var goal in value.Goals)
                {
                    var entityGoal = new EntityFinancialGoal();
                    entityGoal.Set(goal,this.Id);
                    this.Goals.Add(entityGoal);
                }
            }
            if (value.Savings != null)
            {
                this.Savings = new List<EntitySaving>();
                foreach (var saving in value.Savings)
                {
                    var entitySaving = new EntitySaving();
                    entitySaving.Set(saving,this.Id);
                    this.Savings.Add(entitySaving);
                }
            }
            if (value.Notifications != null)
            {
                this.Notifications = new List<EntityNotification>();
                foreach (var notification in value.Notifications)
                {
                    var entityNotification = new EntityNotification();
                    entityNotification.Set(notification,this.Id);
                    this.Notifications.Add(entityNotification);
                }
            }
            if (value.TransactionHistory != null)
            {
                this.TransactionHistory = new List<EntityTransactionHistory>();
                foreach (var history in value.TransactionHistory)
                {
                    var entityHistory = new EntityTransactionHistory();
                    entityHistory.Set(history,this.Id);
                    this.TransactionHistory.Add(entityHistory);
                }
            }
        }
    }
}
