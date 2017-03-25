using GbfRaidfinder.Models;

namespace GbfRaidfinder.Interfaces {
    public interface IBlacklistController {
        BlacklistModel Blacklist { get; set; }
        void Load();
        void Save();
    }
}