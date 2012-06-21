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
            Listeners = new List<IListener>();

            IRC = new IRCListener(pipe);
            IRC.Start();
            Listeners.Add(IRC);

            Console = new ConsoleListener(pipe);
            //Console.Start();
            Listeners.Add(Console);

            Voice = new VoiceListener(pipe);
            Voice.Start();
            Listeners.Add(Voice);

            new TLListener(pipe).Start();

            CurrentListener = Voice;
        }

        public IListener CurrentListener { get; private set; }

        public IRCListener IRC { get; private set; }
        public ConsoleListener Console { get; private set; }
        public VoiceListener Voice { get; private set; }

        public List<IListener> Listeners { get; private set; } 
    }
}
