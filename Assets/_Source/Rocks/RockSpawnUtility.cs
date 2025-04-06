using UnityEngine;

namespace Rocks
{
    public class RockSpawnUtility
    {
        private readonly RockUtilityConfig _config;
        
        public RockSpawnUtility(RockUtilityConfig config)
        {
            _config = config;
        }
        public GameObject GetRandomRockPrefab()
        {
            return _config.RocksPrefabs[Random.Range(0, _config.RocksPrefabs.Count)];
        }
        public void SpawnRandomRock(Vector2 spawnPosition)
        {
            var rockPrefab = GetRandomRockPrefab();
            Object.Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
        }
        public void SpawnRock(GameObject rockPrefab, Vector2 spawnPosition)
        {
            Object.Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
        }
    }
}