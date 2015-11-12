using System;
using System.Collections.ObjectModel;
using System.Linq;
using CashTerminal.Data;
using System.Collections.Generic;
using System.Data.Entity;

namespace CashTerminal.Models
{
    internal class DataBaseProvider : IDisposable
    {
        private SupermarketDataEntities ent;
        public ObservableCollection<ArticleRecord> Items { get; private set; }

        public void AddArticle(Article art)
        {
            try
            {
                var searchItem = Items.First(item => item.ID == art.ID);
                searchItem.Add(art);
            }
            catch (InvalidOperationException)
            {
                Items.Add(new ArticleRecord(art));
            }
        }

        public void Dispose()
        {
            ent.Dispose();
        }

        public Article GetArticle(long id)
        {
            using (ent = new SupermarketDataEntities())
            {
                return ent.Articles.First(item => item.ID == id);
            }
        }

        public async void SearchAsync(string name, ObservableCollection<Article> searchResults)
        {
            using (ent = new SupermarketDataEntities())
            {
                List<Article> result = new List<Article>();

                long parsedID;

                bool idNeeded = long.TryParse(name, out parsedID);

                IQueryable<Article> founded;

                if (idNeeded)
                {
                    string id = parsedID.ToString();

                    founded = from d in ent.Articles
                              where d.ID.ToString().Contains(id)
                              select d;

                    result.AddRange(await founded.ToListAsync());
                }

                string[] words = name.Split(' ');

                founded = from d in ent.Articles
                          where words.All(item => d.Name.Contains(item))
                          select d;

                result.AddRange(await founded.ToListAsync());

                searchResults = new ObservableCollection<Article>(result);
            }
        }
    }
}