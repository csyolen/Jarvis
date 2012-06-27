using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Videos = _json["Videos"].Select(o => new DirectoryInfo(o.ToString())).ToList();
        }

        public List<string> Twitters { get; private set; }
        public List<DirectoryInfo> Videos { get; private set; }
    }
}
