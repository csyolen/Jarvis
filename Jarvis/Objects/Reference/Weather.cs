using System;
using System.Net;
using System.Xml;
using Jarvis.Locale;

namespace Jarvis.Objects.Reference
{
    class Weather
    {
        private XmlNode _current;
        public string Link { get; private set; }

        public Weather()
            :this("08536")
        {
            
        }

        public Weather(object location)
        {
            var url = "http://www.google.com/ig/api?weather=" + location;
            Link = "http://www.weather.com/search/enhancedlocalsearch?where={0}&loctypes=1003%2C1001%2C1000%2C1%2C9%2C5%2C11%2C13%2C19%2C20&from=hdr_localsearch".Template(location);
            var doc = new XmlDocument();
            doc.LoadXml(new WebClient().DownloadString(url));
            _current = doc.SelectSingleNode("//current_conditions");
        }

        public int Temperature
        {
            get
            {
                return int.Parse(_current.SelectSingleNode("temp_f").Attributes["data"].Value);
            }
        }

        public string Condition
        {
            get
            {
                return _current.SelectSingleNode("condition").Attributes["data"].Value;
            }
        }

        public override string ToString()
        {
            var temp = Temperature;
            var condition = Condition;
            var more = temp > 50 ? "" : Speech.Jacket.Parse();

            return Speech.Weather.Parse(condition.ToLower(), temp) + " " + more;
        
        }
    }
}
