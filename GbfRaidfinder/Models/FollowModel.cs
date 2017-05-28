using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GbfRaidfinder.Common;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;

namespace GbfRaidfinder.Models {
    public class FollowModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        [JsonIgnore]
        public readonly ObservableCollection<ITweetInfo> TweetInfos = new ObservableCollection<ITweetInfo>();

        public FollowModel(string jp, string en, string image, IBlacklistController blacklistController) {
            English = en;
            Japanese = jp;
            Image = image;
            CopyCommand = new ActionCommand(c => Copy((ITweetInfo) c));
            BlacklistCommand = new ActionCommand(s => Blacklist((string) s));
            CopyUrlCommand = new ActionCommand(a => CopyUrl((string) a));
            TranslateCommand = new ActionCommand(async a => await TranslateMessage((ITweetInfo) a));
            Tweets = new ReadOnlyObservableCollection<ITweetInfo>(TweetInfos);
            if (blacklistController != null) {
                BlacklistController = blacklistController;
            }
        }

        [JsonIgnore]
        public IBlacklistController BlacklistController { get; set; }

        public string English { get; set; }
        public string Japanese { get; set; }
        public string Image { get; set; }
        public bool ImageVisibility { get; set; }
        public bool AutoCopy { get; set; }
        public bool Sound { get; set; }

        [JsonIgnore]
        public List<SoundFileModel> SoundFiles => GetSoundFiles();

        public SoundFileModel SelectedSoundFile { get; set; }
        public int SelectedSoundFileIndex { get; set; }

        [JsonIgnore]
        public ReadOnlyObservableCollection<ITweetInfo> Tweets { get; }

        [JsonIgnore]
        public ICommand CopyCommand { get; }

        [JsonIgnore]
        public ICommand BlacklistCommand { get; }

        [JsonIgnore]
        public ICommand CopyUrlCommand { get; }

        [JsonIgnore]
        public ICommand TranslateCommand { get; }

        private void Blacklist(string user) {
            if (BlacklistController.Blacklist.Contains(user)) {
                return;
            }
            BlacklistController.Blacklist.Add(user);
            TweetInfos.Remove(u => u.User == user);
        }

        private void CopyUrl(string url) {
            try {
                Clipboard.SetDataObject(url);
            }
            catch (Exception) {
                //ignore
            }
        }

        private void Copy(ITweetInfo tweetInfo) {
            try {
                Clipboard.SetDataObject(tweetInfo.Id);
            }
            catch (Exception) {
                //ignore
            }
            tweetInfo.Clicked = !tweetInfo.Clicked;
        }

        private async Task TranslateMessage(ITweetInfo tweet) {
            await tweet.Translate();
        }

        private List<SoundFileModel> GetSoundFiles() {
            var assets = Path.Combine(Directory.GetCurrentDirectory(), "assets");
            var soundfiles = Directory.GetFiles(assets, "*.wav");
            return soundfiles.Select(soundfile => new SoundFileModel {
                Name = soundfile.Split('\\').Last(),
                Path = soundfile
            }).ToList();
        }
    }
}