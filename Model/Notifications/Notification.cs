using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Notifications
{
    public class Notification : INotification
    {
        private string _id;
        private string _name;
        private string _referenceObject;
        private NotificationType _type;
        private DateTime _date;
        private string _message;
        private bool _isRead;
        public Notification(string id, string name, string referenceObjectId, NotificationType type, string message, DateTime date)
        {
            _id = id;
            _name = name;
            _referenceObject = referenceObjectId;
            _type = type;
            _message = message;
            _date = date;
            _isRead = false;
        }

        // Properties
        public string Id => _id;
        public string Name => _name;
        public string ReferenceObjectId => _referenceObject;
        public NotificationType Type => _type;
        public DateTime Date => _date;
        public string Message => _message;
        public bool IsRead => _isRead;

        // Accent color based on type
        public string AccentColor => GetAccentColor(_type);

        // Mark notification as read
        public void MarkAsRead()
        {
            _isRead = true;
        }
        public void MarkAsUnRead()
        {
            _isRead = false;
        }

        // Internal helper for accent color
        private string GetAccentColor(NotificationType type)
        {
            return type switch
            {
                NotificationType.Debited => "#FF4C4C",       // red
                NotificationType.Credited => "#4CAF50",      // green
                NotificationType.GoalAchieved => "#FFD700",  // gold/yellow
                NotificationType.LowBalance => "#FF9800",    // orange
                NotificationType.HighExpense => "#F44336",   // dark red
                NotificationType.Reminder => "#2196F3",      // blue
                NotificationType.Warning => "#FF5722",       // deep orange
                NotificationType.Other => "#9E9E9E",         // gray
                NotificationType.Income => "#4CAF50",        // green (same as Credited)
                NotificationType.Outcome => "#FF4C4C",       // red (same as Debited)
                _ => "#9E9E9E",                              // fallback gray
            };
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
