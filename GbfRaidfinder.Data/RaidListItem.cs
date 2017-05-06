using PropertyChanged;

namespace GbfRaidfinder.Data {
    [ImplementPropertyChanged]
    public class RaidListItem {
        public string English { get; set; }
        public string Japanese { get; set; }
        public string Image { get; set; }
        public bool Following { get; set; }
        public bool Visibility { get; set; } = true;
    }
}