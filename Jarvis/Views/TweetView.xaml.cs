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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Jarvis.Views
{
    /// <summary>
    /// Interaction logic for TweetView.xaml
    /// </summary>
    public partial class TweetView : Fadeable
    {
        public TweetView(string text, string author)
        {
            InitializeComponent();

            Text.Text = text;
            Author.Text = author;

        }

        public static void Create(string text, string author)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = new TweetView(text, author);
                view.FadeIn();
            });
        }
    }
}
