﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Listeners
{
    public class VoiceListener : ListenerBase
    {
        private readonly SpeechSynthesizer _synthesizer;

        public VoiceListener(Pipe pipe) : base(pipe)
        {
            _synthesizer = new SpeechSynthesizer();
        }

        public override void Loop()
        {
            return;
            using (var recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US")))
            {
                recognizer.LoadGrammar(new DictationGrammar());
                while (true)
                {
                    recognizer.SetInputToDefaultAudioDevice();
                    var result = recognizer.Recognize();
                    if(result == null) continue;
                    Handle(result.Text);
                }
            }
        }

        private Prompt _current;
        public override void RawOutput(string output)
        {
            if(!Brain.Awake)
                return;
            if(output.Contains("\r\n"))
                return;
            /*
            if(_current != null)
                _synthesizer.SpeakAsyncCancel(_current);
            _current = _synthesizer.SpeakAsync(output);
            */
            _synthesizer.SpeakAsync(output);
         }
    }
}
