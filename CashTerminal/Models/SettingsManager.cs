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
        /// Имя порта, используемого для подачи чека на печать.
        /// </summary>
        public string PrinterPort { get; set; }

        /// <summary>
        /// Номер кассы, на которой развернута текущая сессия.
        /// </summary>
        public int TerminalNumber { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса SettingsManager.
        /// </summary>
        public SettingsManager()
        {
            PortNames = SerialPort.GetPortNames();
            if (PortNames.Length > 1)
            {
                ScannerPort = PortNames.First();
                PrinterPort = PortNames.Last();
            }
            TerminalNumber = 0;
        }
    }
}