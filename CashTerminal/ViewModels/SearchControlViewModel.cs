using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CashTerminal.Commons;
using CashTerminal.Data;

namespace CashTerminal.ViewModels
{
    internal class SearchControlViewModel : ViewModelBase
    {
        public string SearchPattern { get; set; }
        public ObservableCollection<Article> SearchResults { get; set; }

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
        }

        private void Search(object obj)
        {
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