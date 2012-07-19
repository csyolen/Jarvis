using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Locale;
using Jarvis.Views;

namespace Jarvis.Commands
{
    class NetflixCommand : ICommand
    {
        public string Handle(string input, Match match, IListener listener)
        {
            BrowserView.Create("http://netflix.com");
            return Speech.Yes.Parse();
        }

        public string Regexes { get { return "netflix"; } }
    }
}
