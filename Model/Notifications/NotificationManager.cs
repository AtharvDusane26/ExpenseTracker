using ExpenseTracker.DataManagement.Serialization;
using ExpenseTracker.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Notifications
{
    public class NotificationManager
    {
        private INotificationProvider _provider;

        public NotificationManager()
        {
        }
        internal void Initialize(INotificationProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
        /// <summary>
        /// Get all notifications.
        /// </summary>
        public List<INotification> GetAllNotifications()
        {
            return _provider.Notifications.OrderByDescending(n => n.Date).ToList();
        }

        /// <summary>
        /// Add a new notification.
        /// </summary>
        public void AddNotification(string name, string referenceObject, NotificationType type, string message, DateTime date)
        {
            var id = Guid.NewGuid().ToString();
            var notification = new Notification(id, name, referenceObject, type, message, date);
            _provider.AddNotification(notification);
            Save();
        }

        /// <summary>
        /// Delete a notification by ID or Name.
        /// </summary>
        public void DeleteNotification(string id)
        {
            _provider.DeleteNotification(id);
            Save();
        }

        /// <summary>
        /// Mark a notification as read.
        /// </summary>
        public void MarkAsRead(string id)
        {
            var notification = _provider.Notifications.FirstOrDefault(n => n.Id == id);
            notification?.MarkAsRead();
            Save();
        }
        public void MarkAsUnRead(string id)
        {
            var notification = _provider.Notifications.FirstOrDefault(n => n.Id == id);
            notification?.MarkAsUnRead();
            Save();
        }
        /// <summary>
        /// Get unread notifications.
        /// </summary>
        public List<INotification> GetUnreadNotifications()
        {
            return _provider.Notifications
                .Where(n => !n.IsRead)
                .OrderByDescending(n => n.Date)
                .ToList();
        }

        /// <summary>
        /// Clear all notifications.
        /// </summary>
        public void ClearAll()
        {
            var all = _provider.Notifications.ToList();
            foreach (var n in all)
            {
                _provider.DeleteNotification(n.Name);
            }
            Save();
        }

        /// <summary>
        /// Get notifications filtered by period: daily or monthly.
        /// </summary>
        public List<INotification> GetNotificationsByPeriod(DateTime date, string period = "daily")
        {
            return period.ToLower() switch
            {
                "daily" => _provider.Notifications
                    .Where(n => n.Date.Date == date.Date)
                    .OrderByDescending(n => n.Date)
                    .ToList(),

                "monthly" => _provider.Notifications
                    .Where(n => n.Date.Year == date.Year && n.Date.Month == date.Month)
                    .OrderByDescending(n => n.Date)
                    .ToList(),

                _ => new List<INotification>()
            };
        }

        /// <summary>
        /// Get notifications filtered by type/scenario.
        /// </summary>
        public List<INotification> GetNotificationsByType(NotificationType type)
        {
            return _provider.Notifications
                .Where(n => n.Type == type)
                .OrderByDescending(n => n.Date)
                .ToList();
        }
        /// <summary>
        /// Get notifications by period and type together.
        /// </summary>
        public List<INotification> GetNotifications(DateTime date, string period, NotificationType type)
        {
            return GetNotificationsByPeriod(date, period)
                .Where(n => n.Type == type)
                .ToList();
        }
        private void Save()
        {
            var userManager = ServiceProvider.Instance.Resolve<UserManager>();
            userManager.Save(_provider as User);
        }
    }
}
