using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public class EntitySaving: EntityBase
    {
        [DataMember]
        public virtual double Amount { get; set; }
        [DataMember]
        public virtual DateTime Date { get; set; }
        [DataMember]
        public virtual string Category { get; set; }
        public virtual ISaving Get()
        {
            var saving = new Saving(Id, Amount, Date,Category);
            saving.UpdateDate(Date);
            return saving;
        }

        public virtual void Set(ISaving value, string parentId = "")
        {
            var saving = value as Saving;
            if (saving != null)
            {
                this.Id = saving.SavingId;
                if (!String.IsNullOrWhiteSpace(parentId))
                    this.ParentId = parentId;
                Amount = saving.Amount;
                Date = saving.Date;
                Category = saving.Category;
            }
        }
    }
}
