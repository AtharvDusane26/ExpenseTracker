using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.SavingsAndFinancialGoals
{
    public class Saving : ISaving
    {
        private string _savingId;
        private double _amount;
        private DateTime _date;
        private string _category;

        public Saving(string id ,double amount, string category = "General")
        {
            _savingId = id;
            _amount = amount;
            _category = category;
        }

        public string SavingId
        {
            get
            {
                return _savingId;
            }
        }
        public double Amount
        {
            get
            {
                return _amount;
            }
        }
        public DateTime Date
        {
            get
            {
                return _date;
            }
        }
        public string Category
        {
            get
            {
                return _category;
            }
        }
        public void UpdateAmount(double newAmount)
        {
            _amount = newAmount;
        }
        public void UpdateDate(DateTime date)
        {
            _date = date;   
        }
    }
}
