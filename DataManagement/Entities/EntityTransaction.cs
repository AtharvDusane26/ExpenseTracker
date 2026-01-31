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
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual double Amount { get; set; }
        [DataMember]
        public virtual bool Freeze { get; set; }
        [DataMember]
        public virtual int DayOfTransaction { get; set; }
        [DataMember]
        public virtual bool GiveReminder { get; set; }
     //   public  abstract ITransaction Get();
    //    public abstract void Set(ITransaction value, string parentId = "");      
    }
}
