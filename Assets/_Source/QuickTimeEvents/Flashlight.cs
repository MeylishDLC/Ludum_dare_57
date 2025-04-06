using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;
using Random = UnityEngine.Random;

namespace QuickTimeEvents
{
    public class Flashlight : MonoBehaviour
    {
        [SerializeField] private bool isWorking = true;
        [SerializeField] private Light2D flashlight;

        [SerializeField] private float minBreakTime = 5f;
        [SerializeField] private float maxBreakTime = 15f;

        private QTEManager _qteManager;
        private CancellationToken _ctOnDestroy;
        private void Start()
        {
            _qteManager = FindObjectOfType<QTEManager>();
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            BreakRandomlyAsync(_ctOnDestroy).Forget();
        }
        private void Update()
        {
            flashlight.enabled = isWorking;
        }
        private async UniTask BreakRandomlyAsync(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    var randomDelay = Random.Range(minBreakTime, maxBreakTime);
                    await UniTask.Delay(TimeSpan.FromSeconds(randomDelay), cancellationToken: token);
                    isWorking = false;
                    _qteManager.StartQTE(OnQTESuccess);
                }
            }
            catch (OperationCanceledException)
            {
                //
            }
        }
        private void OnQTESuccess()
        {
            isWorking = true;
        }
    }
}