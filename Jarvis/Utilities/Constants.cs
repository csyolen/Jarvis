using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jarvis
{
    static class Constants
    {
        public static DirectoryInfo ConfigDirectory
        {
            get
            {
                var path = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Jarvis");
                if (!path.Exists)
                    path.Create();
                return path;
            }
        }

        public static DirectoryInfo TorrentDirectory
        {
            get
            {
                var path = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Jarvis\Torrents");
                if (!path.Exists)
                    path.Create();
                return path;
            }
        }

        public static FileInfo SettingsFile
        {
            get
            {
                var path = new FileInfo(ConfigDirectory.FullName + @"\settings");
                if (!path.Exists)
                    path.Create().Close();
                return path;
            }
        }

        private static Random _random;
        public static int Random(int min, int max)
        {
            _random = _random ?? new Random();
            return _random.Next(min, max);
        }
    }
}
