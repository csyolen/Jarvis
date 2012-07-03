using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Runnables
{
    class ActionRunnable : IRunnable
    {
        private readonly Action _action;

        public ActionRunnable(Action action)
        {
            _action = action;
        }

        public void Run()
        {
            _action();
        }
    }
}
