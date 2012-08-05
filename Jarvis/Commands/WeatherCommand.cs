using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Jarvis.Listeners;
using Jarvis.Locale;
using Jarvis.Objects;
using Jarvis.Objects.Reference;
using Jarvis.Views;

namespace Jarvis.Commands
{
    class WeatherCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var weather = new Weather();
            
            WeatherView.Create(weather);
            yield return weather.ToString();
        }

        public string Regexes { get { return "weather"; } }
    }
}