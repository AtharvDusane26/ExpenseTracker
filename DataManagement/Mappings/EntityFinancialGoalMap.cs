using ExpenseTracker.DataManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public class EntityFinancialGoalMap : EntityBaseMap<EntityFinancialGoal>
    {
        public EntityFinancialGoalMap()
        {
            Table("FinancialGoals");

            Map(x => x.Name).Length(100);
            Map(x => x.TargetAmount);
            Map(x => x.DurationInMonths);
            Map(x => x.MonthlyContribution);
            Map(x => x.StartDate);
            Map(x => x.Running);
            Map(x => x.CollectedAmount);
            Map(x => x.EndDate);
            Map(x => x.DateOfLastContribution);
            Map(x => x.MonthlyInterestRate);
        }
    }

}
