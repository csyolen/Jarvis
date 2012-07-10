﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Objects.Reference;

namespace Jarvis.Commands
{
    class RatingCommand : ICommand
    {
        public string Handle(string input, Match match, IListener listener)
        {
            var query = match.Groups[1].Value.Trim();
            var imdb = IMDB.FromQuery(query);
            var r = "{0} received a rating of {1}.".Template(imdb.Title, imdb.ImdbRating);
            if (double.Parse(imdb.ImdbRating) > 6)
                r += Environment.NewLine + "You should probably watch it.";
            return r;
        }

        public string Regexes { get { return "how was (.+)"; } }
    }
}
