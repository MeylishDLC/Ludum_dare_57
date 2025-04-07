using System;
using UnityEngine;
using Zenject;

namespace Rocks
{
    [RequireComponent(typeof(Collider2D))]
    public class RockTrigger: MonoBehaviour
    {
        [SerializeField] private GameObject rockPrefab;
        [SerializeField] private Transform rockFallPoint;
        [SerializeField] private float delayBeforeRockSpawn;
        private RockSpawnUtility _rockSpawnUtility;
        private bool _isTriggered;
        
        [Inject]
        public void Initialize(RockSpawnUtility rockSpawnUtility)
        {
            _rockSpawnUtility = rockSpawnUtility;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isTriggered)
            {
                return;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                TriggerRockFall();
            }
        }

        private void TriggerRockFall()
        {
            _isTriggered = true;
            if (rockPrefab)
            {
                _rockSpawnUtility.SpawnRock(rockPrefab, rockFallPoint.position);
            }
            else
            {
                _rockSpawnUtility.SpawnRandomRock(rockFallPoint.position);
            }
        }
    }
}