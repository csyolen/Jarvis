using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Jarvis.Views
{
    static class FadeableManager
    {
        private static readonly List<Fadeable> Fadeables;

        static FadeableManager()
        {
            Fadeables = new List<Fadeable>();
        }

        public static double AddFadeable(Fadeable f)
        {
            Fadeables.Add(f);
            if (Fadeables.Count == 1)
                return 10;
            var last = Fadeables[Fadeables.Count - 2];
            return last.Top + last.Height + 10;
        }

        public static void RemoveFadeable(Fadeable f)
        {
            var index = Fadeables.IndexOf(f);
            var height = f.Height + 10;
            Fadeables.Remove(f);
            for (int i = index; i < Fadeables.Count; i++)
            {
                var fadeable = Fadeables[i];
                var da = new DoubleAnimation(fadeable.Top, fadeable.Top - height, 150.Milliseconds());
                fadeable.BeginAnimation(Window.TopProperty, da);
            }
        }
    }
}
