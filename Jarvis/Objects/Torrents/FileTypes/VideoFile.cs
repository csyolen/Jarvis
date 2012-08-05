using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarvis.Utilities;

namespace Jarvis.Objects.Torrents.FileTypes
{
    class VideoFile : IFileType
    {
        public void Handle(FileInfo fi, TorrentHandler th)
        {
            var friendly = fi.Name.TorrentName();
            var di = new FileInfo(@"C:\home\media\tv\" + friendly + fi.Extension);
            if (di.Exists)
                di.Delete();
            fi.MoveTo(di.FullName);
            Brain.Pipe.ListenNext((s, match, listener) => Process.Start(di.FullName), "play|open|ok");
            Brain.ListenerManager.CurrentListener.Output("{0} is ready to be watched.".Template(friendly));
        }

        public IEnumerable<string> Extensions
        {
            get { return new[] {"avi", "mp4", "mov", "mkv"}; }
        }
    }
}
