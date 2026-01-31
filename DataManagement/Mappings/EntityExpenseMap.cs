using ExpenseTracker.DataManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public class EntityExpenseMap : EntityBaseMap<EntityExpense>
    {
        public EntityExpenseMap()
        {
            Table("Expenses");

            Map(x => x.Name).Length(100);
            Map(x => x.Amount);
            Map(x => x.DateOfExpense);
            Map(x => x.Description).Length(500);
            Map(x => x.Category).Length(50);
            Map(x => x.Freeze);
        }
    }

}
