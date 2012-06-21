using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AIMLbot;
using Jarvis.Handlers;
using Jarvis.Listeners;
using Jarvis.Locale;
using Jarvis.Runnables;
using Jarvis.Tickers;

namespace Jarvis
{
    public class Brain
    {
        public static ListenerManager ListenerManager { get; private set; }
        public static RunnableManager RunnableManager { get; private set; }
        

        private static Pipe Pipe { get; set; }
        private static readonly Bot Bot = new Bot();
        private static readonly User User = new User("Jarvis", Bot); 
  
        public static void Start()
        {
            Pipe = new Pipe();

            //Bot.loadSettings();
            //Bot.loadAIMLFromFiles();
            
            //Listeners
            ListenerManager = new ListenerManager(Pipe);

            //Runnables
            RunnableManager = new RunnableManager();

            //Handlers
            typeof(IHandler).Assembly
                .GetTypes()
                .Where(o => o.GetInterface(typeof(IHandler).FullName) != null && o.IsClass)
                .Select(source => (IHandler)Activator.CreateInstance(source)).ToList()
                .ForEach(o => Pipe.AddHandler(o));

            //Tickers 
            typeof(TickerBase).Assembly
                .GetTypes()
                .Where(o => o.BaseType == typeof (TickerBase))
                .Select(source => (TickerBase) Activator.CreateInstance(source)).ToList()
                .ForEach(o => o.Start());

        }

        public static string Chat(string input)
        {
            var request = new Request(input, User, Bot);
            return Bot.Chat(request).Output;
        }

        public static bool Think { get; set; }
    }
}
