using GbfRaidfinder.Models;

namespace GbfRaidfinder.Interfaces {
    public interface ISettingsController {
        SettingsModel Settings { get; set; }
        void Load();
        void Save();
    }
}