using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Locale;
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
            var movies = tl.GetEntries(TorrentLeech.Movies).Distinct().Take(20).ToArray();
            var names = movies.Select(o => o.Friendly).ToArray();
            Brain.Pipe.ListenOnce((i, m, l) =>
                {
                    var movie = m.Groups[1].Value;
                    var entry = movies.FirstOrDefault(o => o.Friendly.ToLower().Contains(movie));
                    if(entry == null)
                        return;
                    if(!entry.Download())
                    {
                        Brain.Pipe.ListenNext((s, match1, listener1) =>
                            {
                                if (match1.Value == "yes")
                                {
                                    entry.Download(true);
                                    listener1.Output(Speech.Yes.Parse());
                                    listener1.Output("I shall redownload it.");
                                    return;
                                }
                                listener1.Output(Speech.Yes.Parse());
                                listener1.Output("I won't redownload it.");
                            }, "yes", "no");
                        l.Output("You've already downloaded that sir. Do you want to redownload it?");
                    }
                    else
                    {
                        l.Output("Downloading " + entry.Friendly + "...");
                    }
                }, "download (.+)");
            yield return "Here are the latest films. Do you want to download any of them?";
            yield return string.Join(", ", names) + "\r\n";
        }

        public string Regexes { get { return "movies"; } }
    }
}
