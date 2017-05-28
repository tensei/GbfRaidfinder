using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GbfRaidfinder.Interfaces;
using PropertyChanged;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace GbfRaidfinder.Twitter {
    public class TweetObserver : ITweetObserver, INotifyPropertyChanged {
        private readonly ISettingsController _settingsController;
        private readonly ITweetProcessor _tweetProcessor;
        private readonly IGlobalVariables _globalVariables;

        private Action<ITweetInfo> _addTweet;

        private int _tries;

        public TweetObserver(ISettingsController settingsController, ITweetProcessor tweetProcessor, IGlobalVariables globalVariables) {
            Stream = Tweetinvi.Stream.CreateFilteredStream();
            _settingsController = settingsController;
            _tweetProcessor = tweetProcessor;
            _globalVariables = globalVariables;

            Stream.MatchingTweetReceived += (sender, args) => StreamOnMatchingTweetReceived(args, _addTweet);
            Stream.NonMatchingTweetReceived += (sender, args) => StreamOnNonMatchingTweetReceived(args, _addTweet);
            Stream.StreamStopped += StreamOnStreamStopped;
            Stream.DisconnectMessageReceived += StreamOnDisconnectMessageReceived;

        }

        public IFilteredStream Stream { get; set; }

        public void SetAddAction(Action<ITweetInfo> act) {
            _addTweet = act;
        }

        public void Run() {
            if (Stream == null) {
                Stream = Tweetinvi.Stream.CreateFilteredStream();
            }
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackOnly;
            Stream.Credentials = Auth.Credentials;
            for (var i = 5; i < 151; i += 5) {
                Stream.AddTrack($"Lv{i}");
            }
            Stream.AddTrack("I need backup!Battle ID:");
            Start();
        }

        public void Stop() {
            Stream?.PauseStream();
            Stream?.StopStream();
            if (Stream?.StreamState == StreamState.Stop) {
                Stream = null;
            }
        }

        private async void Start() {
            await Task.Delay(3000);
            try {
                await Stream.StartStreamMatchingAllConditionsAsync().ConfigureAwait(false);
                _globalVariables.IsLoggedIn = true;
            }
            catch (TwitterInvalidCredentialsException) {
                _globalVariables.IsLoggedIn = false;
                _settingsController.Settings.AccessToken = null;
                _settingsController.Settings.AccessTokenSecret = null;
                _settingsController.Settings.Autologin = false;
                _globalVariables.IsLoggedIn = false;
                _settingsController.Save();
                MessageBox.Show("Click Login again!", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Stream?.StopStream();
            }
            catch (TwitterException tw) {
                _globalVariables.IsLoggedIn = false;
                MessageBox.Show(tw.TwitterDescription, "Twitter Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Stream?.StopStream();
            }
            catch (ArgumentException ar) {
                _globalVariables.IsLoggedIn = false;
                _settingsController.Settings.AccessToken = null;
                _settingsController.Settings.AccessTokenSecret = null;
                _settingsController.Settings.Autologin = false;
                _globalVariables.IsLoggedIn = false;
                _settingsController.Save();
                MessageBox.Show(ar.Message, "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Stream?.StopStream();
            }
            catch (Exception) {
                _globalVariables.IsLoggedIn = false;
                Stream?.StopStream();
            }
        }

        private void StreamOnDisconnectMessageReceived(object sender, DisconnectedEventArgs disconnectedEventArgs) {
            _globalVariables.IsLoggedIn = false;
        }

        private void StreamOnStreamStopped(object sender, StreamExceptionEventArgs streamExceptionEventArgs) {
            if (streamExceptionEventArgs.Exception == null) {
                _globalVariables.IsLoggedIn = false;
                return;
            }
            if (streamExceptionEventArgs.Exception.Message.Contains("Error 401 Unauthorized")) {
                _settingsController.Settings.AccessToken = null;
                _settingsController.Settings.AccessTokenSecret = null;
                _settingsController.Save();
            }
            _globalVariables.IsLoggedIn = false;
        }

        private void StreamOnNonMatchingTweetReceived(TweetEventArgs tweetEventArgs, Action<ITweetInfo> addTweet) {
            var tweet = _tweetProcessor.RecievedTweetInfo(tweetEventArgs.Tweet);
            if (tweet == null) {
                return;
            }
            addTweet(tweet);
        }

        private void StreamOnMatchingTweetReceived(TweetEventArgs tweetEventArgs, Action<ITweetInfo> addTweet) {
            var tweet = _tweetProcessor.RecievedTweetInfo(tweetEventArgs.Tweet);
            if (tweet == null) {
                return;
            }
            addTweet(tweet);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}