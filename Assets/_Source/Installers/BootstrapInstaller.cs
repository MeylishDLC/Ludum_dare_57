using AudioSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BootstrapInstaller: MonoInstaller
    {
        [SerializeField] private SoundManager soundManagerPrefab;
        public override void InstallBindings()
        {
            BindSoundManager();
        }
        private void BindSoundManager()
        {
            var soundManager = Container.InstantiatePrefabForComponent<SoundManager>(soundManagerPrefab);
            Container.Bind<SoundManager>().FromInstance(soundManager).AsSingle();
        }
    }
}