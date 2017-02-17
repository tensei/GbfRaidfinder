using System;
using Tweetinvi.Events;
using Tweetinvi.Models;

namespace GbfRaidfinder.Interfaces {
    public interface ITweetObserver {
        void Stop();
        bool Running { get; set; }
        void Run(ITwitterCredentials userCredentials, ISettingsController settingsController);
        void Subscribe(EventHandler<TweetEventArgs> action, EventHandler<MatchedTweetReceivedEventArgs> match);
    }
}