using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using GbfRaidfinder.Factorys;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Properties;
using GbfRaidfinder.ViewModels;

namespace GbfRaidfinder {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly ControllerFactory _controllerFactory;

        public MainWindow(ITweetProcessor tweetProcessor, ControllerFactory controllerFactory) {
            _controllerFactory = controllerFactory;
            InitializeComponent();
            DataContext = new MainViewModel(tweetProcessor, controllerFactory);
        }


        private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            Settings.Default.Height = Height;
            Settings.Default.Width = Width;
            //Settings.Default.BossViewHeight = slider.Value;
            Settings.Default.Save();
            _controllerFactory.GetSettingsController.Save();
            _controllerFactory.GetRaidlistController.Save();
            _controllerFactory.GetBlacklistController.Save();
            _controllerFactory.GetRaidsController.Save();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e) {
            var tgglbtn = sender as ToggleButton;
            var ch = tgglbtn?.IsChecked;
            if (ch == null) return;
            if ((bool) ch) {
                tgglbtn.ToolTip = "Global Sounds ON.";
                return;
            }
            tgglbtn.ToolTip = "Global Sounds OFF.";
        }

        private void ToggleButton2_Click(object sender, RoutedEventArgs e) {
            var tgglbtn = sender as ToggleButton;
            var ch = tgglbtn?.IsChecked;
            if (ch == null) return;
            if ((bool)ch) {
                tgglbtn.ToolTip = "Global Copy ON.";
                return;
            }
            tgglbtn.ToolTip = "Global Copy OFF.";
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}