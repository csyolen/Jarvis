﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jarvis.Commands;
using Jarvis.Listeners;
using Jarvis.Locale;
using Jarvis.Tickers;
using Jarvis.Utilities;
using Jarvis.Views;
using Newtonsoft.Json.Linq;

namespace Jarvis
{
    public class Brain
    {
        public static ListenerManager ListenerManager { get; private set; }
        public static Settings Settings { get; set; }

        public static Pipe Pipe { get; set; }

        public static bool Awake { get; set; }
  
        public static void Start()
        {
            var processes = Process.GetProcessesByName("jarvis");
            var p = Process.GetCurrentProcess();
            foreach (var process in processes.Where(process => p.Id != process.Id))
            {
                process.Kill();
            }

            Awake = true;
            Settings = new Settings();
            Pipe = new Pipe();

            //Bot.loadSettings();
            //Bot.loadAIMLFromFiles();
            
            //Listeners
            ListenerManager = new ListenerManager(Pipe);

            //Legacy Command Support
            typeof(ICommand).Assembly
                .GetTypes()
                .Where(o => o.GetInterface(typeof(ICommand).FullName) != null && o.IsClass)
                .Select(source => (ICommand)Activator.CreateInstance(source)).ToList()
                .ForEach(o => Brain.Pipe.Listen((input, match, listener) =>
                    {
                        foreach (var r in o.Handle(input,match, listener))
                        {
                            listener.Output(r);
                        }
                    }, o.Regexes));

            //Tickers 
            typeof(TickerBase).Assembly
                .GetTypes()
                .Where(o => o.BaseType == typeof (TickerBase))
                .Where(o => o.GetConstructors().Any(x => x.GetParameters().Length == 0))
                .Select(source => (TickerBase) Activator.CreateInstance(source)).ToList()
                .ForEach(o => o.Start());
            foreach (var rss in Brain.Settings.RSS)
            {
                new RssTicker(rss).Start();
            }

        }
    }
}
