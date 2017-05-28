using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using GbfRaidfinder.Data;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using PropertyChanged;
using Tweetinvi;
using Tweetinvi.Models;

namespace GbfRaidfinder.Twitter {
    public class TwitterAuthenticator : ITwitterAuthenticator, INotifyPropertyChanged {
        
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ISettingsController _settingsController;
        private IAuthenticationContext _authenticationContext;

        public TwitterAuthenticator(ISettingsController settingsController) {
            _settingsController = settingsController;
        }

        public string Pin { get; set; }

        public async Task<bool> AuthenticateUser() {
            const string consumerKey = Credentials.TwitterConsumerKey;
            const string consumerSecret = Credentials.TwitterConsumerSecret;
            var userCredentials = GetCredentials();
            if (userCredentials != null) {
                Auth.SetUserCredentials(consumerKey, consumerSecret,
                    userCredentials.AccessToken, userCredentials.AccessTokenSecret);
                return true;
            }
            var appCredentials = new TwitterCredentials(consumerKey, consumerSecret);
            _authenticationContext = AuthFlow.InitAuthentication(appCredentials);

            if (_authenticationContext == null) {
                return false;
            }
            Process.Start(_authenticationContext.AuthorizationURL);
            Pin = string.Empty;
            var dialog = new LoginDialog {
                DataContext = this
            };
            await DialogHost.Show(dialog);
            if (string.IsNullOrWhiteSpace(Pin)) {
                return false;
            }

            CreateAndSetCredentials(Pin);
            return true;
        }

        public void CreateAndSetCredentials(string pinCode) {
            var userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(pinCode, _authenticationContext);

            Auth.SetCredentials(userCredentials);
            SaveCredentials(userCredentials);
        }

        public void SaveCredentials(ITwitterCredentials credentials) {
            if (credentials == null) {
                return;
            }
            _settingsController.Settings.AccessTokenSecret = credentials.AccessTokenSecret;
            _settingsController.Settings.AccessToken = credentials.AccessToken;
            _settingsController.Save();
        }

        private ITwitterCredentials GetCredentials() {
            if (string.IsNullOrWhiteSpace(_settingsController.Settings.AccessToken)
                || string.IsNullOrWhiteSpace(_settingsController.Settings.AccessTokenSecret)) {
                return null;
            }
            return new TwitterCredentials("", "", _settingsController.Settings.AccessToken,
                _settingsController.Settings.AccessTokenSecret);
        }
    }
}