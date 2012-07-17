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
    }
}
