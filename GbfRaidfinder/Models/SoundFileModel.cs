using PropertyChanged;

namespace GbfRaidfinder.Models {
    [ImplementPropertyChanged]
    public class SoundFileModel {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
