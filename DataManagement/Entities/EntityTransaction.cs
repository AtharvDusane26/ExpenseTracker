using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    public abstract class EntityTransaction : EntityBase
    {
        public EntityTransaction(string primaryKey, string foreignKey = null) : base(primaryKey, foreignKey) { }
        public string Name { get; set; }
        public double Amount { get; set; }
        public bool Freeze { get; set; }
        public int DayOfTransaction { get; set; }
        public bool GiveReminder { get; set; }
        public abstract ITransaction Get();
        public abstract void Set(ITransaction value);      
    }
}
