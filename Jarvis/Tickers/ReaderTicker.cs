using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Badger.Api;

namespace Jarvis.Tickers
{
    class ReaderPoller : TickerBase
    {
        private string _token;
        private Cookie _cookie;
        private string _sid;
        private string _auth;
        private BadgerClient _badger;

        public ReaderPoller() : base(60.Seconds())
        {
            Connect();
            _badger = new BadgerClient("Reader");
            _badger.SetColor("#07AD07");
        }


        private bool Connect()
        {
            GetToken();
            return _token != null;
        }

        private void GetToken()
        {
            GetSid();
            _cookie = new Cookie("SID", _sid, "/", ".google.com");

            string url = "http://www.google.com/reader/api/0/token";

            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(_cookie);
            req.Headers.Add("Authorization", "GoogleLogin auth=" + _auth);

            var response = (HttpWebResponse)req.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                var r = new StreamReader(stream);
                _token = r.ReadToEnd();
            }
        }

        private void GetSid()
        {
            string requestUrl = string.Format
                ("https://www.google.com/accounts/ClientLogin?service=reader&Email={0}&Passwd={1}",
                 Brain.Settings.EmailAccounts[0].Email, Brain.Settings.EmailAccounts[0].Password);
            var req = (HttpWebRequest)WebRequest.Create(requestUrl);
            req.Method = "GET";

            var response = (HttpWebResponse)req.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                var r = new StreamReader(stream);
                string resp = r.ReadToEnd();

                foreach (string cLine in resp.Split('\n'))
                {
                    if (cLine.StartsWith("SID="))
                    {
                        _sid = cLine.Replace("SID=", "");
                    }
                    //if (cLine.StartsWith("LSID=")) { cLSID = cLine.Replace("LSID=", ""); } 
                    if (cLine.StartsWith("Auth="))
                    {
                        _auth = cLine.Replace("Auth=", "");
                    }
                }
            }
        }

        private HttpWebResponse HttpGet(string requestUrl, string getArgs)
        {
            string url = string.Format("{0}?{1}", requestUrl, getArgs);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(_cookie);
            request.Headers.Add("Authorization", "GoogleLogin auth=" + _auth);

            try
            {
                return (HttpWebResponse)request.GetResponse();
            }
            catch
            {
                // handle error 
                return null;
            }
        }

        public int GetUnreadCount(out string details, out int numberOfFeed)
        {
            var r =
                new StreamReader(
                    HttpGet("http://www.google.com/reader/api/0/unread-count", "all=true&output=xml").GetResponseStream());
            string c = r.ReadToEnd();
            r.Close();

            var d = new XmlDocument();
            d.LoadXml(c);

            details = string.Empty;
            int count = d.DocumentElement.SelectNodes("/object/list/object").Count;
            if (count > 0)
                count = count - 1;
            numberOfFeed = count;

            foreach (XmlElement e in d.DocumentElement.SelectNodes("/object/list/object"))
            {
                var nameNode = (XmlElement)e.SelectSingleNode("string[@name='id']");

                if (nameNode.InnerText.Contains("reading-list"))
                {
                    return int.Parse(e.SelectSingleNode("number[@name='count']").InnerText);
                }
                else
                {
                    details += e.SelectSingleNode("number[@name='count']").InnerText + " @ " +
                               e.SelectSingleNode("string[@name='id']").InnerText + Environment.NewLine;
                }

            }
            return 0;
        }

        private int _last;

        protected override void Tick()
        {
            int numberOfFeed;
            string details;
            int unread = GetUnreadCount(out details, out numberOfFeed);
            _badger.Set(unread);
            if(unread - _last > 15)
            {
                Brain.ListenerManager.CurrentListener.Output("Sir you have " + unread + " unread items in Google Reader.");
                Brain.Pipe.ListenOnce((input, match, listener) => Process.Start("https://www.google.com/reader/"), "more|open|show");
                _last = unread;
            } 
            if(unread < _last)
            {
                _last = 0;
            }
                
        }
    }
}
