using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;
using CashTerminal.Data;

namespace CashTerminal.Models
{
    internal class MainModel : INotifyPropertyChanged
    {
        private SerialPortProvider _port;
        private IPrintable _printer;
        public Authorization Validator { get; set; }
        public DataBaseProvider DataBase { get; }
        public HistoryManager History { get; set; }
        public List<ArticleRecord> Items => DataBase.Items;

        public MainModel()
        {
            History = new HistoryManager();
            DataBase = new DataBaseProvider();
            try
            {
                if (SerialPortProvider.AllPortsName.Count != 0)
                _port = new SerialPortProvider(SerialPortProvider.AllPortsName[0], DataReceived);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
}

        public MainModel(IPrintable printer, string username, string password, string portName = "")
        {
            History = new HistoryManager();
            try
            {
                if (SerialPortProvider.AllPortsName.Count != 0)
                    _port = new SerialPortProvider(SerialPortProvider.AllPortsName[0], DataReceived);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


            DataBase = new DataBaseProvider();
            _printer = printer;
            Validator = new Authorization(username, password);
        }

        public void SetPrinter(IPrintable printer)
        {
            _printer = printer;
        }


        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            long id = -1;

            bool isValid = long.TryParse(((SerialPort)sender).ReadExisting(), out id);

            if (!isValid) return;

            Article art = DataBase.GetArticle(id);

            Application.Current.Dispatcher.Invoke(() => { DataBase.AddArticle(art); });

            if (art != null)
                History.Log($"Добавлен товар: {art.Name}");
            OnPropertyChanged("Items");

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}