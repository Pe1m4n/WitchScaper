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

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private AmmoContainer _ammoContainer;
        private GameState _gameState;
        private PlayerCharacterController _playerController;
        private QTEProgress _lastProgress;
        
        [Inject]
        public void SetDependencies(AmmoContainer ammoContainer, GameState gameState, PlayerCharacterController playerController)
        {
            _ammoContainer = ammoContainer;
            _gameState = gameState;
            _playerController = playerController;
        }
        
        public IObservable<QTEProgress> StartQTE()
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
                    currentTime += Time.unscaledDeltaTime;
                    var progress = currentTime / _secondsToFill;
                    _progressImage.fillAmount = progress;
                    var passedSuccess = progress > successStart + _successWidth;
                    if (passedSuccess)
                    {
                        progressContainer.Successful = false;
                        o.OnNext(progressContainer);
                        o.OnCompleted();
                        gameObject.SetActive(false);
                        _ammoContainer.gameObject.SetActive(true);
                        _gameState.CurrentState = GameState.State.Battle;
                    }

                    var inSuccess = progress > successStart && !passedSuccess;
                    
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        progressContainer.Successful = inSuccess;
                        o.OnNext(progressContainer);
                        o.OnCompleted();
                        gameObject.SetActive(false);
                        _ammoContainer.gameObject.SetActive(true);
                        _gameState.CurrentState = GameState.State.Battle;
                    }

                    if (progress > 1f)
                    {
                        progressContainer.Successful = false;
                        o.OnNext(progressContainer);
                        o.OnCompleted();
                        gameObject.SetActive(false);
                        _ammoContainer.gameObject.SetActive(true);
                        _gameState.CurrentState = GameState.State.Battle;
                    }
                    
                }).AddTo(disposable);

                return disposable;
            });
        }

        private int GetSuccessZone(out float successStart)
        {
            var zone = Random.Range(0, 4);
            successStart = (zone + 1) * 0.25f;
            
            return zone;
        }
    }
}