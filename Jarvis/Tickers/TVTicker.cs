using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Jarvis.Objects;

namespace Jarvis.Tickers
{
    class TVTicker : TickerBase
    {
        public TVTicker() : base(1000*60*60*24)
        {
        }

        protected override void Tick()
        {
            string url = "http://followmy.tv/calendar.ics?apikey=4cf8599d-8d00-46c0-a2f5-0c5abc287203";

            var lines = new WebClient().DownloadString(url).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var trimmed = lines.Where(o => o.StartsWith("SUMMARY") || o.StartsWith("DTSTART")).Select(o => o.Replace("SUMMARY:", "").Replace("DTSTART:", "")).ToList();
            var sched = ScheduleTicker.Instance;
            Brain.Settings.Shows = new HashSet<string>();
            for (int i = 0; i < trimmed.Count; i += 2)
            {
                try
                {
                    var name = trimmed[i + 1];
                    var entry = new TVEntry(name, trimmed[i]);
                    if (entry.Time < DateTime.Now) continue;
                    Brain.Settings.Shows.Add(entry.Name.ToLower().Trim());
                    sched.AddTask(entry.Time, "Watch " + entry.Name.Trim());
                }
                catch
                {
                }
            }
        }
    }
}
