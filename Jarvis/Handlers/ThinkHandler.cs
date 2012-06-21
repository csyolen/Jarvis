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

namespace Jarvis.Handlers
{
    class ThinkHandler : IHandler
    {
        public string Handle(string input, Match match)
        {
            Brain.Think = input.Contains("start");
            return Brain.Think ? "I shall start thinking." : "I will stop thinking.";
        }

        public string Regexes { get { return @"(start|stop)\s+(talking|thinking)"; } }
    }
}