using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Jarvis.Listeners;

namespace Jarvis.Commands
{
    public interface ICommand
    {
        IEnumerable<string> Handle(string input, Match match, IListener listener);
        string Regexes { get; }
    }
}
