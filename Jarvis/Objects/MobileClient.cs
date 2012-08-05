using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Objects
{
    public class MobileClient : BrowserClient
    {
        public MobileClient()
        {
            this.Headers[HttpRequestHeader.UserAgent] =
                "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5";
        }

        public MobileClient(string url)
            :base(url)
        {
            this.Headers[HttpRequestHeader.UserAgent] =
                "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_2 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8H7 Safari/6533.18.5";
        }
    }
}
