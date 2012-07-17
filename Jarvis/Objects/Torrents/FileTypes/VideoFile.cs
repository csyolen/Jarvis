using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarvis.Runnables;
using Jarvis.Utilities;

namespace Jarvis.Objects.Torrents.FileTypes
{
    class VideoFile : IFileType
    {
        public void Handle(FileInfo fi, TorrentHandler th)
        {
            var friendly = fi.Name.TorrentName();
            var path = @"C:\home\media\tv\" + friendly + fi.Extension;
            fi.MoveTo(path);
            Brain.RunnableManager.Runnable = new ProcessRunnable(path);
            Brain.ListenerManager.CurrentListener.Output("{0} is ready to be watched.".Template(friendly));
        }

        public IEnumerable<string> Extensions
        {
            get { return new[] {"avi", "mp4", "mov", "mkv"}; }
        }
    }
}
