using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GbfRaidfinder.Views {
    /// <summary>
    ///     Interaction logic for InfoView.xaml
    /// </summary>
    public partial class InfoView : UserControl {
        public InfoView() {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            try {
                Process.Start("https://twitch.streamlabs.com/tenseyi#/");
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
            }
        }

        private void ButtonBase_OnClick1(object sender, RoutedEventArgs e) {
            try {
                Process.Start("https://github.com/tensei/GbfRaidfinder");
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
            }
        }
    }
}