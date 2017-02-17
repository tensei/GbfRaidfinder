using System.Collections.ObjectModel;
using GbfRaidfinder.Models;

namespace GbfRaidfinder.Interfaces {
    public interface IRaidsController {
        ObservableCollection<FollowModel> Follows { get; set; }
        void Load();
        void Save();
    }
}