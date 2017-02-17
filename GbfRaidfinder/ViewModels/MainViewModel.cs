using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Input;
using GbfRaidfinder.Data;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Models;
using GbfRaidfinder.Twitter;
using Hardcodet.Wpf.TaskbarNotification;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tweetinvi.Events;

namespace GbfRaidfinder.ViewModels {
    public class MainViewModel {
        private readonly IRaidsController _raidsController;
        private readonly ITweetProcessor _tweetProcessor;
        private readonly SoundPlayer _soudplayer = new SoundPlayer(@"assets\notification.wav");
        private TaskbarIcon _taskbarIcon;

        public MainViewModel(Raids raids, ITweetProcessor tweetProcessor, IRaidsController raidsController, IRaidlistController raidlistController,
            ISettingsController settingsController, ILoginController loginController, TaskbarIcon taskbarIcon) {
            _tweetProcessor = tweetProcessor;
            settingsController.Load();
            _raidsController = raidsController;
            _raidsController.Load();
            _taskbarIcon = taskbarIcon;
            raidlistController.Load();
            Follows = new ReadOnlyObservableCollection<FollowModel>(raidsController.Follows);
            RaidBosses = new ReadOnlyObservableCollection<RaidListItem>(raidlistController.RaidBossListItems);
            
            AddCommand = new ActionCommand(r => Add((RaidListItem) r));
            RemoveCommand = new ActionCommand(r => Remove((string) r));
            StartLoginCommand = new ActionCommand(loginController.StartLogin);
            MoveLeftCommand = new ActionCommand(f => MoveLeft((FollowModel) f));
            MoveRightCommand = new ActionCommand(f => MoveRight((FollowModel) f));
            //TweetObservers = new ReadOnlyObservableCollection<TweetObserver>();
            //tweetProcessor._tweetObservers.Add(new TweetObserver("Lv75 シュヴァリエ・マグナ", "Lvl 75 Luminiera Omega", Application.Current.Dispatcher));

            loginController.TweetObserver.Subscribe(StreamOnNonMatchingTweetReceived, StreamOnNonMatchingTweetReceived);
        }

        public ReadOnlyObservableCollection<FollowModel> Follows { get; }
        private readonly ObservableCollection<RaidListItem> _raidBosses = new ObservableCollection<RaidListItem>();
        public ReadOnlyObservableCollection<RaidListItem> RaidBosses { get; }

        public ICommand StartLoginCommand { get; }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand MoveLeftCommand { get; }
        public ICommand MoveRightCommand { get; }

        private void StreamOnNonMatchingTweetReceived(object sender, TweetEventArgs tweetEventArgs) {
            var tweet = _tweetProcessor.RecievedTweetInfo(tweetEventArgs.Tweet);
            if (tweet == null) {
                return;
            }
            AddTweet(tweet);
        }

        private void StreamOnNonMatchingTweetReceived(object sender, MatchedTweetReceivedEventArgs tweetEventArgs) {
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
                    follow?._TweetInfos.Insert(0, tweet);
                    if (follow != null && follow.AutoCopy) {
                        Clipboard.SetText(tweet.Id);
                    }
                    if (follow != null && follow.Sound) {
                        try {
                            _soudplayer.Play();
                        }
                        catch (Exception) {
                            //no file
                        }
                    }
                    if (follow?._TweetInfos.Count > 30) {
                        follow._TweetInfos.RemoveAt(follow._TweetInfos.Count - 1);
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