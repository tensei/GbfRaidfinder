using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace GbfRaidfinder.Data {
    public class RaidListItem : INotifyPropertyChanged {
        public string English { get; set; }
        public string Japanese { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }
        public bool Visibility { get; set; } = true;
        public event PropertyChangedEventHandler PropertyChanged;
        
    }
}