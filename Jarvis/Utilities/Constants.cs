using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jarvis
{
    static class Constants
    {
        public static DirectoryInfo Path
        {
            get
            {
                var path = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Jarvis");
                if(!path.Exists)
                    path.Create();
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
