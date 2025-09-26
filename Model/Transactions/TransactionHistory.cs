using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Transactions
{
    using System;
    using System.Text;

    public class TransactionHistory : ITransactionHistory
    {
        // Backing fields
        private string _id;
        private DateTime _date;
        private double _balance;
        private double _savingBalance;
        private double _totalExpenseAmount;
        private StringBuilder _history;
        public TransactionHistory(string id)
        {
            _id = id; 
            _history = new StringBuilder();
        }
        public string Id
        {
            get { return _id; }
        }
        // Properties implementing interface
        public DateTime Date
        {
            get { return _date; }
        }

        public double Balance
        {
            get { return _balance; }
        }

        public double SavingBalance
        {
            get { return _savingBalance; }
        }

        public double TotalExpenseAmount
        {
            get { return _totalExpenseAmount; }
        }

        public StringBuilder History
        {
            get { return _history; }
            set { _history = value ?? new StringBuilder(); }
        }
        public void Update( double balance, double savingBalance, double totalExpenseAmount,DateTime date)
        {
            _date = date;
            _balance = balance;
            _savingBalance = savingBalance;
            _totalExpenseAmount = totalExpenseAmount;
        }
        // Constructor
      

        // Optional method to add a transaction entry
        public void AddHistoryEntry(string entry)
        {
            if (!string.IsNullOrWhiteSpace(entry))
                _history.AppendLine($"{DateTime.Now}: {entry}");
        }
        public override string ToString()
        {
           StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Last updated date: {_date}");
            sb.AppendLine($"Balance: {_balance}");
            sb.AppendLine($"Saving Balance: {_savingBalance}");
            sb.AppendLine($"Total Expense Amount: {_totalExpenseAmount}");
            sb.AppendLine("Transaction History:");
            sb.AppendLine(_history.ToString());
            return sb.ToString();
        }
    }

}
