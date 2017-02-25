using GbfRaidfinder.Common;
using GbfRaidfinder.Interfaces;

namespace GbfRaidfinder.Factorys {
    public class ControllerFactory {
        public ControllerFactory(ISettingsController settingsController, ITweetObserver tweetObserver,
            IRaidsController raidsController, IRaidlistController raidlistController) {
            GetSettingsController = settingsController;
            GetTweetObserver = tweetObserver;
            GetRaidsController = raidsController;
            GetRaidlistController = raidlistController;

            GetSettingsController.Load();
            GetRaidsController.Load();
            GetRaidlistController.Load();
        }

        public ISettingsController GetSettingsController { get; }

        public IRaidsController GetRaidsController { get; }

        public IRaidlistController GetRaidlistController { get; }

        public ITweetObserver GetTweetObserver { get; }

        public ILoginController CreateLoginController => new LoginController(GetSettingsController);
    }
}