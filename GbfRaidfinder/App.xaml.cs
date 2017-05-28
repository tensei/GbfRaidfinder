using System.Diagnostics;
using System.Windows;
using GbfRaidfinder.Common;
using GbfRaidfinder.Data;
using GbfRaidfinder.Factorys;
using GbfRaidfinder.Interfaces;
using GbfRaidfinder.Twitter;
using GbfRaidfinder.ViewModels;
using Microsoft.Practices.Unity;

namespace GbfRaidfinder {
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private void AppStartup(object sender, StartupEventArgs args) {
            if (!Debugger.IsAttached) {
                ExceptionHandler.AddGlobalHandlers();
            }

            var container = new UnityContainer();
            container.RegisterType<Raids>();
            container.RegisterInstance<IGlobalVariables>(new GlobalVariables());
            container.RegisterType<IControllerFactory, ControllerFactory>();
            container.RegisterInstance<ISettingsController>(new SettingsController());
            container.RegisterInstance<ITwitterAuthenticator>(
                new TwitterAuthenticator(container.Resolve<ISettingsController>()));
            container.RegisterInstance<IBlacklistController>(new BlacklistController());
            container.RegisterInstance<IRaidsController>(
                new RaidsController(container.Resolve<IBlacklistController>()));
            container.RegisterInstance<IRaidlistController>(new RaidListController());
            container.RegisterInstance<ITweetProcessor>(new TweetProcessor());
            container.RegisterType<ITweetObserver, TweetObserver>();
            container.RegisterType<SettingsViewModel>();
            container.RegisterType<MainViewModel>();
            container.RegisterType<MainWindow>();
            container.Resolve<MainWindow>().Show();
        }
    }
}