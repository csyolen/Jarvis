using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace Jarvis.Objects.Torrents
{
    public class Announce
    {
        private static WebClient _wc;

        static Announce()
        {
            _wc = new BrowserClient();
            _wc.Headers[HttpRequestHeader.Cookie] = "itemMarking_forums_items=eJxLtDK0qs60MjI2MjOyzrQyNDYxMDIyMDIzs64FXDBhAga0; member_id=53563; pass_hash=ef5be2fa3121fe9947bbbf247bddbc99; tluid=522483; tlpass=997ac112d4b8787eaadff2326ff8fad12eac71b4; PHPSESSID=5plugo9jm63m791413humut3r6; lastBrowse1=1342406478; lastBrowse2=1342407091; __utma=194598568.1583004845.1334096772.1342400621.1342404444.404; __utmb=194598568.17.10.1342404444; __utmc=194598568; __utmz=194598568.1334096772.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)";
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

        public string Download()
        {
            Console.WriteLine("Downloading {0}", Name);
            Thread.Sleep(60000);
            var doc = _wc.DownloadString(Link);
            var url = "http://torrentleech.org" +
                      Regex.Match(doc, "\"(.+\\.torrent)\"").Groups[1].Value;
            var path = Constants.TorrentDirectory + "/" + Name + ".torrent";
            _wc.DownloadFile(url, path);
            return path;
        }
    }
}
