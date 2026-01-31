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
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual double Amount { get; set; }
        [DataMember]
        public virtual DateTime DateOfExpense { get; set; }
        [DataMember]
        public virtual string Description { get; set; }
        [DataMember]
        public virtual string Category { get; set; }
        [DataMember]
        public virtual bool Freeze { get; set; }

        public virtual IExpense Get()
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
        public virtual void Set(IExpense value, string parentId = "")
        {
            var expense = value as IExpense;
            if (expense != null)
            {
                this.Id = value.ExpenseId;
                if (!String.IsNullOrWhiteSpace(parentId))
                    this.ParentId = parentId;
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
