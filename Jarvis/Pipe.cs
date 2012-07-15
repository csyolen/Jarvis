using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Jarvis.Commands;
using Jarvis.Listeners;
using WolframAlpha.WrapperCore;

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

            foreach (var command in _commands)
            {
                var match = input.RegexMatch(command.Regexes);
                if(!match.Success)
                    continue;
                try
                {
                    var output = command.Handle(input, match, listener);
                    output = Brain.Settings.RegexFilters
                        .Aggregate(output, (current, regex) => 
                            current.RegexReplace(regex.Item1, regex.Item2));
                    listener.Output(output);
                }
                catch
                {
                    listener.Output("Error at " + command.GetType().Name);
                }
            }
        }
    }
}
