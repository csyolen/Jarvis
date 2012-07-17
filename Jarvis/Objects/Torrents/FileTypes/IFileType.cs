using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Objects.Torrents
{
    interface IFileType
    {
        void Handle(FileInfo fi, TorrentHandler th);

        IEnumerable<string> Extensions { get; }
    }
}
