using System;
using UnityEngine;
using WitchScaper.Common;
using WitchScaper.Core.State;
using Zenject;

namespace WitchScaper.Core.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterControllerData _data;
        [SerializeField] private Transform _shootingPivot;
        
        private MovementController _movementController;
        private ShootingController _shootingController;

        [Inject]
        public void SetDependencies(GameState gameState, ProjectileFactory projectileFactory,
            IInputSystem inputSystem, ProjectileDataContainer projectileDataContainer)
        {
            _movementController = new MovementController(GetComponent<Rigidbody2D>(), _data, gameState);
            _shootingController = new ShootingController(projectileFactory, _shootingPivot, gameState, inputSystem,
                projectileDataContainer, _data);
        }

        private void Update()
        {
            _movementController?.Update();
            _shootingController?.Update();
        }

        private void FixedUpdate()
        {
            _movementController?.ApplyMovement();
        }
    }
}