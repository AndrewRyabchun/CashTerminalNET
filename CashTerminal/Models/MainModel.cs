using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;
using CashTerminal.Data;

namespace CashTerminal.Models
{
    internal class MainModel
    {
        private SerialPortProvider _port;
        private IPrintable _printer;
        public Authorization Validator { get; set; }
        public DataBaseProvider DataBase { get; }
        public HistoryManager History { get; set; }

        public MainModel(string portName = "")
        {
            History = new HistoryManager();
            DataBase = new DataBaseProvider();

            _port = (portName == "") ?
                new SerialPortProvider(DataReceived) : new SerialPortProvider(portName, DataReceived);
        }

        public MainModel(IPrintable printer, string username, string password, string portName = "")
        {
            History = new HistoryManager();

            _port = (portName == "") ?
                new SerialPortProvider(DataReceived) : new SerialPortProvider(portName, DataReceived);

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
        }
    }

}