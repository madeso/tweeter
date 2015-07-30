namespace tweeter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Tweetinvi;
    using Tweetinvi.Core.Exceptions;
    using Tweetinvi.Core.Interfaces;
    using Tweetinvi.Core.Interfaces.Credentials;

    public static class TwitterUtil
    {
        const string ConsumerKey = "u3wLAxnyYch9AyRr5A23oGPHG";
        const string ConsumerSecret = "oaBHesBpIDUKpXEi0wy7fBYgVvDYI0xvEImVIGBMpXJmjxxxws";

        private static ITemporaryCredentials applicationCredentials = null;
        public static ITemporaryCredentials ApplicationCredentials 
        {
            get
            {
                if (applicationCredentials == null)
                {
                    applicationCredentials = CredentialsCreator.GenerateApplicationCredentials(ConsumerKey, ConsumerSecret);
                }
                return applicationCredentials;
            }
        }

        // We need the user to go on the Twitter Website to authorize our application
        // On the Twitter website the user will get a captcha that he will need to enter in the application
        public static string TwitterCaptchaPage
        {
            get
            {
                var creds = ApplicationCredentials;
                if (creds == null) return null;
                var url = CredentialsCreator.GetAuthorizationURL(creds);
                return url;
            }
        }

        public static List<ITwitterException> Exceptions
        {
            get
            {
                var exs = ExceptionHandler.GetExceptions().ToList();
                return exs;
            }
        }

        public static IEnumerable<ITweet> HomeTweets
        {
            get
            {
                return Timeline.GetHomeTimeline();
            }
        }

        public static Credentials LoginWithCaptcha(string captcha)
        {
            var credentials = CredentialsCreator.GetCredentialsFromVerifierCode(captcha, ApplicationCredentials);
            TwitterCredentials.SetCredentials(credentials);
            if (credentials == null) return null;
            return new Credentials(credentials.AccessToken, credentials.AccessTokenSecret);
        }

        public static void Login(Credentials credentials)
        {
            TwitterCredentials.SetCredentials(credentials.Token, credentials.Secret, ConsumerKey, ConsumerSecret);
        }

        public static void ClearExceptions()
        {
            ExceptionHandler.ClearLoggedExceptions();
        }

        public static UserName LoggedInUser()
        {
            var usr = User.GetLoggedUser();
            return CreateUserName(usr); // new UserName(usr.ScreenName, usr.Name));
        }

        public static UserName CreateUserName(IUser usr)
        {
            return new UserName(usr.ScreenName, usr.Name);
        }
    }
}
