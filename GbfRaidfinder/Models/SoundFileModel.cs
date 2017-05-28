using System.ComponentModel;
using PropertyChanged;

namespace GbfRaidfinder.Models {
    public class SoundFileModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public string Path { get; set; }
    }
}