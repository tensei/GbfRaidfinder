using System.Diagnostics;
using System.Windows;
using GbfRaidfinder.Common;
using GbfRaidfinder.Data;
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
            if (!Debugger.IsAttached)
                ExceptionHandler.AddGlobalHandlers();

            var container = new UnityContainer();
            container.RegisterType<Raids>();
            container.RegisterInstance<ISettingsController>(new SettingsController());
            container.RegisterInstance<IRaidsController>(new RaidsController());
            container.RegisterInstance<IRaidlistController>(new RaidListController());
            container.RegisterInstance<ITweetProcessor>(new TweetProcessor());
            container.RegisterType<ILoginController, LoginController>();
            container.RegisterType<ITweetObserver, TweetObserver>();
            container.RegisterType<MainViewModel>();
            container.RegisterType<MainWindow>();
            container.Resolve<MainWindow>().Show();
        }
    }
}