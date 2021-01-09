using System;
using Pathfinding;
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
        [SerializeField] private AIPath _aiPath;
        [SerializeField] private AIDestinationSetter _destinationSetter;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private bool _turned;
        private bool _aggroed;
        
        [Inject]
        public void SetDependencies()
        {
            
        }

        private void Awake()
        {
            _aiPath.maxSpeed = _data.Speed;
        }

        public void OnHit(ProjectileData projectileData)
        {
            if (_data.Color != projectileData.ProjectileColor)
            {
                return;
            }
            
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
            _aiPath.canMove = !isTurned;
            
            if (isTurned)
            {
                Observable.Timer(TimeSpan.FromSeconds(_data.TurnedTimeSeconds)).Subscribe(o => Turn(false))
                    .AddTo(_disposable);
            }
        }

        private void Update()
        {
            if (_aggroed)
            {
                return;
            }
            
            var distance = (_destinationSetter.target.position - transform.position).magnitude;
            if (distance <= _data.AggroRadius)
            {
                _aiPath.canMove = true;
                _aiPath.canSearch = true;
                _aggroed = true;
            }
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}