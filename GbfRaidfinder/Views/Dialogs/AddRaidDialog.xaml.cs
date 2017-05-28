using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using GbfRaidfinder.Data;

namespace GbfRaidfinder.Views.Dialogs {
    /// <summary>
    ///     Interaction logic for AddRaidDialog.xaml
    /// </summary>
    public partial class AddRaidDialog : UserControl {
        private readonly ObservableCollection<RaidListItem> _raidList;

        public AddRaidDialog(ObservableCollection<RaidListItem> list) {
            InitializeComponent();
            _raidList = list;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            var newraid = new RaidListItem {
                English = En.Text,
                Japanese = Ja.Text,
                Image = Link.Text ?? "",
                Following = false
            };
            _raidList.Add(newraid);
        }
    }
}