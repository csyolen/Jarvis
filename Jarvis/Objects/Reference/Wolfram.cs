using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WolframAlpha.WrapperCore;

namespace Jarvis.Objects.Reference
{
    class Wolfram
    {
        private const string AppId = "2PWVJ9-9XEHHYT93V";

        public Wolfram(string query)
        {
            var url = "http://api.wolframalpha.com/v2/query?input={0}&appid=2PWVJ9-9XEHHYT93V".Template(query);
            var doc = new XmlDocument();
            doc.LoadXml(new BrowserClient().DownloadString(url));
            Result = "I have no idea.";
            Images = new List<string>();
            foreach (XmlNode node in doc.SelectNodes("//pod/subpod/img"))
            {
                Images.Add(node.Attributes["src"].Value);
            }
            try
            {
                Result = doc.SelectSingleNode("//pod[@title='Result']/subpod/plaintext").InnerText;
            }
            catch
            {
            }

            var engine = new WolframAlphaEngine(AppId);
            var result = engine.GetWolframAlphaQueryResult(engine.GetStringRequest(new WolframAlphaQuery()
                {
                    Query = query,
                    APIKey = AppId
                }));
        }

        public List<string> Images { get; private set; }
        public string Result { get; private set; }
    }
}
