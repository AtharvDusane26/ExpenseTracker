using ExpenseTracker.Model.SavingsAndFinancialGoals;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExpenseTracker.DataManagement.Entities
{
    public class EntitySaving: EntityBase
    {
        public EntitySaving(string primaryKey, string foreignKey = null) : base(primaryKey, foreignKey) { }
       
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public ISaving Get()
        {
            var saving = new Saving(Id, Amount,Category);
            saving.UpdateDate(Date);
            return saving;
        }

        public void Set(ISaving value)
        {
            var saving = value as Saving;
            if (saving != null)
            {
                Amount = saving.Amount;
                Date = saving.Date;
                Category = saving.Category;
            }
        }
    }
}
