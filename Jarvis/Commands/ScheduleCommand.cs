using System;
using System.Globalization;
using System.Text.RegularExpressions;
using DateTimeParser;
using Jarvis.Listeners;
using Jarvis.Tickers;

namespace Jarvis.Commands
{
    class ScheduleCommand : ICommand
    {
        public string Handle(string input, Match match, IListener listener)
        {
            var task = match.Groups[1].Value;
            var time = match.Groups[2].Value;
            var dateTime = DateTimeEnglishParser.ParseRelative(DateTime.Now, time);
            ScheduleTicker.Instance.AddTask(dateTime,task);
            return "I will remind you to {0} on {1}".Template(task, dateTime.ToShortDateString());
        }

        public string Regexes
        {
            get { return @"remind.+to(.+)on(.+)"; }
        }
    }
}
