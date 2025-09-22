using ExpenseTracker.Model.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public class EntityExpense : EntityBase
    {
        public EntityExpense(string primaryKey, string foreignKey = null) : base(primaryKey, foreignKey) { }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public DateTime DateOfExpense { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public bool Freeze { get; set; }
      
        public IExpense Get()
        {
            var expense = new Expense(Id);
            expense.Name = this.Name;
            expense.Amount = this.Amount;
            expense.DateOfExpense = this.DateOfExpense;
            expense.Description = this.Description;
            expense.Category = this.Category;
            expense.FreezeTransaction(this.Freeze);
            return expense;
        }      
        public void Set(IExpense value)
        {
           var expense = value as IExpense;
            if (expense != null)
            {
                this.Name = expense.Name;
                this.Amount = expense.Amount;
                this.DateOfExpense = expense.DateOfExpense;
                this.Description = expense.Description;
                this.Category = expense.Category;
                this.Freeze = expense.Freeze;
            }
        }    
    }
}
