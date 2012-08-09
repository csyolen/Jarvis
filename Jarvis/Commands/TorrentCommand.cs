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
            yield return "";
        }

        public string Regexes { get { return "torrent"; } }
    }
}
