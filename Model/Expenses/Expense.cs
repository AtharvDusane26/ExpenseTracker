using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Expenses
{
    public class Expense : IExpense
    {
        private string _expenseId;
        private string _name;
        private double _amount;
        private DateTime _date;
        private string _description;
        private string _category;
        private bool _freeze = false;
        public Expense(string id)
        {
            _expenseId =id;
        }
        public string ExpenseId
        {
            get
            {
                return _expenseId;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            internal set
            {
                _name = value;
            }
        }

        public double Amount
        {
            get
            {
                return _amount;
            }
            internal set
            {
                _amount = value;
            }
        }

        public DateTime DateOfExpense
        {
            get
            {
                return _date;
            }
            internal set
            {
                _date = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            internal set
            {
                _description = value;
            }
        }
        public string Category 
        { 
            get
            {
                return _category;
            }
            internal set 
            {
                if (!_freeze)
                    _category = value;
            } 
        }
        public bool Freeze
        {
            get
            {
                return _freeze;
            }
        }
        public void FreezeTransaction(bool status)
        {
            _freeze = status;
        }
    }
}
