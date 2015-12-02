using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CashTerminal.Commons
{
    /// <summary>
    /// Реализация паттерна "Посредник" с помощью паттерна "Одиночка".
    /// Позволяет отправлять интерфейсу уведомление о изменении статуса приложения.
    /// </summary>
    class UIMediator
    {
        public Action<string> Action { get; set; }

        private static UIMediator _instance;
        public static UIMediator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UIMediator();
                return _instance;
            }
        }

        private UIMediator()
        {
        }

        public void Update(string status)
        {
            Action?.Invoke(status);
        }
    }
}
