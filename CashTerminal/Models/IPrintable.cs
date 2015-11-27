using System.Collections.Generic;
using System.IO.Ports;

namespace CashTerminal.Models
{
    /// <summary>
    /// Определяет методы для вывода информации с необходимым форматированием.
    /// </summary>
    internal interface IPrintable
    {
        /// <summary>
        /// Возвращает отформатированный текст.
        /// </summary>
        /// <param name="items">Список строк с информацией, подлежащей форматированию.</param>
        /// <returns>Коллекция, которая реализовывает интерфейс IEnumerable.</returns>
        IEnumerable<string> GenerateOutput(List<ArticleRecord> items);

        void Send(List<ArticleRecord> items, SerialPort sp);
    }
}