using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using WitchScaper.Common;
using WitchScaper.Core.Character;
using Zenject;

namespace WitchScaper.Core
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private Rigidbody2D _rigidbody2D;
        private ProjectileData _data;
        private AudioManager _audioManager;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var col = GetComponent<Collider2D>();
            
            Observable.Timer(TimeSpan.FromSeconds(3f)).Subscribe(o => Destroy(gameObject)).AddTo(_disposable);
            col.OnTriggerEnter2DAsObservable().Subscribe(HandleColliderEnter).AddTo(_disposable);
        }

        [Inject]
        public void SetDependencies(ProjectileData data, AudioManager audioManager)
        {
            _data = data;
            _audioManager = audioManager;
        }

        public void SetForce(Vector3 direction)
        {
            _rigidbody2D.AddForce(direction * _data.Speed, ForceMode2D.Impulse);
        }

        private void HandleColliderEnter(Collider2D other)
        {
            var enemy = other.GetComponent<EnemyController>();

            if (enemy)
            {
                Hit(enemy);
                return;
            }
            
            Miss();
        }

        private void Hit(EnemyController enemy)
        {
            enemy.OnHit(_data);
            Explode();
        }

        private void Miss()
        {
            Explode();
        }

        private void Explode()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}