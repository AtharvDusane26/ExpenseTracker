using ExpenseTracker.Model;
using ExpenseTracker.Model.Expenses;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class User : IUser
    {
        private string _userId;
        private string _userName;
        private string _phoneNumber;
        private int _age;
        private double _balance;
        private List<ITransaction> _transactions;
       private List<IExpense> _userExpenses ;
        public User(string userId)
        {
            _userId = userId;
            _transactions = new List<ITransaction>();
            _userExpenses = new List<IExpense>();
        }
        public string UserId
        {
            get
            {
                return _userId;
            }
            internal set
            {
                _userId = value;
            }
        }

        public string Name
        {
            get
            {
                return _userName;
            }
            internal set
            {
                _userName = value;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            internal set
            {
                _phoneNumber = value;
            }
        }

        public int Age
        {
            get
            {
                return _age;
            }
            internal set
            {
                _age = value;
            }
        }

        public double Balance
        {
            get            
            {
                return _balance;
            }           
        }

        public List<ITransaction> Transactions
        {
            get
            {
                return _transactions;
            }
        }

        public List<IExpense> UserExpenses
        {
            get
            {
                return _userExpenses;
            }
        }
        public void Create(string name, string phoneNumber, int age, double initialBalance)
        {
            _userName = name;
            _phoneNumber = phoneNumber;
            _age = age;
            _balance = initialBalance;
        }
    }
}
