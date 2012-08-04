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
    class XboxCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var x = XboxLive.FromGamerTag("dharun");
            var r = x.Friends.Where(o => o.IsOnline).Select(o => o.Description);
            return r;
        }

        public string Regexes { get { return "xbox"; } }
    }
}
