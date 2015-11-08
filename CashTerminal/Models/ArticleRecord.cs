using System;
using CashTerminal.Data;

namespace CashTerminal.Models
{
    internal class ArticleRecord : Article
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

        public void Add(Article art)
        {
            if (ID == art.ID)
            {
                Count += 1;
                FullPrice += Price;
            }

            throw new ArgumentException();
        }
    }
}