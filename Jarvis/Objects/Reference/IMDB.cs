using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AIMLbot;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace Jarvis.Objects.Reference
{
    class IMDB
    {
        private readonly string _description;

        public IMDB(string query, bool api)
        {
            var url = "http://www.imdbapi.com/?t=" + query;
            var json = JToken.Parse(new BrowserClient().DownloadString(url));
            Description = json["Plot"].ToString();
            float r = 0;
            float.TryParse(json["imdbRating"].ToString(), out r);
            Rating = r;
            Title = json["Title"].ToString();
        }

        public IMDB(string url)
        {
            var html = new BrowserClient().DownloadString(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            _description = doc.DocumentNode.SelectSingleNode("//p[@itemprop='description']").InnerText;
            Description = _description;
            Rating = float.Parse(doc.DocumentNode.SelectSingleNode("//span[@itemprop='ratingValue']").InnerText);
        }

        public string Description { get; private set; }
        public float Rating { get; private set; }
        public string Title { get; private set; }

        public override string ToString()
        {
            return _description;
        }
    }
}
