using System.Text.RegularExpressions;
using Jarvis.Locale;

namespace Jarvis.Commands
{
    class RunnableCommand : ICommand
    {
        public string Handle(string input, Match match)
        {
            return Brain.RunnableManager.Run() ? Speech.Open.Parse() : "";
        }

        public string Regexes
        {
            get { return @"\bmore\b|\brun\b|\bopen\b"; }
        }
    }
}
