using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.IO.Ports;
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
        public ObservableCollection<ArticleRecord> ArticleRecords => new ObservableCollection<ArticleRecord>(Model.Items);
        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("LastArticleName");
                OnPropertyChanged("LastArticlePriceData");
            }
        }

        public string ArticleID { get; set; }
        public string TotalValue
        {
            get
            {
                var sum = (from item in Model.Items select item.FullPrice).Sum();
                return string.Format("{0:F} грн.", sum);
            }
        }

        public string LastArticleName
        {
            get
            {
                string shortenName = Model.Items.Count != 0 ? Model.Items[SelectedIndex].Name.Substring(0,
                    Math.Min(Model.Items[SelectedIndex].Name.Length, 90)) : String.Empty;

                return shortenName;
            }
        }

        public string LastArticlePriceData
        {
            get
            {
                if (Model.Items.Count == 0)
                    return String.Empty;
                var item = Model.Items[SelectedIndex];
                return $"{item.Price} x {item.Count} = {item.FullPrice}";
            }
        }

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

        #endregion

        public Visibility OverlayVisibility
        {
            get { return OverlayedControl?.Count != 0 ? Visibility.Visible : Visibility.Collapsed; }
        }

        public SessionTimer Timer { get; }
        public SettingsManager Settings { get; }
        public MainModel Model { get; }



        public MainViewModel()
        {
            Timer = new SessionTimer();
            Timer.PropertyChanged += (sender, args) => { OnPropertyChanged("Uptime"); };

            Settings = new SettingsManager();

            Model = new MainModel();
            Model.PropertyChanged += (sender, args) =>
            {
                UpdateUI();
            };

            Model.SetPrinter(new RawPrinter(80, Settings.TerminalNumber));

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

            //show login overlay
            OverlayedControl = new ObservableCollection<ViewModelBase> { new LoginControlViewModel(this) };
        }

        #region OverlayCommandHandlers

        public void Logoff(object obj)
        {
            Model.Items.Clear();

            OverlayedControl.Clear();
            OverlayedControl.Add(new LoginControlViewModel(this));
            OnPropertyChanged("OverlayVisibility");
            UpdateUI();
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
                Model.DataBase.AddArticle(article);
                UpdateUI();
            }
            else
            {
                MessageBox.Show("Артикула не существует");
            }

        }

        private void Checkout(object obj)
        {
            Model.Printer.Send(Model.Items, new SerialPort(Settings.PrinterPort));
            Model.DataBase.Items.Clear();
            Model.History.History.Clear();

            UpdateUI();
        }

        private void ChangeCount(object obj)
        {
            var index = SelectedIndex;
            if (index < 0 || Model.Items.Count==0) return;

            var temp = Model.Items[index];
            Model.Items.RemoveAt(index);
            temp.Count++;
            Model.Items.Insert(index,temp);
            UpdateUI();
        }

        private void Delete(object obj)
        {
            if (SelectedIndex>=0 && Model.Items.Count != 0)
                Model.Items.RemoveAt(SelectedIndex);
            UpdateUI();
        }
        #endregion

        public void CloseOverlay()
        {
            OverlayedControl.Clear();
            OnPropertyChanged("OverlayVisibility");
            OnPropertyChanged("UserName");

        }

        public void UpdateUI()
        {
            OnPropertyChanged("ArticleRecords");
            OnPropertyChanged("TotalValue");
            OnPropertyChanged("LastArticleName");
            OnPropertyChanged("LastArticlePriceData");
        }
    }
}