using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace GbfRaidfinder.Interfaces {
    public interface ITweetObserver {
        bool Running { get; set; }
        IFilteredStream Stream { get; }
        void Run(ITwitterCredentials userCredentials);
    }
}