using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public abstract class EntityBase
    {
        [DataMember]
        public string Id { get; private set; }
        [DataMember]
        public string ParentId { get; private set; }
        public EntityBase(string primaryKey, string foreignKey = null) 
        {
            Id = primaryKey;
            ParentId = foreignKey;
        }
     
    }
}
