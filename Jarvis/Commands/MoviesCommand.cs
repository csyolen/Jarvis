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
            var movies = tl.Movies().Distinct().Take(10).ToArray();
            var names = movies.Select(o => o.Friendly).ToArray();
            Brain.Pipe.ListenOnce((i, m, l) =>
                {
                    var movie = m.Groups[1].Value;
                    var entry = movies.FirstOrDefault(o => o.Friendly.ToLower().Contains(movie));
                    if(entry == null)
                        return;
                    entry.Download();
                    l.Output("Downloading " + entry.Friendly + "...");
                }, "download (.+)");
            return names;
        }

        public string Regexes { get { return "movies"; } }
    }
}
