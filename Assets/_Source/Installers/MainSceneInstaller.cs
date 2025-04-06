using Core;
using QuickTimeEvents;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private SceneContext sceneContext;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private PlayerController player;
        [SerializeField] private QTEManager qteManager;
        public override void InstallBindings()
        {
            BindCamera();
            BindPlayer();
            BindQTEManager();
        }
        private void BindPlayer()
        {
            Container.Bind<PlayerController>().FromInstance(player).AsSingle();
        }
        private void BindCamera()
        {
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
        }
        private void BindQTEManager()
        {
            Container.Bind<QTEManager>().FromInstance(qteManager).AsSingle();
        }
      
    }
}