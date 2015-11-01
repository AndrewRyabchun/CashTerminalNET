using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashTerminal.Commons;
using System.IO.Ports;
using System.Windows.Input;

namespace CashTerminal.ViewModels
{
    class SettingsControlViewModel:ViewModelBase
    {
        public ObservableCollection<string> SerialPortList { get; set; }
        public string ScannerPort { get; set; }
        public string PrinterPort { get; set; }

        public ICommand SaveCommand { get; set; }

        private int _terminalNumber;
        public string TerminalNumber
        {
            get
            {
                return _terminalNumber.ToString();
            }
            set
            {
                int.TryParse(value, out _terminalNumber);
            }
        }

        private IOverlayable _parent;

        public SettingsControlViewModel(IOverlayable parent)
        {
            _parent = parent;
            SaveCommand=new RelayCommand(Save, IsValidTerminalNumber);
            var portnames = SerialPort.GetPortNames();
            SerialPortList=new ObservableCollection<string>(portnames);
        }

        private void Save(object obj)
        {
            _parent.CloseOverlay();
        }

        private bool IsValidTerminalNumber(object obj)
        {
            int _;
            var a = int.TryParse(TerminalNumber, out _);
            return a;
        }

        public override string ToString()
        {
            return "SettingsControl";
        }
    }
}
