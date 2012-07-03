using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Jarvis.Objects;

namespace Jarvis.Tickers
{
    class RssTicker : TickerBase
    {
        public string Url { get; private set; }

        public RssTicker(string url) : base(1.Minutes())
        {
            Url = url;
        }

        protected override void Tick()
        {
            foreach (var item in new RssFeed(Url).New(DateTime.Now.Subtract(1.Minutes())))
            {
                Brain.ListenerManager.CurrentListener.Output(item.Title);
            }
        }
    }
}
