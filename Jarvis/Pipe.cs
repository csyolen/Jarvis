using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Jarvis.Listeners;

namespace Jarvis
{
    public class Pipe
    {
        private readonly HashSet<IHandler> _handlers;

        public Pipe()
        {
            _handlers = new HashSet<IHandler>();
        }

        public void AddHandler(IHandler handler) 
        {
            _handlers.Add(handler);
        }


        public void Handle(string input, IListener listener)
        {
            if(input == null) return;
            input = input.ToLower();
            bool handled = false;
            foreach (var output in _handlers
                .Where(o => input.IsRegexMatch(o.Regexes))
                .Select(handler => handler.Handle(input, input.RegexMatch(handler.Regexes))))
            {
                handled = true;
                listener.Output(output);
                Brain.ListenerManager.CurrentListener.Output(output);
            }
            if (handled || !Brain.Think) return;
            var chat = Brain.Chat(input);
            Brain.ListenerManager.CurrentListener.Output(chat);
            listener.Output(chat);
        }
    }
}
