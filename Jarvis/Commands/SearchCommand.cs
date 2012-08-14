using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Objects.Reference;

namespace Jarvis.Commands
{
    class SearchCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var subject = match.Groups[1].Value;
            var search = new Search(subject);
            Brain.Pipe.ListenOnce((s, match1, arg3) => Process.Start(search.Link), "open|more|show");
            yield return search.Description;
        }

        public string Regexes
        {
            get { return "search (.+)"; }
        }
    }
}
