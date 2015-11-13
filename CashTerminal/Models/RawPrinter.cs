using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashTerminal.Models
{
    class RawPrinter : IPrintable
    {
        int _lineWidth;
        int _cashDeskNumber;

        public RawPrinter(int width, int cashDeskNumber)
        {
            _lineWidth = width;
            _cashDeskNumber = cashDeskNumber;
        }

        public IEnumerable<string> GenerateOutput(ObservableCollection<ArticleRecord> items)
        {
            List<string> cheque = new List<string>();

            cheque.AddRange(Heading());

            foreach (ArticleRecord item in items)
            {
                cheque.Add("\n");
                cheque.AddRange(CompressString($"- {item.Price} * {item.Count} = {item.FullPrice} | {item.Name}"));
            }

            cheque.Add(new string('=', _lineWidth));

            decimal sum = items.Sum(x => x.FullPrice);

            cheque.AddRange(CompressString($"Сума: {sum} грн."));

            return cheque;
        }

        private string[] Heading()
        {
            List<string> arr = new List<string>();

            arr.Add(new string('=', _lineWidth));

            arr.AddRange(CompressString($"* Добро пожаловать!!!"));

            DateTime now = DateTime.Now;
            arr.AddRange(CompressString($"* Чек создан: {now.Day}.{now.Month}.{now.Year} {now.Hour}:{now.Minute}:{now.Second}"));
            arr.AddRange(CompressString($"* Номер касы: {_cashDeskNumber}"));
            arr.Add(new string('=', _lineWidth));

            return arr.ToArray();
        }

        private string[] CompressString(string str)
        {
            if (str.Length < _lineWidth)
                return new[] { str };

            else
            {
                int start = 0;

                List<string> lines = new List<string>();

                while (true)
                {
                    lines.Add(Convert.ToString(str.Substring(start, _lineWidth)));

                    start += _lineWidth;

                    if (start + _lineWidth >= str.Length)
                    {
                        lines.Add(str.Substring(start, str.Length - start));

                        break;
                    }
                }

                return lines.ToArray();
            }
        }
    }
}
