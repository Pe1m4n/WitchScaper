using System;
using UniRx;
using UnityEngine;
using WitchScaper.Core.State;

namespace WitchScaper.Core.Character
{
    public class MovementController : IDisposable
    {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly CharacterControllerData _controllerData;
        private readonly GameState _state;
        private readonly Transform _transform;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private Vector2 _movement;

        public MovementController(Rigidbody2D rigidbody2D, CharacterControllerData controllerData, GameState state, Transform transform)
        {
            _rigidbody2D = rigidbody2D;
            _controllerData = controllerData;
            _state = state;
            _transform = transform;
        }

        public void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        }

        public void ApplyMovement()
        {
            if (_state.CurrentState != GameState.State.Battle)
            {
                return;
            }
            
            _rigidbody2D.MovePosition(_rigidbody2D.position + _movement * (_controllerData.Speed * Time.fixedDeltaTime));
        }
        
        public void DashToEnemy(EnemyController enemy)
        {
            var direction = new Vector2(enemy.transform.position.x, enemy.transform.position.y) - _rigidbody2D.position;

            const float dashTime = 0.2f;

            var currentTime = 0f;
            var startPosition = _rigidbody2D.position;
            DashAsObservable(dashTime, startPosition, enemy.transform.position).Subscribe(o => { }, enemy.Kill).AddTo(_disposable).AddTo(_disposable);
        }

        private IObservable<Unit> DashAsObservable(float dashTime, Vector2 startPos, Vector2 endPos)
        {
            return Observable.Create<Unit>(o =>
            { 
                var currentTime = 0f;
                var disposable = new CompositeDisposable();

                Observable.EveryFixedUpdate().Subscribe(u =>
                {
                    var targetPos = Vector2.Lerp(startPos, endPos, currentTime / dashTime);
                    _transform.position = new Vector3(targetPos.x, targetPos.y, -0.1f);
                    currentTime += Time.fixedDeltaTime;
                    o.OnNext(Unit.Default);
                    if (currentTime >= dashTime)
                    {
                        o.OnCompleted();
                    }
                }).AddTo(disposable);
                return disposable;
            });
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}