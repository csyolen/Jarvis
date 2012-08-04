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
            :base()
        {
            this.Headers[HttpRequestHeader.UserAgent] =
                "Mozilla/5.0 (Linux; U; Android 4.0.3; ko-kr; LG-L160L Build/IML74K) AppleWebkit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
        }
    }
}
