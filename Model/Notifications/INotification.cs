using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Notifications
{
    public enum NotificationType
    {
        Income,
        Outcome,
        Debited,
        Credited,
        GoalAchieved,
        LowBalance,
        HighExpense,
        Reminder,
        Warning,
        Other
    }
    public interface INotification
    {
        string Id { get; }
        string Name { get; }
        string ReferenceObjectId { get; }
        NotificationType Type { get; }
        DateTime Date { get; }
        string Message { get; }
        bool IsRead { get; }
        string AccentColor { get; }
        void MarkAsRead();
        void MarkAsUnRead();
    }
}
