using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Input;
using GbfRaidfinder.Common;
using GbfRaidfinder.Data;
using GbfRaidfinder.Factorys;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Models;
using GbfRaidfinder.Twitter;
using PropertyChanged;
using Tweetinvi.Events;
using Tweetinvi.Models;

namespace GbfRaidfinder.ViewModels {
    [ImplementPropertyChanged]
    public class MainViewModel {
        private readonly ILoginController _loginController;
        private readonly IRaidsController _raidsController;
        private readonly ISettingsController _settingsController;
        private readonly SoundPlayer _soudplayer = new SoundPlayer(@"assets\notification.wav");
        private readonly ITweetObserver _tweetObserver;
        private readonly ITweetProcessor _tweetProcessor;

        private readonly ITwitterCredentials _twitterCredentials = new TwitterCredentials("cYX749T1Fryfp4pjAGa0NxpBt",
            "A1WxMPmFK7xooaGinBUM6nv4ysvL3nM23Xm83E2nRadqsizAnw");

        public MainViewModel(ITweetProcessor tweetProcessor, ControllerFactory controllerFactory) {
            _tweetProcessor = tweetProcessor;
            _raidsController = controllerFactory.GetRaidsController;
            _loginController = controllerFactory.CreateLoginController;
            _tweetObserver = controllerFactory.GetTweetObserver;
            _settingsController = controllerFactory.GetSettingsController;
            Follows = new ReadOnlyObservableCollection<FollowModel>(_raidsController.Follows);
            RaidBosses =
                new ReadOnlyObservableCollection<RaidListItem>(controllerFactory.GetRaidlistController.RaidBossListItems);
            Settings = _settingsController.Settings;
            AddCommand = new ActionCommand(r => Add((RaidListItem) r));
            RemoveCommand = new ActionCommand(r => Remove((string) r));
            StartLoginCommand = new ActionCommand(() => _loginController.StartNewLogin());
            MoveLeftCommand = new ActionCommand(f => MoveLeft((FollowModel) f));
            MoveRightCommand = new ActionCommand(f => MoveRight((FollowModel) f));

            _tweetObserver.Stream.MatchingTweetReceived += StreamOnMatchingTweetReceived;
            _tweetObserver.Stream.NonMatchingTweetReceived += StreamOnNonMatchingTweetReceived;
            _tweetObserver.Stream.StreamStopped += StreamOnStreamStopped;
            _tweetObserver.Stream.DisconnectMessageReceived += StreamOnDisconnectMessageReceived;
            Startup();
        }

        public SettingsModel Settings { get; set; }

        public ReadOnlyObservableCollection<FollowModel> Follows { get; }
        public ReadOnlyObservableCollection<RaidListItem> RaidBosses { get; }

        public ICommand StartLoginCommand { get; }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand MoveLeftCommand { get; }
        public ICommand MoveRightCommand { get; }


        private void Startup() {
            if (string.IsNullOrWhiteSpace(_settingsController.Settings.AccessToken) &&
                string.IsNullOrWhiteSpace(_settingsController.Settings.AccessTokenSecret)) {
                return;
            }
            _twitterCredentials.AccessTokenSecret = _settingsController.Settings.AccessTokenSecret;
            _twitterCredentials.AccessToken = _settingsController.Settings.AccessToken;
            _tweetObserver.Run(_twitterCredentials);
        }

        private void StreamOnDisconnectMessageReceived(object sender, DisconnectedEventArgs disconnectedEventArgs) {
            _tweetObserver.Running = false;
            Startup();
        }

        private void StreamOnStreamStopped(object sender, StreamExceptionEventArgs streamExceptionEventArgs) {
            var ok = MessageBox.Show("Something went wrong relog please!", "Error", MessageBoxButton.OKCancel,
                MessageBoxImage.Error);
            if (ok != MessageBoxResult.OK) {
                return;
            }
            Application.Current.Dispatcher.BeginInvoke(new Action(async () => {
                var creds = await _loginController.StartNewLogin();
                _tweetObserver.Run(creds);
            }));
        }

        private void StreamOnNonMatchingTweetReceived(object sender, TweetEventArgs tweetEventArgs) {
            var tweet = _tweetProcessor.RecievedTweetInfo(tweetEventArgs.Tweet);
            if (tweet == null) {
                return;
            }
            AddTweet(tweet);
        }

        private void StreamOnMatchingTweetReceived(object sender, MatchedTweetReceivedEventArgs tweetEventArgs) {
            var tweet = _tweetProcessor.RecievedTweetInfo(tweetEventArgs.Tweet);
            if (tweet == null) {
                return;
            }
            AddTweet(tweet);
        }


        private void AddTweet(TweetInfo tweet) {
            try {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    var follow =
                        _raidsController.Follows.FirstOrDefault(
                            f => f.English.Contains(tweet.Boss.Trim()) || f.Japanese.Contains(tweet.Boss.Trim()));
                    follow?.TweetInfos.Insert(0, tweet);
                    if (follow != null && follow.AutoCopy && Settings.GlobalCopy) {
                        Clipboard.SetText(tweet.Id);
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

        private void Add(RaidListItem raidBoss) {
            try {
                if (_raidsController.Follows.Select(f => f.English).ToList().Contains(raidBoss.English)) {
                    return;
                }
                _raidsController.Follows.Add(new FollowModel(raidBoss.Japanese, raidBoss.English, raidBoss.Image));
                _raidsController.Save();
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private void Remove(string name) {
            _raidsController.Follows.RemoveAll(f => f.Japanese == name || f.English == name);
            _raidsController.Save();
        }

        private void MoveLeft(FollowModel followModel) {
            var index = 0;
            for (var i = 0; i < _raidsController.Follows.Count; i++) {
                if (followModel.English != _raidsController.Follows[i].English) {
                    continue;
                }
                index = i;
                break;
            }
            if (index == 0) {
                return;
            }
            _raidsController.Follows.RemoveAt(index);
            _raidsController.Follows.Insert(index - 1, followModel);
            _raidsController.Save();
        }

        private void MoveRight(FollowModel followModel) {
            var index = 0;
            for (var i = 0; i < _raidsController.Follows.Count; i++) {
                if (followModel.English != _raidsController.Follows[i].English) {
                    continue;
                }
                index = i;
                break;
            }
            if (index == _raidsController.Follows.Count - 1) {
                return;
            }
            _raidsController.Follows.RemoveAt(index);
            _raidsController.Follows.Insert(index + 1, followModel);
            _raidsController.Save();
        }
    }
}