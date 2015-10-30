using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CashTerminal.Commons;
using CashTerminal.Models;

namespace CashTerminal.ViewModels
{
    internal class MainViewModel : ViewModelBase, IOverlayable
    {
        public string UserName => "Пользователь: " + Environment.UserName;

        public string Uptime => "Время сеанса: " + _timer.SessionTime.ToString(@"hh\:mm\:ss");

        private ObservableCollection<ViewModelBase> _overlayedControl=new ObservableCollection<ViewModelBase>();
        public ObservableCollection<ViewModelBase> OverlayedControl
        {
            get
            {
                return _overlayedControl;
            }
            set
            {
                _overlayedControl = value;
                OnPropertyChanged("OverlayVisibility");
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand UnlockCommand { get; set; }



        public Visibility OverlayVisibility
        {
            get
            {
                return OverlayedControl?.Count!=0 ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {
                OnPropertyChanged();
            }
        }


        private SessionTimer _timer;
        public SessionTimer Timer => _timer;

        public MainViewModel()
        {
            _timer=new SessionTimer();
            _timer.PropertyChanged += (sender, args) => {OnPropertyChanged("Uptime"); };
            LoginCommand=new RelayCommand(Login);
            UnlockCommand=new RelayCommand(Unlock);
        }

        public void Login(object obj)
        {
            if (OverlayedControl.Count<1)
                OverlayedControl = new ObservableCollection<ViewModelBase> {new LoginControlViewModel(this)};
        }

        public void Unlock(object obj)
        {
            if (OverlayedControl.Count < 1)
                OverlayedControl = new ObservableCollection<ViewModelBase> { new UnlockControlViewModel(this)};
        }

        public void CloseOverlay()
        {
            OverlayedControl=new ObservableCollection<ViewModelBase>();
        }
    }
}
