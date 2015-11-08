using System;
using System.Collections.Generic;

namespace CashTerminal.Models
{
    internal class HistoryManager
    {
        public List<string> History { get; private set; }

        public HistoryManager()
        {
            Log("Создан файл истории");
        }

        public void Log(string message)
        {
            string date = DateTime.Now.ToShortDateString();
            string time = DateTime.Now.ToShortTimeString();

            History.Add($"{date}|{time}|{message}");
        }
    }
}