using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using Jarvis.Locale;
using Jarvis.Objects;

namespace Jarvis.Tickers
{
    class GmailTicker : TickerBase
    {
        private const string Url = "https://mail.google.com/mail/feed/atom";

        public GmailTicker() : base(1.Minutes())
        {
        }

        protected override void Tick()
        {
            foreach (var account in Brain.Settings.EmailAccounts)
            {
                var doc = GetAtom(account.Email, account.Password);
                var last = DateTime.Now.Subtract(1.Minutes());
                foreach (
                    var entry in
                        from XmlNode node in doc.DocumentElement.SelectNodes("/feed/entry") select new AtomEntry(node))
                {
                    if (entry.Issued < last)
                        continue;
                    Brain.ListenerManager.CurrentListener.Output(Speech.Email.Parse(entry.Author.Name, entry.Title));
                    Brain.Pipe.ListenOnce((s, match, arg3) => Process.Start(entry.Link), "open|more|show");
                }
            }
        }

        public XmlDocument GetAtom(string username, string password)
        {
            byte[] buffer = new byte[8192];
            int byteCount = 0;
            XmlDocument feedXml = null;
            try
            {
                var sBuilder = new StringBuilder();
                var webRequest = WebRequest.Create(Url);

                webRequest.PreAuthenticate = true;

                var credentials = new NetworkCredential(username, password);
                webRequest.Credentials = credentials;

                var webResponse = webRequest.GetResponse();
                var stream = webResponse.GetResponseStream();

                while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                    sBuilder.Append(Encoding.ASCII.GetString(buffer, 0, byteCount));


                feedXml = new XmlDocument();
                feedXml.LoadXml(sBuilder.ToString().Replace("<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">", "<feed>"));


            }
            catch (Exception ex)
            {
            }
            return feedXml;
        }
    }
}
