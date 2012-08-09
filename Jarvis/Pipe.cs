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

        private class DynamicCommand : ICloneable
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
                    new Thread(() =>
                        {
                            try
                            {
                                Function(input, match, listener);
                            }
                            catch (Exception e)
                            {
                                listener.Output("Error: " + e.Message);
                            }
                        }).Start();
                    return true;
                }
                return false;
            }

            public object Clone()
            {
                return new DynamicCommand()
                    {
                        Function = Function,
                        Regexes = Regexes
                    };
            }
        }

        private readonly List<DynamicCommand> _permanent;
        private readonly List<DynamicCommand> _once;
        private DynamicCommand _next;
        private void Register(List<DynamicCommand> list, Command function, string regex, params string[] regexes)
        {
            var cmd = new DynamicCommand()
            {
                Function = function,
                Regexes = regexes.Select(o => o.ToLower()).ToList()
            };
            cmd.Regexes.Add(regex);
            foreach (var command in list.ToList())
            {
                command.Regexes = command.Regexes.Where(o => !cmd.Regexes.Contains(o)).ToList();
                if (command.Regexes.Count == 0)
                    list.Remove(command);
            }
            list.Add(cmd);
        }
        public void Listen(Command function, string regex, params string[] regexes)
        {
            Register(_permanent, function, regex, regexes);
        }

        public void ListenOnce(Command function, string regex, params string[] regexes)
        {
            Register(_once, function, regex, regexes);
        }

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

            if (_next != null)
            {
                var clone = (DynamicCommand) _next.Clone();
                _next = null;
                if(clone.Execute(input, listener))
                    return;
            }
            _next = null;
            foreach (var c in _permanent.ToArray())
            {
                if(c.Execute(input, listener))
                    break;
            }

            foreach (var c in _once.ToArray())
            {
                if(c.Execute(input, listener))
                    _once.Remove(c);
                
            }
        }
    }
}
