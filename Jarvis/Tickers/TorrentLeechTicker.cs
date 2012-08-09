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

        private static readonly FileInfo ShowsFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/TLBot/shows.txt");
        private HashSet<string> _shows;

        public TorrentLeechTicker() : base(10.Minutes())
        {
            if (!ShowsFile.Exists)
            {
                Debug.Assert(ShowsFile.Directory != null, "_showsFile.Directory != null");
                ShowsFile.Directory.Create();
                ShowsFile.Create().Close();
            }

            _shows = new HashSet<string>(File.ReadAllLines(ShowsFile.FullName).Where(o => o.Trim().Length > 0).Select(o => o.ToLower()));

        }

        protected override void Tick()
        {
            var tl = new TorrentLeech();
            var entries = tl.GetEntries(TorrentLeech.Home);
            var downloaded =
                entries.Where(o => _shows.Any(x => o.Title.ToLower().Contains(x))).Where(o => o.Download());
            foreach (var entry in downloaded)
            {
                Brain.ListenerManager.CurrentListener.Output("Downloading " + entry.Friendly);
            }
        }
    }
}
