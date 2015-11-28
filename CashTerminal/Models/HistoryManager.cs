using System;
using System.Collections.Generic;

namespace CashTerminal.Models
{
    /// <summary>
    /// Ведет историю сеанса программы.
    /// </summary>
    internal class HistoryManager
    {
        /// <summary>
        /// Содержит набор записей, созданных во время сеанса программы.
        /// </summary>
        public List<string> History { get; }

        private static HistoryManager _instance;
        public static HistoryManager Instance
        {
            get
            {
                if (_instance==null)
                    _instance = new HistoryManager();
                return _instance;
            }
        }

        /// <summary>
        /// Инициализирует экземпляр класса HistoryManager.
        /// </summary>
        public HistoryManager()
        {
            History = new List<string>();
            Log("Создан файл истории");
        }

        /// <summary>
        /// Записывает переданное сообщение в историю.
        /// </summary>
        /// <param name="message">Сообщение для записи</param>
        public void Log(string message)
        {
            string date = DateTime.Now.ToShortDateString();
            string time = DateTime.Now.ToShortTimeString();

            History.Add($"{date}|{time}|{message}");
        }
    }
}