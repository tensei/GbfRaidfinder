using System.Collections.ObjectModel;

namespace GbfRaidfinder.Interfaces {
    public interface IBlacklistController {
        ObservableCollection<string> Blacklist { get; set; }
        void Load();
        void Save();
    }
}