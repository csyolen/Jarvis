using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Jarvis.Listeners
{
    public abstract class ListenerBase : IListener
    {
        protected Pipe Pipe;
        protected ListenerBase(Pipe pipe)
        {
            Pipe = pipe;
        }

        private Thread _thread;
        public void Start()
        {
            if(_thread != null && _thread.IsAlive)
                Stop();

            _thread = new Thread(Loop);
            _thread.Start();
        }

        public void Stop()
        {
            _thread.Abort();
        }

        protected void Handle(string input)
        {
            Pipe.Handle(input, this);
        }

        public abstract void Loop();
        public void Output(string output)
        {
            output = Brain.Settings.RegexFilters
                .Aggregate(output, (current, regex) =>
                    Regex.Replace(current, regex.Item1, regex.Item2, RegexOptions.IgnoreCase));
            RawOutput(output);
        }
        public abstract void RawOutput(string output);
    }
}
