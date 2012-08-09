using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Jarvis.Listeners;

namespace Jarvis.Commands
{
    class VideoCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var q = match.Groups[1].Value.Trim();
            var search = "*{0}*".Template(q.RegexReplace(@"[\s]", "*"));
            var videos = Brain.Settings.Videos
                .SelectMany(d => d.GetFiles(search, SearchOption.AllDirectories)).ToList();
            Parse(q, videos, listener);
            yield return "";
        }

        private void Parse(string q, List<FileInfo> videos, IListener listener)
        {
            videos = videos.Where(o => o.Name.ToLower().Contains(q)).ToList();
            if(videos.Count > 1)
            {
                listener.Output("Which one of these should I play?");
                listener.Output(string.Join("\r\n", videos.Select(o => o.Name.ToLower())));
                Brain.Pipe.ListenNext((input, match, listener1) => Parse(input, videos, listener), "(.+)");
            }
            if (videos.Count == 0)
            {
                listener.Output("I could not find that video sir.");
            }
            if (videos.Count == 1)
            {
                var video = videos.First();
                Process.Start(video.FullName);
                var name = video.Name.Replace(".", " ").Trim().RegexRemove(video.Extension);
                name = name.RegexReplace(@"s(\d+)", "Season $1 ");
                name = name.RegexReplace(@"e(\d+)", "Episode $1 ");
                name = name.Trim().RemoveExtraSpaces();
                name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
                listener.Output("Playing " + name);
            }
        }

        public string Regexes { get { return "play(.*)"; } }
    }
}
