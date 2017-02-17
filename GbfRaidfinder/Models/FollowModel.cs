using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Twitter;
using GbfRaidfinder.ViewModels;
using Newtonsoft.Json;

namespace GbfRaidfinder.Models {
    public class FollowModel {
        [JsonIgnore] public ObservableCollection<TweetInfo> _TweetInfos = new ObservableCollection<TweetInfo>();

        public FollowModel(string jp, string en, string image) {
            English = en;
            Japanese = jp;
            Image = image;
            CopyCommand = new ActionCommand(c => Copy((TweetInfo) c));
            Tweets = new ReadOnlyObservableCollection<TweetInfo>(_TweetInfos);
        }

        public string English { get; set; }
        public string Japanese { get; set; }
        public string Image { get; set; }
        public bool ImageVisibility { get; set; }
        public bool AutoCopy { get; set; }
        public bool Sound { get; set; }

        [JsonIgnore]
        public ReadOnlyObservableCollection<TweetInfo> Tweets { get; }

        [JsonIgnore]
        public ICommand CopyCommand { get; }


        private void Copy(ITweetInfo tweetInfo) {
            Clipboard.SetText(tweetInfo.Id);
            tweetInfo.Clicked = !tweetInfo.Clicked;
        }
    }
}