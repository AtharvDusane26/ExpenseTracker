using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.ViewModel
{
    //Create UI for User class
    //bind properties of ExpenseTrackerViewModel class to UI elements
    //you have to create UI in MainWindow.xaml file
    //Create commands for buttons in UI


    public class ExpenseTrackerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
