using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace Rocks
{
    public class RocksSpawner: MonoBehaviour
    {
        [SerializeField] private Transform ropeHook;
        [SerializeField] private float spawnDistanceFromHook = 5f;
        [SerializeField] private float maxSpawnXPosition;
        [SerializeField] private float minSpawnXPosition;
        
        [Header("Timings")]
        [SerializeField] private float minSpawnInterval;
        [SerializeField] private float maxSpawnInterval;
        [SerializeField] private float timeBeforeStartSpawn = 30;

        private CancellationToken _ctOnDestroy;
        private RockSpawnUtility _rockSpawnUtility;

        [Inject]
        public void Initialize(RockSpawnUtility rockSpawnUtility)
        {
            _rockSpawnUtility = rockSpawnUtility;
        }
        private void Start()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            SpawnRocksAsync(_ctOnDestroy).Forget();
        }
        private async UniTask SpawnRocksAsync(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(timeBeforeStartSpawn), cancellationToken: token);
                while (true)
                {
                    var spawnPos = GetSpawnPosition();
                    _rockSpawnUtility.SpawnRandomRock(spawnPos);
                    var randomDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
                    await UniTask.Delay(TimeSpan.FromSeconds(randomDelay), cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        private Vector2 GetSpawnPosition()
        {
            var spawnYPos = ropeHook.position.y + spawnDistanceFromHook;
            var spawnXPos = Random.Range(minSpawnXPosition, maxSpawnXPosition);
            return new Vector2(spawnXPos, spawnYPos);
        }
    }
}