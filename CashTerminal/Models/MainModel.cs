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
        public Authorization Validator { get; private set; }

        public MainModel(IPrintable printer, string username, string password, string portName = "")
        {
            _history = new HistoryManager();

            _port = (portName == "") ?
                new SerialPortProvider(DataReceived) : new SerialPortProvider(portName, DataReceived);

            _db = new DataBaseProvider();
            _printer = printer;
            Validator = new Authorization(username, password);
        }

        public void ChangeOutputType(IPrintable printer)
        {
            _printer = printer;
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Article art = _db.GetArticle(long.Parse(((SerialPort)sender).ReadExisting()));

            _db.AddArticle(art);

            _history.Log($"Добавлен товар: {art.Name}");
        }
    }
}