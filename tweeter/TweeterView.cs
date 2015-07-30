using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweeter
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.Practices.Prism.Mvvm;

    using tweeter.Properties;

    using Tweetinvi.Core.Exceptions;
    using Tweetinvi.Core.Extensions;
    using Tweetinvi.Core.Interfaces;

    public class TweeterView : BindableBase
    {
        private string captcha = "captcha";
        private UserName userName;

        private ICommand fnLogin;
        private ICommand fnOpenLoginUrl;

        public ObservableCollection<TweetView> HomeTweets
        {
            get; set;
        }

        private static Settings Set
        {
            get
            {
                return Properties.Settings.Default;
            }
        }

        public TweeterView()
        {
            this.HomeTweets = new ObservableCollection<TweetView>();
        }

        public string Captcha
        {
            get
            {
                return this.captcha;
            }
            set
            {
                this.SetProperty(ref this.captcha, value);
            }
        }

        public UserName UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.SetProperty(ref this.userName, value);
            }
        }

        public ICommand Login
        {
            get
            {
                return RelayCommand.Return(ref this.fnLogin, () => new RelayCommand(p => this.PerformLogin()));
            }
        }

        public ICommand OpenLoginUrl
        {
            get
            {
                return RelayCommand.Return(ref this.fnOpenLoginUrl, () => new RelayCommand(p => this.PerformOpenLoginUrl()));
            }
        }

        private bool ShowError()
        {
            var ret = false;
            foreach (var x in TwitterUtil.Exceptions)
            {
                MessageBox.Show(x.TwitterDescription, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ret = true;
            }
            return ret;
        }

        public void PerformLogin()
        {
            if (Set.Secret.IsNullOrEmpty() == false)
            {
                MainWindow.Instance.ChangePage(@"/Pages/home.xaml");
                return;
            }

            TwitterUtil.ClearExceptions();
            var cred = TwitterUtil.LoginWithCaptcha(this.Captcha);
            if (cred != null)
            {
                Set.Token = cred.Token;
                Set.Secret = cred.Secret;
                Set.Save();
            }
            this.ShowError();
            this.UpdateTweetData();
        }

        private void UpdateTweetData()
        {
            TwitterUtil.ClearExceptions();
            this.UserName = TwitterUtil.LoggedInUser();
            this.ShowError();

            Sync(this.HomeTweets, TwitterUtil.HomeTweets);
        }

        private void Sync(ObservableCollection<TweetView> dst, IEnumerable<ITweet> src)
        {
            var map = new Dictionary<long, TweetView>();
            foreach (var t in dst)
            {
                map.Add(t.Id, t);
            }

            foreach (var tweet in src)
            {
                TweetView t;
                if (map.ContainsKey(tweet.Id) == false)
                {
                    t = new TweetView { Id = tweet.Id };
                    
                    dst.Add(t);
                }
                else
                {
                    t = map[tweet.Id];
                }
                this.SyncTweet(t, tweet);
            }
        }

        private void SyncTweet(TweetView t, ITweet tweet)
        {
            t.Favorited = tweet.Favourited;
            t.FavoriteCount = tweet.FavouriteCount;
            t.Retweeted = tweet.Retweeted;
            t.RetweetCount = tweet.RetweetCount;
            t.Text = tweet.Text;
            t.Source = tweet.Source;
            t.CreatedBy = TwitterUtil.CreateUserName(tweet.CreatedBy);
            t.Date = tweet.CreatedAt;
            // t.Media = tweet.Media;
        }

        public bool PerformAutoLogin()
        {
            if (false == string.IsNullOrEmpty(Set.Secret))
            {
                TwitterUtil.ClearExceptions();
                TwitterUtil.Login(new Credentials(Set.Token, Set.Secret));
                var err = this.ShowError();

                if( err == false ) {
                    this.UpdateTweetData();
                }

                return err == false;
            }

            return false;
        }

        public void PerformOpenLoginUrl()
        {
            TwitterUtil.ClearExceptions();
            var url = TwitterUtil.TwitterCaptchaPage;
            if (url != null) {
                Process.Start(url);
            }
            this.ShowError();
        }
    }
}
