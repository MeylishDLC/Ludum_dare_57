using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Rocks
{
    public class RocksSpawner: MonoBehaviour
    {
        [SerializeField] private List<GameObject> rocksPrefabs;
        [SerializeField] private float minSpawnInterval;
        [SerializeField] private float maxSpawnInterval;
        [SerializeField] private Transform ropeHook;
        [SerializeField] private float spawnDistanceFromHook = 5f;
        [SerializeField] private float maxSpawnXPosition;
        [SerializeField] private float minSpawnXPosition;

        private CancellationToken _ctOnDestroy;
        private void Start()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            SpawnRocksAsync(_ctOnDestroy).Forget();
        }
        private async UniTask SpawnRocksAsync(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    var randomDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
                    await UniTask.Delay(TimeSpan.FromSeconds(randomDelay), cancellationToken: token);
                    SpawnRock();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        private void SpawnRock()
        {
            var spawnPos = GetSpawnPosition();
            var rockPrefab = GetRandomRockPrefab();
            Instantiate(rockPrefab, spawnPos, Quaternion.identity);
        }

        private Vector2 GetSpawnPosition()
        {
            var spawnYPos = ropeHook.position.y + spawnDistanceFromHook;
            var spawnXPos = Random.Range(minSpawnXPosition, maxSpawnXPosition);
            return new Vector2(spawnXPos, spawnYPos);
        }
        private GameObject GetRandomRockPrefab()
        {
            return rocksPrefabs[Random.Range(0, rocksPrefabs.Count)];
        }
    }
}