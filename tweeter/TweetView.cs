namespace tweeter
{
    using System;

    using Microsoft.Practices.Prism.Mvvm;

    public class TweetView : BindableBase
    {
        public TweetView()
        {
        }

        public long Id { get; set; }

        public bool Favorited { get; set; }

        public int FavoriteCount { get; set; }

        public bool Retweeted { get; set; }

        public int RetweetCount { get; set; }

        public string Text { get; set; }

        public string Source { get; set; }

        public UserName CreatedBy { get; set; }

        public DateTime Date { get; set; }
    }
}