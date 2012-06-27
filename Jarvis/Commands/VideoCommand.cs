using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jarvis.Commands
{
    class VideoCommand : ICommand
    {
        public string Handle(string input, Match match)
        {
            var q = match.Groups[1].Value.Trim();
            q = "*{0}*".Template(q.RegexReplace(@"[\s]", "*"));
            var videos = Brain.Settings.Videos
                .SelectMany(d => d.GetFiles(q, SearchOption.AllDirectories)).ToList();
            var video = videos
                .OrderByDescending(o => o.CreationTime)
                .FirstOrDefault();
            if(video == null)
                return "I could not find that video sir.";
            Process.Start(video.FullName);
            return "Playing " + video.Name.Replace(".", " ").Trim().RegexRemove(video.Extension);
        }

        public string Regexes { get { return "play(.+)"; } }
    }
}
