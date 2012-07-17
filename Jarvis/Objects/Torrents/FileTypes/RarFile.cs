using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Objects.Torrents.FileTypes
{
    class RarFile : IFileType
    {
        public void Handle(FileInfo fileInfo, TorrentHandler th)
        {
            string tmp = Constants.TmpDirectory.FullName + "/" + fileInfo.Name.ToLower().Replace(".rar", "");
            //th.MarkForDeletion(tmp);

            Directory.CreateDirectory(tmp);
            string args = String.Format("e -o- \"{0}\" \"{1}\"", fileInfo.FullName, tmp);

            var psi = new ProcessStartInfo("unrar.exe", args);
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            var p = Process.Start(psi);
            p.WaitForExit();

            foreach (string path in Directory.GetFiles(tmp, "*.*", SearchOption.AllDirectories))
            { th.AddFile(path); }

        }

        public IEnumerable<string> Extensions
        {
            get { return new[] {"rar"}; }
        }
    }
}
