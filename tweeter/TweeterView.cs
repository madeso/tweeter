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

    public class TweeterView : BindableBase
    {
        private string captcha = "captcha";
        private UserName userName;

        private ICommand fnLogin;
        private ICommand fnOpenLoginUrl;


        private static Settings Set
        {
            get
            {
                return Properties.Settings.Default;
            }
        }

        public TweeterView()
        {
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
