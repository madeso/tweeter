namespace tweeter
{
    using System;
    using System.Linq;

    using Tweetinvi;
    using Tweetinvi.Core.Interfaces.Credentials;

    public static class TwitterUtil
    {
        public static ITemporaryCredentials ApplicationCredentials 
        {
            get
            {
                ITemporaryCredentials credentials = null;
                const string ConsumerKey = "u3wLAxnyYch9AyRr5A23oGPHG";
                const string ConsumerSecret = "oaBHesBpIDUKpXEi0wy7fBYgVvDYI0xvEImVIGBMpXJmjxxxws";
                credentials = CredentialsCreator.GenerateApplicationCredentials(ConsumerKey, ConsumerSecret);
                return credentials;
            }
        }

        // We need the user to go on the Twitter Website to authorize our application
        // On the Twitter website the user will get a captcha that he will need to enter in the application
        public static string TwitterCaptchaPage
        {
            get
            {
                var creds = ApplicationCredentials;
                var ex = ExceptionHandler.GetExceptions().ToArray();
                var url = CredentialsCreator.GetAuthorizationURL(creds);
                return url;
            }
        }

        public static void GenerateCredentialsAndLogin(string captcha)
        {
            var newCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(captcha, ApplicationCredentials);
            TwitterCredentials.SetCredentials(newCredentials);
        }
    }
}
