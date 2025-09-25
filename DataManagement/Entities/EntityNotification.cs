using ExpenseTracker.Model;
using ExpenseTracker.Model.Notifications;
using System;
using System.Runtime.Serialization;

namespace ExpenseTracker.DataManagement.Entities
{
    [DataContract]
    public class EntityNotification : EntityBase
    {
        public EntityNotification(string primaryKey, string foreignKey = null)
            : base(primaryKey, foreignKey) { }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ReferenceObjectId { get; set; } // you can store object ID or type info

        [DataMember]
        public NotificationType Type { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool IsRead { get; set; }

        [DataMember]
        public string AccentColor { get; set; }

        public INotification Get()
        {
            var notification = new Notification(Id,Name, ReferenceObjectId, Type, Message,Date);
            if (IsRead) notification.MarkAsRead();
            return notification;
        }

        public void Set(INotification value)
        {
            if (value == null) return;

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
