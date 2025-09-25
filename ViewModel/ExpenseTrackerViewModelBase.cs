using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    //Create UI for User class
    //bind properties of ExpenseTrackerViewModelBase class to UI elements
    //you have to create UI in MainWindow.xaml file
    //Create commands for buttons in UI


    public class ExpenseTrackerViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Action OnTabChanged;
        public ExpenseTrackerViewModelBase()
        {
            OnTabChanged += () => Refresh();
        }
        protected virtual void Refresh()
        {
            OnPropertyChanged("");
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
