using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweeter
{
    using System.Diagnostics;
    using System.Windows.Input;

    using Microsoft.Practices.Prism.Mvvm;

    public class TweeterView : BindableBase
    {
        private string captcha = "captcha";

        private ICommand fnLogin;
        private ICommand fnOpenLoginUrl;

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

        public void PerformLogin()
        {
            TwitterUtil.GenerateCredentialsAndLogin(this.Captcha);
        }

        public void PerformOpenLoginUrl()
        {
            var url = TwitterUtil.TwitterCaptchaPage;
            Process.Start(url);
        }
    }
}
