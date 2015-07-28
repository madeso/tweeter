namespace tweeter
{
    public class Credentials
    {
        public Credentials(string token, string secret)
        {
            this.Token = token;
            this.Secret = secret;
        }

        public string Token { get; set; }
        public string Secret { get; set; }
    }
}