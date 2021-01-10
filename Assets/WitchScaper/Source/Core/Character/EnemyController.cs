using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using WitchScaper.Core.State;
using Zenject;

namespace WitchScaper.Core.Character
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyData _data;
        [SerializeField] private GameObject _mainLayer;
        [SerializeField] private GameObject _turnedLayer;
        [SerializeField] private NavMeshAgent _agent;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        public bool Turned { get; private set; }
        private bool _aggroed;
        private GameState _gameState;
        private PlayerCharacterController _playerCharacterController;
        
        [Inject]
        public void SetDependencies(GameState gameState, PlayerCharacterController playerCharacterController)
        {
            _gameState = gameState;
            _playerCharacterController = playerCharacterController;
        }

        private void Awake()
        {
            var target = _playerCharacterController.transform.position;
            target.z = transform.position.z;

            _agent.speed = _data.Speed;
        }

        public void OnHit(ProjectileData projectileData)
        {
            if (_data.Color != projectileData.ProjectileColor)
            {
                return;
            }
            
            if (!Turned && projectileData.ProjectileType == ProjectileData.Type.Hex)
            {
                Turn(true);
            }
        }

        public void Turn(bool isTurned)
        {
            if (isTurned == Turned) return;
            
            Turned = isTurned;
            _mainLayer.SetActive(!isTurned);
            _turnedLayer.SetActive(isTurned);
            
            if (isTurned)
            {
                _gameState.EnemiesForQTE.Add(this);
                Observable.Timer(TimeSpan.FromSeconds(_data.TurnedTimeSeconds)).Subscribe(o => Turn(false))
                    .AddTo(_disposable);
            }
            else
            {
                _gameState.EnemiesForQTE.Remove(this);   
            }
        }

        public void Kill()
        {
            _gameState.EnemiesForQTE.Remove(this);
            Destroy(gameObject);
        }
        
        private void Update()
        {
            if (_aggroed)
            {
                _agent.SetDestination(_playerCharacterController.transform.position);
                _agent.isStopped = Turned;
                return;
            }
            
            var distance = (_playerCharacterController.transform.position - transform.position).magnitude;
            if (distance <= _data.AggroRadius)
            {
                _aggroed = true;
            }
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}