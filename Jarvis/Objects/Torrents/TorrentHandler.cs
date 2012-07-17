using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MonoTorrent.Client;

namespace Jarvis.Objects.Torrents
{
    public class TorrentHandler
    {
        private List<FileInfo> _files;
        private static List<IFileType> _fileTypes;
        private List<string> _delete;

        public TorrentManager TorrentManager { get; private set; }

        static TorrentHandler()
        {
            _fileTypes = Assembly.GetExecutingAssembly().GetTypes()
                            .Where(o => o.GetInterface(typeof (IFileType).FullName) != null)
                            .Select(source => (IFileType)Activator.CreateInstance(source))
                            .ToList();
        }

        public TorrentHandler(TorrentManager tm)
        {
            TorrentManager = tm;
            _delete = new List<string>();
            _files = new List<FileInfo>();
            _files.AddRange(Directory.GetFiles(tm.SavePath, "*.*", SearchOption.AllDirectories).Select(o => new FileInfo(o)));
        }

        public void Handle()
        {
            TorrentManager.Stop();

            int i = 0;
            while (i < _files.Count)
            {
                var fileInfo = _files[i];
                string extension = fileInfo.Extension.Substring(1).ToLower();
                foreach (IFileType handleable in _fileTypes.Where(o => o.Extensions.Contains(extension)))
                {
                    handleable.Handle(fileInfo, this);
                    break;
                }
                i++;
            }
            CleanUp();
            TorrentManager.Start();
        }

        private void CleanUp()
        {
            foreach (string file in _delete)
            {
                if (File.Exists(file)) 
                    File.Delete(file);
                else if(Directory.Exists(file))
                    Directory.Delete(file, true);
            }
        }

        public void MarkForDeletion(string file)
        {
            _delete.Add(file);
        }

        public void AddFile(string file)
        {
            _files.Add(new FileInfo(file));
        }
    }
}
