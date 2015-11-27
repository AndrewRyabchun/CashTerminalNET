using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashTerminal.Models
{
    /// <summary>
    /// Предоставляет методы для создания чека с заданной шириной строки.
    /// </summary>
    internal class RawPrinter : IPrintable
    {
        /// <summary>
        /// Ширина строки текста.
        /// </summary>
        readonly int _lineWidth;

        /// <summary>
        /// Номер кассы, на которой был создан чек.
        /// </summary>
        readonly int _cashDeskNumber;

        /// <summary>
        /// Инициализирует экземпляр класса RawPrinter, используя заданные ширину текста и номер кассы.
        /// </summary>
        /// <param name="width">Ширина линии текста</param>
        /// <param name="cashDeskNumber">Номер кассы</param>
        public RawPrinter(int width, int cashDeskNumber)
        {
            _lineWidth = width;
            _cashDeskNumber = cashDeskNumber;
        }

        /// <summary>
        /// Создает чек с заданной шириной линии.
        /// </summary>
        /// <param name="items">Содержит всю информацию об артикулах, содержащихся в чеке.</param>
        /// <returns></returns>
        public IEnumerable<string> GenerateOutput(List<ArticleRecord> items)
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

        /// <summary>
        /// Создает заголовок чека
        /// </summary>
        /// <returns>Заголовок чека</returns>
        private string[] Heading()
        {
            List<string> arr = new List<string> { new string('=', _lineWidth) };

            arr.AddRange(CompressString("* Добро пожаловать!!!"));

            DateTime now = DateTime.Now;
            arr.AddRange(CompressString($"* Чек создан: {now.Day}.{now.Month}.{now.Year} {now.Hour}:{now.Minute}:{now.Second}"));
            arr.AddRange(CompressString($"* Номер касы: {_cashDeskNumber}"));
            arr.Add(new string('=', _lineWidth));

            return arr.ToArray();
        }

        /// <summary>
        /// Разбивает строку на массив строк фиксированной длинны.
        /// </summary>
        /// <param name="str">Строка для разбивания.</param>
        /// <returns>Массив строк фиксированной длинны.</returns>
        private string[] CompressString(string str)
        {
            if (str.Length < _lineWidth)
                return new[] { str };

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
