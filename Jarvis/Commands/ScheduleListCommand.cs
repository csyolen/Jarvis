using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Jarvis.Listeners;
using Jarvis.Locale;
using Jarvis.Objects;
using Jarvis.Objects.Reference;
using Jarvis.Tickers;

namespace Jarvis.Commands
{
    class ScheduleListCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var output = ScheduleTicker.Instance.Tasks
                .Where(o => o.DateTime < DateTime.Now.AddDays(2))
                .Select(task => task.Description + " at " + task.DateTime.ToShortTimeString());
            return output;
        }

        public string Regexes { get { return "schedule"; } }
    }
}