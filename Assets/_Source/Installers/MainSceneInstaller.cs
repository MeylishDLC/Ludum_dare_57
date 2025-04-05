using Core;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private SceneContext sceneContext;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private PlayerController player;
        public override void InstallBindings()
        {
            BindCamera();
            BindPlayer();
        }
        private void BindPlayer()
        {
            Container.Bind<PlayerController>().FromInstance(player).AsSingle();
        }
        private void BindCamera()
        {
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
        }

      
    }
}