using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

namespace Jarvis.Views
{
    /// <summary>
    /// Interaction logic for ImageView.xaml
    /// </summary>
    /// 
    public partial class BrowserView
    {
        public static void Create(string url, string title = "Browser")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = new BrowserView(url, title);
                view.Left = 10;
                view.Top = SystemParameters.FullPrimaryScreenHeight - view.Height - 20;
                view.Show();
            });
        }

        public BrowserView(string url, string title)
        {
            InitializeComponent();
            Browser.Loaded += (sender, args) => Browser.Navigate(new Uri(url));
            this.Title = title;
            this.Closing += (sender, args) =>
                {
                    Browser.Dispose();
                    this.Grid.Children.Remove(Browser);
                };
            Browser.KeyUp += (sender, args) =>
                {
                    if(args.SystemKey != Key.F10)
                        return;
                    this.WindowStyle = this.WindowStyle == WindowStyle.ToolWindow ? WindowStyle.None : WindowStyle.ToolWindow;
                };
        }
    }
}
