using System;
using UniRx;
using UnityEngine;
using WitchScaper.Common;
using WitchScaper.Core.State;
using WitchScaper.Core.UI;
using Zenject;

namespace WitchScaper.Core.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterControllerData _data;
        [SerializeField] private Transform _shootingPivot;
        [SerializeField] private Animator _animator;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private MovementController _movementController;
        private ShootingController _shootingController;
        private AnimationController _animationController;
        private QTEController.QTEProgress _lastProgress;
        private GameState _gameState;
        private QTEController _qteController;

        [Inject]
        public void SetDependencies(GameState gameState, ProjectileFactory projectileFactory,
            IInputSystem inputSystem, ProjectileDataContainer projectileDataContainer, QTEController qteController)
        {
            _gameState = gameState;
            _qteController = qteController;
            _animationController = new AnimationController(_animator);
            _movementController = new MovementController(GetComponent<Rigidbody2D>(), _data, gameState, transform, _animationController);
            _shootingController = new ShootingController(projectileFactory, _shootingPivot, gameState, inputSystem,
                projectileDataContainer, _data, transform);
        }

        private void Update()
        {
            _movementController?.Update();
            _shootingController?.Update();
            
            
            if (_gameState.EnemiesForQTE.Count == 0 || _gameState.CurrentState == GameState.State.QTE)
            {
                return;
            }

            var enemy = GetClosestEnemy(out var distance);

            if (distance < 10f && Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 0f;
                StartQTE(enemy, () =>
                {
                    Time.timeScale = 1f;
                    _movementController.DashToEnemy(enemy);
                }, () => Time.timeScale = 1f);
            }
        }

        private void StartQTE(EnemyController enemy, Action success, Action fail)
        {
            _qteController.StartQTE(enemy).Subscribe(p =>
            {
                _lastProgress = p;
            }, () =>
            {
                if (_lastProgress?.Successful != null && _lastProgress.Successful.Value)
                {
                    success?.Invoke();
                    return;
                }
                
                fail?.Invoke();
            }).AddTo(_disposable);
        }

        private EnemyController GetClosestEnemy(out float distance)
        {
            var playerPos = transform.position;

            var minDistance = float.MaxValue;
            EnemyController closestEnemy = null;
            for (int i = 0; i < _gameState.EnemiesForQTE.Count; i++)
            {
                var enemy = _gameState.EnemiesForQTE[i];
                var enemyDistance = (enemy.transform.position - playerPos).magnitude;
                if (enemyDistance < minDistance)
                {
                    closestEnemy = enemy;
                    minDistance = enemyDistance;
                }
            }

            distance = minDistance;
            return closestEnemy;
        }

        private void FixedUpdate()
        {
            _movementController?.ApplyMovement();
        }
    }
}