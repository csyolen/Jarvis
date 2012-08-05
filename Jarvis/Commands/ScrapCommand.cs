using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Objects;

namespace Jarvis.Commands
{
    public class ScrapCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var gv = GoogleVoice.Inbox();
            foreach (var phoneCall in gv.Conversations_response.Conversation.SelectMany(conversation => conversation.Phone_call).Where(o => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)o.Start_time) > DateTime.Now.Subtract(1.Hours())))
            {
                yield return phoneCall.Contact.Name + ": " + phoneCall.Message_text;
            }
            yield return "";
        }

        public string Regexes { get { return "scrap"; } }
    }
}
