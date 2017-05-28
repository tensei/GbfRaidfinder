using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using GbfRaidfinder.Data;
using GbfRaidfinder.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GbfRaidfinder.Common {
    public class RaidListController : IRaidlistController {
        private readonly string _configFile = Path.Combine(Directory.GetCurrentDirectory(), "Raidlist.json");
        public ObservableCollection<RaidListItem> RaidBossListItems { get; set; }

        public void Load() {
            ObservableCollection<RaidListItem> remote;
            try {
                var web = new WebClient {
                    Encoding = Encoding.UTF8
                };
                var js = web.DownloadString(
                    "https://raw.githubusercontent.com/tensei/GbfRaidfinder/master/List/Raidlist.json");

                remote = JsonConvert.DeserializeObject<ObservableCollection<RaidListItem>>(js);
            }
            catch {
                remote = new ObservableCollection<RaidListItem>(Raids.RaidBosses);
            }
            ObservableCollection<RaidListItem> local;
            if (File.Exists(_configFile)) {
                var input = File.ReadAllText(_configFile);

                var jsonSettings = new JsonSerializerSettings {
                    ObjectCreationHandling = ObjectCreationHandling.Auto,
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    NullValueHandling = NullValueHandling.Ignore
                };
                local = JsonConvert.DeserializeObject<ObservableCollection<RaidListItem>>(input,
                    jsonSettings);
            }
            else {
                local = new ObservableCollection<RaidListItem>(Raids.RaidBosses);
            }
            CombineLists(local.ToList(), remote.ToList());
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

        private void CombineLists(List<RaidListItem> local, List<RaidListItem> remote) {
            var finallist = new ObservableCollection<RaidListItem>();
            foreach (var t in remote) {
                finallist.Add(t);
            }
            var finalenamesEn = finallist.Select(l => l.English.ToLower());
            var finenamesJa = finallist.Select(l => l.Japanese.ToLower());

            for (var i = 0; i < local.Count; i++) {
                if (!finalenamesEn.Contains(local[i].English.ToLower()) &&
                    !finenamesJa.Contains(local[i].Japanese.ToLower())) {
                    try {
                        finallist.Insert(i, local[i]);
                    }
                    catch (Exception) {
                        finallist.Add(local[i]);
                    }
                }
            }
            RaidBossListItems = finallist;
            Save();
        }
    }
}