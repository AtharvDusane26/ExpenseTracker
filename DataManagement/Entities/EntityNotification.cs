using ExpenseTracker.Model;
using ExpenseTracker.Model.Notifications;
using System;
using System.Runtime.Serialization;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public class EntityNotification : EntityBase
    {
      
        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual string ReferenceObjectId { get; set; } // you can store object ID or type info

        [DataMember]
        public virtual NotificationType Type { get; set; }

        [DataMember]
        public virtual DateTime Date { get; set; }

        [DataMember]
        public virtual string Message { get; set; }

        [DataMember]
        public virtual bool IsRead { get; set; }

        [DataMember]
        public virtual string AccentColor { get; set; }

        public virtual INotification Get()
        {
            var notification = new Notification(Id,Name, ReferenceObjectId, Type, Message,Date);
            if (IsRead) notification.MarkAsRead();
            return notification;
        }

        public virtual void Set(INotification value, string parentId = "")
        {
            if (value == null) return;
            this.Id = value.Id; 
            if (!String.IsNullOrWhiteSpace(parentId))
                this.ParentId = parentId;
            Name = value.Name;
            ReferenceObjectId = value.ReferenceObjectId;
            Type = value.Type;
            Date = value.Date;
            Message = value.Message;
            IsRead = value.IsRead;
            AccentColor = value.AccentColor;
        }
    }
}
