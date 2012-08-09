using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using IrcDotNet;

namespace Jarvis.Listeners
{
    public class IRCListener : ListenerBase
    {
        private IrcClient _client;
        public IRCListener(Pipe pipe)
            : base(pipe)
        {
            _client = new IrcClient();
            _client.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
        }

        public override void Loop()
        {
            while (true)
            {

                using (var mre = new ManualResetEvent(false))
                {
                    if (_client != null)
                        _client.Disconnect();

                    _client = new IrcClient();
                    _client.Registered +=
                        (sender, args) =>
                            {

                                _client.LocalUser.JoinedChannel += (sender1, args1) =>
                                    {
                                        args1.Channel.MessageReceived += (o, eventArgs) => Handle(eventArgs.Text);
                                        RawOutput("I have joined " + args1.Channel.Name);
                                        mre.Set();
                                    };
                                _client.Channels.Join("#jarvis");
                                Brain.ListenerManager.CurrentListener.Output("I have made a connection to Rizon.");
                            };
                    _client.Connect("irc.rizon.net", 6667, false, new IrcUserRegistrationInfo()
                        {
                            NickName = "Jarvis",
                            UserName = "Jarvis",
                            RealName = "Jarvis"
                        });
                    mre.WaitOne(30.Seconds());
                    while (_client.IsConnected)
                    {
                        5.Seconds().Sleep();
                    }
                }
                Brain.ListenerManager.CurrentListener.Output("I've failed to maintain a connection to Rizon.");
            }
        }

        public override void RawOutput(string output)
        {
            if (_client.IsRegistered)
            {
                var lines = output.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    foreach (var channel in _client.Channels)
                    {
                        _client.LocalUser.SendMessage(channel, line);
                    }
                }
            }
            Brain.ListenerManager.Voice.Output(output);
        }
    }
}
