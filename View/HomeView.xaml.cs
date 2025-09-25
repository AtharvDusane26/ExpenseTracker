using ExpenseTracker.Model.Notifications;
using ExpenseTracker.Model.ViewModels;
using ExpenseTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpenseTracker.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private HomeViewModel _viewModel;
        public HomeView()
        {
            InitializeComponent();
            DataContext = _viewModel = new HomeViewModel();
            Loaded += (o, e) => _viewModel.OnTabChanged?.Invoke();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(sender is CheckBox cb && cb.IsChecked.HasValue)
            {
               // _viewModel.HandleNotificationRead(cb.IsChecked.Value);
            }
        }
    }
    public class NotificationIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NotificationType type)
            {
                return type switch
                {
                    NotificationType.Income => "💰",
                    NotificationType.Outcome => "💸",
                    NotificationType.Debited => "📤",
                    NotificationType.Credited => "📥",
                    NotificationType.GoalAchieved => "🏆",
                    NotificationType.LowBalance => "⚠️",
                    NotificationType.HighExpense => "🔥",
                    NotificationType.Reminder => "⏰",
                    NotificationType.Warning => "❗",
                    NotificationType.Other => "🔔",
                    _ => "🔔"
                };
            }
            return "🔔";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ReadToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isRead = (bool)value;
            return isRead ? new SolidColorBrush(Colors.LightGray) : new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    public class StringToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString && !string.IsNullOrWhiteSpace(colorString))
            {
                try
                {
                    var brush = (SolidColorBrush)new BrushConverter().ConvertFromString(colorString);
                    return brush ?? Brushes.Gray; // fallback if parsing fails
                }
                catch
                {
                    return Brushes.Gray; // fallback on invalid color string
                }
            }
            return Brushes.Gray; // default
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
