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

        public ICommand SaveCommand { get; set; }


        private IOverlayable _parent;

        public SettingsControlViewModel(IOverlayable parent)
        {
            _parent = parent;
            SaveCommand = new RelayCommand(Save);
            SerialPortList = new ObservableCollection<string>(_parent.Settings.PortNames);
            ScannerPort = _parent.Settings.ScannerPort;
        }

        private void Save(object obj)
        {
            _parent.Settings.ScannerPort = ScannerPort;
            _parent.CloseOverlay();
        }


        public override string ToString()
        {
            return "SettingsControl";
        }
    }
}