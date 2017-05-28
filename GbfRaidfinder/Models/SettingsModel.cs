using System.ComponentModel;
using Newtonsoft.Json;
using PropertyChanged;

namespace GbfRaidfinder.Models {
    public class SettingsModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        [DefaultValue(true), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool GlobalCopy { get; set; } = true;

        [DefaultValue(true), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool GlobalSound { get; set; } = true;

        [DefaultValue(300), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public double BossFollowHeight { get; set; } = 200;

        [DefaultValue(false), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Autologin { get; set; }
    }
}