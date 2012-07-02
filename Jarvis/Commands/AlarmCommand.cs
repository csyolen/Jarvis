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
        public string Handle(string input, Match match, IListener listener)
        {
            ClockTicker.Instance.StopAlarm();
            Brain.Awake = true;
            return "Good morning sir.";
        }

        public string Regexes { get { return "alarm"; } }
    }
}
