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
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual double TargetAmount { get; set; }
        [DataMember]
        public virtual int DurationInMonths { get; set; }
        [DataMember]
        public virtual double MonthlyContribution { get; set; }
        [DataMember]
        public virtual DateTime StartDate { get; set; }
        [DataMember]
        public virtual double MonthlyInterestRate { get; set; }
        [DataMember]
        public virtual bool Running { get; set; }

        [DataMember]
        public virtual double CollectedAmount { get; set; }

        [DataMember]
        public virtual DateTime EndDate { get; set; }

        [DataMember]
        public virtual DateTime DateOfLastContribution { get; set; }

        public virtual IFinancialGoal Get()
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

        public virtual void Set(IFinancialGoal value, string parentId = "")
        {
            var goal = value as FinancialGoal;
            if (goal != null)
            {
                this.Id = goal.GoalId;
                if (!String.IsNullOrWhiteSpace(parentId))
                    this.ParentId = parentId;
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
