using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Jarvis.Commands;
using Jarvis.Listeners;

namespace Jarvis
{
    public class Pipe
    {
        private readonly HashSet<ICommand> _commands;

        public Pipe()
        {
            _commands = new HashSet<ICommand>();
        }

        public void AddCommand(ICommand handler) 
        {
            _commands.Add(handler);
        }


        public void Handle(string input, IListener listener)
        {
            if(input == null) return;
            input = input.ToLower();
            bool handled = false;
            foreach (var output in _commands
                .Where(o => input.IsRegexMatch(o.Regexes))
                .Select(handler => handler.Handle(input, input.RegexMatch(handler.Regexes))))
            {
                handled = true;
                listener.Output(output);
            }
            if (handled || !Brain.Think) return;
            var chat = Brain.Chat(input);
            listener.Output(chat);
        }
    }
}
