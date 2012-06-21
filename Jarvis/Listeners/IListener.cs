using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jarvis.Listeners
{
    public interface IListener
    {
        void Loop();
        void Output(string output);
        void Start();
        void Stop();
    }
}
