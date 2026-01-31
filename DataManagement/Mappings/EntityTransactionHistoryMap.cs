using ExpenseTracker.DataManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public class EntityTransactionHistoryMap : EntityBaseMap<EntityTransactionHistory>
    {
        public EntityTransactionHistoryMap()
        {
            Table("TransactionHistory");

            Map(x => x.Date);
            Map(x => x.Balance);
            Map(x => x.SavingBalance);
            Map(x => x.TotalExpenseAmount);

            Map(x => x.HistoryData)
                .CustomSqlType("TEXT")
                .Nullable();
        }
    }


}
