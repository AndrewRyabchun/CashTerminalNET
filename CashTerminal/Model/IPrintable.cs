using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashTerminal.Model
{
    interface IPrintable
    {
        IEnumerable<string> GenerateOutput();
    }
}
