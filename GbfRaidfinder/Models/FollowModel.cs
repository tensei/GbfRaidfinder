using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GbfRaidfinder.Common;
using GbfRaidfinder.Data;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Twitter;
using GbfRaidfinder.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;

namespace GbfRaidfinder.Models {
    [ImplementPropertyChanged]
    public class FollowModel {
        [JsonIgnore] public readonly ObservableCollection<TweetInfo> TweetInfos = new ObservableCollection<TweetInfo>();
        [JsonIgnore]
        public IBlacklistController BlacklistController { get; set; }
        public FollowModel(string jp, string en, string image, IBlacklistController blacklistController) {
            English = en;
            Japanese = jp;
            Image = image;
            CopyCommand = new ActionCommand(c => Copy((TweetInfo) c));
            BlacklistCommand = new ActionCommand(s => Blacklist((string) s));
            CopyUrlCommand = new ActionCommand(a => CopyUrl((string) a));
            TranslateCommand = new ActionCommand(async a => await TranslateMessage((TweetInfo)a));
            Tweets = new ReadOnlyObservableCollection<TweetInfo>(TweetInfos);
            if (blacklistController != null) {
                BlacklistController = blacklistController;
            }
        }

        public string English { get; set; }
        public string Japanese { get; set; }
        public string Image { get; set; }
        public bool ImageVisibility { get; set; }
        public bool AutoCopy { get; set; }
        public bool Sound { get; set; }
        public bool Translate { get; set; }

        [JsonIgnore]
        public List<SoundFileModel> SoundFiles => GetSoundFiles();

        public SoundFileModel SelectedSoundFile { get; set; }
        public int SelectedSoundFileIndex { get; set; }

        [JsonIgnore]
        public ReadOnlyObservableCollection<TweetInfo> Tweets { get; }

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
            TweetInfos.RemoveAll(u => u.User == user);
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
            catch (Exception ) {
                //ignore
            }
            tweetInfo.Clicked = !tweetInfo.Clicked;
        }

        private readonly WebClient _webClient = new WebClient{Headers = {
                {"Application-Id",Credentials.AppId }
        }};
        private async Task TranslateMessage(ITweetInfo tweet) {
            if (string.IsNullOrWhiteSpace(tweet.Text)) {
                return;
            }

            const string target = "en";
            const string source = "ja";
            var link = $"http://tensei.moe/api/v1/translate?t={target}&s={source}&q={tweet.Text}";

            try {
                var response = await _webClient.DownloadStringTaskAsync(new Uri(link));
                if (response == "error") { 
                    return;
                }
                tweet.Text = response;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
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