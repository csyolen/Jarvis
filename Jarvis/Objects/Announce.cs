using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TLBot
{
    class Announce
    {
        private static WebClient _wc;

        static Announce()
        {
            _wc = new WebClient();
            _wc.Headers[HttpRequestHeader.Cookie] = "member_id=53563; tluid=522483; pass_hash=ef5be2fa3121fe9947bbbf247bddbc99;";
        }

        public Announce(string input)
        {
            Name = input.RegexMatch(@"Name:'([^']+)").Groups[1].Value.Trim();
            Link = input.RegexMatch(@"http.*").Value.Trim();
            Category = input.RegexMatch(@"<([^>]+)>").Groups[1].Value.Trim();

        }

        public string Name { get; private set; }
        public string Link { get; private set; }
        public string Category { get; private set; }

        public void Download()
        {
            Console.WriteLine("Downloading {0}", Name);
            new Thread(() =>
                           {
                               Thread.Sleep(60000);
                               var doc = _wc.DownloadString(Link);
                               var url = "http://torrentleech.org" +
                                         Regex.Match(doc, "\"(.+\\.torrent)\"").Groups[1].Value;
                               var path = Name + ".torrent";
                               _wc.DownloadFile(url, path);
                               Process.Start(path);
                           }).Start();
        }
    }
}
