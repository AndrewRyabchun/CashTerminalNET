using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CashTerminal.Commons;
using CashTerminal.Data;
using CashTerminal.Models;

namespace CashTerminal.ViewModels
{
    internal class SearchControlViewModel : ViewModelBase
    {
        public string SearchPattern { get; set; }
        public ObservableCollection<Article> SearchResults { get; set; }
        public Article SelectedResult { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private IOverlayable _parent;

        public SearchControlViewModel(IOverlayable parent)
        {
            SearchResults = new ObservableCollection<Article>();

            _parent = parent;

            AddCommand = new RelayCommand(Add);
            SearchCommand = new RelayCommand(Search);
            CloseCommand = new RelayCommand(Close);
        }

        private void Add(object obj)
        {
            if (SelectedResult==null)
                return;
            _parent.Model.Items.Add(new ArticleRecord(SelectedResult));
            _parent.UpdateUI();
            _parent.CloseOverlay();
        }

        private async void Search(object obj)
        {
            SearchResults = await _parent.Model.DataBase.SearchAsync(SearchPattern);
            OnPropertyChanged("SearchResults");
        }

        private void Close(object obj)
        {
            _parent.CloseOverlay();
        }

        public override string ToString()
        {
            return "SearchControl";
        }
    }
}