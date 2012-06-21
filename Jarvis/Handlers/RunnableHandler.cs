using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Locale;
using Jarvis.Objects.Reference;
using Jarvis.Runnables;

namespace Jarvis.Handlers
{
    class RunnableHandler : IHandler
    {
        public string Handle(string input, Match match)
        {
            return Brain.RunnableManager.Run() ? Speech.Open.Parse() : "";
        }

        public string Regexes
        {
            get { return @"^run\b|^open\b"; }
        }
    }
}
