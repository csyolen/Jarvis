using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarvis.Views;

namespace Jarvis.Runnables
{
    class UrlRunnable : IRunnable
    {
        private readonly string _url;
        private const string YoutubeRegex = @"(?<=v=)[a-zA-Z0-9-_]+(?=&)|(?<=[0-9]/)[^&\n]+|(?<=v=)[^&\n]+";

        public UrlRunnable(string url)
        {
            _url = url.UrlDecode();
        }

        public void Run()
        {
            var match = _url.RegexMatch(YoutubeRegex);
            if(match.Success)
            {
                BrowserView.Create("http://youtube.googleapis.com/v/" + match.Value);
                return;
            }
            Process.Start(_url);
        }
    }
}
