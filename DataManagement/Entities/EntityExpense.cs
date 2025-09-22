using ExpenseTracker.Model.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    public class EntityExpense : EntityBase
    {
        public EntityExpense(string primaryKey, string foreignKey = null) : base(primaryKey, foreignKey) { }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime DateOfExpense { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
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
