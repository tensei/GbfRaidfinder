using System;
using System.Threading.Tasks;
using System.Windows;
using GbfRaidfinder.Interfaces;
using PropertyChanged;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace GbfRaidfinder.Twitter {
    [ImplementPropertyChanged]
    public class TweetObserver : ITweetObserver {
        private readonly ISettingsController _settingsController;

        public TweetObserver(ISettingsController settingsController) {
            Stream = Tweetinvi.Stream.CreateFilteredStream();
            _settingsController = settingsController;
        }

        public IFilteredStream Stream { get; }

        public bool Running { get; set; }

        public void Run(ITwitterCredentials userCredentials) {
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackOnly;
            Stream.Credentials = userCredentials;
            for (var i = 5; i < 151; i += 5) {
                Stream.AddTrack($"Lv{i}");
            }
            Stream.AddTrack("I need backup!Battle ID:");
            //stream.AddTrack("Lv");
            Start();
        }

        private async void Start() {
            await Task.Delay(3000);
            try {
                Running = true;
                await Stream.StartStreamMatchingAllConditionsAsync().ConfigureAwait(false);
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
    }
}