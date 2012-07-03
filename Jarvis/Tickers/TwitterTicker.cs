using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Jarvis.Objects;
using Jarvis.Runnables;
using Newtonsoft.Json.Linq;
using Twitterizer;

namespace Jarvis.Tickers
{
    class TwitterTicker : TickerBase
    {
        private readonly string _api;

        public TwitterTicker() : base(60000)
        {
            _api = "http://search.twitter.com/search.json?include_entities=true&q=";
            foreach (var twitter in Brain.Settings.Twitters)
            {
                _api += "from%3a{0}+OR+".Template(twitter);
            }
        }

        protected override void Tick()
        {
            IEnumerable<Tweet> json;
            try {json = JToken.Parse(new BrowserClient().DownloadString(_api))["results"]
                .Select(o => new Tweet(o));}
            catch { return;}
            var last = DateTime.Now.Subtract(1.Minutes());
            foreach (var tweet in json.Where(o => o.Time.IsFuture(last)))
            {
                Brain.ListenerManager.CurrentListener.Output(tweet.Text);
                if(tweet.Urls.Count > 0)
                    Brain.RunnableManager.Runnable = new ProcessRunnable(tweet.Urls[0]);
            }
            
        }

        private class Tweet
        {
            public string Text { get; private set; }
            public string From { get; private set; }
            public DateTime Time { get; private set; }
            public List<string> Urls { get; private set; }

            public Tweet(JToken token)
            {
                Text = token["text"].ToString();
                From = token["from_user"].ToString();
                Time = DateTime.ParseExact(token["created_at"].ToString(), "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture).ToLocalTime();
                Urls = token["entities"]["urls"].Select(o => o["url"].ToString()).ToList();
                foreach (var url in Urls)
                {
                    Text = Text.Replace(url, "");
                }
            }

        }
    }
}
