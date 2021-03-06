﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Jarvis.Listeners;
using Jarvis.Tickers;
using Jarvis.Utilities;

namespace Jarvis.Commands
{
    class ScheduleCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var task = match.Groups[1].Value;
            var time = match.Groups[2].Value;
            var dateTime = RelativeDateParser.Parse(input);
            ScheduleTicker.Instance.AddTask(dateTime,task);
            yield return "I will remind you to {0} at {1}".Template(task, dateTime.ToString());
        }

        public string Regexes
        {
            get { return @"remind.+to (.+) (tomorrow|in|at|on .+)"; }
        }
    }
}
