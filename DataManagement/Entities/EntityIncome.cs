using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public abstract class EntityIncome : EntityTransaction
    {
        [DataMember]
        public virtual DateTime? DateOfCredited { get; set; }
        [DataMember]
        public virtual string SourceOfIncome { get; set; }
       // public override abstract ITransaction Get();
      //  public override  void Set(ITransaction value, string parentId = "");

    }
    [DataContract]
    public class EntityDailyIncome: EntityIncome
    {
        public virtual ITransaction Get()
        {
            var income = new DailyIncome(Name, Amount, (Model.StaticData.SourceOfIncome)Enum.Parse(typeof(Model.StaticData.SourceOfIncome), SourceOfIncome));
            income.Create(Id);
            income.UpdateTransactionDay(this.DayOfTransaction);
            income.GiveReminder = this.GiveReminder;
            income.FreezeTransaction(this.Freeze);
            income.DateOfCredited = DateOfCredited;
            return income;
        }
        public virtual void Set(ITransaction value, string parentId = "")
        {
            var income = value as DailyIncome;         
            if (income != null)
            {
                this.Id = value.Id;
                if (!String.IsNullOrWhiteSpace(parentId))
                    this.ParentId = parentId;
                Name = income.Name;
                Amount = income.Amount;
                DayOfTransaction = income.DayOfTransaction;
                GiveReminder = income.GiveReminder;
                Freeze = income.Freeze;
                SourceOfIncome = income.SourceOfIncome.ToString();
                DateOfCredited = income.DateOfCredited;
            }
        }
       
    }
    [DataContract]
    public class EntityMonthlyIncome : EntityIncome
    {
        public virtual ITransaction Get()
        {
            var income = new MonthlyIncome(Name, Amount, (Model.StaticData.SourceOfIncome)Enum.Parse(typeof(Model.StaticData.SourceOfIncome), SourceOfIncome));
            income.Create(Id);
            income.UpdateTransactionDay(this.DayOfTransaction);
            income.GiveReminder = this.GiveReminder;
            income.FreezeTransaction(this.Freeze);
            income.DateOfCredited = DateOfCredited;
            return income;
        }
        public virtual void Set(ITransaction value, string parentId = "")
        {
            var income = value as MonthlyIncome;           
            if (income != null)
            {
                Id = value.Id;
                if (!String.IsNullOrWhiteSpace(parentId))
                    this.ParentId = parentId;
                Name = income.Name;
                Amount = income.Amount;
                DayOfTransaction = income.DayOfTransaction;
                GiveReminder = income.GiveReminder;
                Freeze = income.Freeze;
                SourceOfIncome = income.SourceOfIncome.ToString();
                DateOfCredited = income.DateOfCredited;
            }
        }
    }
    [DataContract]
    public class EntityYearlyIncome : EntityIncome
    {
        public virtual ITransaction Get()
        {
            var income = new YearlyIncome(Name, Amount, (Model.StaticData.SourceOfIncome)Enum.Parse(typeof(Model.StaticData.SourceOfIncome), SourceOfIncome));
            income.Create(Id);
            income.UpdateTransactionDay(this.DayOfTransaction);
            income.GiveReminder = this.GiveReminder;
            income.FreezeTransaction(this.Freeze);
            income.DateOfCredited = DateOfCredited;
            return income;
        }
        public virtual void Set(ITransaction value, string parentId = "")
        {
            var income = value as YearlyIncome;
          
            if (income != null)
            {
                Id = value.Id;
                if (!String.IsNullOrWhiteSpace(parentId))
                    this.ParentId = parentId;
                Name = income.Name;
                Amount = income.Amount;
                DayOfTransaction = income.DayOfTransaction;
                GiveReminder = income.GiveReminder;
                Freeze = income.Freeze;
                SourceOfIncome = income.SourceOfIncome.ToString();
                DateOfCredited = income.DateOfCredited;
            }
        }
    }

}
