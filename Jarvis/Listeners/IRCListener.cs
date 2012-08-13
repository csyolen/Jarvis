using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Meebey.SmartIrc4net;

namespace Jarvis.Listeners
{
    public class IRCListener : ListenerBase
    {
        private IrcClient _client;

        public IRCListener(Pipe pipe)
            : base(pipe)
        {
        }

        public override void Loop()
        {
            while (true)
            {

                _client = new IrcClient
                    {
                        AutoJoinOnInvite = true,
                        AutoReconnect = true,
                        AutoRejoin = true,
                        AutoRejoinOnKick = true,
                        AutoRelogin = true,
                        AutoRetry = true
                    };
                var mre = new ManualResetEvent(false);
                _client.OnJoin += (sender, args) =>
                                  Output("I have joined " + args.Channel);
                _client.OnChannelMessage += (sender, args) =>
                                            Handle(args.Data.Message);
                _client.OnConnected += (sender, args) =>
                    {
                        mre.Set();
                        _client.RfcJoin("#jarvis");
                    };
                _client.OnError += (sender, args) =>
                    {
                        _client.Disconnect();
                        Loop();
                    };

                _client.Connect("irc.clossit.com", 6668);
                _client.Login("Jarvis", "Jarvis");
                new Thread(() => _client.Listen()).Start();
                mre.WaitOne(30.Seconds());
                while (_client.IsConnected)
                {
                    5.Seconds().Sleep();
                }
                Output("I have lost connection to I.R.C");
            }
        }

        public override void RawOutput(string output)
        {
            if (true)
            {
                var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    foreach (var channel in _client.JoinedChannels)
                    {
                        _client.SendMessage(SendType.Message, channel, line);
                    }
                }
            }
            Brain.ListenerManager.Voice.Output(output);
        }
    }
}
