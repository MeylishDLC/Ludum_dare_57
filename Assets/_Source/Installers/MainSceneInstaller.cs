using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private SceneContext sceneContext;
        [SerializeField] private Camera mainCamera;
        public override void InstallBindings()
        {
            //BindContainer();
            BindCamera();
        }

        private void BindContainer()
        {
            Container.Bind<SceneContext>().FromInstance(sceneContext);
        }
        private void BindCamera()
        {
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
        }

      
    }
}