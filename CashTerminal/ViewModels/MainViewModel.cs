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
        public string UserName => "Пользователь: " + Model.Validator?.Username;
        public string Uptime => "Время сеанса: " + Timer.SessionTime.ToString(@"hh\:mm\:ss");
        public ObservableCollection<ArticleRecord> ArticleRecords => Model.DataBase.Items;
        public ArticleRecord SelectedRecord { get; set; }
        public MainModel Model { get; }
        public string ArticleID { get; set; }

        public ObservableCollection<ViewModelBase> OverlayedControl { get; }

        #region Commands

        public ICommand LogoffCommand { get; set; }
        public ICommand LockCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand LoggerCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public ICommand ManuallyAddCommand { get; set; }
        public ICommand CheckoutCommand { get; set; }
        public ICommand ChangeCountCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand MoveUpCommand { get; set; }
        public ICommand MoveDownCommand { get; set; }

        #endregion

        public Visibility OverlayVisibility
        {
            get { return OverlayedControl?.Count != 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        public SessionTimer Timer { get; }
        public SettingsManager Settings { get; }

        public string TotalValue => $"{0m:F} грн.";

        public MainViewModel()
        {
            Timer = new SessionTimer();
            Timer.PropertyChanged += (sender, args) => { OnPropertyChanged("Uptime"); };

            Settings = new SettingsManager();

            Model = new MainModel();

            //init commands
            LogoffCommand = new RelayCommand(Logoff);
            LockCommand = new RelayCommand(Lock);
            SettingsCommand = new RelayCommand(ShowSettings);
            LoggerCommand = new RelayCommand(ShowLog);
            SearchCommand = new RelayCommand(ShowSearch);

            ManuallyAddCommand = new RelayCommand(ManuallyAdd);

            CheckoutCommand = new RelayCommand(Checkout);
            ChangeCountCommand = new RelayCommand(ChangeCount);
            DeleteCommand = new RelayCommand(Delete);
            MoveUpCommand = new RelayCommand(MoveUp);
            MoveDownCommand = new RelayCommand(MoveDown);

            //show login overlay
            OverlayedControl = new ObservableCollection<ViewModelBase> { new LoginControlViewModel(this) };
        }

        #region OverlayCommandHandlers

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

        #region ModelCommandHandlers
        private void ManuallyAdd(object obj)
        {
            long id;
            if (!long.TryParse(ArticleID, out id))
                return;

            var article = Model.DataBase.GetArticle(id);
            if (article != null)
            {
                ArticleRecords.Add(new ArticleRecord(article));
                OnPropertyChanged("ArticleRecords");
            }
            else
            {
                MessageBox.Show("Артикула не существует");
            }

        }

        private void Checkout(object obj)
        {

        }

        private void ChangeCount(object obj)
        {
            var index = ArticleRecords.IndexOf(SelectedRecord);
            if (index < 0) return;
            ArticleRecords[index].Count++;
            OnPropertyChanged("ArticleRecords");
        }

        private void Delete(object obj)
        {
            ArticleRecords.Remove(SelectedRecord);
            OnPropertyChanged("ArticleRecords");
        }

        private void MoveUp(object obj)
        {

        }

        private void MoveDown(object obj)
        {

        }
        #endregion

        public void CloseOverlay()
        {
            OverlayedControl.Clear();
            OnPropertyChanged("OverlayVisibility");
            OnPropertyChanged("UserName");
        }
    }
}