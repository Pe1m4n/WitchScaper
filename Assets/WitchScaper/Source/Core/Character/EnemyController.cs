using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace WitchScaper.Core.Character
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyData _data;
        [SerializeField] private GameObject _mainLayer;
        [SerializeField] private GameObject _turnedLayer;

        private CompositeDisposable _disposable = new CompositeDisposable();
        private bool _turned;
        
        [Inject]
        public void SetDependencies()
        {
            
        }

        public void OnHit(ProjectileData projectileData)
        {
            if (!_turned && projectileData.ProjectileType == ProjectileData.Type.Hex)
            {
                Turn(true);
            }
            
            if (_turned && projectileData.ProjectileType == ProjectileData.Type.Damage)
            {
                Destroy(gameObject);
            }
        }

        private void Turn(bool isTurned)
        {
            if (isTurned == _turned) return;
            
            _turned = isTurned;
            _mainLayer.SetActive(!isTurned);
            _turnedLayer.SetActive(isTurned);
            Observable.Timer(TimeSpan.FromSeconds(_data.TurnedTimeSeconds)).Subscribe(o => Turn(false))
                .AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}