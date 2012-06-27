using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Listeners
{
    class SocketListener : ListenerBase
    {
        private readonly TcpListener _tcp;

        public SocketListener(Pipe pipe) : base(pipe)
        {
            _tcp = new TcpListener(4444);
        }

        public override void Loop()
        {
            _tcp.Start();
            while (true)
            {
                var client = _tcp.AcceptTcpClient();
                var stream = client.GetStream();
                var data = new byte[4096];
                int read = stream.Read(data, 0, 4096);
                var input = Encoding.ASCII.GetString(data, 0, read).Trim();
                Handle(input);
            }
        }

        public override void Output(string output)
        {
            Brain.ListenerManager.CurrentListener.Output(output);
        }
    }
}
