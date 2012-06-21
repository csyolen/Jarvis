using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AIMLbot;
using HtmlAgilityPack;

namespace Jarvis.Objects.Reference
{
    class IMDB
    {
        private readonly string _description;

        public IMDB(string url)
        {
            var html = new BrowserClient().DownloadString(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            _description = doc.DocumentNode.SelectSingleNode("//p[@itemprop='description']").InnerText;
        
        }

        public override string ToString()
        {
            return _description;
        }
    }
}
