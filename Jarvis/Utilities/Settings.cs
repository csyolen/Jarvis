using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Jarvis.Utilities
{
    public class Settings
    {
        private JToken _json;

        public Settings()
        {
            _json = JToken.Parse(File.ReadAllText(Constants.SettingsFile.FullName));
            Twitters = _json["Twitters"].Select(o => o.ToString()).ToList();
            RSS = _json["RSS"].Select(o => o.ToString()).ToList();
            Videos = _json["Videos"].Select(o => new DirectoryInfo(o.ToString())).ToList();

            //Emails
            EmailAccounts = new List<EmailAccount>();
            foreach (var account in _json["Emails"])
            {
                EmailAccounts.Add(new EmailAccount(account["Email"].ToString(), account["Password"].ToString()));
            }

            //Alarms
            Wake = DateTime.ParseExact(_json["Wake"].ToString(), "HH:mm", CultureInfo.CurrentCulture).TimeOfDay;
            Sleep = DateTime.ParseExact(_json["Sleep"].ToString(), "HH:mm", CultureInfo.CurrentCulture).TimeOfDay;

            //RegexFilters
            RegexFilters = new List<Tuple<string, string>>();
            foreach (string s in _json["RegexFilters"])
            {
                var splits = s.Split('=');
                RegexFilters.Add(new Tuple<string, string>(splits[0], splits[1]));
            }

            Shows = new HashSet<string>();
        }

        public List<string> Twitters { get; private set; }
        public List<string> RSS { get; private set; }
        public List<Tuple<string, string>>  RegexFilters { get; private set; }
        public List<DirectoryInfo> Videos { get; private set; }
        public List<EmailAccount> EmailAccounts { get; private set; }
        public TimeSpan Wake { get; private set; }
        public TimeSpan Sleep { get; private set; }
        public HashSet<string> Shows { get; set; } 
    }

    public class EmailAccount
    {
        public string Email { get; private set; }
        public string Password { get; private set; }

        public EmailAccount(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
