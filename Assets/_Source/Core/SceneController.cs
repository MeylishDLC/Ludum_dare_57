using System;
using System.Threading;
using AudioSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core
{
    public class SceneController: MonoBehaviour
    {
        [SerializeField] private float minSoundInterval = 15f;
        [SerializeField] private float maxSoundInterval = 40f;
        private SoundManager _soundManager;
        private CancellationToken _ctOnDestroy;
        [Inject]
        public void Initialize(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        private void Start()
        {
            StartAmbient();
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            PlayBatsSoundAsync(_ctOnDestroy).Forget();
        }
        private void OnDestroy()
        {
            _soundManager.CleanUp();
        }
        private async UniTask PlayBatsSoundAsync(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    var randomInterval = UnityEngine.Random.Range(minSoundInterval, maxSoundInterval);
                    await UniTask.Delay(TimeSpan.FromSeconds(randomInterval), cancellationToken: token);
                    _soundManager.PlayOneShot(_soundManager.FMODEvents.BatsSound);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }
        private void StartAmbient()
        {
            _soundManager.InitializeMusic(_soundManager.FMODEvents.CaveAmbient);
            _soundManager.InitializeMusic(_soundManager.FMODEvents.DropsFallingSound);
        }
        public void RestartScene()
        {
            var curSceneIndex = SceneManager.GetActiveScene().buildIndex; 
            SceneManager.LoadSceneAsync(curSceneIndex);
        }
    }
}