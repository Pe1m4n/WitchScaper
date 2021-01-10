using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WitchScaper.Core.Character;
using WitchScaper.Core.State;
using Zenject;
using Random = UnityEngine.Random;

namespace WitchScaper.Core.UI
{
    public class QTEController : MonoBehaviour
    {
        public class QTEProgress
        {
            public bool? Successful;
        }
        
        [SerializeField] private Image _progressImage;
        [SerializeField] private Image _successImage;
        [SerializeField] [Range(0.1f, 0.25f)] private float _successWidth;
        [SerializeField] private float _secondsToFill;

        private AmmoContainer _ammoContainer;
        private GameState _gameState;
        
        [Inject]
        public void SetDependencies(AmmoContainer ammoContainer, GameState gameState, PlayerCharacterController playerController)
        {
            _ammoContainer = ammoContainer;
            _gameState = gameState;
        }
        
        public IObservable<QTEProgress> StartQTE(EnemyController enemy)
        {
            return Observable.Create<QTEProgress>(o =>
            {
                if (_gameState.CurrentState == GameState.State.QTE)
                {
                    o.OnCompleted();
                    return Disposable.Empty;
                }

                _gameState.CurrentState = GameState.State.QTE;
                gameObject.SetActive(true);
                _ammoContainer.gameObject.SetActive(false);
                var progressContainer = new QTEProgress();
                var disposable = new CompositeDisposable();
                _progressImage.fillAmount = 0f;
                _successImage.fillAmount = _successWidth;
                _successImage.fillOrigin = GetSuccessZone(out var successStart);

                var currentTime = 0f;
                Observable.EveryUpdate().Subscribe(u =>
                {
                    void endQte(bool successful)
                    {
                        progressContainer.Successful = successful;
                        o.OnNext(progressContainer);
                        o.OnCompleted();
                        gameObject.SetActive(false);
                        _ammoContainer.gameObject.SetActive(true);
                        _gameState.CurrentState = GameState.State.Battle; 
                    }
                    
                    if (!enemy.Turned)
                    {
                        o.OnCompleted();
                        return;
                    }
                    
                    currentTime += Time.unscaledDeltaTime;
                    var progress = currentTime / _secondsToFill;
                    _progressImage.fillAmount = progress;
                    var passedSuccess = progress >= successStart + _successWidth;
                    if (passedSuccess)
                    {
                        endQte(false);
                    }

                    var inSuccess = progress > successStart && !passedSuccess;
                    
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        endQte(inSuccess);
                    }

                    if (progress > 1f)
                    {
                        endQte(false);
                    }
                    
                }).AddTo(disposable);

                return disposable;
            });
        }

        private int GetSuccessZone(out float successStart)
        {
            var zone = Random.Range(0, 4);
            switch (zone)
            {
                case 0: //Bottom
                    zone = 2;
                    successStart = 0.25f;
                    break;
                case 1: //Right
                    successStart = 0.75f;
                    break;
                case 2: //Top
                    successStart = 0.5f;
                    break;
                case 3: //Left
                    zone = 2;
                    successStart = 0.25f;
                    break;
                default:
                    zone = 2;
                    successStart = 0.25f;
                    break;
            }
            
            return zone;
        }
    }
}