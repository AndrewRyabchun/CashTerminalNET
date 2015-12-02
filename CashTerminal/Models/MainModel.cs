using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;
using CashTerminal.Commons;
using CashTerminal.Data;

namespace CashTerminal.Models
{
    /// <summary>
    /// Основная логика приложения.
    /// </summary>
    internal class MainModel : INotifyPropertyChanged
    {
        private SerialPortProvider _port;
        public IPrintable Printer { get; private set; }
        public Authorization Validator { get; set; }
        public DataBaseProvider DataBase { get; }
        public List<ArticleRecord> Items => DataBase.Items;

        /// <summary>
        /// Имя порта для сканера.
        /// </summary>
        public string ScannerPort
        {
            get { return _port?.PortName; }
            set
            {
                if (_port != null)
                    _port.PortName = value;
            }
        }

        /// <summary>
        /// Инициализирует экземпляр класса MainModel.
        /// </summary>
        public MainModel()
        {
            DataBase = new DataBaseProvider();


            for (int i = 0; i < SerialPortProvider.AllPortsName.Count; i++)
            {
                try
                {
                    _port = new SerialPortProvider(SerialPortProvider.AllPortsName[i], DataReceived);
                    break;
                }
                catch
                {
                    //ignore
                }
            }

            if (_port == null)
            {
                UIMediator.Instance.Update("Нет доступных портов.");
            }
            else
            {
                UIMediator.Instance.Update($"Порт для сканера: {_port.PortName}.");
            }

        }

        /// <summary>
        /// Позволяет сменить метод вывода информации.
        /// </summary>
        /// <param name="printer"></param>
        public void SetPrinter(IPrintable printer)
        {
            Printer = printer;
        }

        /// <summary>
        /// Обработчик события, указывающего на получение информации по COM порту.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Обьект, содержащий данные события.</param>
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            long id = -1;

            bool isValid = long.TryParse(((SerialPort)sender).ReadExisting(), out id);

            if (!isValid) return;

            Article art = DataBase.GetArticle(id);

            Application.Current.Dispatcher.Invoke(() => { DataBase.AddArticle(art); });

            OnPropertyChanged("Items");

        }

        /// <summary>
        /// Представляет метод, который обрабатывает событие PropertyChanged, возникающее при изменении свойства компонента.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged с предоставленными аргументами.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}