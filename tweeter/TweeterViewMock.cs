namespace tweeter
{
    using System.Collections.ObjectModel;
    using System.Windows;

    using Tweetinvi.Core.Exceptions;

    public class TweeterViewMock : ResourceDictionary
    {
        public UserName UserName
        {
            get { return new UserName("screen", "real"); }
        }

        public string Captcha
        {
            get
            {
                return "captcha";
            }
        }

        public ObservableCollection<ITwitterException> Exceptions
        {
            get
            {
                return new ObservableCollection<ITwitterException>();
            } 
        }
    }
}