using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace CashTerminal
{
    class CashMemo
    {
        private SerialPort _sp;
        public  List<Article> _items;

        public CashMemo(string portName)
        {
            _sp = new SerialPort(portName);
            _items = new List<Article>();
        }

        public void AddArticle(Article item)
        {
            if (item == null) return;

            _items.Add(item);
            ItemsChanged();
        }

        public bool RemoveItem(int index)
        {
            if (index < 0 || index >= _items.Count) return false;

            _items.RemoveAt(index);
            ItemsChanged();

            return true;
        }

        public Article GetItem()
        {
            // Here should be used COM port
            throw new NotImplementedException();
        }


        public string[] GetHistory(string filePath)
        {
            throw new NotImplementedException();
        }

        public string PrintCheque(string outputPath = "output.txt")
        {
            throw new NotImplementedException();
        }

        public delegate void ItemChangedDelegate();

        public event ItemChangedDelegate ItemsChanged;
    }
}
