﻿using System;
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
            var query = match.Groups[1].Value;
            var w = new Wolfram(query);
            foreach (var image in w.Images)
            {
                ImageView.Create(image);
            }
            return "";
        }

        public string Regexes
        {
            get { return "wolfram (.+)"; }
        }
    }
}
