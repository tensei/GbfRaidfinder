using System.Globalization;
using Tweetinvi.Models;

namespace GbfRaidfinder.Models {
    public class UserTweet {
        public ITweet Tweet;
        public string Text { get; set; }
        public string Timestamp { get; set; }
        
        public UserTweet(ITweet tweet) {
            Tweet = tweet;
            Text = tweet.FullText;
            Timestamp = tweet.CreatedAt.ToString(CultureInfo.InvariantCulture);
        }
    }
}
