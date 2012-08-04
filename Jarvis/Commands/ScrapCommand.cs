using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Objects;

namespace Jarvis.Commands
{
    public class ScrapCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var gv = GoogleVoice.Inbox();

            yield return "";
        }

        public string Regexes { get { return "scrap"; } }
    }
}
