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
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var subject = match.Groups[1].Value;
            var search = new Search(subject);
            Brain.RunnableManager.Runnable = new UrlRunnable(search.Link);
            yield return search.Description;
        }

        public string Regexes
        {
            get { return "search (.+)"; }
        }
    }
}
