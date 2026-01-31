using DBConfig;
using ExpenseTracker.DataManagement.Entities;
using ExpenseTracker.DataManagement.Serialization;
using ExpenseTracker.Model;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.Notifications;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Services;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Database
{
    public enum CRUDOperation
    {
        Insert,
        Update,
        Delete
    }
    public class DatabaseServices
    {
        private readonly IMessageBoxService _messageBox;
        public DatabaseServices()
        {
            var directory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Expense Tracker");
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            _messageBox = ServiceProvider.Instance.Resolve<IMessageBoxService>();
        }
        public List<User> Get()
        {
            try
            {
                var users = new List<User>();
                var pocos = DbEntity.Instance.GetAll<EntityUser>();
                if (pocos != null)
                {
                    foreach (var user in pocos)
                    {
                        users.Add(user.Get());
                    }

                }
                return users;
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while receiving user data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
                return new List<User>();
            }

        }
        public void Set(List<User> value)
        {
            try
            {
                var pocos = new List<EntityUser>();
                var services = ServiceProvider.Instance;
                var dataManager = services.Resolve<DataManager>();
                foreach (var user in value)
                {
                    var entityUser = new EntityUser();
                    entityUser.Set(user);
                    var dpUser = DbEntity.Instance.GetSingle<EntityUser>(u => u.Id == entityUser.Id);
                    if (dpUser != null)
                    {
                        DbEntity.Instance.Delete(entityUser);
                    }
                    DbEntity.Instance.Insert(entityUser);
                }
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while saving users data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
        }
        public void UpdateUser(User user, CRUDOperation crud)
        {
            try
            {
                var poco = new EntityUser();
                poco.Set(user);
                if (poco != null)
                {
                    PerformCrud(poco, crud);
                }
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while saving users data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
        }
        public void UpdateTransaction(ITransaction transaction, User parent, CRUDOperation crud)
        {
            try
            {
                EntityTransaction poco = null;
                switch (transaction)
                {
                    case DailyIncome dailyIncome:
                        var entityDailyIncome = new EntityDailyIncome();
                        entityDailyIncome.Set(dailyIncome, parent.UserId);
                        poco = entityDailyIncome;
                        break;
                    case MonthlyIncome monthlyIncome:
                        var entityMonthlyIncome = new EntityMonthlyIncome();
                        entityMonthlyIncome.Set(monthlyIncome, parent.UserId);
                        poco = entityMonthlyIncome;
                        break;
                    case YearlyIncome yearlyIncome:
                        var entityYearlyIncome = new EntityYearlyIncome();
                        entityYearlyIncome.Set(yearlyIncome, parent.UserId);
                        poco = entityYearlyIncome;
                        break;
                    case Outcome outcome:
                        var entityOutcome = new EntityOutcome();
                        entityOutcome.Set(outcome, parent.UserId);
                        poco = entityOutcome;
                        break;
                }
                if (poco != null)
                {
                    PerformCrud(poco, crud);
                    UpdateUser(parent, CRUDOperation.Update);
                }
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while saving transaction data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
        }
        public void UpdateExpense(IExpense expense , User parent, CRUDOperation crud)
        {
            try
            {
                var poco = new EntityExpense();
                poco.Set(expense, parent.UserId);
                if(poco != null)
                {
                    PerformCrud(poco, crud);
                    UpdateUser(parent, CRUDOperation.Update);
                }
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while saving expense data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
        }
        public void UpdateSavings(ISaving saving,  User parent, CRUDOperation crud)
        {
            try
            {
                var poco = new EntitySaving();
                poco.Set(saving, parent.UserId);
                if (poco != null)
                {
                    PerformCrud(poco, crud);
                    UpdateUser(parent, CRUDOperation.Update);
                }
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while saving saving data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
        }
        public void UpdateFinancialGoal(IFinancialGoal financialGoal, User parent, CRUDOperation crud)
        {
            try
            {
                var poco = new EntityFinancialGoal();
                poco.Set(financialGoal, parent.UserId);
                if (poco != null)
                {
                    PerformCrud(poco, crud);
                    UpdateUser(parent, CRUDOperation.Update);
                }
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while saving financial goal data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
        }
        public void UpdateNotification(INotification notification, User parent, CRUDOperation crud)
        {
            try
            {
                var poco = new EntityNotification();
                poco.Set(notification, parent.UserId);
                if (poco != null)
                {
                    PerformCrud(poco, crud);
                    UpdateUser(parent, CRUDOperation.Update);
                }
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while saving income source data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
        }
        public void UpdateTransactionHistory(ITransactionHistory transactionHistory, User parent, CRUDOperation crud)
        {
            try
            {
                var poco = new EntityTransactionHistory();
                poco.Set(transactionHistory, parent.UserId);
                if (poco != null)
                {
                    PerformCrud(poco, crud);
                    UpdateUser(parent, CRUDOperation.Update);
                }
            }
            catch (Exception e)
            {
                _messageBox.Show("Something went wrong while saving income source data,please contact service provider", new MessageBoxArgs(MessageBoxButtons.OK, MessageBoxImage.Error), "Error");
            }
        }
        private void PerformCrud<T>(T entity, CRUDOperation crud)  where T : class
        {
            switch (crud)
            {
                case CRUDOperation.Delete:
                    DbEntity.Instance.Delete(entity);
                    return;
                case CRUDOperation.Update:
                case CRUDOperation.Insert:
                    DbEntity.Instance.Insert(entity);
                    return;
            }
        }
    }
}
