﻿using System;
using System.Collections.Specialized;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;

namespace Jarvis.Objects
{
    public class BrowserClient : WebClient
    {
    
        public BrowserClient()
        {
            Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            Headers[HttpRequestHeader.AcceptLanguage] = "en-US,en;q=0.8";
            Headers[HttpRequestHeader.CacheControl] = "max-age=0";
            Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.52 Safari/536.5";
            Timeout = 60000;
        }

        public BrowserClient(string host)
            :this()
        {
            var result = "";
            var splits = host.Split('.');
            var domain = host;
            if (splits.Count() == 3)
                domain = "." + splits[1] + "." + splits[2];
            try
            {
                var strPath = GetChromeCookiePath();
                var strDb = "Data Source=" + strPath + ";pooling=false";

                using (var conn = new SQLiteConnection(strDb))
                {
                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT name || '=' || value || ';' FROM cookies WHERE host_key = '{0}' OR host_key = '{1}';".Template(host, domain);

                        conn.Open();
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result += reader.GetString(0);
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception)
            {
            }
            this.Headers[HttpRequestHeader.Cookie] = result;
        }

        public string PostData(string url, object data)
        {
            var fields = data.GetType().GetFields();
            var collection = new NameValueCollection();
            foreach (var field in fields)
            {
                collection.Add(field.Name, field.GetValue(null).ToString());
            }
            var result = this.UploadValues(url, collection);
            return Encoding.GetString(result);
        }

        private static string GetChromeCookiePath()
        {
            string s = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            s += @"\Google\Chrome\User Data\Default\cookies";

            if (!File.Exists(s))
                return string.Empty;

            return s;
        }

        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var result = base.GetWebRequest(address);
            result.Timeout = Timeout;
            return result;
        }
    }
}
