using CashTerminal.Data;
using System.Collections.ObjectModel;
using System.IO.Ports;

namespace CashTerminal.Model
{
    class MainModel
    {
        HistoryManager _history;
        SerialPortProvider _port;
        DataBaseProvider _db;
        IPrintable _printer;

        public MainModel()
        {
            _history = new HistoryManager();
            _port = new SerialPortProvider(DataReceived);
            _db = new DataBaseProvider();
        }

        void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Article art = _db.GetArticle(long.Parse(((SerialPort)sender).ReadExisting()));

            _db.AddArticle(art);

            _history.Log($"Добавлен товар: {art.Name}");
        }
    }
}
