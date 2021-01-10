using System.Collections.Generic;
using UnityEngine;
using WitchScaper.Common;
using WitchScaper.Core;
using WitchScaper.Core.Character;
using WitchScaper.Core.State;
using WitchScaper.Core.UI;
using Zenject;

namespace WitchScaper.Installers
{
    public class BattleSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _projectilesContainer;
        [SerializeField] private List<ProjectileData> _projectileDatas;
        [SerializeField] private AmmoContainer _ammoContainer;
        [SerializeField] private QTEController _qteController;
        [SerializeField] private PlayerCharacterController _playerCharacterController;
        [SerializeField] private HealthContainer _healthContainer;
        
        public override void InstallBindings()
        {
            Container.Bind<GameState>().AsSingle();
            Container.Bind<Camera>().FromInstance(_camera).AsSingle();
            Container.BindInterfacesTo<UnityInputSystem>().AsSingle();
            Container.Bind<ProjectileFactory>().AsSingle().WithArguments(_projectilesContainer);
            Container.Bind<ProjectileDataContainer>().AsSingle();
            Container.Bind<IEnumerable<ProjectileData>>().FromInstance(_projectileDatas).AsSingle();
            Container.BindInterfacesAndSelfTo<AmmoContainer>().FromInstance(_ammoContainer).AsSingle();
            Container.BindInterfacesTo<StateContainer>().AsSingle().NonLazy();
            Container.Bind<QTEController>().FromInstance(_qteController).AsSingle();
            Container.Bind<PlayerCharacterController>().FromInstance(_playerCharacterController).AsSingle();
            Container.BindInterfacesTo<CameraArmSpring>().AsSingle().NonLazy();
            Container.Bind<HealthContainer>().FromInstance(_healthContainer).AsSingle();
        }
    }
}