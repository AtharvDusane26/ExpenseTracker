using ExpenseTracker.Model.IncomeSources;
using ExpenseTracker.Model.OutcomeSources;
using ExpenseTracker.Model.StaticData;
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
    public class EntityOutcome : EntityTransaction
    {
        [DataMember]
        public string OutComeType { get; set; }
        public EntityOutcome(string primaryKey, string foreignKey = null) : base(primaryKey, foreignKey) { }
        public override ITransaction Get()
        {
            var outcome = new Outcome(Name, Amount, (Model.StaticData.OutcomeType)Enum.Parse(typeof(Model.StaticData.OutcomeType), OutComeType));
            outcome.Create(Id);
            outcome.UpdateTransactionDay(this.DayOfTransaction);
            outcome.GiveReminder = this.GiveReminder;
            outcome.FreezeTransaction(this.Freeze);
            return outcome;
        }
        public override void Set(ITransaction value)
        {
            var income = value as Outcome;
            if (income != null)
            {
                Name = income.Name;
                Amount = income.Amount;
                DayOfTransaction = income.DayOfTransaction;
                GiveReminder = income.GiveReminder;
                Freeze = income.Freeze;
                OutComeType = income.OutComeType.ToString();
            }
        }
    }
}
