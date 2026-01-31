using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    using ExpenseTracker.Model.Transactions;
    using System;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract]
    public class EntityTransactionHistory : EntityBase
    {
        [DataMember]
        public virtual DateTime Date { get; set; }

        [DataMember]
        public virtual double Balance { get; set; }

        [DataMember]
        public virtual double SavingBalance { get; set; }

        [DataMember]
        public virtual double TotalExpenseAmount { get; set; }

        [DataMember]
        public virtual string HistoryData { get; set; }  // StringBuilder cannot be serialized directly

        // Convert Entity to ITransactionHistory
        public virtual ITransactionHistory Get()
        {
            var transactionHistory = new TransactionHistory(Id);
            transactionHistory.Update(Balance, SavingBalance, TotalExpenseAmount, this.Date);
            transactionHistory.AddHistoryEntry(this.HistoryData ?? string.Empty);
            return transactionHistory;
        }

        // Set Entity properties from ITransactionHistory
        public virtual void Set(ITransactionHistory value, string parentId = "")
        {
            if (value != null)
            {
                this.Id = value.Id;
                if (!String.IsNullOrWhiteSpace(parentId))
                    this.ParentId = parentId;
                this.Balance = value.Balance;
                this.SavingBalance = value.SavingBalance;
                this.TotalExpenseAmount = value.TotalExpenseAmount;
                this.HistoryData = value.History?.ToString();
            }
        }
    }

}
