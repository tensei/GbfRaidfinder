using System;
using System.Collections.ObjectModel;
using System.IO;
using GbfRaidfinder.Data;
using GbfRaidfinder.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GbfRaidfinder.Common {
    public class RaidListController : IRaidlistController {
        private readonly string _configFile = Path.Combine(Directory.GetCurrentDirectory(), "Raidlist.json");
        public ObservableCollection<RaidListItem> RaidBossListItems { get; set; }

        public void Load() {
            if (File.Exists(_configFile)) {
                var input = File.ReadAllText(_configFile);

                var jsonSettings = new JsonSerializerSettings {
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    NullValueHandling = NullValueHandling.Ignore
                };
                RaidBossListItems = JsonConvert.DeserializeObject<ObservableCollection<RaidListItem>>(input,
                    jsonSettings);
            }
            else {
                RaidBossListItems = new ObservableCollection<RaidListItem>(Raids.RaidBosses);
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
            var output = JsonConvert.SerializeObject(RaidBossListItems, jsonSettings);

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