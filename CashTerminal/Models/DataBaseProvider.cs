using System;
using System.Collections.ObjectModel;
using System.Linq;
using CashTerminal.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CashTerminal.Models
{
    /// <summary>
    /// Предоставляет все операции взаимодействия с базой данных.
    /// </summary>
    internal class DataBaseProvider
    {
        /// <summary>
        /// Посредник, необходимый для взаимодействия базы данных и классов, описывающих товары.
        /// </summary>
        private SupermarketDataEntities _ent;

        /// <summary>
        /// Список товаров, добавленных в чек.
        /// </summary>
        public List<ArticleRecord> Items { get; private set; }

        /// <summary>
        /// Инициализирует экземпляр класса DataBaseProvider.
        /// </summary>
        public DataBaseProvider()
        {
            Items = new List<ArticleRecord>();
        }

        /// <summary>
        /// Добавляет товар в чек.
        /// </summary>
        /// <param name="art">Товар для добавления в чек.</param>
        public void AddArticle(Article art)
        {
            if (art == null) return;

            try
            {
                var searchItem = Items.First(item => item.ID == art.ID);
                searchItem.Count++;
            }
            catch (InvalidOperationException)
            {
                Items.Add(new ArticleRecord(art));
            }
        }

        /// <summary>
        /// Получить товар за идентификационним кодом.
        /// </summary>
        /// <param name="id">Полный идентификационный код товара.</param>
        /// <returns>Товар с соответствующим идентификационным кодом.</returns>
        public Article GetArticle(long id)
        {
            using (_ent = new SupermarketDataEntities())
            {
                return _ent.Articles.FirstOrDefault(item => item.ID == id);
            }
        }

        /// <summary>
        /// Поиск товаров по имени или идентификационному коду.
        /// </summary>
        /// <param name="name">Значение для поиска.</param>
        /// <returns>Список найденных товаров.</returns>
        public async Task<ObservableCollection<Article>> SearchAsync(string name)
        {
            using (_ent = new SupermarketDataEntities())
            {
                List<Article> result = new List<Article>();

                long parsedId;

                bool idNeeded = long.TryParse(name, out parsedId);

                IQueryable<Article> founded;

                if (idNeeded)
                {
                    string id = parsedId.ToString();

                    founded = from d in _ent.Articles
                              where d.ID.ToString().Contains(id)
                              select d;

                    result.AddRange(await founded.ToListAsync());
                }

                string[] words = name.Split(' ');

                founded = from d in _ent.Articles
                          where words.All(item => d.Name.Contains(item))
                          select d;

                result.AddRange(await founded.ToListAsync());

                return new ObservableCollection<Article>(result);
            }
        }
    }
}