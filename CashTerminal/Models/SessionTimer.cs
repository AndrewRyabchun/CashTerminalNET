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
    public class SessionTimer : INotifyPropertyChanged
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private DateTime _sessionStartTime;

        public TimeSpan SessionTime
        {
            get { return _sessionStartTime - DateTime.Now; }
        }

        public SessionTimer()
        {
            _sessionStartTime = DateTime.Now;
            _timer = new DispatcherTimer();
            _timer.Tick += (sender, e) => { OnPropertyChanged("SessionTime"); };
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Start();
        }

        public void Reset()
        {
            _sessionStartTime = DateTime.Now;
            if (!_timer.IsEnabled)
                _timer.Start();
        }

        public void Suspend()
        {
            _timer.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}