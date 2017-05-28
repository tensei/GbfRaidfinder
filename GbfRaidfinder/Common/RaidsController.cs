using System;
using System.Collections.ObjectModel;
using System.IO;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GbfRaidfinder.Common {
    public class RaidsController : IRaidsController {
        private readonly IBlacklistController _blacklistController;
        private readonly string _configFile = Path.Combine(Directory.GetCurrentDirectory(), "Raids.json");

        public RaidsController(IBlacklistController blacklistController) {
            _blacklistController = blacklistController;
        }

        public ObservableCollection<FollowModel> Follows { get; set; }

        public void Load() {
            if (File.Exists(_configFile)) {
                var input = File.ReadAllText(_configFile);

                var jsonSettings = new JsonSerializerSettings {
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    NullValueHandling = NullValueHandling.Ignore
                };
                Follows = JsonConvert.DeserializeObject<ObservableCollection<FollowModel>>(input, jsonSettings);
                foreach (var followModel in Follows) {
                    followModel.BlacklistController = _blacklistController;
                }
            }
            else {
                Follows = new ObservableCollection<FollowModel>();
            }
            Save();
        }

        public void Save() {
            var jsonSettings = new JsonSerializerSettings {
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.Populate,
                Formatting = Formatting.Indented
            };
            jsonSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
            var output = JsonConvert.SerializeObject(Follows, jsonSettings);

            var folder = Path.GetDirectoryName(_configFile);
            if (folder != null && !Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
            try {
                File.WriteAllText(_configFile, output);
            }
            catch (Exception) {
                //ignore
            }
        }
    }
}