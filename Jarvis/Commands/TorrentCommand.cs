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
        public string Handle(string input, Match match, IListener listener)
        {
            var r = "";
            foreach (var torrent in Brain.ListenerManager.TorrentLeech.Torrents)
            {
                r += "{0} is {1}, {2}% done\r\n".Template(torrent.Torrent.Name.TorrentName(), torrent.State.ToString().ToLower(), Math.Floor(torrent.Progress));
            }
            return r;
        }

        public string Regexes { get { return "torrent"; } }
    }
}
