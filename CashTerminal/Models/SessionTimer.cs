using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CashTerminal.Models
{
    /// <summary>
    /// Таймер сессии пользователя.
    /// </summary>
    public class SessionTimer : INotifyPropertyChanged
    {
        /// <summary>
        ///  Таймер, интегрированный в очередь Dispatcher, обрабатываемый с заданным интервалом времени и заданным приоритетом.
        /// </summary>
        private DispatcherTimer _timer = new DispatcherTimer();

        /// <summary>
        /// Дата запуска таймера.
        /// </summary>
        private DateTime _sessionStartTime;

        /// <summary>
        /// Время нахождения в данной сессии.
        /// </summary>
        public TimeSpan SessionTime
        {
            get { return _sessionStartTime - DateTime.Now; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса SessionTimer.
        /// </summary>
        public SessionTimer()
        {
            _sessionStartTime = DateTime.Now;
            _timer = new DispatcherTimer();
            _timer.Tick += (sender, e) => { OnPropertyChanged("SessionTime"); };
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Start();
        }

        /// <summary>
        /// Перезапускает таймер.
        /// </summary>
        public void Reset()
        {
            _sessionStartTime = DateTime.Now;
            if (!_timer.IsEnabled)
                _timer.Start();
        }

        /// <summary>
        /// Приостанавливает таймер.
        /// </summary>
        public void Suspend()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Представляет метод, который обрабатывает событие PropertyChanged, возникающее при изменении свойства компонента.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged с предоставленными аргументами.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}