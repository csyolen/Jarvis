using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Listeners
{
    public class VoiceListener : GenericListener
    {
        private readonly SpeechSynthesizer _synthesizer;

        public VoiceListener(Pipe pipe) : base(pipe)
        {
            _synthesizer = new SpeechSynthesizer();
        }

        public override void Loop()
        {

        }

        public override void Output(string output)
        {
            _synthesizer.SpeakAsync(output);
        }
    }
}
