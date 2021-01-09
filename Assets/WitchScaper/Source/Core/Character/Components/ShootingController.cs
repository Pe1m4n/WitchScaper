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
        private readonly ProjectileDataContainer _projectileDataContainer;

        public ShootingController(ProjectileFactory projectileFactory, Transform shootingPivot,
            GameState state, IInputSystem inputSystem, ProjectileDataContainer projectileDataContainer)
        {
            _projectileFactory = projectileFactory;
            _shootingPivot = shootingPivot;
            _state = state;
            _inputSystem = inputSystem;
            _projectileDataContainer = projectileDataContainer;
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

            var indexToShoot = 0;
            var ammoContainer = isDamage ? _state.PlayerState.DamageAmmo : _state.PlayerState.HexAmmo;
            var projectileData = _projectileDataContainer.GetDataForColor(ammoContainer[indexToShoot], !isDamage);
            _state.PlayerState.UseAmmo(isDamage, indexToShoot);
            
            var projectile = _projectileFactory.Create(projectileData, _shootingPivot.position, rotation);
            projectile.SetForce((mousePos - new Vector2(_shootingPivot.position.x, _shootingPivot.position.y))
                .normalized);
        }
    }
}