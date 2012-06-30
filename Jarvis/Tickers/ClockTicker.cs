using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Jarvis.Tickers
{
    class ClockTicker : TickerBase
    {
        private SoundPlayer _alarm;

        public ClockTicker() : base(1000)
        {
            _alarm = new SoundPlayer("Sounds/alarm.wav");
            Instance = this;
        }

        public static ClockTicker Instance { get; private set; }

        protected override void Tick(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            if(now.Minute == 0 && now.Second == 0)
                Brain.ListenerManager.CurrentListener.Output("The time is " + DateTime.Now.ToShortTimeString());
            if (now.TimeOfDay.Hours == Brain.Settings.Wake.Hours && now.TimeOfDay.Minutes == Brain.Settings.Wake.Minutes && now.TimeOfDay.Seconds == 0)
                _alarm.PlayLooping();
            if (now.TimeOfDay.Hours == Brain.Settings.Sleep.Hours && now.TimeOfDay.Minutes == Brain.Settings.Sleep.Minutes && now.TimeOfDay.Seconds == 0)
            {
                Brain.ListenerManager.CurrentListener.Output("Sir it is 3am, I suggest you go to sleep.");
                Brain.Awake = false;
            }

        }

        public void StopAlarm()
        {
            _alarm.Stop();
        }
    }
}
