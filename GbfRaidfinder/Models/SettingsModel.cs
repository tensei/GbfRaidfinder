using System.Collections.Generic;

namespace GbfRaidfinder.Models {
    public class SettingsModel {
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        public Dictionary<string, string> Raids { get; set; } = new Dictionary<string, string>();
    }
}