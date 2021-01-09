using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace WitchScaper.Core
{
    public class ProjectileFactory
    {
        private readonly DiContainer _container;
        private readonly Transform _projectileContainer;

        public ProjectileFactory(DiContainer container, Transform projectileContainer)
        {
            _container = container;
            _projectileContainer = projectileContainer;
        }
        
        public Projectile Create(ProjectileData data, Vector3 position, Quaternion rotation)
        {
            var projectile = _container.InstantiatePrefabForComponent<Projectile>(data.Prefab, position, rotation, _projectileContainer, new List<object>(){data});
            return projectile;
        }
        
    }
}