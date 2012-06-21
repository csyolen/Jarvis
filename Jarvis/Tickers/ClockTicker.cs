using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Jarvis.Tickers
{
    class ClockTicker : TickerBase
    {
        public ClockTicker() : base(1000)
        {
        }

        protected override void Tick(object sender, ElapsedEventArgs e)
        {
            if(DateTime.Now.Minute != 0 || DateTime.Now.Second != 0) return;
            Brain.ListenerManager.CurrentListener.Output("The time is " + DateTime.Now.ToShortTimeString());
        }
    }
}
