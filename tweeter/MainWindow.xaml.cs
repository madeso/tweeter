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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tweeter
{
    using FirstFloor.ModernUI.Windows.Controls;
    using FirstFloor.ModernUI.Windows.Navigation;

    using Tweetinvi;
    using Tweetinvi.Core.Interfaces.Credentials;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public void ChangePage(string newPage)
        {
            IInputElement target = NavigationHelper.FindFrame("_parent", this);
            if (target == null) target = this;
            NavigationCommands.GoToPage.Execute(newPage, target);
        }
        public MainWindow()
        {
            // var tweet = Tweet.PublishTweet("hello");
            InitializeComponent();
            Instance = this;
        }

        public static MainWindow Instance { get; private set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var t = this.DataContext as TweeterView;
            if (t == null) return;
            if (t.PerformAutoLogin())
            {
                this.ChangePage(@"/Pages/home.xaml");
            }
        }
    }
}
