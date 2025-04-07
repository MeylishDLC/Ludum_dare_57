using Core;
using QuickTimeEvents;
using Rocks;
using RopeScript;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private SceneContext sceneContext;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private PlayerController player;
        [SerializeField] private RockUtilityConfig rockUtilityConfig;
        [SerializeField] private AnchorController anchorController;
        
        private SceneController _sceneController;
        public override void InstallBindings()
        {
            BindCamera();
            BindPlayer();
            BindSceneController();
            BindRockSpawnUtility();
            BindAnchorController();
        }
        private void BindPlayer()
        {
            Container.Bind<PlayerController>().FromInstance(player).AsSingle();
        }
        private void BindCamera()
        {
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
        }
        private void BindSceneController()
        {
            _sceneController = new SceneController();
            Container.Bind<SceneController>().FromInstance(_sceneController).AsSingle();
        }

        private void BindRockSpawnUtility()
        {
            var utility = new RockSpawnUtility(rockUtilityConfig);
            Container.Bind<RockSpawnUtility>().FromInstance(utility).AsSingle();
        }

        private void BindAnchorController()
        {
            Container.Bind<AnchorController>().FromInstance(anchorController).AsSingle();
        }
    }
}