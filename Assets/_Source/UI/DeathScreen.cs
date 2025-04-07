using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class DeathScreen : MonoBehaviour
    {
        [SerializeField] private Image deathScreen;
        [SerializeField] private float fadeInTime;
        [SerializeField] private Button restartButton;

        [Header("Scene")]
        [SerializeField] private Transform triggersParent;
        
        private PlayerController _player;
        private SceneController _sceneController;
        private CancellationToken _ctOnDestroy;

        [Inject]
        public void Initialize(PlayerController playerController, SceneController sceneController)
        {
            _player = playerController;
            _sceneController = sceneController;
        }
        private void Start()
        {
            restartButton.onClick.AddListener(Restart);
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            _player.OnPlayerDeath += ShowDeathScreen;
        }
        private void ShowDeathScreen()
        {
            restartButton.interactable = false;
            ShowDeathScreenAsync(_ctOnDestroy).Forget();
        }
        private async UniTask ShowDeathScreenAsync(CancellationToken token)
        {
            deathScreen.gameObject.SetActive(true);
            await FadeDeathScreen(0, 0, _ctOnDestroy);
            await FadeDeathScreen(1, fadeInTime, _ctOnDestroy);
            restartButton.interactable = true;
            CleanUp();
        }
        private UniTask FadeDeathScreen(float fadeValue, float duration, CancellationToken token)
        {
            var texts = deathScreen.GetComponentsInChildren<TMP_Text>();
            var buttonsImages = deathScreen.GetComponentsInChildren<Button>()
                .Select(i => i.GetComponent<Image>());

            var tasks = new List<UniTask>
            {
                deathScreen.DOFade(fadeValue, duration).ToUniTask(cancellationToken: token)
            };
            tasks.AddRange(Enumerable.Select(texts, text => text.DOFade(fadeValue, duration)
                .ToUniTask(cancellationToken: token)));
            tasks.AddRange(Enumerable.Select(buttonsImages, image => image.DOFade(fadeValue, duration)
                .ToUniTask(cancellationToken: token)));

            return UniTask.WhenAll(tasks);
        }

        private void CleanUp()
        {
            Destroy(triggersParent.gameObject);
            Destroy(_player.gameObject);
        }
        private void Restart()
        {
            _player.OnPlayerDeath -= ShowDeathScreen;
            restartButton.interactable = false;
            _sceneController.RestartScene();
        }
    }
}