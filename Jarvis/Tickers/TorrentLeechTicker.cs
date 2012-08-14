using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarvis.Objects.Torrents;

namespace Jarvis.Tickers
{
    class TorrentLeechTicker : TickerBase
    {


        public TorrentLeechTicker() : base(10.Minutes())
        {

        }

        protected override void Tick()
        {
            var tl = new TorrentLeech();
            var entries = tl.GetEntries(TorrentLeech.Home);
            var downloaded =
                entries.Where(o => Brain.Settings.Shows.Any(x => o.Title.ToLower().Contains(x))).Where(o => o.Download());
            foreach (var entry in downloaded)
            {
                Brain.ListenerManager.CurrentListener.Output("Downloading " + entry.Friendly);
            }
        }
    }
}
