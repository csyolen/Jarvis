using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jarvis.Listeners;
using Jarvis.Locale;

namespace Jarvis.Commands
{
    class RunnableCommand : ICommand
    {
        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            yield return Brain.RunnableManager.Run() ? Speech.Yes.Parse() : "";
        }

        public string Regexes
        {
            get { return @"\b(more|run|open)\s*(.*)"; }
        }
    }
}
