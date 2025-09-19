using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    public class FinancialGoal : IFinancialGoal
    {
        private string _goalId;
        private string _name;          
        private double _targetAmount;   
        private int _durationInMonths;  
        private double _monthlyContribution;
        private DateTime _startDate;

        public FinancialGoal(string name, double targetAmount, int durationInYears)
        {
            _goalId = Guid.NewGuid().ToString();
            _name = name;
            _targetAmount = targetAmount;
            _durationInMonths = durationInYears * 12;
            _startDate = DateTime.Now;
            CalculateMonthlyContribution();
        }

        public string GoalId
        {
            get
            {
                return _goalId;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public double TargetAmount
        {
            get
            {
                return _targetAmount;
            }
        }
        public int DurationInMonths
        {
            get
            {
                return _durationInMonths;
            }
        }
        public double MonthlyContribution
        {
            get
            {
                return _monthlyContribution;
            }
        }
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
        }

        private void CalculateMonthlyContribution(double monthlyInterestRate = 0)
        {
            if (monthlyInterestRate <= 0)
            {
                _monthlyContribution = _targetAmount / _durationInMonths;
            }
            else
            {
                double r = monthlyInterestRate; // monthly rate in decimal
                int n = _durationInMonths;
                _monthlyContribution = (_targetAmount * r) / (Math.Pow(1 + r, n) - 1);
            }
        }
    }

}
