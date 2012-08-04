using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Objects;
using Jarvis.Objects.Torrents;
using Jarvis.Utilities;

namespace Jarvis.Commands
{
    class MoviesCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var tl = new TorrentLeech();
            return tl.Movies().Select(o => o.Friendly).Distinct().Take(10);
        }

        public string Regexes { get { return "movies"; } }
    }
}
