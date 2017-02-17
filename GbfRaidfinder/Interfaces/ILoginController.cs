namespace GbfRaidfinder.Interfaces {
    public interface ILoginController {
        ITweetObserver TweetObserver { get; }
        string Pin { get; set; }
        void StartLogin();
        void Stop();
    }
}