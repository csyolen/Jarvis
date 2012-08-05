using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Jarvis.Commands;
using Jarvis.Listeners;
namespace Jarvis
{
    public delegate void Command(string input, Match match, IListener listener);

    public class Pipe
    {
        private readonly HashSet<ICommand> _commands;

        public Pipe()
        {
            _commands = new HashSet<ICommand>();

            _permanent = new List<DynamicCommand>();
            _once = new List<DynamicCommand>();
            Commands.Commands.Register(this);
        }

        private class DynamicCommand
        {
            public Command Function {get; set; }
            public List<string> Regexes { get; set; }

            public bool Execute(string input, IListener listener)
            {
                foreach (var regex in Regexes)
                {
                    var match = input.RegexMatch(regex);
                    if (!match.Success)
                        continue;
                    try
                    {
                        Function(input, match, listener);
                    }
                    catch(Exception e)
                    {
                        listener.Output("Error: " + e.Message);
                    }
                    return true;
                }
                return false;
            }
        }

        private readonly List<DynamicCommand> _permanent;
        public void Listen(Command function, string regex, params string[] regexes)
        {
            var cmd = new DynamicCommand()
            {
                Function = function,
                Regexes = regexes.Select(o => o.ToLower()).ToList()
            };
            cmd.Regexes.Add(regex);
            _permanent.Add(cmd);
        }

        private readonly List<DynamicCommand> _once;
        public void ListenOnce(Command function, string regex, params string[] regexes)
        {
            var cmd = new DynamicCommand()
            {
                Function = function,
                Regexes = regexes.Select(o => o.ToLower()).ToList()
            };
            cmd.Regexes.Add(regex);
            _once.Add(cmd);
        }

        private DynamicCommand _next;
        public void ListenNext(Command function, string regex, params string[] regexes)
        {
            var cmd = new DynamicCommand()
                {
                    Function = function,
                    Regexes = regexes.Select(o => o.ToLower()).ToList()
                };
            cmd.Regexes.Add(regex);
            _next = cmd;
        }

        public void Handle(string input, IListener listener)
        {
            if(input == null) return;
            input = input.ToLower();

            foreach (var c in _permanent.ToArray())
            {
                if(c.Execute(input, listener))
                    break;
            }

            foreach (var c in _once.ToArray())
            {
                if(c.Execute(input, listener))
                {
                    _once.Remove(c);
                    break;
                }
            }

            if(_next != null)
                _next.Execute(input, listener);

            _next = null;

        }
    }
}
