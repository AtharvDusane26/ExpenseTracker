using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model.Services
{
    public enum MessageBoxButtons
    {
        YesNo, YesNoCancle, OK, OKCancle
    }
    public enum MessageBoxImage
    {
        Information, Error, Question, None, Exclamation, Asterisk, Warning, Stop, Hand
    }
    public enum MessageBoxResult
    {
        Yes, No, Cancel, OK, None
    }
    public class MessageBoxArgs
    {
        private MessageBoxButtons _button;
        private MessageBoxImage _icon;
        public MessageBoxArgs(MessageBoxButtons buttons, MessageBoxImage icon)
        {
            _button = buttons;
            _icon = icon;
        }
        public MessageBoxButtons MessageBoxButton
        {
            get => _button;
        }
        public MessageBoxImage MessageBoxIcon
        {
            get => _icon;
        }
    }
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string message, MessageBoxArgs args, string messageBoxTitle = "");
        MessageBoxResult Show(string message, string messageBoxTitle = "");
    }
}
