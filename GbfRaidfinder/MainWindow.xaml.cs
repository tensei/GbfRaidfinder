using System.ComponentModel;
using GbfRaidfinder.Data;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Properties;
using GbfRaidfinder.ViewModels;

namespace GbfRaidfinder {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly ISettingsController _settingsController;
        private readonly IRaidsController _raidsController;
        private readonly ILoginController _loginController;
        public MainWindow(Raids raids, ITweetProcessor tweetProcessor, IRaidsController raidsController, IRaidlistController raidlistController,
            ISettingsController settingsController, ILoginController loginController) {
            InitializeComponent();
            _settingsController = settingsController;
            _raidsController = raidsController;
            _loginController = loginController;
            DataContext = new MainViewModel(raids, tweetProcessor, raidsController, raidlistController, settingsController, loginController,
                TaskbarNotifyIcon);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            Settings.Default.Height = Height;
            Settings.Default.Width = Width;
            Settings.Default.Save();
            _settingsController.Save();
            _raidsController.Save();
            _loginController.Stop();

        }
    }
}