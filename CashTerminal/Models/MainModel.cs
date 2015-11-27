using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;
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
        public HistoryManager History { get; set; }
        public List<ArticleRecord> Items => DataBase.Items;

        /// <summary>
        /// Инициализирует экземпляр класса MainModel.
        /// </summary>
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

        /// <summary>
        /// Позволяет сменить метод вывода информации
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

            if (art != null)
                History.Log($"Добавлен товар: {art.Name}");
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