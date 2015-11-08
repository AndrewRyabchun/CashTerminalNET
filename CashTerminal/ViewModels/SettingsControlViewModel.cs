using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashTerminal.Commons;
using System.IO.Ports;
using System.Windows.Input;
using CashTerminal.Data;

namespace CashTerminal.ViewModels
{
    internal class SettingsControlViewModel : ViewModelBase
    {
        public ObservableCollection<string> SerialPortList { get; set; }
        public string ScannerPort { get; set; }
        public string PrinterPort { get; set; }

        public ICommand SaveCommand { get; set; }

        private int _terminalNumber;

        public string TerminalNumber
        {
            get { return _terminalNumber.ToString(); }
            set { int.TryParse(value, out _terminalNumber); }
        }

        private IOverlayable _parent;

        public SettingsControlViewModel(IOverlayable parent)
        {
            _parent = parent;
            SaveCommand = new RelayCommand(Save, IsValidTerminalNumber);
            SerialPortList = new ObservableCollection<string>(_parent.Settings.PortNames);
            PrinterPort = _parent.Settings.PrinterPort;
            ScannerPort = _parent.Settings.ScannerPort;
            _terminalNumber = _parent.Settings.TerminalNumber;
        }

        private void Save(object obj)
        {
            _parent.Settings.PrinterPort = PrinterPort;
            _parent.Settings.ScannerPort = ScannerPort;
            _parent.Settings.TerminalNumber = _terminalNumber;
            _parent.CloseOverlay();
        }

        private bool IsValidTerminalNumber(object obj)
        {
            int _;
            return int.TryParse(TerminalNumber, out _);
        }

        public override string ToString()
        {
            return "SettingsControl";
        }
    }
}