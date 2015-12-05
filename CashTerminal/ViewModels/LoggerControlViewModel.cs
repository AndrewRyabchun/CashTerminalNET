using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CashTerminal.Commons;
using CashTerminal.Models;
using Microsoft.Win32;

namespace CashTerminal.ViewModels
{
    internal class LoggerControlViewModel : ViewModelBase
    {
        /// <summary>
        /// Комманда закрытия перекрытия.
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// Комманда экспорта истории.
        /// </summary>
        public ICommand ExportCommand { get; set; }
        public string LogText { get; }

        private readonly IOverlayable _parent;

        public LoggerControlViewModel(IOverlayable parent)
        {
            _parent = parent;
            CloseCommand = new RelayCommand(Close);
            ExportCommand = new RelayCommand(Export);

            var sb = new StringBuilder();
            var hist = HistoryManager.Instance.History;
            foreach (var str in hist)
                sb.AppendLine(str);
            LogText = sb.ToString();
        }

        private void Close(object obj)
        {
            _parent.CloseOverlay();
        }

        private async void Export(object obj)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                using (var file = new StreamWriter(sfd.FileName))
                {
                    await file.WriteAsync(LogText);
                    UIMediator.Instance.Update("Экспортировано: " + sfd.FileName);
                }

            }
        }

        public override string ToString()
        {
            return "LoggerControl";
        }
    }
}