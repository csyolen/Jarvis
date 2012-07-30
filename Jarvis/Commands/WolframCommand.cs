using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jarvis.Listeners;
using Jarvis.Objects.Reference;
using Jarvis.Views;
using WolframAlpha.WrapperCore;

namespace Jarvis.Commands
{
    class WolframCommand : ICommand
    {
        private const string AppId = "2PWVJ9-9XEHHYT93V";
        public string Handle(string input, Match match, IListener listener)
        {
            var query = input;
            if (match.Groups[1].Value == "wolfram")
                query = match.Groups[2].Value;
            var engine = new WolframAlphaEngine(AppId);
            var result = engine.GetWolframAlphaQueryResult(engine.GetStringRequest(new WolframAlphaQuery()
            {
                Query = query,
                APIKey = AppId
            }));
            if (!result.Success)
                return "";
            var r = "";
            var answerPod = result.Pods.FirstOrDefault(o => o.Title.ToLower().Contains("result"));
            var interpretationPod = result.Pods.FirstOrDefault(o => o.Title.ToLower().Contains("input interpretation"));
            if (answerPod == null || interpretationPod == null)
                return "";

            var interpretation = interpretationPod.SubPods[0].PodText.Replace(" |", "'s").UppercaseFirst();
            var answer = answerPod.SubPods[0].PodText.RemoveExtraSpaces().Trim();

            return ("{0} is {1}".Template(interpretation, answer)).Replace("\n", " ");
        }

        public string Regexes
        {
            get { return "(wolfram|what is|when|how) (.+)"; }
        }
    }
}
