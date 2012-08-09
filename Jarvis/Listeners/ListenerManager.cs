using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Listeners
{
    public class ListenerManager
    {
        public ListenerManager(Pipe pipe)
        {
            IRC = new IRCListener(pipe);
            IRC.Start();

            Console = new ConsoleListener(pipe);
            //Console.Start();

            Voice = new VoiceListener(pipe);
            Voice.Start();

            //new SocketListener(pipe).Start();

            new FileWatcherListener(pipe).Start();

            foreach (var account in Brain.Settings.EmailAccounts)
            {
                new ImapListener(pipe, account).Start();
            }

            CurrentListener = IRC;
        }

        public IListener CurrentListener { get; private set; }

        public IRCListener IRC { get; private set; }
        public ConsoleListener Console { get; private set; }
        public VoiceListener Voice { get; private set; }
    }
}
