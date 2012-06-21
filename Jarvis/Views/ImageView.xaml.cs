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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Jarvis.Views
{
    /// <summary>
    /// Interaction logic for ImageView.xaml
    /// </summary>
    /// 
    public partial class ImageView : Window
    {
        public static void Create(string url)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = new ImageView(url);
                view.Show();
            });
        }

        public static void Create(string url, string link)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = new ImageView(url, link);
                view.Show();
            });
        }

        public ImageView(string url)
        {
            InitializeComponent();
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.Left = 0;
            this.Top = 0;
            this.MouseLeftButtonUp += (sender, args) => this.Close();
            var wc = new WebClient();
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(wc.DownloadData(url));
            image.EndInit();
            image.Freeze();
            Image.Source = image;
            Image.Stretch = Stretch.Uniform;
        }

        public ImageView(string url, string link)
            :this(url)
        {
            Image.MouseLeftButtonUp += (sender, args) => Process.Start(link);
        }
    }
}
