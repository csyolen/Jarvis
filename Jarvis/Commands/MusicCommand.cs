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
                    yield return "Would you like me to play something specific or just continue?";
                    Brain.Pipe.ListenNext((s, match1, listener1) =>
                        {
                            if (s == "continue")
                            {
                                listener1.Output("Playing " + itunes.CurrentTrack.Name + " by " + itunes.CurrentTrack.Artist);
                                itunes.Play();
                            }

                            foreach (IITPlaylist playlist in itunes.LibrarySource.Playlists)
                            {
                                if (!playlist.Name.ToLower().Contains(s)) continue;
                                playlist.PlayFirstTrack();
                                listener1.Output("Playing playlist " + playlist.Name);
                                return;
                            }

                            foreach (IITTrack track in itunes.LibraryPlaylist.Tracks)
                            {
                                if (!track.Name.ToLower().Contains(s) && !track.Artist.ToLower().Contains(s)) continue;
                                track.Play();
                                listener1.Output("Playing " + track.Name + " by " + track.Artist);
                                return;
                            }
                        }, ".+");
                        break;
            }
            yield return "";
        }

        public string Regexes { get { return "music"; } }
    }
}
