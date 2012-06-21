using System;
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

        protected override void Tick(object sender, ElapsedEventArgs e)
        {
            var data = new BrowserClient().DownloadString(Url);
            var doc = new XmlDocument();
            doc.LoadXml(data);

            var last = DateTime.Now.Subtract(1.Minutes());
            var items = doc.SelectNodes("//item");
            foreach (XmlNode item in items)
            {
                var date = DateTime.Parse(item.SelectSingleNode(".//pubDate").InnerText);
                if(date < last) continue;
                Brain.ListenerManager.CurrentListener.Output(item.SelectSingleNode(".//title").InnerText);
            }
        }
    }
}
