using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Utilities
{
    static class StringExtensions
    {
        public static string TorrentName(this string name)
        {
            var input = name.Substring(0, name.LastIndexOf('.'));
            var splits = input.RegexSplit(@"[\.\s]");
            var output = "";
            foreach (var split in splits)
            {
                if (split.ToLower().IsRegexMatch(@"(-|dvdrip|720p|internal|x264|hdtv|\d\d\d\d)")) break;
                output += split.UppercaseFirst() + " ";
            }
            output = output.Trim();
            return output;
        }

        public static DateTime ParseNaturalDateTime(this string input)
        {
            var now = DateTime.Now;
            input = input.ToLower().RegexRemove(@"^(in)\s+");
            if(input == "tomorrow")
            {
                var output = now.AddDays(1);
                output = new DateTime(output.Year, output.Month, output.Day, 9,0,0);
                return output;
            }

            var match = input.RegexMatch(@"(\d+) (second|minute|hour|day|week)[s]*");
            if(match.Success)
            {
                var i = int.Parse(match.Groups[1].Value);
                var unit = match.Groups[2].Value;
                switch (unit)
                {
                    case "second":
                        return now.Add(i.Seconds());
                    case "minute":
                        return now.Add(i.Minutes());
                    case "hour":
                        return now.Add(i.Hours());
                    case "day":
                        return now.Add((i * 24).Hours());
                    case "week":
                        return now.Add((i * 24 * 7).Hours());
                }
            }

            return now;
        }
    }
}
