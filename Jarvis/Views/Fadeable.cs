using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Jarvis.Views
{
    public class Fadeable : Window
    {
        public Fadeable()
        {
            this.Top = FadeableManager.AddFadeable(this);
            this.MouseLeftButtonUp += (sender, args) => this.FadeOut();
            this.Cursor = Cursors.Hand;
        }

        public void FadeIn()
        {
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - 10;
            Opacity = 0;
            this.Show();
            var da = new DoubleAnimation(0, 1.0, 300.Milliseconds());
            this.BeginAnimation(OpacityProperty, da);
        }

        public void FadeOut()
        {
            var da = new DoubleAnimation(1.0, 0, 200.Milliseconds());
            da.Completed += (sender, args) => this.Close();
            this.BeginAnimation(OpacityProperty, da);
            FadeableManager.RemoveFadeable(this);
        }
    }
}
