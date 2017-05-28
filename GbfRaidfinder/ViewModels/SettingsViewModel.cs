using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Models;
using PropertyChanged;
using Tweetinvi;

namespace GbfRaidfinder.ViewModels {
    public class SettingsViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public IGlobalVariables GlobalVariables { get; }
        private readonly ITweetObserver _observer;
        public SettingsModel Settings { get; }
        private ISettingsController SettingsController { get; }

        public SettingsViewModel(ISettingsController settingsController, IGlobalVariables globalVariables, ITweetObserver observer) {
            GlobalVariables = globalVariables;
            _observer = observer;
            Settings = settingsController.Settings;
            SettingsController = settingsController;
            LogoutCommand = new ActionCommand(async () => await Logout());
        }

        public ICommand LogoutCommand { get; }

        private async Task Logout() {
            if (GlobalVariables.IsLoggedIn) {
                _observer.Stop();
                Auth.Credentials = null;
                await Task.Delay(2000);
                GlobalVariables.IsLoggedIn = false;
                Settings.AccessToken = null;
                Settings.AccessTokenSecret = null;
                Settings.Autologin = false;
                SettingsController.Save();
            }
        }
    }
}
