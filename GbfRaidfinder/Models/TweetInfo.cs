using System.ComponentModel;
using GbfRaidfinder.Interfaces;
using PropertyChanged;

namespace GbfRaidfinder.Models {
    public class TweetInfo : ITweetInfo, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Profile { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public string Boss { get; set; }
        public string Time { get; set; }
        public string Id { get; set; }
        public string Language { get; set; }
        public bool Clicked { get; set; }
        public bool Translated { get; set; }
        public string Avatar { get; set; }
    }
}