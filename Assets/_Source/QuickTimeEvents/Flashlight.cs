using System;
using System.Threading;
using System.Threading.Tasks;
using AudioSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace QuickTimeEvents
{
    public class Flashlight : BaseQte
    {
        public override event Action<BaseQte, Action> OnTryStartQte;
        
        [SerializeField] private float delayBeforeEvent;
        [SerializeField] private float minBreakTime = 5f;
        [SerializeField] private float maxBreakTime = 15f;
        
        [Header("Light Settings")]
        [SerializeField] private Light2D flashlight;
        [SerializeField] private Light2D playerLight;
        [SerializeField] private float playerLightIntensityOnOff;
        
        [Header("Flickering")]
        [SerializeField] private float flickers = 3;
        [SerializeField] private float flickerInterval = 0.2f;

        private CancellationToken _ctOnDestroy;
        private SoundManager _soundManager;
        private float _playerLightIntensityDefault;
        [Inject]
        public void Initialize(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        private void Start()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            BreakRandomlyAsync(_ctOnDestroy).Forget();
            _playerLightIntensityDefault = playerLight.intensity;
        }
        private void Update()
        {
            flashlight.enabled = IsWorking;
        }
        public override void StartQte()
        {
            _soundManager.PlayOneShot(_soundManager.FMODEvents.FlashlightSound);
            IsWorking = false;
            playerLight.intensity = playerLightIntensityOnOff;
        }

        public async UniTask BlinkAndTurnOffAsync(CancellationToken token)
        {
            for (int i = 0; i < flickers; i++)
            {
                flashlight.enabled = true;
                playerLight.intensity = _playerLightIntensityDefault;
                await UniTask.Delay(TimeSpan.FromSeconds(flickerInterval), cancellationToken: token);
                flashlight.enabled = false;
                playerLight.intensity = playerLightIntensityOnOff;
                await UniTask.Delay(TimeSpan.FromSeconds(flickerInterval), cancellationToken: token);
            }
            playerLight.intensity = playerLightIntensityOnOff;
            flashlight.enabled = false;
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
            OnQTESuccessAsync(_ctOnDestroy).Forget();
        }
        private async UniTask OnQTESuccessAsync(CancellationToken token)
        {
            for (int i = 0; i < flickers; i++)
            {
                flashlight.enabled = true;
                playerLight.intensity = _playerLightIntensityDefault;
                await UniTask.Delay(TimeSpan.FromSeconds(flickerInterval), cancellationToken: token);
                flashlight.enabled = false;
                playerLight.intensity = playerLightIntensityOnOff;
                await UniTask.Delay(TimeSpan.FromSeconds(flickerInterval), cancellationToken: token);
            }
            IsWorking = true;
            _soundManager.PlayOneShot(_soundManager.FMODEvents.FlashlightSound);
            playerLight.intensity = _playerLightIntensityDefault;
            flashlight.enabled = true;
        }
    }
}