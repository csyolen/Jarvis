using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jarvis
{
    public interface IHandler
    {
        string Handle(string input, Match match);
        string Regexes { get; }
    }
}
