using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CashTerminal.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public string UserName {
            get { return "Пользователь: " + Environment.UserName; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
