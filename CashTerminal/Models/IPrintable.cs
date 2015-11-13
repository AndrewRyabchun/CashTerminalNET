using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CashTerminal.Models
{
    internal interface IPrintable
    {
        IEnumerable<string> GenerateOutput(ObservableCollection<ArticleRecord> items);
    }
}