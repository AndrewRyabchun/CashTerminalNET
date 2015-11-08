using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CashTerminal.Commons;
using CashTerminal.Models;
using CashTerminal.Data;
using SettingsProvider = System.Configuration.SettingsProvider;

namespace CashTerminal.ViewModels
{
    internal class MainViewModel : ViewModelBase, IOverlayable
    {
        public string UserName => "Пользователь: " + Environment.UserName;
        public string Uptime => "Время сеанса: " + Timer.SessionTime.ToString(@"hh\:mm\:ss");

        public ObservableCollection<ViewModelBase> OverlayedControl { get; }

        #region Commands

        public ICommand LogoffCommand { get; set; }
        public ICommand LockCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand LoggerCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        #endregion

        public Visibility OverlayVisibility
        {
            get { return OverlayedControl?.Count != 0 ? Visibility.Visible : Visibility.Collapsed; }
            set { OnPropertyChanged(); }
        }

        public SessionTimer Timer { get; }
        public SettingsManager Settings { get; }
        public string LogText { get; }

        public string TotalValue => $"{0m:F} грн.";

        public MainViewModel()
        {
            Timer = new SessionTimer();
            Timer.PropertyChanged += (sender, args) => { OnPropertyChanged("Uptime"); };

            Settings = new SettingsManager();

            //init commands
            LogoffCommand = new RelayCommand(Logoff);
            LockCommand = new RelayCommand(Lock);
            SettingsCommand = new RelayCommand(ShowSettings);
            LoggerCommand = new RelayCommand(ShowLog);
            SearchCommand = new RelayCommand(ShowSearch);

            //show login overlay
            OverlayedControl = new ObservableCollection<ViewModelBase> {new LoginControlViewModel(this)};
        }

        #region CommandHandlers

        public void Logoff(object obj)
        {
            OverlayedControl.Clear();
            OverlayedControl.Add(new LoginControlViewModel(this));
            OnPropertyChanged("OverlayVisibility");
        }

        public void Lock(object obj)
        {
            OverlayedControl.Clear();
            OverlayedControl.Add(new UnlockControlViewModel(this));
            OnPropertyChanged("OverlayVisibility");
        }

        public void ShowSettings(object obj)
        {
            OverlayedControl.Clear();
            OverlayedControl.Add(new SettingsControlViewModel(this));
            OnPropertyChanged("OverlayVisibility");
        }

        public void ShowLog(object obj)
        {
            OverlayedControl.Clear();
            OverlayedControl.Add(new LoggerControlViewModel(this));
            OnPropertyChanged("OverlayVisibility");
        }

        public void ShowSearch(object obj)
        {
            OverlayedControl.Clear();
            OverlayedControl.Add(new SearchControlViewModel(this));
            OnPropertyChanged("OverlayVisibility");
        }

        #endregion

        public void CloseOverlay()
        {
            OverlayedControl.Clear();
            OnPropertyChanged("OverlayVisibility");
        }
    }
}