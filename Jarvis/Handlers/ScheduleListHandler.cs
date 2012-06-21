using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Jarvis.Locale;
using Jarvis.Objects;
using Jarvis.Objects.Reference;
using Jarvis.Tickers;

namespace Jarvis.Handlers
{
    class ScheduleListHandler : IHandler
    {
        public string Handle(string input, Match match)
        {
            var output = ScheduleTicker.Instance.Tasks
                .Where(o => o.DateTime < DateTime.Now.AddDays(2))
                .Aggregate("", (current, task) => current + (task.Description + " at " + task.DateTime.ToShortTimeString() + Environment.NewLine));
            return output.Trim();
        }

        public string Regexes { get { return "schedule"; } }
    }
}