using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Jarvis.Objects.Torrents;
using Jarvis.Utilities;
using Meebey.SmartIrc4net;
using MonoTorrent.Client;
using MonoTorrent.Common;

namespace Jarvis.Listeners
{
    public class TLListener : ListenerBase
    {
        private static IrcClient _bot;
        private static HashSet<string> _shows;
        private static readonly FileInfo ShowsFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/TLBot/shows.txt");

        public ClientEngine Engine { get; private set; }
        public List<TorrentManager> Torrents { get; private set; } 

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

            Engine = new ClientEngine(new EngineSettings());
            Torrents = new List<TorrentManager>();
            Engine.TorrentRegistered += (sender, args) => Torrents.Add(args.TorrentManager);

            foreach (var fileInfo in Constants.TorrentDirectory.GetFiles())
            {
                DownloadTorrent(fileInfo.FullName);
            }
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

            new Thread(() =>
                {
                    var path = announce.Download();
                    DownloadTorrent(path);
                    Output("Downloading " + announce.Name.TorrentName());

                }).Start();
        }

        private void DownloadTorrent(string path)
        {
            var torrent = Torrent.Load(path);
            var manager = new TorrentManager(torrent, @"C:/torrents/", new TorrentSettings());
            Engine.Register(manager);
            manager.TorrentStateChanged += (sender, args) =>
            {
                if (args.OldState != TorrentState.Downloading) return;
                Brain.Pipe.ListenNext((s, match, arg3) => Process.Start(args.TorrentManager.SavePath), "open");
                Output("Finished downloading " + args.TorrentManager.Torrent.Name.TorrentName());
                new TorrentHandler(args.TorrentManager).Handle();
            };
            manager.Start();
            
        }

        public override void RawOutput(string output)
        {
            Brain.ListenerManager.CurrentListener.Output(output);
        }
    }
}
