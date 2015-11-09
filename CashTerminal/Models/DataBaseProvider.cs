using System;
using System.Collections.ObjectModel;
using System.Linq;
using CashTerminal.Data;
using System.Collections.Generic;

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

        public Article[] Search(string name)
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

                    result.AddRange(founded);
                }

                string[] words = name.Split(' ');

                founded = from d in ent.Articles
                          where words.All(d.Name.Contains)
                          select d;

                result.AddRange(founded);

                return result.ToArray();
            }
        }
    }
}