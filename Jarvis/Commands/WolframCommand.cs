using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Objects.Reference;
using Jarvis.Views;

namespace Jarvis.Commands
{
    class WolframCommand : ICommand
    {
        public string Handle(string input, Match match, IListener listener)
        {
            var w = new Wolfram(input);
            return w.Result;
        }

        public string Regexes
        {
            get { return "(what is|when|how many) (.+)"; }
        }
    }
}
