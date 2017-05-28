using System.Text.RegularExpressions;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Models;
using Tweetinvi.Models;

namespace GbfRaidfinder.Twitter {
    public class TweetProcessor : ITweetProcessor {
        private const string RaidRegexEnglish = "((?s).*)I need backup!Battle ID: (?<id>[0-9A-F]+)\n(.+)\n?(.*)";
        private const string RaidRegexJapanese = "((?s).*)参加者募集！参戦ID：(?<id>[0-9A-F]+)\n(.+)\n?(.*)";

        public TweetInfo RecievedTweetInfo(ITweet tweet) {
            return StreamOnNonMatchingTweetReceived(tweet);
        }

        private TweetInfo StreamOnNonMatchingTweetReceived(ITweet tweet) {
            //Console.WriteLine(tweet.Text);
            //if (!AutoCopy)return;
            var tweetreg = Regex.Match(tweet.Text, RaidRegexJapanese);
            if (tweetreg.Success) {
                return new TweetInfo {
                    Id = tweetreg.Groups["id"].Value,
                    Time = tweet.CreatedAt.ToShortTimeString(),
                    User = tweet.CreatedBy.Name,
                    Boss = tweetreg.Groups[2].Value,
                    Language = "JP",
                    Text = tweetreg.Groups[1].Value.Trim(),
                    Avatar = tweet.CreatedBy.ProfileImageUrlHttps,
                    Profile = tweet.Url
                };
            }
            tweetreg = Regex.Match(tweet.Text, RaidRegexEnglish);
            if (!tweetreg.Success) {
                return null;
            }
            return new TweetInfo {
                Id = tweetreg.Groups["id"].Value,
                Time = tweet.CreatedAt.ToShortTimeString(),
                User = tweet.CreatedBy.Name,
                Boss = tweetreg.Groups[2].Value,
                Language = "EN",
                Text = tweetreg.Groups[1].Value.Trim(),
                Avatar = tweet.CreatedBy.ProfileImageUrlHttps,
                Profile = tweet.Url
            };
            //Clipboard.SetText(id);
        }
    }
}