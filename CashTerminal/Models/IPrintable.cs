using System.Collections.Generic;

namespace CashTerminal.Models
{
    internal interface IPrintable
    {
        IEnumerable<string> GenerateOutput();
    }
}