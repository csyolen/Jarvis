using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Objects.Reference;
using Jarvis.Runnables;

namespace Jarvis.Handlers
{
    class SearchHandler : IHandler
    {
        public string Handle(string input, Match match)
        {
            var subject = match.Groups[1].Value.Trim();
            var search = new Search(subject);
            Brain.RunnableManager.Runnable = new ProcessRunnable(search.Link);
            return search.Description;
        }

        public string Regexes
        {
            get { return "search(.+)"; }
        }
    }
}
