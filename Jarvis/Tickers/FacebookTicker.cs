﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using Jarvis.Objects;

namespace Jarvis.Tickers
{
    class FacebookTicker : TickerBase
    {
        private const string Url =
            "http://www.facebook.com/feeds/notifications.php?id=1342020455&viewer=1342020455&key=AWghxjJ6roByavRp&format=rss20";


        public FacebookTicker() : base(1.Minutes())
        {
        }

        protected override void Tick()
        {
            foreach (RssItem item in new RssFeed(Url).New(DateTime.Now.Subtract(1.Minutes())))
            {
                Brain.ListenerManager.CurrentListener.Output(item.Title);
            }
        }
    }
}
