using System;
using System.Collections.Generic;
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
using GbfRaidfinder.ViewModels;

namespace GbfRaidfinder.Views {
    /// <summary>
    /// Interaction logic for RaidBossesView.xaml
    /// </summary>
    public partial class RaidBossesView : UserControl {
        public RaidBossesView() {
            InitializeComponent();
        }
        
        private void ShowAll() {
            foreach (var itemsControlItem in itemsControl.Items) {
                var item = (RaidListItem)itemsControlItem;
                item.Visibility = true;
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e) {
            var searchbox = (TextBox)sender;
            var term = searchbox.Text;
            if (string.IsNullOrWhiteSpace(term)) {
                ShowAll();
                return;
            }
            foreach (var itemsControlItem in itemsControl.Items) {
                var item = (RaidListItem)itemsControlItem;
                if (!item.English.ToLower().Contains(term.ToLower()) && !item.Japanese.ToLower().Contains(term.ToLower())) {
                    item.Visibility = false;
                    continue;
                }
                item.Visibility = true;
            }
        }
    }
}
