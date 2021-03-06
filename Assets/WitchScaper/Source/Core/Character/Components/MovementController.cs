﻿using System;
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
        private readonly AnimationController _animationController;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private Vector2 _movement;

        private Vector2 _impulse;

        public MovementController(Rigidbody2D rigidbody2D, CharacterControllerData controllerData,
            GameState state, Transform transform, AnimationController animationController)
        {
            _rigidbody2D = rigidbody2D;
            _controllerData = controllerData;
            _state = state;
            _transform = transform;
            _animationController = animationController;
        }

        public void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        }

        public void SetImpulse(Vector2 impulse, float force)
        {
            _impulse = impulse;
        }

        public void ApplyMovement()
        {
            if (_state.CurrentState != GameState.State.Battle)
            {
                return;
            }

            if (_movement == Vector2.zero)
            {
                _animationController.SetState(AnimationController.State.Idle);
                return;
            }
            _rigidbody2D.MovePosition(_rigidbody2D.position + _movement * (_controllerData.Speed * Time.fixedDeltaTime));
            _animationController.SetState(_movement.magnitude <= 0.01? AnimationController.State.Idle :AnimationController.State.Moving);
        }
        
        public void DashToEnemy(EnemyController enemy)
        {
            const float dashTime = 0.2f;

            var startPosition = _rigidbody2D.position;
            DashAsObservable(dashTime, startPosition, enemy.transform.position).Subscribe(o => { }, enemy.Kill).AddTo(_disposable).AddTo(_disposable);
        }

        private IObservable<Unit> DashAsObservable(float dashTime, Vector2 startPos, Vector2 endPos)
        {
            return Observable.Create<Unit>(o =>
            {
                _state.PlayerState.Invulnerable = true;
                var currentTime = 0f;
                var disposable = new CompositeDisposable();

                Observable.EveryFixedUpdate().Subscribe(u =>
                {
                    var targetPos = Vector2.Lerp(startPos, endPos, currentTime / dashTime);
                    _transform.position = new Vector3(targetPos.x, targetPos.y, -0.3f);
                    currentTime += Time.fixedDeltaTime;
                    o.OnNext(Unit.Default);
                    if (currentTime >= dashTime)
                    {
                        _state.PlayerState.Invulnerable = false;
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