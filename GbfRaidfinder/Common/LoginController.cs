using System.Diagnostics;
using System.Threading.Tasks;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Views;
using MaterialDesignThemes.Wpf;
using Tweetinvi;
using Tweetinvi.Models;

namespace GbfRaidfinder.Common {
    public class LoginController : ILoginController {
        private readonly ISettingsController _settingsController;

        private readonly ITwitterCredentials _twitterCredentials = new TwitterCredentials("cYX749T1Fryfp4pjAGa0NxpBt",
            "A1WxMPmFK7xooaGinBUM6nv4ysvL3nM23Xm83E2nRadqsizAnw");

        public LoginController(ISettingsController settingsController) {
            _settingsController = settingsController;
        }

        public string Pin { get; set; }

        public async Task<ITwitterCredentials> StartNewLogin() {
            await Task.Delay(1000);
            // Create a new set of credentials for the application.
            var appCredentials = _twitterCredentials;

            // Init the authentication process and store the related `AuthenticationContext`.
            var authenticationContext = AuthFlow.InitAuthentication(appCredentials);

            // Go to the URL so that Twitter authenticates the user and gives him a PIN code.
            Process.Start(authenticationContext.AuthorizationURL);
            var dialog = new LoginDialog {
                DataContext = this
            };
            await DialogHost.Show(dialog);
            // With this pin code it is now possible to get the credentials back from Twitter
            var userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(Pin, authenticationContext);
            // Use the user credentials in your application

            _settingsController.Settings.AccessToken = userCredentials.AccessToken;
            _settingsController.Settings.AccessTokenSecret = userCredentials.AccessTokenSecret;
            _settingsController.Save();
            return userCredentials;
        }
    }
}