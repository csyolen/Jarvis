using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace Jarvis.Listeners
{
    public class IRCListener : GenericListener
    {
        private IrcClient _client;
        public IRCListener(Pipe pipe) : base(pipe)
        {
        }

        public override void Loop()
        {
            _client = new IrcClient {ChannelSyncing = true, SendDelay = 200, AutoRetry = true};
            _client.OnChannelMessage += ChannelMessage;
            _client.OnInvite += OnInvite;
            _client.Connect("irc.freenode.net", 6667);
            _client.Login("[Jarvis]", "[Jarvis]");
            //_client.Join("#clossit");
            _client.Join("#jarvis");
            _client.Listen();
            Output("Jarvis is online.");
        }

        private void ChannelMessage(Data ircdata)
        {
            Handle(ircdata.Message);
        }

        private void OnInvite(string inviter, string channel, Data ircdata)
        {
            _client.Join(ircdata.Message);
        }

        public override void Output(string output)
        {
            var lines = output.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                foreach (var channel in _client.JoinedChannels)
                {
                    _client.Message(SendType.Message, channel, line);
                }
            }
        }
    }
}
