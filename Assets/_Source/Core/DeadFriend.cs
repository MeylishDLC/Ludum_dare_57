using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core
{
    public class DeadFriend: MonoBehaviour
    {
        [Header("Light Settings")]
        [SerializeField] private Light2D mainLight;
        [SerializeField] private Light2D characterLight;
        [SerializeField] private float charLightIntensityOnOff;
        
        [Header("Flickering")]
        [SerializeField] private float flickers = 3;
        [SerializeField] private float flickerInterval = 0.2f;
        
        private float _charLightIntensityDefault;

        private void Start()
        {
            _charLightIntensityDefault = characterLight.intensity;
        }

        public async UniTask BlinkAndTurnOffAsync(CancellationToken token)
        {
            for (int i = 0; i < flickers; i++)
            {
                mainLight.enabled = true;
                characterLight.intensity = _charLightIntensityDefault;
                await UniTask.Delay(TimeSpan.FromSeconds(flickerInterval), cancellationToken: token);
                mainLight.enabled = false;
                characterLight.intensity = charLightIntensityOnOff;
                await UniTask.Delay(TimeSpan.FromSeconds(flickerInterval), cancellationToken: token);
            }
            characterLight.intensity = charLightIntensityOnOff;
            mainLight.enabled = false;
        }
    }
}