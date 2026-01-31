using ExpenseTracker.DataManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public class EntityUserMap : EntityBaseMap<EntityUser>
    {
        public EntityUserMap()
        {
            Table("Users");

            Map(x => x.Name).Length(100).Not.Nullable();
            Map(x => x.PhoneNumber).Length(20);
            Map(x => x.Age);
            Map(x => x.Balance);

            HasMany(x => x.Transactions)
                .KeyColumn("ParentId")
                .Cascade.AllDeleteOrphan()
                .Inverse();

            HasMany(x => x.UserExpenses)
                .KeyColumn("ParentId")
                .Cascade.AllDeleteOrphan()
                .Inverse();

            HasMany(x => x.Goals)
                .KeyColumn("ParentId")
                .Cascade.AllDeleteOrphan()
                .Inverse();

            HasMany(x => x.Savings)
                .KeyColumn("ParentId")
                .Cascade.AllDeleteOrphan()
                .Inverse();

            HasMany(x => x.Notifications)
                .KeyColumn("ParentId")
                .Cascade.AllDeleteOrphan()
                .Inverse();

            HasMany(x => x.TransactionHistory)
                .KeyColumn("ParentId")
                .Cascade.AllDeleteOrphan()
                .Inverse();
        }
    }

}
