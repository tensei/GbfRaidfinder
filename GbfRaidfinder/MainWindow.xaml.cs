using System.ComponentModel;
using GbfRaidfinder.Data;
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
            Settings.Default.Save();
            _controllerFactory.GetSettingsController.Save();
            _controllerFactory.GetRaidlistController.Save();
            _controllerFactory.GetRaidsController.Save();
        }
    }
}