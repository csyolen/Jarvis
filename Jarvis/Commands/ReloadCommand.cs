﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Utilities;

namespace Jarvis.Commands
{
    class ReloadCommand : ICommand 
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            Brain.Settings = new Settings();
            yield return "Reloaded.";
        }

        public string Regexes { get { return "reload"; } }
    }
}
