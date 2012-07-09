using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarvis.Locale;
using Jarvis.Utilities;
using Limilabs.Client.IMAP;
using Limilabs.Mail;

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
            while (true)
            {
                try
                {
                    using (Imap client = new Imap())
                    {
                        client.ConnectSSL("imap.gmail.com");
                        client.Login(_account.Email, _account.Password);
                        client.SelectInbox();

                        while (true)
                        {
                            FolderStatus currentStatus = client.Idle();
                            foreach (long uid in client.SearchFlag(Flag.Unseen))
                            {
                                IMail email = new MailBuilder().CreateFromEml(
                                    client.GetHeadersByUID(uid));
                                Output(Speech.Email.Parse(email.From[0].Name, email.Subject));
                            }
                        }
                        client.Close();
                    }
                }
                catch {}
            }
        }

        public override void Output(string output)
        {
            Brain.ListenerManager.CurrentListener.Output(output);
        }
    }
}
