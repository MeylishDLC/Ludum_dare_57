using System;
using System.Threading;
using AudioSystem;
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
        private PlayerController _playerController;
        private SoundManager _soundManager;

        [Inject]
        public void Initialize(PlayerController playerController, SoundManager soundManager)
        {
            _soundManager = soundManager;
            _playerController = playerController;
        }
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
            _soundManager.PlayOneShot(_soundManager.FMODEvents.HookBrokenSound);
            _playerController.DisableMovement();
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
            _playerController.EnableMovement();
            anchorController.CanMoveDown = true;
        }
        
    }
}