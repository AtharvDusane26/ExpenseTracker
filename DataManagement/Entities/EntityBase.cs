using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DataManagement.Entities
{
    public abstract class EntityBase
    {
        public string Id { get; private set; }
        public string ParentId { get; private set; }
        public EntityBase(string primaryKey, string foreignKey = null) 
        {
            Id = primaryKey;
            ParentId = foreignKey;
        }
     
    }
}
