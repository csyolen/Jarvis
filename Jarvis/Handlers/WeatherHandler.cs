using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Jarvis.Locale;
using Jarvis.Objects;
using Jarvis.Objects.Reference;
using Jarvis.Runnables;

namespace Jarvis.Handlers
{
    class WeatherHandler : IHandler
    {
        public string Handle(string input, Match match)
        {
            var weather = new Weather();
            Brain.RunnableManager.Runnable = new ProcessRunnable(weather.Link);
            return weather.ToString();
        }

        public string Regexes { get { return "weather"; } }
    }
}