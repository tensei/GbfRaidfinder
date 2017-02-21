using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GbfRaidfinder.Data;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Twitter;
using GbfRaidfinder.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;

namespace GbfRaidfinder.Models {
    [ImplementPropertyChanged]
    public class FollowModel {
        [JsonIgnore]
        public readonly ObservableCollection<TweetInfo> TweetInfos = new ObservableCollection<TweetInfo>();

        public FollowModel(string jp, string en, string image) {
            English = en;
            Japanese = jp;
            Image = image;
            CopyCommand = new ActionCommand(c => Copy((TweetInfo) c));
            Tweets = new ReadOnlyObservableCollection<TweetInfo>(TweetInfos);
        }

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
        public ReadOnlyObservableCollection<TweetInfo> Tweets { get; }

        [JsonIgnore]
        public ICommand CopyCommand { get; }
        
        private void Copy(ITweetInfo tweetInfo) {
            Clipboard.SetText(tweetInfo.Id);
            tweetInfo.Clicked = !tweetInfo.Clicked;
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