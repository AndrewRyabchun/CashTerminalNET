using System;
using System.Collections.ObjectModel;
using System.Linq;
using CashTerminal.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CashTerminal.Models
{
    internal class DataBaseProvider
    {
        private SupermarketDataEntities ent;
        public ObservableCollection<ArticleRecord> Items { get; private set; }

        public void AddArticle(Article art)
        {
            try
            {
                var searchItem = Items.First(item => item.ID == art.ID);
                searchItem.Add();
            }
            catch (InvalidOperationException)
            {
                Items.Add(new ArticleRecord(art));
            }
        }

        public DataBaseProvider()
        {
            Items=new ObservableCollection<ArticleRecord>();
        }

        public Article GetArticle(long id)
        {
            using (ent = new SupermarketDataEntities())
            {
                return ent.Articles.First(item => item.ID == id);
            }
        }

        public async Task<ObservableCollection<Article>> SearchAsync(string name)
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

                return new ObservableCollection<Article>(result);
            }
        }
    }
}