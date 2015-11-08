using System.IO.Ports;
using CashTerminal.Data;

namespace CashTerminal.Models
{
    internal class MainModel
    {
        private HistoryManager _history;
        private SerialPortProvider _port;
        private DataBaseProvider _db;
        private IPrintable _printer;

        public MainModel()
        {
            _history = new HistoryManager();
            _port = new SerialPortProvider(DataReceived);
            _db = new DataBaseProvider();
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Article art = _db.GetArticle(long.Parse(((SerialPort) sender).ReadExisting()));

            _db.AddArticle(art);

            _history.Log($"Добавлен товар: {art.Name}");
        }
    }
}