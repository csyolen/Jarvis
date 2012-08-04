using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Utilities;

namespace Jarvis.Commands
{
    class TorrentCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            return Brain.ListenerManager.TorrentLeech.Torrents.Select(
                torrent => "{0} is {1}, {2}% done"
                    .Template(torrent.Torrent.Name.TorrentName(), torrent.State.ToString().ToLower(), Math.Floor(torrent.Progress)));
        }

        public string Regexes { get { return "torrent"; } }
    }
}
