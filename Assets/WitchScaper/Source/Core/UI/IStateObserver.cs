using System;
using WitchScaper.Core.State;

namespace WitchScaper.Core.UI
{
    public interface IStateObserver
    {
        void SubscribeTo(IObservable<GameState> state);
    }
}