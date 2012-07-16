using System;
using System.Collections.Generic;
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
        public IRCListener(Pipe pipe) : base(pipe)
        {
        }

        public override void Loop()
        {
            while (true)
            {
                try
                {
                    Setup();
                    _client.Listen();

                }
                catch {}
                Thread.Sleep(5.Seconds());
            }
        }

        private void Setup()
        {
            if (_client != null)
                _client.Disconnect();

            _client = new IrcClient {ChannelSyncing = true, SendDelay = 200, AutoRetry = true};
            _client.OnChannelMessage += ircdata => Handle(ircdata.Message); ;
            _client.OnInvite += (inviter, channel, ircdata) => _client.Join(ircdata.Message);
            //_client.OnDisconnected += Setup;

            _client.Connect("irc.freenode.net", 6667);
            _client.Login("[Jarvis]", "[Jarvis]");
            //_client.Join("#clossit");
            _client.Join("#jarvis");
            
        }

        public override void RawOutput(string output)
        {
            var lines = output.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                foreach (var channel in _client.JoinedChannels)
                {
                    _client.Message(SendType.Message, channel, line);
                }
            }
            Brain.ListenerManager.Voice.Output(output);
        }
    }
}
