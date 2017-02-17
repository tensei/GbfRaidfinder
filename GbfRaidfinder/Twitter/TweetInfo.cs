using GbfRaidfinder.Interfaces;
using PropertyChanged;

namespace GbfRaidfinder.Twitter {
    [ImplementPropertyChanged]
    public class TweetInfo : ITweetInfo {
        public string User { get; set; }
        public string Boss { get; set; }
        public string Time { get; set; }
        public string Id { get; set; }
        public string Language { get; set; }
        public bool Clicked { get; set; }
    }
}