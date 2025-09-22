using ExpenseTracker.Model.StaticData;
using ExpenseTracker.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public abstract class EntityTransaction : EntityBase
    {
        public EntityTransaction(string primaryKey, string foreignKey = null) : base(primaryKey, foreignKey) { }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public bool Freeze { get; set; }
        [DataMember]
        public int DayOfTransaction { get; set; }
        [DataMember]
        public bool GiveReminder { get; set; }
        public abstract ITransaction Get();
        public abstract void Set(ITransaction value);      
    }
}
