using System;
using CashTerminal.Data;

namespace CashTerminal.Models
{
    /// <summary>
    /// Запись, содержащая всю необходимую информацию про товар для отображения в чеке.
    /// </summary>
    internal class ArticleRecord
    {
        /// <summary>
        /// Инициализирует экземпляр класса ArticleRecord, используя базовую информацию о товаре.
        /// </summary>
        /// <param name="sample">Содержит базовую информацию о товаре.</param>
        public ArticleRecord(Article sample)
        {

            ID = sample.ID;
            Name = sample.Name;
            Price = sample.Price;
            Count = 1;
            FullPrice = sample.Price;
        }

        /// <summary>
        /// Количество единиц товара.
        /// </summary>
        private int _count;

        /// <summary>
        /// Предоставляет сведения о количестве единиц товара.
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (value < 0)
                    return;
                _count = value;
                FullPrice = Price * value;
            }
        }

        /// <summary>
        /// Полная стоимость с учетом необходимого числа единиц товара.
        /// </summary>
        public decimal FullPrice { get; private set; }

        /// <summary>
        /// Идентификационный код товара
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// Название товара
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Стоимость единицы товара
        /// </summary>
        public decimal Price { get; private set; }
    }
}