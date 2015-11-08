using System.Windows.Input;
using CashTerminal.Commons;

namespace CashTerminal.ViewModels
{
    class LoggerControlViewModel:ViewModelBase
    {
        public ICommand CloseCommand { get; set; }
        public ICommand ExportCommand { get; set; }

        private string _logText;
        public string LogText => _parent.LogText;

        private readonly IOverlayable _parent;

        public LoggerControlViewModel(IOverlayable parent)
        {
            _parent = parent;
            CloseCommand=new RelayCommand(Close);
            ExportCommand=new RelayCommand(Export);
        }

        private void Close(object obj)
        {
            _parent.CloseOverlay();
        }

        private void Export(object obj)
        {
            
        }

        public override string ToString()
        {
            return "LoggerControl";
        }
    }
}