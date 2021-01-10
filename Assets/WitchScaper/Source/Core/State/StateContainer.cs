using System;
using System.Collections.Generic;
using UniRx;
using WitchScaper.Core.UI;
using Zenject;

namespace WitchScaper.Core.State
{
    public class StateContainer : IDisposable, ITickable
    {
        private readonly GameState _state;
        private readonly ReactiveProperty<GameState> _reactiveState;

        public StateContainer(GameState state, IEnumerable<IStateObserver> observers, HealthContainer healthContainer)
        {
            _state = state;
            _reactiveState = new ReactiveProperty<GameState>(_state);
            foreach (var stateObserver in observers)
            {
                stateObserver.SubscribeTo(_reactiveState);
            }

            healthContainer.SubscribeTo(_reactiveState);
        }

        public void Dispose()
        {
            _reactiveState?.Dispose();
        }

        public void Tick()
        {
            _reactiveState.SetValueAndForceNotify(_state);
        }
    }
}