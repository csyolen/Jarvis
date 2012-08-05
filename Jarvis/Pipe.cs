using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            _once = new List<DynamicCommand>();
            _next = new List<DynamicCommand>();
        }

        public void AddCommand(ICommand handler) 
        {
            _commands.Add(handler);
        }

        private class DynamicCommand
        {
            public Action<string, Match, IListener> Function {get; set; }
            public string[] Regexes { get; set; }
        }

        private List<DynamicCommand> _once;
        public void ListenOnce(Action<string, Match, IListener> function, params string[] regexes)
        {
            _once.Add(new DynamicCommand()
            {
                Function = function,
                Regexes = regexes.Select(o => o.ToLower()).ToArray()
            });
        }

        private List<DynamicCommand> _next;
        public void ListenNext(Action<string, Match, IListener> function, params string[] regexes)
        {
            _next.Add(new DynamicCommand()
            {
                Function = function,
                Regexes = regexes.Select(o => o.ToLower()).ToArray()
            });
        }

        public void Handle(string input, IListener listener)
        {
            if(input == null) return;
            input = input.ToLower();
            bool handled = false;

            foreach (var command in _commands)
            {
                var closure = command;
                new Thread(() =>
                    {
                        var match = input.RegexMatch(closure.Regexes);
                        if (!match.Success)
                            return;
                        try
                        {
                            var output = closure.Handle(input, match, listener);
                            foreach (var o in output)
                            {
                                listener.Output(o);
                            }
                        }
                        catch
                        {
                            listener.Output("Error at " + closure.GetType().Name);
                        }
                    }).Start();
            }

            foreach (var c in _once.ToArray())
            {
                foreach (var regex in c.Regexes)
                {
                    var match = input.RegexMatch(regex);
                    if(!match.Success)
                        continue;
                    c.Function(input, match, listener);
                    _once.Remove(c);
                    goto BreakOnceLoops;
                }
            }
            BreakOnceLoops:

            foreach (var c in _next.ToArray())
            {
                foreach (var regex in c.Regexes)
                {
                    var match = input.RegexMatch(regex);
                    if (!match.Success)
                        continue;
                    c.Function(input, match, listener);
                    _next.Remove(c);
                    break;
                }
            }
            _next.Clear();

        }
    }
}
