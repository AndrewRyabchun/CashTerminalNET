using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashTerminal.Models
{
    public class SettingsManager
    {
        public string[] PortNames { get; set; }
        public string ScannerPort { get; set; }
        public string PrinterPort { get; set; }
        public int TerminalNumber { get; set; }

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