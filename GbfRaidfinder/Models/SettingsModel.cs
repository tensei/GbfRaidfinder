using System.ComponentModel;
using Newtonsoft.Json;
using PropertyChanged;

namespace GbfRaidfinder.Models {
    [ImplementPropertyChanged]
    public class SettingsModel {
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        [DefaultValue(true), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool GlobalCopy { get; set; } = true;

        [DefaultValue(true), JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool GlobalSound { get; set; } = true;
    }
}