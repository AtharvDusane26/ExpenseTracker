using DBConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public abstract class EntityBase : BaseIdentifier
    {
        [DataMember]
        public virtual string Id { get;  set; }
        [DataMember]
        public virtual string ParentId { get; set; }
        //public EntityBase(string primaryKey, string foreignKey = null) 
        //{
        //    Id = primaryKey;
        //    ParentId = foreignKey;
        //}

        //for db 
        public EntityBase()
        {
            
        }


    }
}
