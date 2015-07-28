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
            this.Exceptions = new ObservableCollection<ITwitterException>();
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

        public ObservableCollection<ITwitterException> Exceptions { get; set; }

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

        private void UpdateExceptions()
        {
            foreach (var x in TwitterUtil.Exceptions)
            {
                if (false == this.Exceptions.Contains(x))
                {
                    this.Exceptions.Add(x);
                }
            }
        }

        public void PerformLogin()
        {
            if (this.NeedLogin())
            {
                var cred = TwitterUtil.LoginWithCaptcha(this.Captcha);
                if (cred != null)
                {
                    Set.Token = cred.Token;
                    Set.Secret = cred.Secret;
                    Set.Save();
                }
            }
            else
            {
                TwitterUtil.Login(new Credentials(Set.Token, Set.Secret));
            }
            this.UpdateExceptions();
        }

        private bool NeedLogin()
        {
            return string.IsNullOrEmpty(Set.Secret);
        }

        public void PerformOpenLoginUrl()
        {
            if (this.NeedLogin() == false)
            {
                MessageBox.Show("just hit login");
                return;
            }
            var url = TwitterUtil.TwitterCaptchaPage;
            if (url != null) {
                Process.Start(url);
            }
            this.UpdateExceptions();
        }
    }
}
