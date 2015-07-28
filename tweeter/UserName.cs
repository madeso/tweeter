namespace tweeter
{
    public class UserName
    {
        public UserName(string screenName, string name)
        {
            this.Screen = screenName;
            this.Real = name;
        }

        public string Real { get; set; }

        public string Screen { get; set; }
    }
}