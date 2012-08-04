using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Jarvis.Listeners;
using iTunesLib;

namespace Jarvis.Commands
{
    class MusicCommand : ICommand
    {
        public MusicCommand()
        {
        }

        public IEnumerable<string> Handle(string input, Match match, IListener listener)
        {
            var itunes = new iTunesAppClass();
            switch (itunes.PlayerState)
            {
                case ITPlayerState.ITPlayerStatePlaying:
                    itunes.Stop();
                    yield return "Music paused.";
                    break;
                case ITPlayerState.ITPlayerStateStopped:
                    itunes.Play();
                    itunes.PlayerPosition += 30;
                    yield return "Playing music.";
                    break;
            }
            yield return "";
        }

        public string Regexes { get { return "music"; } }
    }
}
