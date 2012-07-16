using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jarvis.Listeners
{
    public class ConsoleListener : ListenerBase
    {
        public ConsoleListener(Pipe pipe) : base(pipe)
        {

        }

        public override void Loop()
        {
            while (true)
            {
                Console.Write("Input: ");
                var line = Console.ReadLine();
                Handle(line);
            }
        }

        public override void RawOutput(string output)
        {
            Console.WriteLine("Jarvis: {0}", output);
        }
    }
}
