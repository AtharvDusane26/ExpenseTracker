using System;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    public class Saving : ISaving
    {
        private readonly string _savingId;
        private double _amount;
        private DateTime _date;
        private string _category;

        public Saving(string id, double amount, DateTime date, string category = "General")
        {
            _savingId = id;
            _amount = amount;
            _date = date;
            _category = category;
        }

        public string SavingId => _savingId;

        public double Amount => _amount;

        public DateTime Date => _date;

        public string Category => _category;


        public void UpdateAmount(double newAmount)
        {
            _amount = newAmount;
        }

        public void UpdateDate(DateTime date)
        {
            _date = date;
        }

        public void UpdateCategory(string category)
        {
            _category = category;
        }
    }
}
