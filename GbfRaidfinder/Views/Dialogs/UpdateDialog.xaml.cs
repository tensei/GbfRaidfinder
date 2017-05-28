using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GbfRaidfinder.Views.Dialogs {
    /// <summary>
    ///     Interaction logic for UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : UserControl {
        public UpdateDialog() {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            //https://github.com/tensei/GbfRaidfinder/releases
            try {
                Process.Start("https://github.com/tensei/GbfRaidfinder/releases");
            }
            catch (Exception exception) {
                Console.WriteLine(exception);
            }
        }
    }
}