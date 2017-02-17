using System.Collections.ObjectModel;
using GbfRaidfinder.Data;
using GbfRaidfinder.Models;

namespace GbfRaidfinder.Interfaces {
    public interface IRaidlistController {
        ObservableCollection<RaidListItem> RaidBossListItems { get; set; }
        void Load();
        void Save();
    }
}