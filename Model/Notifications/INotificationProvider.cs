using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Notifications
{
    public interface INotificationProvider
    {
        List<INotification> Notifications { get; }
        void AddNotification(INotification notification);
        void DeleteNotification(string id);

    }
}
