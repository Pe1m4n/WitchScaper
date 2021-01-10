using System;
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
        private readonly Transform _transform;
        private readonly Transform _arm;

        public ShootingController(ProjectileFactory projectileFactory, Transform shootingPivot,
            GameState state, IInputSystem inputSystem,
            ProjectileDataContainer projectileDataContainer,
            CharacterControllerData data,
            Transform transform, Transform arm)
        {
            _projectileFactory = projectileFactory;
            _shootingPivot = shootingPivot;
            _state = state;
            _inputSystem = inputSystem;
            _projectileDataContainer = projectileDataContainer;
            _data = data;
            _transform = transform;
            _arm = arm;
        }

        public void Update()
        {
            _state.PlayerState.TimeToReload -= Time.deltaTime;
            
            if (_state.CurrentState != GameState.State.Battle)
            {
                return;
            }

            var mousePos = _inputSystem.GetMousePosition();
            _transform.localScale =
                new Vector3(mousePos.x > _transform.position.x? 1 : -1, _transform.localScale.y, _transform.localScale.z);

            _arm.transform.rotation = Quaternion.FromToRotation(_arm.position, mousePos);
            
            if (Input.GetKey(KeyCode.Mouse0) && _state.PlayerState.TimeToReload <= 0f)
            {
                _state.PlayerState.LoadProgress += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && _state.PlayerState.TimeToReload <= 0f)
            {
                Shoot(mousePos);
            }
        }

        private void Shoot(Vector2 mousePos)
        {
            var rotation = Quaternion.FromToRotation(_shootingPivot.transform.position, mousePos);

            var indexToShoot = (int) (_state.PlayerState.LoadProgress / 0.33f);
            indexToShoot = Mathf.Clamp(indexToShoot, 0, 2);
            var projectileData = _projectileDataContainer.GetDataForColor(_state.PlayerState.Ammo[indexToShoot]);
            _state.PlayerState.UseAmmo(indexToShoot);
            
            var projectile = _projectileFactory.Create(projectileData, _shootingPivot.position, rotation);
            projectile.SetForce((mousePos - new Vector2(_shootingPivot.position.x, _shootingPivot.position.y))
                .normalized);

            _state.PlayerState.TimeToReload = _data.ReloadSeconds;
            _state.PlayerState.LoadProgress = 0f;
        }
    }
}