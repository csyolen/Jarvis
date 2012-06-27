using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jarvis.Commands
{
    class SayCommand : ICommand
    {
        public string Handle(string input, Match match)
        {
            var text = match.Groups[1].Value;
            return text;
        }

        public string Regexes { get { return "say(.+)"; } }
    }
}
