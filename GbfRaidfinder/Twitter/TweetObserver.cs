using System;
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
    [ImplementPropertyChanged]
    public class TweetObserver : ITweetObserver {
        private readonly IFilteredStream _stream;
        private ISettingsController _settingsController;

        public TweetObserver() {
            _stream = Stream.CreateFilteredStream();
        }

        public bool Running { get; set; }

        public void Run(ITwitterCredentials userCredentials, ISettingsController settingsController) {
            _settingsController = settingsController;
           
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackOnly;
            _stream.Credentials = userCredentials;
            for (var i = 5; i < 151; i += 5) {
                _stream.AddTrack($"Lv{i}");
            }
            _stream.AddTrack("I need backup!Battle ID:");
            //stream.AddTrack("Lv");
            _stream.DisconnectMessageReceived += StreamOnDisconnectMessageReceived;
            Start();
        }

        public void Subscribe(EventHandler<TweetEventArgs> action, EventHandler<MatchedTweetReceivedEventArgs> match) {
            _stream.NonMatchingTweetReceived += action;
            _stream.MatchingTweetReceived += match;
        }

        private async void Start() {
            await Task.Delay(3000);
            try {
                Running = true;
                await _stream.StartStreamMatchingAllConditionsAsync();
            }
            catch (TwitterInvalidCredentialsException) {
                Running = false;
                _settingsController.Settings.AccessToken = null;
                _settingsController.Settings.AccessTokenSecret = null;
                _settingsController.Save();
                MessageBox.Show("click Login again!", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception) {
                Running = false;
            }
        }

        private void StreamOnDisconnectMessageReceived(object sender, DisconnectedEventArgs disconnectedEventArgs) {
            Start();
        }

        public void Stop() {
            _stream.StopStream();
        }
    }
}