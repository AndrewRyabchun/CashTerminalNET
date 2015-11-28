using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashTerminal.Models
{
    /// <summary>
    /// Предоставляет управление настройками пользователя.
    /// </summary>
    public class SettingsManager
    {
        /// <summary>
        /// Имена всех доступных портов.
        /// </summary>
        public string[] PortNames { get; set; }

        /// <summary>
        /// Имя порта, используемого для получения идентификационных кодов артикулов.
        /// </summary>
        public string ScannerPort { get; set; }



        /// <summary>
        /// Инициализирует новый экземпляр класса SettingsManager.
        /// </summary>
        public SettingsManager()
        {
            PortNames = SerialPort.GetPortNames();
        }
    }
}