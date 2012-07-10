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
            var query = input;
            if (match.Groups[1].Value == "wolfram")
                query = match.Groups[2].Value;
            var w = new Wolfram(query);
            return w.Result;
        }

        public string Regexes
        {
            get { return "(wolfram|what is|when|how many|how much) (.+)"; }
        }
    }
}
