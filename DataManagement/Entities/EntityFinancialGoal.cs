using ExpenseTracker.Model.SavingsAndFinancialGoals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public class EntityFinancialGoal: EntityBase
    {
        public EntityFinancialGoal(string primaryKey, string foreignKey = null) : base(primaryKey, foreignKey) { }
       [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double TargetAmount { get; set; }
        [DataMember]
        public int DurationInMonths { get; set; }
        [DataMember]
        public double MonthlyContribution { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }

        public IFinancialGoal Get()
        {
            var goal = new FinancialGoal(Id,Name, TargetAmount, DurationInMonths);
            goal.UpdateDate(StartDate);
            return goal;
        }

        public void Set(IFinancialGoal value)
        {
            var goal = value as FinancialGoal;
            if (goal != null)
            {
                this.Name = goal.Name;
                this.TargetAmount = goal.TargetAmount;
                this.DurationInMonths = goal.DurationInMonths;
                this.MonthlyContribution = goal.MonthlyContribution;
                this.StartDate = goal.StartDate;
            }
        }
    }
}
