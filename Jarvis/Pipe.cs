using System;
using System.Collections.Generic;
using System.Threading;
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

            foreach (var command in _commands)
            {
                new Thread(() =>
                    {
                        var match = input.RegexMatch(command.Regexes);
                        if (!match.Success)
                            return;
                        try
                        {
                            var output = command.Handle(input, match, listener);
                            listener.Output(output);
                        }
                        catch
                        {
                            listener.Output("Error at " + command.GetType().Name);
                        }
                    }).Start();
            }
        }
    }
}
