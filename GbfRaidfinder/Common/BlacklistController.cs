using System;
using System.Collections.ObjectModel;
using System.IO;
using GbfRaidfinder.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GbfRaidfinder.Common {
    public class BlacklistController : IBlacklistController {
        private readonly string _configFile = Path.Combine(Directory.GetCurrentDirectory(), "Blacklist.json");
        public ObservableCollection<string> Blacklist { get; set; }

        public void Load() {
            if (File.Exists(_configFile)) {
                var input = File.ReadAllText(_configFile);

                var jsonSettings = new JsonSerializerSettings {
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    NullValueHandling = NullValueHandling.Ignore
                };
                Blacklist = JsonConvert.DeserializeObject<ObservableCollection<string>>(input, jsonSettings);
            }
            else {
                Blacklist = new ObservableCollection<string>();
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
            var output = JsonConvert.SerializeObject(Blacklist, jsonSettings);

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