using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Jarvis.Objects.Reference;

namespace Jarvis.Views
{
    /// <summary>
    /// Interaction logic for WeatherView.xaml
    /// </summary>
    public partial class WeatherView : Fadeable
    {
        public static void Create(Weather w)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = new WeatherView(w);
                view.FadeIn();
            });
        }

        public WeatherView(Weather w)
        {
            InitializeComponent();

            this.Temperature.Content = w.Temperature;
            this.Condition.Content = w.Condition;
            var u = "/Jarvis;component/Images/Weather/{0}.png".Template(w.Condition.RegexRemove(@"\s").ToLower());
            this.Icon.Source = new BitmapImage(new Uri(u, UriKind.Relative));
        }
    }
}
