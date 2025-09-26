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
    public class EntityFinancialGoal : EntityBase
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
        [DataMember]
        double MonthlyInterestRate { get; set; }
        [DataMember]
        public bool Running { get; set; }

        [DataMember]
        public double CollectedAmount { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public DateTime DateOfLastContribution { get; set; }

        public IFinancialGoal Get()
        {
            var goal = new FinancialGoal(Id, Name, TargetAmount, DurationInMonths);
            goal.MonthlyInterestRate = MonthlyInterestRate;
            if (Running)
                goal.Start(StartDate);
            else
                goal.Stop();
            EndDate = goal.EndDate;
            CollectedAmount = goal.CollectedAmount;
            DateOfLastContribution = goal.DateOfLastContribution;
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
                this.MonthlyInterestRate = goal.MonthlyInterestRate;
                this.Running = goal.Running;
                this.CollectedAmount = goal.CollectedAmount;
                this.EndDate = goal.EndDate;
                this.DateOfLastContribution = goal.DateOfLastContribution;
            }
        }
    }
}
