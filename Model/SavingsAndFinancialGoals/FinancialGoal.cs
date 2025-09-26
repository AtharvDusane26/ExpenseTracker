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
        private double _monthlyInterestRate;
        private bool _running;
        private double _collectedAmount; // 0% monthly interest
        private DateTime _endDate = DateTime.MaxValue;
        private DateTime _dateOfLastContribution;
        public FinancialGoal(string id, string name, double targetAmount, int durationInYears)
        {
            _goalId = id;
            _name = name;
            _targetAmount = targetAmount;
            _durationInMonths = durationInYears * 12;
            CalculateMonthlyContribution();
            _collectedAmount = 0;
            _running = false;
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
        public bool Running
        {
            get
            {
                return _running;
            }
        }
        public double MonthlyInterestRate
        {
            get
            {
                return _monthlyInterestRate;
            }
            set
            {
                _monthlyInterestRate = value;
                CalculateMonthlyContribution(_monthlyInterestRate / 100 / 12);
            }
        }
        public double CollectedAmount
        {
            get => _collectedAmount;
            internal set { _collectedAmount = value; }
        }
        public DateTime EndDate
        {
            get => _endDate;
            internal set { _endDate = value; }
        }
        public DateTime DateOfLastContribution
        {
            get => _dateOfLastContribution;
            internal set { _dateOfLastContribution = value; }
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
            _monthlyContribution = Math.Ceiling(_monthlyContribution);
        }
        public void Start(DateTime date)
        {
            _startDate = date;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public void AddAmount()
        {
            if (_running)
            {
                _collectedAmount += _monthlyContribution;
                _dateOfLastContribution = DateTime.Now;
                if (_monthlyInterestRate > 0)
                {
                    _collectedAmount *= (1 + _monthlyInterestRate / 100 / 12);
                }
                if (_collectedAmount >= _targetAmount)
                {
                    Stop();
                    _endDate = DateTime.Now;
                }
            }

        }
    }

}
