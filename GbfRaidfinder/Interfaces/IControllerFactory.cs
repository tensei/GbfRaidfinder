namespace GbfRaidfinder.Interfaces {
    public interface IControllerFactory {
        ISettingsController GetSettingsController { get; }
        IRaidsController GetRaidsController { get; }
        IRaidlistController GetRaidlistController { get; }
        ITweetObserver GetTweetObserver { get; }
        IBlacklistController GetBlacklistController { get; }
    }
}