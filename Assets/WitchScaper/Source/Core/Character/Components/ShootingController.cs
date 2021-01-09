using UnityEngine;
using WitchScaper.Common;
using WitchScaper.Core.State;

namespace WitchScaper.Core.Character
{
    public class ShootingController
    {
        private readonly ProjectileFactory _projectileFactory;
        private readonly Transform _shootingPivot;
        private readonly GameState _state;
        private readonly IInputSystem _inputSystem;
        private readonly ProjectileData _projectileDataHex;
        private readonly ProjectileData _projectileDataDamage;

        public ShootingController(ProjectileFactory projectileFactory, Transform shootingPivot,
            GameState state, IInputSystem inputSystem, ProjectileData projectileDataHex, ProjectileData projectileDataDamage)
        {
            _projectileFactory = projectileFactory;
            _shootingPivot = shootingPivot;
            _state = state;
            _inputSystem = inputSystem;
            _projectileDataHex = projectileDataHex;
            _projectileDataDamage = projectileDataDamage;
        }

        public void Update()
        {
            if (_state.CurrentState != GameState.State.Battle)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot(false);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Shoot(true);
            }
        }

        private void Shoot(bool isDamage)
        {
            var mousePos = _inputSystem.GetMousePosition();
            var rotation = Quaternion.FromToRotation(_shootingPivot.transform.position, mousePos);

            var projectile = _projectileFactory.Create(isDamage? _projectileDataDamage : _projectileDataHex, _shootingPivot.position, rotation);
            projectile.SetForce((mousePos - new Vector2(_shootingPivot.position.x, _shootingPivot.position.y))
                .normalized);
        }
    }
}