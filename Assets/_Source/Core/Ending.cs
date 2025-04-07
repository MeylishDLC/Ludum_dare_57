using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using AudioSystem;
using Cinemachine;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using QuickTimeEvents;
using Rocks;
using RopeScript;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;
using Random = System.Random;

namespace Core
{
    public class Ending: MonoBehaviour
    {
        public event Action OnEnd;

        [Header("Ending Stuff")] 
        [SerializeField] private Flashlight flashlight;
        [SerializeField] private DeadFriend deadFriend;
        [SerializeField] private GiantRockKiller giantRockKillerPrefab;
        [SerializeField] private List<GameObject> otherRocksToSpawn;
        [SerializeField] private Transform giantRockKillerSpawnPoint;
        
        [SerializeField] private CinemachineVirtualCamera mainCamera;
        [SerializeField] private CinemachineVirtualCamera bestieCamera;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private float impulseStrength;

        [Header("Timings")]
        [SerializeField] private float timeToListenLastWords = 1;
        [SerializeField] private float timeBeforeRockFall = 1;
        [SerializeField] private float timeBeforeSceneFadeOut = 2;
        
        private GiantRockKiller _spawnedGiantRockKiller;
        private int _mainCamPriority;
        private CancellationToken _ctOnDestroy;
        private AnchorController _anchorController;
        private SoundManager _soundManager;

        [Inject]
        public void Initialize(AnchorController anchorController, SoundManager soundManager)
        {
            _anchorController = anchorController;
            _soundManager = soundManager;
        }
        private void Start()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            _mainCamPriority = mainCamera.Priority;
            _anchorController.OnEndReached += PlayCutscene;
            //PlayCutscene();
        }
        private void PlayCutscene()
        {
            _anchorController.OnEndReached -= PlayCutscene;
            PlayCutsceneAsync(_ctOnDestroy).Forget();
        }
        private async UniTask PlayCutsceneAsync(CancellationToken token)
        {
            bestieCamera.Priority = _mainCamPriority++;
            _soundManager.PlayOneShot(_soundManager.FMODEvents.LastMessageSound);
            await UniTask.Delay(TimeSpan.FromSeconds(timeToListenLastWords), cancellationToken: token);
            
            _soundManager.PlayOneShot(_soundManager.FMODEvents.RocksStartFallingSound);
            await UniTask.Delay(TimeSpan.FromSeconds(timeBeforeRockFall), cancellationToken: token);
            SpawnGiantRock();
        }
        private void SpawnGiantRock()
        {
            _spawnedGiantRockKiller = Instantiate(giantRockKillerPrefab, giantRockKillerSpawnPoint.position, Quaternion.identity);
            _spawnedGiantRockKiller.OnFall += ShakeCam;
            SpawnSmallRocksAsync(_ctOnDestroy).Forget();
        }
        private void ShakeCam()
        {
            _spawnedGiantRockKiller.OnFall -= ShakeCam;
            TurnOffLight();
            _soundManager.PlayOneShot(_soundManager.FMODEvents.GiantRockKillsPlayerSound);
            impulseSource.GenerateImpulse(impulseStrength);
            ShowEndingAsync(_ctOnDestroy).Forget();
        }
        private async UniTask ShowEndingAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeBeforeSceneFadeOut), cancellationToken: token);
            OnEnd?.Invoke();
        }

        private void TurnOffLight()
        {
            flashlight.BlinkAndTurnOffAsync(_ctOnDestroy).Forget();
            deadFriend.BlinkAndTurnOffAsync(_ctOnDestroy).Forget();
        }
        private async UniTask SpawnSmallRocksAsync(CancellationToken token)
        {
            Vector2 spawnCenter = giantRockKillerSpawnPoint.position + new Vector3(0, 3f, 0);

            foreach (var rock in otherRocksToSpawn.OrderBy(x => UnityEngine.Random.Range(0f, 1f)))
            {
                if (!rock)
                {
                    continue;
                }

                var offset = new Vector2(UnityEngine.Random.Range(-1.5f, 1.5f), UnityEngine.Random.Range(0f, 1.5f));
                var spawnPos = spawnCenter + offset;

                Instantiate(rock, spawnPos, Quaternion.identity);

                var delay = UnityEngine.Random.Range(0.02f, 0.15f);
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            }
        }
    }
}