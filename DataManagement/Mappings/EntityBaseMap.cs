using ExpenseTracker.DataManagement.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public abstract class EntityBaseMap<T> : ClassMap<T> where T : EntityBase
    {
        protected EntityBaseMap()
        {
            Id(x => x.Id)
                .Column("Id")
                .Length(50)
                .GeneratedBy.Assigned();

            Map(x => x.ParentId)
                .Column("ParentId")
                .Length(50)
                .Nullable();
        }
    }

}
