﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public List<TorrentLeechEntry> Movies()
        {
            const string url = "http://www.torrentleech.org/torrents/browse/index/query/-pack+-collection/categories/10%2C11%2C14/orderby/leechers/order/desc";
            return Fetch(url);
        }
    }

    public class TorrentLeechEntry
    {
        private readonly BrowserClient _client;

        public TorrentLeechEntry(HtmlNode node, BrowserClient client)
        {
            _client = client;
            Title = node.SelectSingleNode(".//span[@class='title']/a").InnerText;
            Friendly = Title.TorrentName();
            var size = node.SelectSingleNode(".//td[5]").InnerText;
            double number = double.Parse(size.RegexMatch(@"\d+").Value);
            if (size.Contains("GB"))
                number *= 1024;
            Size = number;
            Torrent = "http://torrentleech.org" + node.SelectSingleNode(".//td[@class='quickdownload']/a").Attributes["href"].Value;
        }
        
        public string Title { get; private set; }
        public string Friendly { get; private set; }
        public string Torrent { get; private set; }
        public double Size { get; private set; }

        public void Download()
        {
            var path = Friendly + ".torrent";
            _client.DownloadFile(Torrent, path);
            Process.Start(path);
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