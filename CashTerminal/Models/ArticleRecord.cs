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

        public int Count { get; private set; }
        public decimal FullPrice { get; private set; }
        public long ID { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public void Add()
        {
            Count += 1;
            FullPrice += Price;
        }
    }
}