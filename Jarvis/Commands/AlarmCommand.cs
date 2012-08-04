using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Tickers;

namespace Jarvis.Commands
{
    class AlarmCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            ClockTicker.Instance.StopAlarm();
            Brain.Awake = true;
            yield return "Good morning sir.";
        }

        public string Regexes { get { return "alarm"; } }
    }
}
