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
        [SerializeField] private SceneController sceneControllerPrefab;
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
            var controller = Container.InstantiatePrefabForComponent<SceneController>(sceneControllerPrefab);
            Container.Bind<SceneController>().FromInstance(controller).AsSingle();
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