using ExpenseTracker.DataManagement.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public class EntityOutcomeMap : EntityTransactionMapBase<EntityOutcome>
    {
        public EntityOutcomeMap()
        {
            DiscriminatorValue("Outcome");

            Map(x => x.OutComeType).Length(50);
            Map(x => x.LastPaidDate).Nullable();
        }
    }


}
