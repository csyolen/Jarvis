using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Jarvis.Objects;
using Jarvis.Runnables;
using Jarvis.Views;
using Newtonsoft.Json.Linq;

namespace Jarvis.Tickers
{
    class TwitterTicker : TickerBase
    {

        public TwitterTicker() : base(60000)
        {
        }

        protected override void Tick()
        {
            var last = DateTime.Now.Subtract(1.Minutes());
            var tweets = TwitterSearch.FromUsers(Brain.Settings.Twitters.ToArray());
            foreach (var tweet in tweets.Results.Where(o => o.Time > last))
            {
                if (tweet.Entities != null && tweet.Entities.TwitterEntityUrls != null)
                {
                    Brain.RunnableManager.Runnable = new ProcessRunnable(tweet.Entities.TwitterEntityUrls.First().Url);
                    foreach (var twitterEntityUrl in tweet.Entities.TwitterEntityUrls)
                    {
                        tweet.Text = tweet.Text.Replace(twitterEntityUrl.Url, "");
                    }
                }
                TweetView.Create(tweet.Text, tweet.From_user); 
                Brain.ListenerManager.CurrentListener.Output("{0}: {1}".Template(tweet.From_user_name, tweet.Text));
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
