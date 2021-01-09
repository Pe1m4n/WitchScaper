using UnityEngine;
using WitchScaper.Common;
using WitchScaper.Core;
using WitchScaper.Core.State;
using Zenject;

namespace WitchScaper.Installers
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _projectilesContainer;
        
        public override void InstallBindings()
        {
            Container.Bind<GameState>().AsSingle();
            Container.Bind<Camera>().FromInstance(_camera).AsSingle();
            Container.BindInterfacesTo<UnityInputSystem>().AsSingle();
            Container.Bind<ProjectileFactory>().AsSingle().WithArguments(_projectilesContainer);
        }
    }
}