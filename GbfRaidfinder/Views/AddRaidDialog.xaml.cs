using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GbfRaidfinder.Data;

namespace GbfRaidfinder.Views {
    /// <summary>
    /// Interaction logic for AddRaidDialog.xaml
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
