using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Objects.Reference;

namespace Jarvis.Commands
{
    class ActorsCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var query = match.Groups[2].Value;
            var imdb = IMDB.FromQuery(query);
            yield return "{0} were in {1}".Template(imdb.Actors, imdb.Title);
        }

        public string Regexes
        {
            get { return "who(.+)in (.+)"; }
        }
    }
}
