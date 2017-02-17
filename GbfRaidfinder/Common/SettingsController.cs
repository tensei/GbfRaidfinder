using System;
using System.IO;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GbfRaidfinder.Common {
    internal class SettingsController : ISettingsController {
        private readonly string ConfigFile = Path.Combine(Directory.GetCurrentDirectory(), "Settings.json");
        public SettingsModel Settings { get; set; }

        public void Load() {
            if (File.Exists(ConfigFile)) {
                var input = File.ReadAllText(ConfigFile);

                var jsonSettings = new JsonSerializerSettings {
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    NullValueHandling = NullValueHandling.Ignore
                };
                Settings = JsonConvert.DeserializeObject<SettingsModel>(input, jsonSettings);
            }
            else {
                Settings = new SettingsModel();
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
            var output = JsonConvert.SerializeObject(Settings, jsonSettings);

            var folder = Path.GetDirectoryName(ConfigFile);
            if (folder != null && !Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
            try {
                File.WriteAllText(ConfigFile, output);
            }
            catch (Exception) {
                //ignore
            }
        }
    }
}