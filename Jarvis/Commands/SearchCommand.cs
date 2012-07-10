using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Objects.Reference;
using Jarvis.Runnables;

namespace Jarvis.Commands
{
    class SearchCommand : ICommand
    {
        public string Handle(string input, Match match, IListener listener)
        {
            var subject = match.Groups[1].Value.Trim();
            var search = new Wolfram(subject);
            return search.Result;
        }

        public string Regexes
        {
            get { return "search(.+)"; }
        }
    }
}
