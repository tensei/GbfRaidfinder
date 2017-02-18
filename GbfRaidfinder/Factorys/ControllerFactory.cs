using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GbfRaidfinder.Common;
using GbfRaidfinder.Interfaces;

namespace GbfRaidfinder.Factorys {
    public class ControllerFactory {
        private readonly ISettingsController _settingsController;
        private readonly ITweetObserver _tweetObserver;
        private readonly IRaidsController _raidsController;
        private readonly IRaidlistController _raidlistController;
        public ControllerFactory(ISettingsController settingsController, ITweetObserver tweetObserver, 
            IRaidsController raidsController, IRaidlistController raidlistController) {
            _settingsController = settingsController;
            _tweetObserver = tweetObserver;
            _raidsController = raidsController;
            _raidlistController = raidlistController;

            _settingsController.Load();
            _raidsController.Load();
            _raidlistController.Load();
        }

        public ISettingsController GetSettingsController => _settingsController;
        public IRaidsController GetRaidsController => _raidsController;
        public IRaidlistController GetRaidlistController => _raidlistController;
        public ITweetObserver GetTweetObserver => _tweetObserver;
        public ILoginController CreateLoginController => new LoginController(_settingsController);
    }
}
