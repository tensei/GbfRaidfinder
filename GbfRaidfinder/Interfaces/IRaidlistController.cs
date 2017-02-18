using System.Collections.ObjectModel;
using GbfRaidfinder.Data;

namespace GbfRaidfinder.Interfaces {
    public interface IRaidlistController {
        ObservableCollection<RaidListItem> RaidBossListItems { get; set; }
        void Load();
        void Save();
    }
}