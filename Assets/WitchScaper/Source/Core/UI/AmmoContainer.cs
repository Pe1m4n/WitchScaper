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
        [SerializeField] private ProjectileData.Type Type;
        [SerializeField] private Image _reloadMask;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public void SubscribeTo(IObservable<GameState> state)
        {
            List<ColorType> lastColorTypes = new List<ColorType>();
            state.Select(s => Type == ProjectileData.Type.Hex ? s.PlayerState.HexAmmo : s.PlayerState.DamageAmmo)
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

            state.Select(s =>
                    Type == ProjectileData.Type.Hex ? s.PlayerState.TimeToReloadHex : s.PlayerState.TimeToReloadAmmo)
                .Subscribe(
                    v =>
                    {
                        _reloadMask.fillAmount = Mathf.Clamp01(v);
                    }).AddTo(_disposable);
        }
        
        public void SetColors(List<ColorType> colorTypes)
        {
            for (int i = 0; i < colorTypes.Count; i++)
            {
                _slots[i].SetColor(colorTypes[i]);
            }
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}