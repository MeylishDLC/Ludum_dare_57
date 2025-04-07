using System;
using AudioSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core
{
    public class SceneController: MonoBehaviour
    {
        private SoundManager _soundManager;
        [Inject]
        public void Initialize(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        private void Start()
        {
            StartAmbient();
        }
        private void OnDestroy()
        {
            _soundManager.CleanUp();
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