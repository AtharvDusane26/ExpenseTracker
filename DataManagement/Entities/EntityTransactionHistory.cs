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
        public EntityTransactionHistory(string primaryKey, string foreignKey = null)
            : base(primaryKey, foreignKey) { }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public double Balance { get; set; }

        [DataMember]
        public double SavingBalance { get; set; }

        [DataMember]
        public double TotalExpenseAmount { get; set; }

        [DataMember]
        public string HistoryData { get; set; }  // StringBuilder cannot be serialized directly

        // Convert Entity to ITransactionHistory
        public ITransactionHistory Get()
        {
            var transactionHistory = new TransactionHistory(Id);
            transactionHistory.Update(Balance, SavingBalance, TotalExpenseAmount, this.Date);
            transactionHistory.AddHistoryEntry(this.HistoryData ?? string.Empty);
            return transactionHistory;
        }

        // Set Entity properties from ITransactionHistory
        public void Set(ITransactionHistory value)
        {
            if (value != null)
            {
                this.Date = value.Date;
                this.Balance = value.Balance;
                this.SavingBalance = value.SavingBalance;
                this.TotalExpenseAmount = value.TotalExpenseAmount;
                this.HistoryData = value.History?.ToString();
            }
        }
    }

}
