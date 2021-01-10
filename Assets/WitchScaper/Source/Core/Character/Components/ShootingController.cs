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
        private readonly CharacterControllerData _data;

        public ShootingController(ProjectileFactory projectileFactory, Transform shootingPivot,
            GameState state, IInputSystem inputSystem, ProjectileDataContainer projectileDataContainer, CharacterControllerData data)
        {
            _projectileFactory = projectileFactory;
            _shootingPivot = shootingPivot;
            _state = state;
            _inputSystem = inputSystem;
            _projectileDataContainer = projectileDataContainer;
            _data = data;
        }

        public void Update()
        {
            _state.PlayerState.TimeToReload -= Time.deltaTime;
            
            if (_state.CurrentState != GameState.State.Battle)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && _state.PlayerState.TimeToReload <= 0f)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            var mousePos = _inputSystem.GetMousePosition();
            var rotation = Quaternion.FromToRotation(_shootingPivot.transform.position, mousePos);

            var indexToShoot = 0;
            var projectileData = _projectileDataContainer.GetDataForColor(_state.PlayerState.Ammo[indexToShoot]);
            _state.PlayerState.UseAmmo(indexToShoot);
            
            var projectile = _projectileFactory.Create(projectileData, _shootingPivot.position, rotation);
            projectile.SetForce((mousePos - new Vector2(_shootingPivot.position.x, _shootingPivot.position.y))
                .normalized);

            _state.PlayerState.TimeToReload = _data.ReloadSeconds;
        }
    }
}