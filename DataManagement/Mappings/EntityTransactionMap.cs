using ExpenseTracker.DataManagement.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public abstract class EntityTransactionMapBase<T> : SubclassMap<T>
      where T : EntityTransaction
    {
        protected void MapTransactionFields()
        {
            Map(x => x.Name).Length(100).Not.Nullable();
            Map(x => x.Amount);
            Map(x => x.Freeze);
            Map(x => x.DayOfTransaction);
            Map(x => x.GiveReminder);
        }
    }
    public class EntityTransactionRootMap : ClassMap<EntityTransaction>
    {
        public EntityTransactionRootMap()
        {
            Table("Transactions");

            Id(x => x.Id)
                .GeneratedBy.Assigned()
                .Length(50);

            Map(x => x.ParentId).Length(50);

            DiscriminateSubClassesOnColumn("TransactionType");

            Map(x => x.Name).Length(100).Not.Nullable();
            Map(x => x.Amount);
            Map(x => x.Freeze);
            Map(x => x.DayOfTransaction);
            Map(x => x.GiveReminder);
        }
    }


}
