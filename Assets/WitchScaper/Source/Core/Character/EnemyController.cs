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
            
            _agent.SetDestination(target);
            //_aiPath.maxSpeed = _data.Speed; TODO: AI
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
            //_aiPath.canMove = !isTurned; //TODO: AI
            
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
                return;
            }
            
            //TODO:AI
            // var distance = (_destinationSetter.target.position - transform.position).magnitude;
            // if (distance <= _data.AggroRadius)
            // {
            //     _aiPath.canMove = true;
            //     _aiPath.canSearch = true;
            //     _aggroed = true;
            // }
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}