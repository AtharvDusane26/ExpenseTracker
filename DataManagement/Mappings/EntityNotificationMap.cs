using ExpenseTracker.DataManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.DataManagement.Mappings
{
    public class EntityNotificationMap : EntityBaseMap<EntityNotification>
    {
        public EntityNotificationMap()
        {
            Table("Notifications");

            Map(x => x.Name).Length(100);
            Map(x => x.ReferenceObjectId).Length(50);
            Map(x => x.Type).CustomType<int>();
            Map(x => x.Date);
            Map(x => x.Message).Length(500);
            Map(x => x.IsRead);
            Map(x => x.AccentColor).Length(20);
        }
    }

}
