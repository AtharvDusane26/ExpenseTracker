using ExpenseTracker.DataManagement.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public class EntityDailyIncomeMap : EntityTransactionMapBase<EntityDailyIncome>
    {
        public EntityDailyIncomeMap()
        {
            DiscriminatorValue("DailyIncome");

            Map(x => x.SourceOfIncome).Length(50);
            Map(x => x.DateOfCredited).Nullable();
        }
    }

    public class EntityMonthlyIncomeMap : EntityTransactionMapBase<EntityMonthlyIncome>
    {
        public EntityMonthlyIncomeMap()
        {
            DiscriminatorValue("MonthlyIncome");

            Map(x => x.SourceOfIncome).Length(50);
            Map(x => x.DateOfCredited).Nullable();
        }
    }

    public class EntityYearlyIncomeMap : EntityTransactionMapBase<EntityYearlyIncome>
    {
        public EntityYearlyIncomeMap()
        {
            DiscriminatorValue("YearlyIncome");

            Map(x => x.SourceOfIncome).Length(50);
            Map(x => x.DateOfCredited).Nullable();
        }
    }


}
