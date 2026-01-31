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
        public virtual string OutComeType { get; set; }
        [DataMember]
        public virtual DateTime? LastPaidDate { get; set; }

        public virtual ITransaction Get()
        {
            var outcome = new Outcome(Name, Amount, (Model.StaticData.OutcomeType)Enum.Parse(typeof(Model.StaticData.OutcomeType), OutComeType));
            outcome.Create(Id);
            outcome.UpdateTransactionDay(this.DayOfTransaction);
            outcome.GiveReminder = this.GiveReminder;
            outcome.FreezeTransaction(this.Freeze);
            outcome.LastPaidDate = LastPaidDate;
            return outcome;
        }
        public virtual void Set(ITransaction value, string parentId = "")
        {
            var outcome = value as Outcome;
            if (outcome != null)
            {
                Id = outcome.Id;
                if (!String.IsNullOrWhiteSpace(parentId))
                    this.ParentId = parentId;
                Name = outcome.Name;
                Amount = outcome.Amount;
                DayOfTransaction = outcome.DayOfTransaction;
                GiveReminder = outcome.GiveReminder;
                Freeze = outcome.Freeze;
                LastPaidDate = outcome.LastPaidDate;
                OutComeType = outcome.OutComeType.ToString();
            }
        }
    }
}
