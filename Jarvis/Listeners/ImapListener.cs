using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImapX;
using Jarvis.Locale;
using Jarvis.Utilities;

namespace Jarvis.Listeners
{
    class ImapListener : ListenerBase
    {
        private readonly EmailAccount _account;

        public ImapListener(Pipe pipe, EmailAccount account) : base(pipe)
        {
            _account = account;
        }

        public override void Loop()
        {
            /*
            var client = new ImapClient("imap.gmail.com", 993, true);
            client.LogIn(_account.Email, _account.Password);
            var inbox = client.Folders["INBOX"];
            inbox.Select();
            inbox.
            client.SuscribeMailbox("INBOX");
            client.NewMessage += (sender, args) =>
                {
                    var i = args.MessageCount;
                    var msgs = client.GetMessages(0, 1000);
                    foreach (var message in msgs)
                    {
                        Output(Speech.Email.Parse(message.From.DisplayName, message.Subject));
                    }
                };
             */
        }

        public override void RawOutput(string output)
        {
            Brain.ListenerManager.CurrentListener.Output(output);
        }
    }
}
