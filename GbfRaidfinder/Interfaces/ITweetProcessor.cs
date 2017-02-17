using GbfRaidfinder.Twitter;
using Tweetinvi.Models;

namespace GbfRaidfinder.Interfaces {
    public interface ITweetProcessor {
        TweetInfo RecievedTweetInfo(ITweet tweet);
    }
}