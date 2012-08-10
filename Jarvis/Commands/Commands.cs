using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Jarvis.Objects.Reference;
using Jarvis.Tickers;

namespace Jarvis.Commands
{
    static class Commands
    {
        public static void Register(Pipe pipe)
        {
            //Actors
            pipe.Listen((input, match, listener) =>
                {
                    var query = match.Groups[1].Value;
                    var imdb = IMDB.FromQuery(query);
                    listener.Output("{0} were in {1}".Template(imdb.Actors, imdb.Title));
                }, "who.+in (.+)");

            //Alarm
            pipe.Listen((input, match, listener) =>
                {
                    ClockTicker.Instance.StopAlarm();
                    Brain.Awake = true;
                }, "alarm");

            //Close
            pipe.Listen((input, match, listener) =>
            {
                var process = match.Groups[1].Value.ToLower();
                var list = Process.GetProcesses().Where(o => o.ProcessName.ToLower().Contains(process)).ToList();
                if(list.Count< 0)
                    return;
                foreach (var p in list)
                {
                    try
                    {
                        p.Kill();

                    }
                    catch (Exception)
                    {
                    }
                }
                listener.Output("I've closed " + list.FirstOrDefault());
            }, "close (.+)");

            //Restart 
            pipe.Listen((input, match, listener) =>
                {
                    listener.Output("Restarting...");
                    var path = Assembly.GetAssembly(typeof (Brain)).Location;
                    Process.Start(path);
                    Environment.Exit(0);
                }, "restart");
        }
    }       
}