using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RopeScript;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class EndingScreen: MonoBehaviour
    {
        [SerializeField] private Image endingScreen;
        [SerializeField] private float fadeInTime;
        [SerializeField] private Ending ending;

        private CancellationToken _ctOnDestroy;
        
        private void Start()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            ending.OnEnd += ShowEndingScreen;
        }
        private void ShowEndingScreen()
        {
            ShowEndingScreenAsync(_ctOnDestroy).Forget();
        }
        private async UniTask ShowEndingScreenAsync(CancellationToken token)
        {
            endingScreen.gameObject.SetActive(true);
            await FadeScreen(0, 0, _ctOnDestroy);
            await FadeScreen(1, fadeInTime, _ctOnDestroy);
        }
        private UniTask FadeScreen(float fadeValue, float duration, CancellationToken token)
        {
            var texts = endingScreen.GetComponentsInChildren<TMP_Text>();

            var tasks = new List<UniTask>
            {
                endingScreen.DOFade(fadeValue, duration).ToUniTask(cancellationToken: token)
            };
            tasks.AddRange(Enumerable.Select(texts, text => text.DOFade(fadeValue, duration)
                .ToUniTask(cancellationToken: token)));

            return UniTask.WhenAll(tasks);
        }
    }
}