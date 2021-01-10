using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WitchScaper.Core.Character;
using WitchScaper.Core.State;

namespace WitchScaper.Core.UI
{
    public class AmmoContainer : MonoBehaviour, IStateObserver
    {
        [SerializeField] private List<AmmoSlot> _slots;
        [SerializeField] private Image _reloadMask;
        [SerializeField] private Image _loadProgressbar;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public void SubscribeTo(IObservable<GameState> state)
        {
            List<ColorType> lastColorTypes = new List<ColorType>();
            state.Select(s => s.PlayerState.Ammo)
                .Subscribe(l =>
                {
                    if (lastColorTypes != null && l.SequenceEqual(lastColorTypes))
                    {
                        return;
                    }

                    SetColors(l);
                    lastColorTypes.Clear();
                    lastColorTypes.AddRange(l);
                }).AddTo(_disposable);

            state.Select(s => s.PlayerState.TimeToReload)
                .Subscribe(v =>
                    {
                        _reloadMask.fillAmount = Mathf.Clamp01(v);
                    }).AddTo(_disposable);

            state.Select(s => s.PlayerState.LoadProgress)
                .Subscribe(v =>
                {
                    SetLoadProgress(Mathf.Clamp01(v));
                }).AddTo(_disposable);
        }
        
        public void SetColors(List<ColorType> colorTypes)
        {
            for (int i = 0; i < colorTypes.Count; i++)
            {
                _slots[i].SetColor(colorTypes[i]);
            }
        }

        public void SetLoadProgress(float progress)
        {
            _loadProgressbar.fillAmount = progress;
            if (progress <= 0f)
            {
                _slots.ForEach(s => s.SetEnabled(true));
                return;
            }
            
            var indexToShoot = (int) (progress / 0.33f);
            indexToShoot = Mathf.Clamp(indexToShoot, 0, 2);

            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].SetEnabled(indexToShoot == i);
            }
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}