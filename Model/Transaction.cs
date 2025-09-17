using ExpenseTracker.Model.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class Transaction : ITransaction
    {
        private string _name;
        private string _id="";
        private double _amount;
        private bool _freeze = false;
        private int _dayOfTransaction = 1;
        private bool _giveReminder;
        public Transaction(string name, float amount)
        {
            _name = name;
            _amount = amount;
        }
        public string Id
        {
            get
            {
                return _id;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public double Amount
        {
            get
            {
                return _amount;
            }
        }
        public bool Freeze
        {
            get
            {
                return _freeze;
            }
        }
        public bool GiveReminder
        {
            get
            {
                return _giveReminder;
            }
            internal set
            {
                _giveReminder = value;
            }

        }
        public virtual int DayOfTransaction
        {
            get
            {
                return _dayOfTransaction;
            }
        }
        public virtual void FreezeTransaction(bool status)
        {
            _freeze = status;
        }
        public virtual void Create(string id)
        {
            _id = id;
        }
        public virtual bool UpdateAmount(float newAmount)
        {
            if (_freeze)
                return false;
            _amount = newAmount;
            return true;
        }
        public virtual void UpdateTransactionDay(int day)
        {
            _dayOfTransaction = day;
        }
        public virtual string Remind()
        {
            return $"This is a gentle reminder for your upcoming transaction ({_name}) of amount {_amount}";
        }
        public virtual string WarnForPendingTransaction()
        {
            return $"This is a gentle reminder that your transaction ({_name}) of Rs.{_amount} is pending";

        }
    }
}
