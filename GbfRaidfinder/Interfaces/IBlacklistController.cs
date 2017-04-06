using System.Collections.ObjectModel;
using GbfRaidfinder.Models;

namespace GbfRaidfinder.Interfaces {
    public interface IBlacklistController {
        ObservableCollection<string> Blacklist { get; set; }
        void Load();
        void Save();
    }
}