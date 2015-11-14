using System;
using CashTerminal.Data;

namespace CashTerminal.Models
{
    internal class ArticleRecord
    {
        public ArticleRecord(Article sample)
        {
            ID = sample.ID;
            Name = sample.Name;
            Price = sample.Price;
            Count = 1;
            FullPrice = sample.Price;
        }

        private int _count;
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (value<0)
                    return;
                _count = value;
                FullPrice = Price*value;
            }
        }
        public decimal FullPrice { get; private set; }
        public long ID { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
    }
}