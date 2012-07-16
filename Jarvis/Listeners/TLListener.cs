﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;
using TLBot;

namespace Jarvis.Listeners
{
    class TLListener : ListenerBase
    {
        private static IrcClient _bot;
        private static HashSet<string> _shows;
        private static readonly FileInfo ShowsFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/TLBot/shows.txt");

        public TLListener(Pipe pipe)
            : base(pipe)
        {
            if (!ShowsFile.Exists)
            {
                Debug.Assert(ShowsFile.Directory != null, "_showsFile.Directory != null");
                ShowsFile.Directory.Create();
                ShowsFile.Create().Close();
            }

            _shows = new HashSet<string>(File.ReadAllLines(ShowsFile.FullName).Where(o => o.Trim().Length > 0));
        }

        public override void Loop()
        {
            while (true)
            {
                _bot = new IrcClient {ChannelSyncing = true, SendDelay = 200, AutoRetry = true, AutoReconnect = true};

                _bot.OnChannelMessage += ChannelMessage;

                _bot.Login("[Jarvis]", "[Jarvis]");
                _bot.Connect("irc.torrentleech.org", 7011);
                _bot.Join("#tlannounces");
                try
                {
                    _bot.Listen();
                }
                catch
                {
                }
            }
        }

        private void ChannelMessage(Data ircdata)
        {
            var announce = new Announce(ircdata.Message);

            if (announce.Category != "TV :: Episodes" || !_shows.Any(o => announce.Name.ToLower().Contains(o))) return;
            announce.Download();
            Output("Downloading " + announce.Name);
        }

        public override void RawOutput(string output)
        {
            Brain.ListenerManager.CurrentListener.Output(output);
        }
    }
}
