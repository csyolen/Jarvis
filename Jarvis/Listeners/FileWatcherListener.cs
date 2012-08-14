using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jarvis.Locale;
using Jarvis.Utilities;

namespace Jarvis.Listeners
{
    class FileWatcherListener : ListenerBase
    {
        public FileWatcherListener(Pipe pipe)
            : base(pipe)
        {
        }

        public override void Loop()
        {
            foreach (var dir in Brain.Settings.Videos)
            {
                var fw = new FileSystemWatcher { Filter = "*.*", Path = dir.FullName };
                fw.Created += (sender, args) =>
                    {
                        var video = new FileInfo(args.FullPath);
                        var name = video.Name.Replace(".", " ").Trim().RegexRemove(video.Extension);
                        name = name.RegexReplace(@"s(\d+)", "Season $1 ");
                        name = name.RegexReplace(@"e(\d+)", "Episode $1 ");
                        name = name.Trim().RemoveExtraSpaces();
                        name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
                        Output("Sir, would you like to watch " + name + " right now?");
                        Brain.Pipe.ListenNext((input, match, listener) =>
                            {
                                listener.Output(Speech.Yes.Parse());
                                Process.Start(args.FullPath);
                            }, "yes|sure");
                    };
                fw.EnableRaisingEvents = true;
            }
            var zips = new FileSystemWatcher { Filter = "*.zip", Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads"};
            zips.Renamed += (sender, args) =>
                {
                    var fi = new FileInfo(args.FullPath);
                    var path = fi.DirectoryName + "/" + args.Name.RegexRemove(@"\.zip");
                    var psi = new ProcessStartInfo("unzip",
                                                   "-d \"{0}\" \"{1}\"".Template(path, args.FullPath));
                    var p = Process.Start(psi);
                    p.WaitForExit();
                    Output("Extracted zip file.");
                    Process.Start(path);
                };
            zips.EnableRaisingEvents = true;
        }

        public override void RawOutput(string output)
        {
            Brain.ListenerManager.CurrentListener.Output(output);
        }
    }
}
