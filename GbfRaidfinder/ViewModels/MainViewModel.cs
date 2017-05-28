using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GbfRaidfinder.Common;
using GbfRaidfinder.Data;
using GbfRaidfinder.Factorys;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Models;
using GbfRaidfinder.Views.Dialogs;
using MaterialDesignThemes.Wpf;
using PropertyChanged;
using Tweetinvi;
using Tweetinvi.Models;

namespace GbfRaidfinder.ViewModels {
    public class MainViewModel :INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public IGlobalVariables GlobalVariables { get; }
        private readonly IBlacklistController _blacklistController;

        private readonly IControllerFactory _controllerFactory;
        private readonly IRaidsController _raidsController;
        private readonly SoundPlayer _soudplayer = new SoundPlayer(@"assets\notification.wav");
        private readonly ITweetObserver _tweetObserver;
        private readonly ITwitterAuthenticator _twitterAuthenticator;

        private IAuthenticatedUser _user;

        public MainViewModel(IControllerFactory controllerFactory,
            ITwitterAuthenticator twitterAuthenticator, IGlobalVariables globalVariables, SettingsViewModel settingsViewModel) {
            GlobalVariables = globalVariables;
            _raidsController = controllerFactory.GetRaidsController;
            _tweetObserver = controllerFactory.GetTweetObserver;
            var settingsController = controllerFactory.GetSettingsController;
            _blacklistController = controllerFactory.GetBlacklistController;
            _controllerFactory = controllerFactory;
            _twitterAuthenticator = twitterAuthenticator;
            Follows = _raidsController.Follows;
            SettingsDataContext = settingsViewModel;
            RaidListCtx = new RaidListViewModel(controllerFactory);
            RaidBosses =
                new ReadOnlyObservableCollection<RaidListItem>(
                    controllerFactory.GetRaidlistController.RaidBossListItems);
            Settings = settingsController.Settings;
            RemoveCommand = new ActionCommand(r => Remove((string) r));
            StartLoginCommand = new ActionCommand(async () => await Startup());
            AddNewRaidCommand =
                new ActionCommand(async () => await DialogHost.Show(new AddRaidDialog(controllerFactory
                    .GetRaidlistController.RaidBossListItems)));
            ChangeViewCommand = new ActionCommand(i => ChangeView((string)i));

            _tweetObserver.SetAddAction(AddTweet);
            CheckUpdate();
            Id = UniqueId.Create();
            if (settingsController.Settings.Autologin && 
                !string.IsNullOrWhiteSpace(settingsController.Settings.AccessToken) && 
                !string.IsNullOrWhiteSpace(settingsController.Settings.AccessTokenSecret)) {
                Startup().ConfigureAwait(false);
            }
            //Console.WriteLine(new List<string>()[4]);
        }

        public SettingsModel Settings { get; set; }
        public string Id { get; set; }
        public int TweetCount { get; set; }

        public ObservableCollection<FollowModel> Follows { get; }
        public ReadOnlyObservableCollection<RaidListItem> RaidBosses { get; }

        public ICommand StartLoginCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand AddNewRaidCommand { get; }

        public RaidListViewModel RaidListCtx { get; set; }

        public string LoginButtonText { get; set; }
        
        public int TransitionerIndex { get; set; }

        public ICommand ChangeViewCommand { get; }

        public SettingsViewModel SettingsDataContext { get; }

        private void ChangeView(string viewIndex) {
            var index = int.Parse(viewIndex);
            if (index == TransitionerIndex) {
                TransitionerIndex = 0;
                return;
            }
            TransitionerIndex = index;
        }

        private async Task Startup() {
            if (_tweetObserver.Stream.StreamState == StreamState.Running
                || _tweetObserver.Stream.StreamState == StreamState.Pause) {
                _tweetObserver.Stream.StopStream();
            }
            //_tweetObserver.Stream?.StopStream();
            if (await _twitterAuthenticator.AuthenticateUser()) {
                _tweetObserver.Run();
                if (_user == null) {
                    _user = User.GetAuthenticatedUser();
                    LoginStatus($"{_user?.Name}");
                    GlobalVariables.IsLoggedIn = _user.Name != null;
                }
            }
        }

        private void AddTweet(ITweetInfo tweet) {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    var follow =
                        _raidsController.Follows.FirstOrDefault(
                            f => f.English.Contains(tweet.Boss.Trim()) || f.Japanese.Contains(tweet.Boss.Trim()));
                    if (_blacklistController.Blacklist.Contains(tweet.User)) {
                        return;
                    }
                    follow?.TweetInfos.Insert(0, tweet);
                    TweetCount++;
                    if (follow != null && follow.AutoCopy && Settings.GlobalCopy) {
                        try {
                            Clipboard.SetDataObject(tweet.Id);
                        }
                        catch (Exception) {
                            //ignore
                        }
                    }
                    if (follow != null && follow.Sound && Settings.GlobalSound) {
                        try {
                            _soudplayer.SoundLocation = follow.SelectedSoundFile?.Path;
                            _soudplayer.Play();
                        }
                        catch (Exception) {
                            //no file
                        }
                    }
                    if (follow?.TweetInfos.Count > 30) {
                        follow.TweetInfos.RemoveAt(follow.TweetInfos.Count - 1);
                    }
                }));
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private void Remove(string name) {
            _raidsController.Follows.Remove(f => f.Japanese == name || f.English == name);
            _controllerFactory.GetRaidlistController.RaidBossListItems.First(
                f => f.Japanese == name || f.English == name).Following = false;
            _raidsController.Save();
        }

        private async void CheckUpdate() {
            if (await UpdateController.Check()) {
                await DialogHost.Show(new UpdateDialog());
            }
        }

        private void LoginStatus(string text) {
            LoginButtonText = text;
        }
    }
}