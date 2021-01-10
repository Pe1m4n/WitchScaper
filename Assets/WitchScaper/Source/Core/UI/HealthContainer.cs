using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using WitchScaper.Core.State;

namespace WitchScaper.Core.UI
{
    public class HealthContainer : MonoBehaviour, IStateObserver
    {
        [SerializeField] private List<GameObject> _slots;
        public void SubscribeTo(IObservable<GameState> state)
        {
            state.Select(s => s.PlayerState.HP).Subscribe(v =>
            {
                for (int i = 0; i < _slots.Count; i++)
                {
                    _slots[i].SetActive(i < v);
                }
            });
        }
    }
}