using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Jarvis.Tickers;

namespace Jarvis.Commands
{
    class ScheduleCommand : ICommand
    {
        public string Handle(string input, Match match)
        {
            var task = input.RegexMatch(@".*(?=at)").Value;
            var time = input.RegexMatch(@"\d+:\d+").Value;
            var dateTime = DateTime.ParseExact(time, "HH:mm", CultureInfo.CurrentCulture);
            ScheduleTicker.Instance.AddTask(dateTime,task);
            return "Task scheduled.";
        }

        public string Regexes
        {
            get { return @"(at)[^\d+](\d+:\d+)"; }
        }
    }
}
