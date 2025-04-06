using System;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using RopeScript;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace QuickTimeEvents
{
    public class Harpoon: BaseQte
    {
        public override event Action<BaseQte, Action> OnTryStartQte;

        [SerializeField] private float delayBeforeEvent;
        [SerializeField] private float minBreakTime = 5f;
        [SerializeField] private float maxBreakTime = 15f;
        [SerializeField] private AnchorController anchorController;

        private CancellationToken _ctOnDestroy;
        private void Start()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            BreakRandomlyAsync(_ctOnDestroy).Forget();
        }
        private void Update()
        {
            anchorController.CanMoveDown = IsWorking;
        }
        public override void StartQte()
        {
            IsWorking = false;
        }
        private async UniTask BreakRandomlyAsync(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeEvent), cancellationToken: token);
                while (true)
                {
                    var randomDelay = Random.Range(minBreakTime, maxBreakTime);
                    await UniTask.Delay(TimeSpan.FromSeconds(randomDelay), cancellationToken: token);
                    OnTryStartQte?.Invoke(this, OnQTESuccess);
                }
            }
            catch (OperationCanceledException)
            {
                //
            }
        }
        private void OnQTESuccess()
        {
            IsWorking = true;
            anchorController.CanMoveDown = true;
        }
        
    }
}