using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Jarvis.Tickers
{
    abstract class TickerBase
    {
        private readonly Timer _timer;
        protected TimeSpan Interval;

        protected TickerBase(double interval)
        {
            _timer = new Timer(interval);
            _timer.Elapsed += Tick;
        }

        protected TickerBase(TimeSpan interval)
            :this(interval.TotalMilliseconds)
        {
            
        }

        protected abstract void Tick(object sender, ElapsedEventArgs e);

        public void Start()
        {
            _timer.Start();
            Tick(null,null);
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
