using System;
using System.Collections.ObjectModel;
using System.Linq;
using CashTerminal.Data;

namespace CashTerminal.Models
{
    internal class DataBaseProvider : IDisposable
    {
        private SupermarketDataEntities ent;
        public ObservableCollection<ArticleRecord> Items { get; private set; }

        public void AddArticle(Article art)
        {
            var searchItem = Items.First(item => item.ID == art.ID);

            if (searchItem != null) searchItem.Add(art);

            else Items.Add(new ArticleRecord(art));
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

        public Article[] Search(string name, string id = "")
        {
            using (ent = new SupermarketDataEntities())
            {
                var founded = from d in ent.Articles
                    where d.Name.Contains(name) || d.ID.ToString().Contains(id)
                    select d;

                return founded.ToArray();
            }
        }
    }
}