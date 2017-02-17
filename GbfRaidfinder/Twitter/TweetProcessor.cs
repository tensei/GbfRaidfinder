using System.Collections.Generic;
using System.Text.RegularExpressions;
using GbfRaidfinder.Interfaces;
using Tweetinvi.Models;

namespace GbfRaidfinder.Twitter {
    public class TweetProcessor : ITweetProcessor {
        private const string RaidRegexEnglish = "((?s).*)I need backup!Battle ID: (?<id>[0-9A-F]+)\n(.+)\n?(.*)";
        private const string RaidRegexJapanese = "((?s).*)参加者募集！参戦ID：(?<id>[0-9A-F]+)\n(.+)\n?(.*)";
        public readonly Queue<TweetInfo> Queue = new Queue<TweetInfo>();

        public TweetInfo RecievedTweetInfo(ITweet tweet) => StreamOnNonMatchingTweetReceived(tweet);

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
                    Language = "JP"
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
                Language = "EN"
            };
            //Clipboard.SetText(id);
        }
    }
}