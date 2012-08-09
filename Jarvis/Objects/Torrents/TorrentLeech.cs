using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Jarvis.Utilities;

namespace Jarvis.Objects.Torrents
{
    public class TorrentLeech
    {
        public static readonly TorrentLeechPage Movies = new TorrentLeechPage("http://www.torrentleech.org/torrents/browse/index/query/-pack+-collection/categories/10%2C11%2C14/orderby/leechers/order/desc");
        public static readonly TorrentLeechPage Home = new TorrentLeechPage("http://torrentleech.org/torrents/browse");

        private readonly BrowserClient _browserClient;

        public TorrentLeech()
        {
            _browserClient = new BrowserClient("torrentleech.org");
        }

        private List<TorrentLeechEntry> Fetch(string url)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(_browserClient.DownloadString(url));
            return doc.DocumentNode.SelectNodes("//*[@id='torrenttable']/tbody/tr").Select(o => new TorrentLeechEntry(o, _browserClient)).ToList();
        }

        public List<TorrentLeechEntry> GetEntries(TorrentLeechPage page)
        {
            return Fetch(page.Url);
        }
    }

    public class TorrentLeechPage
    {
        public string Url { get; private set; }

        public TorrentLeechPage(string url)
        {
            Url = url;
        }
    }

    public class TorrentLeechEntry
    {
        private readonly BrowserClient _client;

        public TorrentLeechEntry(HtmlNode node, BrowserClient client)
        {
            _client = client;
            var titleNode = node.SelectSingleNode(".//span[@class='title']/a");
            Title = titleNode.InnerText;
            Id = int.Parse(titleNode.Attributes["href"].Value.RegexMatch(@"\d+").Value);
            Friendly = Title.TorrentName();
            var size = node.SelectSingleNode(".//td[5]").InnerText;
            double number = double.Parse(size.RegexMatch(@"\d+").Value);
            if (size.Contains("GB"))
                number *= 1024;
            Size = number;
            Torrent = "http://www.torrentleech.org/rss/download/{0}/ed2597d8977cde9da218/{1}".Template(Id,Title.RegexReplace(@"\s", "."));
        }
        
        public string Title { get; private set; }
        public int Id { get; private set; }
        public string Friendly { get; private set; }
        public string Torrent { get; private set; }
        public double Size { get; private set; }

        public bool Download(bool force=false)
        {
            var path = Title.RegexReplace(@"\s", ".") + ".torrent";
            path = Path.Combine(Constants.TorrentDirectory.FullName, path);
            if(!force && File.Exists(path))
                return false;
            if(force)
                File.Delete(path);
            new BrowserClient().DownloadFile(Torrent, path);
            Process.Start(path);
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TorrentLeechEntry) obj);
        }

        protected bool Equals(TorrentLeechEntry other)
        {
            return string.Equals(Friendly, other.Friendly);
        }

        public override int GetHashCode()
        {
            return (Friendly != null ? Friendly.GetHashCode() : 0);
        }
    }
}
