using System;
using Tweetinvi.Streaming;

namespace GbfRaidfinder.Interfaces {
    public interface ITweetObserver {
        IFilteredStream Stream { get; }
        void SetAddAction(Action<ITweetInfo> act);
        void Run();
        void Stop();
    }
}