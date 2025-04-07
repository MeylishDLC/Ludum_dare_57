using System;
using AudioSystem;
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
        
        private SoundManager _soundManager;

        [Inject]
        public void Initialize(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        private void Start()
        {
            startGameButton.onClick.AddListener(LoadGameScene);
            settingsButton.onClick.AddListener(OpenSettingsScreen);
            returnButton.onClick.AddListener(CloseSettingsScreen);
            
            _soundManager.InitializeMusic(_soundManager.FMODEvents.MainMenuMusic);
        }
        private void LoadGameScene()
        {
            _soundManager.CleanUp();
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        private void OpenSettingsScreen()
        {
            settingsScreen.gameObject.SetActive(true);
        }
        private void CloseSettingsScreen()
        {
            settingsScreen.gameObject.SetActive(false);
        }
    }
}