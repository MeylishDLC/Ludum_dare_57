using System;
using System.Threading;
using AudioSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class MainMenu: MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button settingsButton;
        
        [Header("Settings")]
        [SerializeField] private Button returnButton;
        [SerializeField] private RectTransform settingsScreen;
        
        [Header("Screen")]
        [SerializeField] private RectTransform startScreen;
        [SerializeField] private float startScreenDuration;
        
        private SoundManager _soundManager;
        private CancellationToken _ctOnDestroy;

        [Inject]
        public void Initialize(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        private void Start()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            startGameButton.onClick.AddListener(LoadGameScene);
            settingsButton.onClick.AddListener(OpenSettingsScreen);
            returnButton.onClick.AddListener(CloseSettingsScreen);
            
            _soundManager.InitializeMusic(_soundManager.FMODEvents.MainMenuMusic);
        }
        private void LoadGameScene()
        {
            ShowStartScreen(_ctOnDestroy).Forget();
        }
        private void OpenSettingsScreen()
        {
            settingsScreen.gameObject.SetActive(true);
        }
        private void CloseSettingsScreen()
        {
            settingsScreen.gameObject.SetActive(false);
        }
        private async UniTask ShowStartScreen(CancellationToken token)
        {
            startScreen.gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(startScreenDuration), cancellationToken: token);
            _soundManager.CleanUp();
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}