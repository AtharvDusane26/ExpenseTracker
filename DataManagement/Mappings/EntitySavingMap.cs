using ExpenseTracker.DataManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public class EntitySavingMap : EntityBaseMap<EntitySaving>
    {
        public EntitySavingMap()
        {
            Table("Savings");

            Map(x => x.Amount);
            Map(x => x.Date);
            Map(x => x.Category).Length(50);
        }
    }

}
